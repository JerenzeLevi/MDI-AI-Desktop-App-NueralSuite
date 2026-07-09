using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MDI
{
    public partial class Login : Form
    {
        // ── Palette ──────────────────────────────────────────────────────────────
        static readonly Color Accent    = Color.FromArgb(255, 45, 120);
        static readonly Color PanelLeft = Color.FromArgb(14, 12, 30);
        static readonly Color TxtMain   = Color.FromArgb(240, 238, 255);
        static readonly Color TxtSub    = Color.FromArgb(130, 125, 170);

        Point _dragStart;

        public Login()
        {
            InitializeComponent();

            // Left panel custom GDI+ paint
            pnlLeft.Paint += PaintLeftPanel;

            // Drag support
            pnlLeft.MouseDown  += DragStart; pnlLeft.MouseMove  += DragMove;
            pnlRight.MouseDown += DragStart; pnlRight.MouseMove += DragMove;

            // Chrome
            btnClose.Click    += (s, e) => Application.Exit();
            btnMinimize.Click += (s, e) => WindowState = FormWindowState.Minimized;

            // Password eye toggles — deferred to Load so layout is finalised
            this.Load += (s, e) =>
            {
                AttachEyeToggle(txtPassword, pnlLoginView);
                AttachEyeToggle(txtSuPass,   pnlSignUpView);
                AttachEyeToggle(txtSuConf,   pnlSignUpView);
            };

            // Login view
            btnSignIn.Click    += (s, e) => { DialogResult = DialogResult.OK; Close(); };
            btnFaceLogin.Click += (s, e) => ShowView("face");
            lblGoSignUp.Click  += (s, e) => ShowView("signup");

            // Sign up view
            btnCreate.Click  += (s, e) => MessageBox.Show("Registration functionality coming soon!", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            lblGoLogin.Click += (s, e) => ShowView("login");

            // Face view
            btnFaceCapture.Click += (s, e) => MessageBox.Show("Face auth functionality coming soon!", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnFaceCancel.Click  += (s, e) => ShowView("login");

            // Camera preview + scan corners painted on picFace
            picFace.Paint += PaintFaceBox;

            ShowView("login");
        }

        // ── View switching ───────────────────────────────────────────────────────

        void ShowView(string view)
        {
            pnlLoginView.Visible  = view == "login";
            pnlSignUpView.Visible = view == "signup";
            pnlFaceView.Visible   = view == "face";

            if (view == "login")   pnlLoginView.BringToFront();
            if (view == "signup")  pnlSignUpView.BringToFront();
            if (view == "face")    pnlFaceView.BringToFront();
        }

        // ── Left panel paint ─────────────────────────────────────────────────────

        void PaintLeftPanel(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var grad = new LinearGradientBrush(
                pnlLeft.ClientRectangle,
                Color.FromArgb(14, 12, 30),
                Color.FromArgb(25, 18, 50),
                LinearGradientMode.ForwardDiagonal))
                g.FillRectangle(grad, pnlLeft.ClientRectangle);

            // Glowing circle behind logo
            for (int r = 120; r > 0; r -= 8)
            {
                int alpha = (int)(18 * (1f - r / 120f));
                using (var br = new SolidBrush(Color.FromArgb(alpha, Accent)))
                    g.FillEllipse(br, 180 - r, 150 - r, r * 2, r * 2);
            }

            // Logo circle
            using (var br = new SolidBrush(Color.FromArgb(40, Accent)))
                g.FillEllipse(br, 120, 90, 120, 120);
            using (var pen = new Pen(Color.FromArgb(160, Accent), 2))
                g.DrawEllipse(pen, 122, 92, 116, 116);

            // Logo text
            using (var f = new Font("Segoe UI", 22, FontStyle.Bold))
            using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                g.DrawString("MDI", f, new SolidBrush(TxtMain), new Rectangle(120, 90, 120, 120), sf);

            // App name
            using (var f = new Font("Segoe UI", 16, FontStyle.Bold))
            using (var sf = new StringFormat { Alignment = StringAlignment.Center })
                g.DrawString("Neural Suite", f, new SolidBrush(TxtMain), new Rectangle(30, 232, 300, 30), sf);

            // Tagline
            using (var f = new Font("Segoe UI", 9))
            using (var sf = new StringFormat { Alignment = StringAlignment.Center })
                g.DrawString("AI · Face Recognition · Object Detection", f,
                    new SolidBrush(TxtSub), new Rectangle(30, 264, 300, 22), sf);

            // Feature pills
            string[] features = { "Gemini AI Chatbot", "Face Enrollment", "YOLO Detection" };
            for (int i = 0; i < features.Length; i++)
            {
                int py = 340 + i * 50;
                using (var br = new SolidBrush(Color.FromArgb(35, Accent)))
                {
                    var gp = RoundRect(new Rectangle(60, py, 240, 34), 17);
                    g.FillPath(br, gp);
                }
                using (var pen = new Pen(Color.FromArgb(70, Accent), 1))
                {
                    var gp = RoundRect(new Rectangle(60, py, 240, 34), 17);
                    g.DrawPath(pen, gp);
                }
                using (var f = new Font("Segoe UI", 9))
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    g.DrawString(features[i], f, new SolidBrush(TxtMain), new Rectangle(60, py, 240, 34), sf);
            }

            // Bottom accent line
            using (var grad2 = new LinearGradientBrush(
                new Point(0, 556), new Point(360, 556),
                Color.Transparent, Color.Transparent))
            {
                grad2.InterpolationColors = new ColorBlend
                {
                    Colors    = new[] { Color.Transparent, Accent, Color.Transparent },
                    Positions = new[] { 0f, 0.5f, 1f }
                };
                g.FillRectangle(grad2, 0, 556, 360, 4);
            }
        }

        // ── Face box paint (border + scan corners + placeholder text) ────────────

        void PaintFaceBox(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var pen = new Pen(Color.FromArgb(80, Accent), 2))
            {
                var gp = RoundRect(new Rectangle(1, 1, picFace.Width - 2, picFace.Height - 2), 12);
                g.DrawPath(pen, gp);
            }

            if (picFace.Image == null)
            {
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (var f = new Font("Segoe UI", 10))
                    g.DrawString("Camera preview will appear here\nwhen functionality is connected",
                        f, new SolidBrush(TxtSub), picFace.ClientRectangle, sf);
            }

            DrawScanCorners(g, picFace.Size);
        }

        // ── Helpers ──────────────────────────────────────────────────────────────

        void DrawScanCorners(Graphics g, Size sz)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int len = 20;
            using (var pen = new Pen(Accent, 2.5f))
            {
                g.DrawLine(pen, 0, len, 0, 0); g.DrawLine(pen, 0, 0, len, 0);
                g.DrawLine(pen, sz.Width - len, 0, sz.Width, 0); g.DrawLine(pen, sz.Width, 0, sz.Width, len);
                g.DrawLine(pen, 0, sz.Height - len, 0, sz.Height); g.DrawLine(pen, 0, sz.Height, len, sz.Height);
                g.DrawLine(pen, sz.Width - len, sz.Height, sz.Width, sz.Height); g.DrawLine(pen, sz.Width, sz.Height - len, sz.Width, sz.Height);
            }
        }

        static GraphicsPath RoundRect(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(r.Right - radius * 2, r.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(r.Right - radius * 2, r.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(r.X, r.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            return path;
        }

        // ── Eye toggle ───────────────────────────────────────────────────────────

        void AttachEyeToggle(Guna.UI2.WinForms.Guna2TextBox txtBox, System.Windows.Forms.Panel containingPanel)
        {
            const int BtnW = 46;
            const int BtnH = 36;

            // Shrink the textbox to make room for the button
            txtBox.Width -= BtnW + 6;

            // Convert the textbox position from panel-space → form-space so the
            // button lives on the form itself, above all Guna panel Z-ordering.
            Point screenPt = containingPanel.PointToScreen(
                new Point(txtBox.Right + 6, txtBox.Top + (txtBox.Height - BtnH) / 2));
            Point formPt = this.PointToClient(screenPt);

            var eye = new Button
            {
                Text      = "●",
                Font      = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(180, 170, 220),
                BackColor = Color.FromArgb(45, 42, 78),
                FlatStyle = FlatStyle.Flat,
                Cursor    = Cursors.Hand,
                Size      = new Size(BtnW, BtnH),
                Location  = formPt,
                TabStop   = false
            };
            eye.FlatAppearance.BorderSize  = 1;
            eye.FlatAppearance.BorderColor = Color.FromArgb(90, 80, 140);

            bool visible = false;
            eye.Click += (s, e) =>
            {
                visible = !visible;
                txtBox.UseSystemPasswordChar = !visible;
                eye.Text      = visible ? "○" : "●";
                eye.ForeColor = visible
                    ? Color.FromArgb(255, 45, 120)
                    : Color.FromArgb(180, 170, 220);
            };

            // Add directly to the FORM — bypasses all panel Z-ordering
            this.Controls.Add(eye);
            eye.BringToFront();
        }

        // ── Drag ─────────────────────────────────────────────────────────────────

        void DragStart(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) _dragStart = e.Location; }
        void DragMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                Location = new Point(Location.X + e.X - _dragStart.X, Location.Y + e.Y - _dragStart.Y);
        }

        private void pnlFaceView_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
