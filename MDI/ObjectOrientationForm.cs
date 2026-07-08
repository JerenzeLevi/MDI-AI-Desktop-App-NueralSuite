using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace MDI
{
    public partial class ObjectOrientationForm : Form
    {
        // ── Data model ────────────────────────────────────────────────────────

        private class ObjectTemplate
        {
            public string       Name   { get; set; }
            public string       Dir    { get; set; }
            public List<Bitmap> Photos { get; } = new List<Bitmap>();
        }

        private struct MatchResult
        {
            public string     Name;
            public Color      Color;
            public RectangleF Rect;   // normalized 0-1 in frame space
            public float      Score;
        }

        // ── State ─────────────────────────────────────────────────────────────

        private string TemplatesDir => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "obj_templates");

        private List<ObjectTemplate> _templates        = new List<ObjectTemplate>();
        private ObjectTemplate       _selectedTemplate = null;

        private VideoCapture                 _capture     = null;
        private System.Windows.Forms.Timer   _detectTimer = null;
        private bool                         _isDetecting = false;
        private volatile float               _confThreshold = 0.60f;
        private List<MatchResult>            _detections  = new List<MatchResult>();
        private readonly object              _detLock     = new object();
        private DateTime                     _lastInfer   = DateTime.MinValue;

        private static readonly Color[] Palette = {
            Color.HotPink, Color.DodgerBlue, Color.SeaGreen, Color.Orange,
            Color.MediumOrchid, Color.Crimson, Color.Teal, Color.Gold
        };

        // ── Init ──────────────────────────────────────────────────────────────

        public ObjectOrientationForm()
        {
            InitializeComponent();
        }

        private void ObjectOrientationForm_Load(object sender, EventArgs e)
        {
            Directory.CreateDirectory(TemplatesDir);
            LoadAllTemplates();
            UpdateDetectStatus();
        }

        // ── Template loading ──────────────────────────────────────────────────

        private void LoadAllTemplates()
        {
            DisposeAllPhotos();
            _templates.Clear();

            foreach (var dir in Directory.GetDirectories(TemplatesDir).OrderBy(d => d))
            {
                string name = Path.GetFileName(dir);
                var tmpl = new ObjectTemplate { Name = name, Dir = dir };
                foreach (var f in GetPhotoFiles(dir))
                {
                    try
                    {
                        using (var tmp = Image.FromFile(f))
                            tmpl.Photos.Add(new Bitmap(tmp));
                    }
                    catch { }
                }
                _templates.Add(tmpl);
            }
            RefreshTemplateList();
        }

        private static IEnumerable<string> GetPhotoFiles(string dir) =>
            Directory.GetFiles(dir, "*.jpg")
                     .Concat(Directory.GetFiles(dir, "*.png"))
                     .OrderBy(f => f);

        private void DisposeAllPhotos()
        {
            foreach (var t in _templates)
                foreach (var p in t.Photos)
                    p.Dispose();
        }

        // ── Template list UI ──────────────────────────────────────────────────

        private void RefreshTemplateList()
        {
            lstTemplates.Items.Clear();
            foreach (var t in _templates)
                lstTemplates.Items.Add($"{t.Name}  ({t.Photos.Count} photo{(t.Photos.Count == 1 ? "" : "s")})");
            UpdatePhotoPreview();
            UpdateDetectStatus();
        }

        private void lstTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = lstTemplates.SelectedIndex;
            _selectedTemplate = idx >= 0 && idx < _templates.Count ? _templates[idx] : null;

            bool hasSel = _selectedTemplate != null;
            btnUploadPhotos.Enabled   = hasSel;
            btnTakePhoto.Enabled      = hasSel;
            btnRemoveTemplate.Enabled = hasSel;
            lblPhotoCount.Text = hasSel
                ? $"{_selectedTemplate.Photos.Count} / 5 photo(s) loaded"
                : "";

            UpdatePhotoPreview();
        }

        private void UpdatePhotoPreview()
        {
            foreach (Control c in flwPreview.Controls)
                if (c is Panel pnl)
                    foreach (Control pc in pnl.Controls)
                        if (pc is PictureBox pb) pb.Image = null; // don't dispose — photos owned by template
            flwPreview.Controls.Clear();

            if (_selectedTemplate == null || _selectedTemplate.Photos.Count == 0)
            {
                var hint = new Label
                {
                    Text      = _selectedTemplate == null
                        ? "Select an object from the list\nor add a new one below."
                        : $"No photos yet for '{_selectedTemplate.Name}'.\nClick Upload Photos or Take Photo.",
                    ForeColor = Color.Gray,
                    Font      = new Font("Segoe UI", 10),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size      = new Size(300, 80),
                    AutoSize  = false
                };
                flwPreview.Controls.Add(hint);
                return;
            }

            for (int i = 0; i < _selectedTemplate.Photos.Count; i++)
            {
                var photo = _selectedTemplate.Photos[i];
                var pnl = new Panel
                {
                    Size      = new Size(140, 120),
                    Margin    = new Padding(8),
                    BackColor = Color.FromArgb(40, 40, 40)
                };

                var pb = new PictureBox
                {
                    Size     = new Size(140, 105),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Image    = photo,
                    BackColor= Color.FromArgb(28, 28, 28),
                    Location = new Point(0, 0)
                };

                var lbl = new Label
                {
                    Text      = $"Photo {i + 1}",
                    ForeColor = Color.Silver,
                    Font      = new Font("Segoe UI", 7.5f),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size      = new Size(140, 15),
                    Location  = new Point(0, 105)
                };

                pnl.Controls.Add(pb);
                pnl.Controls.Add(lbl);
                flwPreview.Controls.Add(pnl);
            }
        }

        private void UpdateDetectStatus()
        {
            int count = _templates.Count(t => t.Photos.Count > 0);
            lblDetectStatus.Text = count == 0
                ? "No objects yet — add templates first."
                : $"{count} object{(count == 1 ? "" : "s")} ready for detection.";
        }

        // ── Template management actions ────────────────────────────────────────

        private void btnAddTemplate_Click(object sender, EventArgs e)
        {
            string name = txtObjectName.Text.Trim().Replace(" ", "_");
            if (string.IsNullOrEmpty(name)) return;

            if (_templates.Any(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("An object with that name already exists.", "Duplicate",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string dir = Path.Combine(TemplatesDir, name);
            Directory.CreateDirectory(dir);

            var tmpl = new ObjectTemplate { Name = name, Dir = dir };
            _templates.Add(tmpl);
            txtObjectName.Clear();
            RefreshTemplateList();
            lstTemplates.SelectedIndex = _templates.Count - 1;
        }

        private void btnRemoveTemplate_Click(object sender, EventArgs e)
        {
            if (_selectedTemplate == null) return;

            if (MessageBox.Show($"Remove '{_selectedTemplate.Name}' and all its photos?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            foreach (var p in _selectedTemplate.Photos) p.Dispose();
            try { Directory.Delete(_selectedTemplate.Dir, true); } catch { }
            _templates.Remove(_selectedTemplate);
            _selectedTemplate = null;
            RefreshTemplateList();
        }

        private void btnUploadPhotos_Click(object sender, EventArgs e)
        {
            if (_selectedTemplate == null) return;
            if (_selectedTemplate.Photos.Count >= 5)
            {
                MessageBox.Show("Maximum 5 photos per object.", "Limit Reached",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dlg = new OpenFileDialog
            {
                Filter    = "Images|*.jpg;*.jpeg;*.png",
                Multiselect = true,
                Title     = $"Choose photos of '{_selectedTemplate.Name}'"
            })
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;

                int slots = 5 - _selectedTemplate.Photos.Count;
                foreach (var file in dlg.FileNames.Take(slots))
                {
                    try
                    {
                        Bitmap loaded;
                        using (var tmp = Image.FromFile(file))
                            loaded = new Bitmap(tmp);

                        // Let user crop out the object from the background
                        Bitmap toSave;
                        using (var crop = new CropDialog(loaded))
                        {
                            if (crop.ShowDialog(this) != DialogResult.OK)
                            { loaded.Dispose(); continue; }
                            toSave = crop.CroppedImage;
                        }
                        loaded.Dispose();

                        string dest = Path.Combine(_selectedTemplate.Dir,
                            $"photo_{DateTime.Now:yyyyMMdd_HHmmss_fff}.jpg");
                        toSave.Save(dest, ImageFormat.Jpeg);
                        toSave.Dispose();

                        using (var tmp = Image.FromFile(dest))
                            _selectedTemplate.Photos.Add(new Bitmap(tmp));
                    }
                    catch { }
                }
            }
            RefreshTemplateList();
            lstTemplates.SelectedIndex = _templates.IndexOf(_selectedTemplate);
        }

        private void btnTakePhoto_Click(object sender, EventArgs e)
        {
            if (_selectedTemplate == null) return;
            if (_selectedTemplate.Photos.Count >= 5)
            {
                MessageBox.Show("Maximum 5 photos per object.", "Limit Reached",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Bitmap captured = null;
            using (var dlg = new PhotoCaptureDialog())
            {
                if (dlg.ShowDialog(this) != DialogResult.OK || dlg.CapturedImage == null) return;
                captured = dlg.CapturedImage;
            }

            // Let user crop the object out of the background
            Bitmap cropped;
            using (var crop = new CropDialog(captured))
            {
                if (crop.ShowDialog(this) != DialogResult.OK)
                { captured.Dispose(); return; }
                cropped = crop.CroppedImage;
            }
            captured.Dispose();

            string savePath = Path.Combine(_selectedTemplate.Dir,
                $"photo_{DateTime.Now:yyyyMMdd_HHmmss}.jpg");
            cropped.Save(savePath, ImageFormat.Jpeg);
            cropped.Dispose();

            using (var tmp = Image.FromFile(savePath))
                _selectedTemplate.Photos.Add(new Bitmap(tmp));

            RefreshTemplateList();
            lstTemplates.SelectedIndex = _templates.IndexOf(_selectedTemplate);
        }

        private void txtObjectName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) btnAddTemplate_Click(sender, e);
        }

        // ── Live detection ────────────────────────────────────────────────────

        private void trkConfidence_Scroll(object sender, EventArgs e)
        {
            _confThreshold     = trkConfidence.Value / 100f;
            lblConfidence.Text = $"Match threshold: {trkConfidence.Value}%";
        }

        private void btnStartDetect_Click(object sender, EventArgs e)
        {
            if (_isDetecting) StopDetection();
            else StartDetection();
        }

        private void StartDetection()
        {
            if (!_templates.Any(t => t.Photos.Count > 0))
            {
                MessageBox.Show("Add at least one object with photos before detecting.", "No Templates",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                _capture = new VideoCapture(0);
                if (!_capture.IsOpened)
                {
                    MessageBox.Show("Cannot open camera.", "Camera Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _capture.Dispose(); _capture = null;
                    return;
                }
                _detectTimer          = new System.Windows.Forms.Timer { Interval = 33 };
                _detectTimer.Tick    += DetectFrame;
                _detectTimer.Start();
                _isDetecting          = true;
                btnStartDetect.Text   = "Stop Detection";
                lblDetectStatus.Text  = "Detecting…";
            }
            catch (Exception ex) { MessageBox.Show("Camera error: " + ex.Message); }
        }

        private void StopDetection()
        {
            _detectTimer?.Stop();
            _detectTimer?.Dispose();
            _detectTimer = null;
            _capture?.Dispose(); _capture = null;
            if (picCamera.Image != null) { picCamera.Image.Dispose(); picCamera.Image = null; }
            _isDetecting = false;
            lock (_detLock) { _detections.Clear(); }
            lstDetected.Items.Clear();
            btnStartDetect.Text = "Start Detection";
            UpdateDetectStatus();
        }

        private void DetectFrame(object sender, EventArgs e)
        {
            if (_capture == null) return;

            var mat = new Mat();
            _capture.Read(mat);
            if (mat.IsEmpty) { mat.Dispose(); return; }

            Bitmap raw;
            using (var img = mat.ToImage<Bgr, byte>())
                raw = img.ToBitmap();
            mat.Dispose();

            if ((DateTime.Now - _lastInfer).TotalMilliseconds >= 400)
            {
                _lastInfer = DateTime.Now;
                var snapshot = new Bitmap(raw);
                System.Threading.Tasks.Task.Run(() => RunTemplateMatching(snapshot));
            }

            List<MatchResult> dets;
            lock (_detLock) { dets = new List<MatchResult>(_detections); }
            DrawDetections(raw, dets);

            if (picCamera.Image != null) picCamera.Image.Dispose();
            picCamera.Image = raw;

            lblDetectStatus.Text = $"Detecting — {dets.Count} match{(dets.Count == 1 ? "" : "es")} found";
            if (lstDetected.Items.Count != dets.Count)
            {
                lstDetected.Items.Clear();
                foreach (var d in dets)
                    lstDetected.Items.Add($"{d.Name}  {d.Score:P0}");
            }
        }

        private void DrawDetections(Bitmap bmp, List<MatchResult> dets)
        {
            using (var g = Graphics.FromImage(bmp))
            using (var font = new Font("Segoe UI", 9, FontStyle.Bold))
            {
                foreach (var det in dets)
                {
                    var r = new Rectangle(
                        (int)(det.Rect.X * bmp.Width),
                        (int)(det.Rect.Y * bmp.Height),
                        (int)(det.Rect.Width  * bmp.Width),
                        (int)(det.Rect.Height * bmp.Height));
                    r.X      = Math.Max(0, r.X);
                    r.Y      = Math.Max(0, r.Y);
                    r.Width  = Math.Min(r.Width,  bmp.Width  - r.X);
                    r.Height = Math.Min(r.Height, bmp.Height - r.Y);
                    if (r.Width <= 0 || r.Height <= 0) continue;

                    using (var pen = new Pen(det.Color, 2))
                        g.DrawRectangle(pen, r);

                    string label = $"{det.Name}  {det.Score:P0}";
                    var sz = g.MeasureString(label, font);
                    float ly = r.Y - sz.Height;
                    if (ly < 0) ly = r.Y;
                    using (var br = new SolidBrush(Color.FromArgb(180, det.Color)))
                        g.FillRectangle(br, r.X, ly, sz.Width + 4, sz.Height);
                    g.DrawString(label, font, Brushes.White, r.X + 2, ly);
                }
            }
        }

        // ── Template matching (background thread) ─────────────────────────────

        private void RunTemplateMatching(Bitmap frameBmp)
        {
            try
            {
                // Downscale frame for speed — keep at most 640px wide
                Bitmap workFrame = frameBmp;
                bool   ownWork   = false;
                float  fScale    = 1f;
                if (frameBmp.Width > 640)
                {
                    fScale    = 640f / frameBmp.Width;
                    int w = 640, h = (int)(frameBmp.Height * fScale);
                    workFrame = new Bitmap(frameBmp, w, h);
                    ownWork   = true;
                }

                var results = new List<MatchResult>();

                using (var frameImg  = new Image<Bgr, byte>(workFrame))
                using (var frameGray = new Mat())
                {
                    CvInvoke.CvtColor(frameImg.Mat, frameGray, ColorConversion.Bgr2Gray);

                    int colorIdx = 0;
                    foreach (var tmpl in _templates.ToList())
                    {
                        if (tmpl.Photos.Count == 0) { colorIdx++; continue; }

                        Color col       = Palette[colorIdx % Palette.Length];
                        float threshold = _confThreshold;
                        var   candidates= new List<(RectangleF rect, float score)>();

                        foreach (var photo in tmpl.Photos.ToList())
                        {
                            // Try a few scales so size changes are tolerated
                            foreach (float s in new[] { 0.5f, 0.75f, 1.0f, 1.25f, 1.5f })
                            {
                                int tw = Math.Max(1, (int)(photo.Width  * s));
                                int th = Math.Max(1, (int)(photo.Height * s));

                                if (tw >= frameGray.Cols || th >= frameGray.Rows) continue;
                                if (tw < 10 || th < 10) continue;

                                Bitmap scaledPhoto = (s == 1.0f) ? photo : new Bitmap(photo, tw, th);
                                try
                                {
                                    using (var tImg  = new Image<Bgr, byte>(scaledPhoto))
                                    using (var tGray = new Mat())
                                    using (var result= new Mat())
                                    {
                                        CvInvoke.CvtColor(tImg.Mat, tGray, ColorConversion.Bgr2Gray);
                                        CvInvoke.MatchTemplate(frameGray, tGray, result,
                                            TemplateMatchingType.CcoeffNormed);

                                        double minVal = 0, maxVal = 0;
                                        System.Drawing.Point minLoc = new System.Drawing.Point(),
                                                             maxLoc = new System.Drawing.Point();
                                        CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal,
                                            ref minLoc, ref maxLoc);

                                        if (maxVal >= threshold)
                                        {
                                            float fx = maxLoc.X / (float)frameGray.Cols;
                                            float fy = maxLoc.Y / (float)frameGray.Rows;
                                            float fw = tw / (float)frameGray.Cols;
                                            float fh = th / (float)frameGray.Rows;
                                            candidates.Add((new RectangleF(fx, fy, fw, fh), (float)maxVal));
                                        }
                                    }
                                }
                                finally
                                {
                                    if (s != 1.0f) scaledPhoto.Dispose();
                                }
                            }
                        }

                        // NMS per object, keep top 2
                        foreach (var (rect, score) in NMS(candidates, 0.4f).Take(2))
                            results.Add(new MatchResult { Name = tmpl.Name, Color = col, Rect = rect, Score = score });

                        colorIdx++;
                    }
                }

                if (ownWork) workFrame.Dispose();
                lock (_detLock) { _detections = results; }
            }
            catch { }
            finally { frameBmp.Dispose(); }
        }

        private static List<(RectangleF rect, float score)> NMS(
            List<(RectangleF rect, float score)> boxes, float iouThresh)
        {
            var sorted     = boxes.OrderByDescending(b => b.score).ToList();
            var result     = new List<(RectangleF, float)>();
            var suppressed = new bool[sorted.Count];
            for (int i = 0; i < sorted.Count; i++)
            {
                if (suppressed[i]) continue;
                result.Add(sorted[i]);
                for (int j = i + 1; j < sorted.Count; j++)
                    if (!suppressed[j] && RectIoU(sorted[i].rect, sorted[j].rect) > iouThresh)
                        suppressed[j] = true;
            }
            return result;
        }

        private static float RectIoU(RectangleF a, RectangleF b)
        {
            float ix1    = Math.Max(a.X, b.X), iy1 = Math.Max(a.Y, b.Y);
            float ix2    = Math.Min(a.Right, b.Right), iy2 = Math.Min(a.Bottom, b.Bottom);
            float inter  = Math.Max(0, ix2 - ix1) * Math.Max(0, iy2 - iy1);
            float aArea  = a.Width * a.Height, bArea = b.Width * b.Height;
            return inter / (aArea + bArea - inter + 1e-6f);
        }

        // ── Cleanup ───────────────────────────────────────────────────────────

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopDetection();
            DisposeAllPhotos();
            base.OnFormClosing(e);
        }

        // ── Crop / ROI selection dialog ───────────────────────────────────────

        private class CropDialog : Form
        {
            public Bitmap CroppedImage { get; private set; }

            private readonly Bitmap _source;
            private PictureBox      _pic;
            private Point           _dragStart, _dragEnd;
            private bool            _dragging;
            private Rectangle       _selection = Rectangle.Empty;

            public CropDialog(Bitmap source)
            {
                _source         = source;
                Text            = "Select the object — drag to draw a box, then click Crop";
                Size            = new Size(820, 600);
                StartPosition   = FormStartPosition.CenterParent;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox     = false;
                BackColor       = Color.FromArgb(28, 28, 28);

                _pic = new PictureBox
                {
                    Dock      = DockStyle.Fill,
                    SizeMode  = PictureBoxSizeMode.Zoom,
                    BackColor = Color.FromArgb(28, 28, 28),
                    Image     = source,
                    Cursor    = Cursors.Cross
                };
                _pic.MouseDown += OnMouseDown;
                _pic.MouseMove += OnMouseMove;
                _pic.MouseUp   += OnMouseUp;
                _pic.Paint     += OnPaint;

                // Right side: three buttons in a FlowLayout so they never get clipped
                var btnFlow = new FlowLayoutPanel
                {
                    Dock          = DockStyle.Right,
                    FlowDirection = FlowDirection.LeftToRight,
                    AutoSize      = true,
                    WrapContents  = false,
                    BackColor     = Color.Transparent,
                    Padding       = new Padding(0, 8, 8, 0),
                };

                var btnCrop = new Guna.UI2.WinForms.Guna2Button
                {
                    Text         = "Crop & Use Selection",
                    FillColor    = Color.HotPink,
                    ForeColor    = Color.White,
                    Font         = new Font("Segoe UI", 9f, FontStyle.Bold),
                    BorderRadius = 6,
                    Size         = new Size(170, 32),
                    Margin       = new Padding(0, 0, 8, 0),
                };

                var btnFull = new Guna.UI2.WinForms.Guna2Button
                {
                    Text         = "Use Full Image",
                    FillColor    = Color.FromArgb(90, 90, 95),
                    ForeColor    = Color.White,
                    Font         = new Font("Segoe UI", 9f, FontStyle.Bold),
                    BorderRadius = 6,
                    Size         = new Size(130, 32),
                    Margin       = new Padding(0, 0, 8, 0),
                };

                var btnCancel = new Guna.UI2.WinForms.Guna2Button
                {
                    Text         = "Cancel",
                    FillColor    = Color.FromArgb(70, 70, 75),
                    ForeColor    = Color.White,
                    Font         = new Font("Segoe UI", 9f, FontStyle.Bold),
                    BorderRadius = 6,
                    Size         = new Size(80, 32),
                    Margin       = new Padding(0),
                };

                btnCrop.Click   += (s, e) => DoCrop();
                btnFull.Click   += (s, e) => { CroppedImage = new Bitmap(_source); DialogResult = DialogResult.OK; Close(); };
                btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

                btnFlow.Controls.Add(btnCrop);
                btnFlow.Controls.Add(btnFull);
                btnFlow.Controls.Add(btnCancel);

                var hint = new Label
                {
                    Text      = "Drag to select only the object — exclude the background",
                    ForeColor = Color.Silver,
                    Font      = new Font("Segoe UI", 8.5f),
                    AutoSize  = true,
                    Dock      = DockStyle.Left,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding   = new Padding(10, 0, 0, 0),
                };

                var pnlBottom = new Panel
                {
                    Dock      = DockStyle.Bottom,
                    Height    = 48,
                    BackColor = Color.FromArgb(40, 40, 40)
                };

                pnlBottom.Controls.Add(btnFlow);
                pnlBottom.Controls.Add(hint);

                Controls.Add(_pic);
                Controls.Add(pnlBottom);
            }

            private void OnMouseDown(object sender, MouseEventArgs e)
            {
                if (e.Button != MouseButtons.Left) return;
                _dragStart = _dragEnd = e.Location;
                _dragging  = true;
            }

            private void OnMouseMove(object sender, MouseEventArgs e)
            {
                if (!_dragging) return;
                _dragEnd = e.Location;
                _pic.Invalidate();
            }

            private void OnMouseUp(object sender, MouseEventArgs e)
            {
                if (!_dragging || e.Button != MouseButtons.Left) return;
                _dragging  = false;
                _dragEnd   = e.Location;
                _selection = NormalizeRect(_dragStart, _dragEnd);
                _pic.Invalidate();
            }

            private void OnPaint(object sender, PaintEventArgs e)
            {
                if (_selection.IsEmpty && !_dragging) return;

                Rectangle sel = _dragging ? NormalizeRect(_dragStart, _dragEnd) : _selection;
                if (sel.Width < 4 || sel.Height < 4) return;

                // Dim outside selection
                using (var dim = new SolidBrush(Color.FromArgb(120, 0, 0, 0)))
                {
                    var r = _pic.ClientRectangle;
                    e.Graphics.FillRectangle(dim, new Rectangle(r.X, r.Y, r.Width, sel.Top - r.Y));
                    e.Graphics.FillRectangle(dim, new Rectangle(r.X, sel.Bottom, r.Width, r.Bottom - sel.Bottom));
                    e.Graphics.FillRectangle(dim, new Rectangle(r.X, sel.Top, sel.Left - r.X, sel.Height));
                    e.Graphics.FillRectangle(dim, new Rectangle(sel.Right, sel.Top, r.Right - sel.Right, sel.Height));
                }

                using (var pen = new Pen(Color.HotPink, 2)
                    { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                    e.Graphics.DrawRectangle(pen, sel);

                // Resize handles at corners
                using (var br = new SolidBrush(Color.HotPink))
                    foreach (var pt in new[] { sel.Location, new Point(sel.Right, sel.Top),
                                               new Point(sel.Left, sel.Bottom), new Point(sel.Right, sel.Bottom) })
                        e.Graphics.FillRectangle(br, pt.X - 4, pt.Y - 4, 8, 8);
            }

            private void DoCrop()
            {
                if (_selection.Width < 4 || _selection.Height < 4)
                {
                    MessageBox.Show("Please drag a selection first.", "No Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Convert screen selection back to image pixel coordinates
                var imgRect = GetImageRect();
                if (imgRect.IsEmpty) return;

                float scaleX = (float)_source.Width  / imgRect.Width;
                float scaleY = (float)_source.Height / imgRect.Height;

                int x = (int)((_selection.X - imgRect.X) * scaleX);
                int y = (int)((_selection.Y - imgRect.Y) * scaleY);
                int w = (int)(_selection.Width  * scaleX);
                int h = (int)(_selection.Height * scaleY);

                x = Math.Max(0, x); y = Math.Max(0, y);
                w = Math.Min(w, _source.Width  - x);
                h = Math.Min(h, _source.Height - y);

                if (w < 4 || h < 4)
                {
                    MessageBox.Show("Selection is outside the image bounds. Try again.", "Invalid Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                CroppedImage = _source.Clone(new Rectangle(x, y, w, h), _source.PixelFormat);
                DialogResult = DialogResult.OK;
                Close();
            }

            // Returns the actual pixel rectangle where the image is drawn inside the Zoom PictureBox
            private Rectangle GetImageRect()
            {
                int pw = _pic.Width, ph = _pic.Height;
                int iw = _source.Width, ih = _source.Height;
                float scale = Math.Min((float)pw / iw, (float)ph / ih);
                int dw = (int)(iw * scale), dh = (int)(ih * scale);
                return new Rectangle((pw - dw) / 2, (ph - dh) / 2, dw, dh);
            }

            private static Rectangle NormalizeRect(Point a, Point b) =>
                new Rectangle(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y),
                               Math.Abs(b.X - a.X), Math.Abs(b.Y - a.Y));
        }

        // ── Webcam capture dialog ─────────────────────────────────────────────

        private class PhotoCaptureDialog : Form
        {
            public Bitmap CapturedImage { get; private set; }

            private VideoCapture                  _cap;
            private System.Windows.Forms.Timer    _feedTimer;
            private PictureBox                    _preview;
            private Guna.UI2.WinForms.Guna2Button _btnCapture;
            private Guna.UI2.WinForms.Guna2Button _btnCancel;

            public PhotoCaptureDialog()
            {
                Text            = "Take Photo";
                Size            = new Size(580, 480);
                StartPosition   = FormStartPosition.CenterParent;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox     = false;
                MinimizeBox     = false;
                BackColor       = Color.FromArgb(28, 28, 28);

                _preview = new PictureBox
                {
                    Dock      = DockStyle.Fill,
                    SizeMode  = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Black
                };

                var pnlBottom = new Panel
                {
                    Dock      = DockStyle.Bottom,
                    Height    = 50,
                    BackColor = Color.FromArgb(40, 40, 40)
                };

                var hint = new Label
                {
                    Text      = "Position the object clearly and click Capture",
                    ForeColor = Color.Silver,
                    Font      = new Font("Segoe UI", 9),
                    AutoSize  = true,
                    Location  = new Point(10, 16)
                };

                _btnCapture = new Guna.UI2.WinForms.Guna2Button
                {
                    Text         = "Capture",
                    FillColor    = Color.HotPink,
                    ForeColor    = Color.White,
                    Font         = new Font("Segoe UI", 9, FontStyle.Bold),
                    BorderRadius = 6,
                    Size         = new Size(100, 32),
                    Location     = new Point(340, 9)
                };

                _btnCancel = new Guna.UI2.WinForms.Guna2Button
                {
                    Text         = "Cancel",
                    FillColor    = Color.FromArgb(90, 90, 95),
                    ForeColor    = Color.White,
                    Font         = new Font("Segoe UI", 9, FontStyle.Bold),
                    BorderRadius = 6,
                    Size         = new Size(80, 32),
                    Location     = new Point(448, 9)
                };

                _btnCapture.Click += (s, e) => DoCapture();
                _btnCancel.Click  += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

                pnlBottom.Controls.Add(hint);
                pnlBottom.Controls.Add(_btnCapture);
                pnlBottom.Controls.Add(_btnCancel);
                Controls.Add(_preview);
                Controls.Add(pnlBottom);
            }

            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);
                try
                {
                    _cap = new VideoCapture(0);
                    if (!_cap.IsOpened)
                    {
                        MessageBox.Show("Cannot open camera.", "Camera Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Close(); return;
                    }
                    _feedTimer       = new System.Windows.Forms.Timer { Interval = 33 };
                    _feedTimer.Tick += FeedPreview;
                    _feedTimer.Start();
                }
                catch (Exception ex) { MessageBox.Show("Camera error: " + ex.Message); Close(); }
            }

            private void FeedPreview(object sender, EventArgs e)
            {
                if (_cap == null) return;
                var mat = new Mat();
                _cap.Read(mat);
                if (mat.IsEmpty) { mat.Dispose(); return; }
                Bitmap bmp;
                using (var img = mat.ToImage<Bgr, byte>())
                    bmp = img.ToBitmap();
                mat.Dispose();
                if (_preview.Image != null) _preview.Image.Dispose();
                _preview.Image = bmp;
            }

            private void DoCapture()
            {
                if (_cap == null) return;
                var mat = new Mat();
                _cap.Read(mat);
                if (mat.IsEmpty) { mat.Dispose(); return; }
                using (var img = mat.ToImage<Bgr, byte>())
                    CapturedImage = img.ToBitmap();
                mat.Dispose();
                DialogResult = DialogResult.OK;
                Close();
            }

            protected override void OnFormClosing(FormClosingEventArgs e)
            {
                _feedTimer?.Stop();
                _feedTimer?.Dispose();
                _cap?.Dispose();
                base.OnFormClosing(e);
            }
        }
    }
}
