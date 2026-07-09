using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace MDI
{
    public partial class FaceRecognitionForm : Form
    {
        private VideoCapture _capture;
        private CascadeClassifier _faceClassifier;
        private LBPHFaceRecognizer _recognizer;
        private bool _isScanning = false;
        private bool _isTrained = false;

        private Dictionary<int, string> _labelNames = new Dictionary<int, string>();

        private const int FaceSize = 100;
        private const double RecognitionThreshold = 70.0;
        private readonly string DatasetRoot = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "faces_dataset");
        private readonly string CapturesRoot = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "captures");

        // Colors for up to 5 simultaneous faces
        private readonly Color[] FaceColors = {
            Color.DeepPink, Color.MediumOrchid, Color.DodgerBlue, Color.SeaGreen, Color.Orange
        };

        public FaceRecognitionForm()
        {
            InitializeComponent();

            try
            {
                string cascadePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "haarcascade_frontalface_default.xml");
                _faceClassifier = new CascadeClassifier(cascadePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load haarcascade_frontalface_default.xml\n\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _recognizer = new LBPHFaceRecognizer(1, 8, 8, 8, RecognitionThreshold);
            Directory.CreateDirectory(DatasetRoot);
            Directory.CreateDirectory(CapturesRoot);
        }

        private void FaceRecognitionForm_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "Status: IDLE";
            RefreshDatasetList();
            TryLoadAndTrain();
        }

        // ──────────────── Scanning ────────────────

        private void btnStartScanning_Click(object sender, EventArgs e)
        {
            if (!_isScanning)
            {
                try
                {
                    _capture = new VideoCapture(0);
                    if (!_capture.IsOpened)
                    {
                        MessageBox.Show("Unable to access camera.");
                        return;
                    }
                    Application.Idle += ProcessFrame;
                    _isScanning = true;
                    lblStatus.Text = "Status: SCANNING...";
                    btnStartScanning.Text = "Stop Scanning";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to start camera.\n\n" + ex.Message,
                        "Camera Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                StopCamera();
            }
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            if (_capture == null) return;

            Mat frame = new Mat();
            _capture.Read(frame);
            if (frame == null || frame.IsEmpty) return;

            Image<Bgr, byte> currentFrame = frame.ToImage<Bgr, byte>();
            Image<Gray, byte> grayFrame = currentFrame.Convert<Gray, byte>();

            // Improved parameters: smaller scaleFactor=1.05 for higher accuracy,
            // minNeighbors=6 to cut false positives, minSize to ignore tiny detections
            Rectangle[] faces = _faceClassifier.DetectMultiScale(
                grayFrame, 1.05, 6, new Size(60, 60));

            string statusText = faces.Length == 0 ? "Status: NO FACE DETECTED" : null;

            for (int i = 0; i < faces.Length; i++)
            {
                Rectangle face = faces[i];
                Color boxColor = FaceColors[i % FaceColors.Length];
                string name = "Unknown";

                if (_isTrained)
                {
                    try
                    {
                        Image<Gray, byte> faceImg = grayFrame.Copy(face)
                            .Resize(FaceSize, FaceSize, Inter.Linear);

                        // Equalize histogram for better recognition under varying light
                        CvInvoke.EqualizeHist(faceImg, faceImg);

                        var result = _recognizer.Predict(faceImg.Mat);
                        if (result.Distance < RecognitionThreshold && _labelNames.ContainsKey(result.Label))
                            name = _labelNames[result.Label];
                    }
                    catch { }
                }

                currentFrame.Draw(face, new Bgr(boxColor), 3);

                // Draw a filled label background rectangle above the face box
                int labelY = Math.Max(0, face.Y - 24);
                CvInvoke.Rectangle(
                    currentFrame,
                    new Rectangle(face.X, labelY, face.Width, 22),
                    new MCvScalar(boxColor.B, boxColor.G, boxColor.R),
                    -1);

                CvInvoke.PutText(
                    currentFrame,
                    name,
                    new Point(face.X + 4, Math.Max(16, face.Y - 6)),
                    FontFace.HersheySimplex,
                    0.6,
                    new MCvScalar(255, 255, 255),
                    2);

                if (statusText == null)
                    statusText = "Status: " + name;
                else if (!statusText.Contains(name))
                    statusText += " | " + name;
            }

            Bitmap bmp = currentFrame.ToBitmap();
            string finalStatus = statusText ?? "Status: NO FACE DETECTED";

            if (InvokeRequired)
                Invoke(new Action(() => UpdateUI(bmp, finalStatus)));
            else
                UpdateUI(bmp, finalStatus);
        }

        private void UpdateUI(Bitmap bmp, string status)
        {
            lblStatus.Text = status;
            if (picCameraView.Image != null)
                picCameraView.Image.Dispose();
            picCameraView.Image = bmp;
        }

        private void StopCamera()
        {
            Application.Idle -= ProcessFrame;
            if (_capture != null) { _capture.Dispose(); _capture = null; }
            if (picCameraView.Image != null) { picCameraView.Image.Dispose(); picCameraView.Image = null; }
            _isScanning = false;
            lblStatus.Text = "Status: IDLE";
            btnStartScanning.Text = "Start Scanning";
        }

        // ──────────────── Dataset ────────────────

        private void btnCaptureFace_Click(object sender, EventArgs e)
        {
            string name = txtPersonName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a person name first.", "Name Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_isScanning || picCameraView.Image == null)
            {
                MessageBox.Show("Start scanning first so the camera is active.", "Camera Not Active",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Bitmap snapshot = new Bitmap(picCameraView.Image);

            // Save a timestamped full snapshot to captures folder
            string captureFile = Path.Combine(CapturesRoot,
                $"{SanitizeName(name)}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            snapshot.Save(captureFile);

            Image<Bgr, byte> img = new Image<Bgr, byte>(snapshot);
            Image<Gray, byte> gray = img.Convert<Gray, byte>();

            Rectangle[] faces = _faceClassifier.DetectMultiScale(gray, 1.05, 6, new Size(60, 60));
            if (faces.Length == 0)
            {
                MessageBox.Show("No face detected in the current frame. Try again.", "No Face",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFaceImages(gray, faces, name);
            RefreshDatasetList();
            MessageBox.Show($"Captured {faces.Length} face(s) for \"{name}\" and saved snapshot to captures folder.", "Captured",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            string name = txtPersonName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a person name first.", "Name Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Select face image(s)";
                dlg.Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp";
                dlg.Multiselect = true;
                if (dlg.ShowDialog() != DialogResult.OK) return;

                int saved = 0;
                foreach (string file in dlg.FileNames)
                {
                    try
                    {
                        // Copy original upload to captures folder
                        string destCapture = Path.Combine(CapturesRoot,
                            $"{SanitizeName(name)}_upload_{DateTime.Now:yyyyMMdd_HHmmss}_{Path.GetFileName(file)}");
                        File.Copy(file, destCapture, true);

                        Image<Bgr, byte> img = new Image<Bgr, byte>(file);
                        Image<Gray, byte> gray = img.Convert<Gray, byte>();
                        Rectangle[] faces = _faceClassifier.DetectMultiScale(gray, 1.05, 6, new Size(60, 60));

                        if (faces.Length > 0)
                        {
                            SaveFaceImages(gray, faces, name);
                            saved += faces.Length;
                        }
                        else
                        {
                            Image<Gray, byte> resized = gray.Resize(FaceSize, FaceSize, Inter.Linear);
                            string personDir = GetPersonDir(name);
                            int idx = Directory.GetFiles(personDir, "*.png").Length;
                            resized.Save(Path.Combine(personDir, $"{idx}.png"));
                            saved++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Could not process {Path.GetFileName(file)}: {ex.Message}");
                    }
                }

                RefreshDatasetList();
                MessageBox.Show($"Saved {saved} face image(s) for \"{name}\".\nOriginals copied to captures folder.", "Uploaded",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnTrainRecognizer_Click(object sender, EventArgs e)
        {
            if (TryLoadAndTrain())
                MessageBox.Show("Recognizer trained successfully!", "Trained",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClearDataset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete all enrolled faces?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            if (Directory.Exists(DatasetRoot))
            {
                Directory.Delete(DatasetRoot, true);
                Directory.CreateDirectory(DatasetRoot);
            }

            _isTrained = false;
            _labelNames.Clear();
            RefreshDatasetList();
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            // Ensure captures folder exists then open it
            Directory.CreateDirectory(CapturesRoot);
            System.Diagnostics.Process.Start("explorer.exe", CapturesRoot);
        }

        // ──────────────── Helpers ────────────────

        private string GetPersonDir(string name)
        {
            string dir = Path.Combine(DatasetRoot, SanitizeName(name));
            Directory.CreateDirectory(dir);
            return dir;
        }

        private void SaveFaceImages(Image<Gray, byte> gray, Rectangle[] faces, string name)
        {
            string personDir = GetPersonDir(name);
            int existingCount = Directory.GetFiles(personDir, "*.png").Length;
            for (int i = 0; i < faces.Length; i++)
            {
                Image<Gray, byte> faceImg = gray.Copy(faces[i])
                    .Resize(FaceSize, FaceSize, Inter.Linear);
                // Apply histogram equalization for better training data
                CvInvoke.EqualizeHist(faceImg, faceImg);
                faceImg.Save(Path.Combine(personDir, $"{existingCount + i}.png"));
            }
        }

        private bool TryLoadAndTrain()
        {
            if (!Directory.Exists(DatasetRoot)) return false;

            var images = new List<Mat>();
            var labels = new List<int>();
            _labelNames.Clear();

            int labelId = 0;
            foreach (string personDir in Directory.GetDirectories(DatasetRoot))
            {
                string personName = Path.GetFileName(personDir);
                string[] files = Directory.GetFiles(personDir, "*.png");
                if (files.Length == 0) continue;

                _labelNames[labelId] = personName;
                foreach (string file in files)
                {
                    try
                    {
                        Image<Gray, byte> img = new Image<Gray, byte>(file);
                        images.Add(img.Mat);
                        labels.Add(labelId);
                    }
                    catch { }
                }
                labelId++;
            }

            if (images.Count == 0 || labelId == 0)
            {
                _isTrained = false;
                return false;
            }

            try
            {
                _recognizer = new LBPHFaceRecognizer(1, 8, 8, 8, RecognitionThreshold);
                _recognizer.Train(images.ToArray(), labels.ToArray());
                _isTrained = true;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Training failed: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _isTrained = false;
                return false;
            }
        }

        private void RefreshDatasetList()
        {
            listFaces.Items.Clear();
            if (!Directory.Exists(DatasetRoot)) return;

            foreach (string dir in Directory.GetDirectories(DatasetRoot))
            {
                string name = Path.GetFileName(dir);
                int count = Directory.GetFiles(dir, "*.png").Length;
                if (count == 0) continue;
                var item = new ListViewItem(name);
                item.SubItems.Add(count.ToString());
                listFaces.Items.Add(item);
            }

            lblEnrolled.Text = $"Enrolled: ({listFaces.Items.Count} people)";
        }

        private static string SanitizeName(string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopCamera();
            base.OnFormClosing(e);
        }

        private void picCameraView_Click(object sender, EventArgs e) { }
    }
}
