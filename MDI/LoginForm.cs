using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace MDI
{
    public partial class LoginForm : Form
    {
        // ── Palette ──────────────────────────────────────────────────────────────
        static readonly Color BgDark    = Color.FromArgb(10,  9,  20);
        static readonly Color PanelLeft = Color.FromArgb(14, 12, 30);
        static readonly Color CardRight = Color.FromArgb(18, 16, 36);
        static readonly Color Accent    = Color.FromArgb(255, 45, 120);
        static readonly Color AccentHov = Color.FromArgb(220, 30,  95);
        static readonly Color InputBg   = Color.FromArgb(28, 26, 50);
        static readonly Color InputBord = Color.FromArgb(65, 60, 100);
        static readonly Color TxtMain   = Color.FromArgb(240, 238, 255);
        static readonly Color TxtSub    = Color.FromArgb(130, 125, 170);

        // ── Firebase ─────────────────────────────────────────────────────────────
        private const string FirebaseApiKey = "AIzaSyA2Yg7UqfUb2Ahv8XuisGSfh-aaGHunYBE";
        private static readonly HttpClient Http = new HttpClient();

        // ── Face login storage ───────────────────────────────────────────────────
        private string FaceLoginDir  => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "facelogin");
        private string FaceModelPath => Path.Combine(FaceLoginDir, "model.yml");
        private string FaceMapPath   => Path.Combine(FaceLoginDir, "map.json");
        private string SessionsPath  => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sessions.json");
        // LBPH: lower distance = better match; 80 is a reliable threshold
        private const double FaceThreshold = 80.0;

        // ── Face camera state ────────────────────────────────────────────────────
        private VideoCapture                 _faceCapture;
        private System.Windows.Forms.Timer   _faceTimer;
        private CascadeClassifier            _cascade;

        // ── Logged-in user (read by Form1 after DialogResult.OK) ─────────────────
        public string LoggedInEmail { get; private set; }
        public string LoggedInName  { get; private set; }

        // ── Drag state ───────────────────────────────────────────────────────────
        Point _dragStart;

        // ── Panels ───────────────────────────────────────────────────────────────
        Panel pnlLeft, pnlRight;
        Panel pnlLoginView, pnlSignUpView, pnlFaceView;

        // ── Login controls ───────────────────────────────────────────────────────
        Label   lblWelcome, lblSub;
        Panel   pnlEmailWrap, pnlPassWrap;
        TextBox txtEmail, txtPassword;
        CheckBox chkRemember;
        Label   lblForgot, lblNoAccount, lblGoSignUp;
        Label   lblOrLeft, lblOrRight, lblOr;
        Button  btnSignIn, btnFaceLogin;
        Label   lblLoginError;

        // ── Sign-up controls ─────────────────────────────────────────────────────
        Label   lblCreateTitle, lblCreateSub;
        Panel   pnlSuNameWrap, pnlSuEmailWrap, pnlSuPassWrap, pnlSuConfWrap;
        TextBox txtSuName, txtSuEmail, txtSuPass, txtSuConf;
        Button  btnCreate;
        Label   lblHaveAccount, lblGoLogin;
        Label   lblSignUpError;

        // ── Face-view controls ───────────────────────────────────────────────────
        Label      lblFaceTitle, lblFaceHint, lblFaceStatus;
        PictureBox picFace;
        Button     btnFaceCapture, btnFaceCancel;

        // ── Chrome ───────────────────────────────────────────────────────────────
        Button btnClose, btnMinimize;

        // ────────────────────────────────────────────────────────────────────────
        public LoginForm()
        {
            InitializeComponent();
            BackColor = BgDark;
            BuildUI();

            pnlLeft.MouseDown  += DragStart; pnlLeft.MouseMove  += DragMove;
            pnlRight.MouseDown += DragStart; pnlRight.MouseMove += DragMove;

            string cascadePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                              "haarcascade_frontalface_default.xml");
            if (File.Exists(cascadePath))
                _cascade = new CascadeClassifier(cascadePath);

            FormClosed += (s, e) => StopFaceCamera();
        }

        // ── Build ────────────────────────────────────────────────────────────────

        void BuildUI()
        {
            btnClose    = Chrome("✕", 862, 12, Color.FromArgb(220, 60, 80));
            btnMinimize = Chrome("─", 832, 12, Color.FromArgb(70, 65, 100));
            Controls.Add(btnClose);
            Controls.Add(btnMinimize);
            btnClose.Click    += (s, e) => Application.Exit();
            btnMinimize.Click += (s, e) => WindowState = FormWindowState.Minimized;

            pnlLeft = new Panel
            {
                Location  = new Point(0, 0),
                Size      = new Size(360, 560),
                BackColor = PanelLeft,
            };
            pnlLeft.Paint += PaintLeftPanel;
            Controls.Add(pnlLeft);

            pnlRight = new Panel
            {
                Location  = new Point(360, 0),
                Size      = new Size(540, 560),
                BackColor = CardRight,
            };
            Controls.Add(pnlRight);

            BuildLoginView();
            BuildSignUpView();
            BuildFaceView();

            ShowView("login");
        }

        void BuildLoginView()
        {
            pnlLoginView = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            pnlRight.Controls.Add(pnlLoginView);

            int cx = 270;

            lblWelcome = Lbl("Welcome Back", new Font("Segoe UI", 22, FontStyle.Bold), TxtMain, cx, 70, true);
            lblSub     = Lbl("Sign in to continue your session", new Font("Segoe UI", 10), TxtSub, cx, 102, true);

            var lblE = Lbl("EMAIL / USERNAME", new Font("Segoe UI", 8, FontStyle.Bold), TxtSub, 60, 148, false);
            pnlEmailWrap = InputWrap(60, 168, 420, out txtEmail, "you@example.com", false);

            var lblP = Lbl("PASSWORD", new Font("Segoe UI", 8, FontStyle.Bold), TxtSub, 60, 218, false);
            pnlPassWrap = InputWrap(60, 238, 420, out txtPassword, "••••••••", true);

            chkRemember = new CheckBox
            {
                Text      = "Remember me",
                Font      = new Font("Segoe UI", 9),
                ForeColor = TxtSub,
                BackColor = Color.Transparent,
                Location  = new Point(60, 292),
                AutoSize  = true,
                Checked   = true,
            };
            chkRemember.FlatAppearance.CheckedBackColor = Accent;
            chkRemember.FlatAppearance.BorderColor      = InputBord;
            chkRemember.FlatStyle = FlatStyle.Flat;

            lblForgot = new Label
            {
                Text      = "Forgot password?",
                Font      = new Font("Segoe UI", 9, FontStyle.Underline),
                ForeColor = Accent,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Cursor    = Cursors.Hand,
                Location  = new Point(338, 294),
            };

            lblLoginError = ErrorLbl(60, 318, 420);

            btnSignIn = RoundBtn("Sign In", 60, 342, 420, 44, Accent, AccentHov, TxtMain);
            btnSignIn.Click += async (s, e) => await HandleSignInAsync();
            txtPassword.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) btnSignIn.PerformClick();
            };

            lblOrLeft  = HRule(60,  402, 168);
            lblOrRight = HRule(248, 402, 168);
            lblOr      = Lbl("OR", new Font("Segoe UI", 8, FontStyle.Bold), TxtSub, cx, 395, true);

            btnFaceLogin = OutlineBtn("  Login with Face Recognition", 60, 420, 420, 44);
            btnFaceLogin.Click += (s, e) => ShowView("face");

            lblNoAccount = Lbl("Don't have an account?", new Font("Segoe UI", 9), TxtSub, 152, 480, false);
            lblGoSignUp  = new Label
            {
                Text      = "Sign Up",
                Font      = new Font("Segoe UI", 9, FontStyle.Bold | FontStyle.Underline),
                ForeColor = Accent,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Cursor    = Cursors.Hand,
                Location  = new Point(302, 480),
            };
            lblGoSignUp.Click += (s, e) => ShowView("signup");

            pnlLoginView.Controls.AddRange(new Control[] {
                lblWelcome, lblSub,
                lblE, pnlEmailWrap, lblP, pnlPassWrap,
                chkRemember, lblForgot, lblLoginError, btnSignIn,
                lblOrLeft, lblOr, lblOrRight,
                btnFaceLogin, lblNoAccount, lblGoSignUp
            });
        }

        void BuildSignUpView()
        {
            pnlSignUpView = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            pnlRight.Controls.Add(pnlSignUpView);

            int cx = 270;

            lblCreateTitle = Lbl("Create Account", new Font("Segoe UI", 22, FontStyle.Bold), TxtMain, cx, 52, true);
            lblCreateSub   = Lbl("Join the MDI Neural Suite", new Font("Segoe UI", 10), TxtSub, cx, 84, true);

            var lN = Lbl("FULL NAME",        new Font("Segoe UI", 8, FontStyle.Bold), TxtSub, 60, 122, false);
            pnlSuNameWrap  = InputWrap(60, 140, 420, out txtSuName,  "John Doe",        false);

            var lE = Lbl("EMAIL",            new Font("Segoe UI", 8, FontStyle.Bold), TxtSub, 60, 188, false);
            pnlSuEmailWrap = InputWrap(60, 206, 420, out txtSuEmail, "you@example.com", false);

            var lP = Lbl("PASSWORD",         new Font("Segoe UI", 8, FontStyle.Bold), TxtSub, 60, 254, false);
            pnlSuPassWrap  = InputWrap(60, 272, 420, out txtSuPass,  "••••••••",        true);

            var lC = Lbl("CONFIRM PASSWORD", new Font("Segoe UI", 8, FontStyle.Bold), TxtSub, 60, 318, false);
            pnlSuConfWrap  = InputWrap(60, 336, 420, out txtSuConf,  "••••••••",        true);

            lblSignUpError = ErrorLbl(60, 382, 420);

            btnCreate = RoundBtn("Create Account", 60, 404, 420, 44, Accent, AccentHov, TxtMain);
            btnCreate.Click += async (s, e) => await HandleSignUpAsync();

            lblHaveAccount = Lbl("Already have an account?", new Font("Segoe UI", 9), TxtSub, 140, 462, false);
            lblGoLogin = new Label
            {
                Text      = "Sign In",
                Font      = new Font("Segoe UI", 9, FontStyle.Bold | FontStyle.Underline),
                ForeColor = Accent,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Cursor    = Cursors.Hand,
                Location  = new Point(296, 462),
            };
            lblGoLogin.Click += (s, e) => ShowView("login");

            pnlSignUpView.Controls.AddRange(new Control[] {
                lblCreateTitle, lblCreateSub,
                lN, pnlSuNameWrap, lE, pnlSuEmailWrap,
                lP, pnlSuPassWrap, lC, pnlSuConfWrap,
                lblSignUpError, btnCreate, lblHaveAccount, lblGoLogin
            });
        }

        void BuildFaceView()
        {
            pnlFaceView = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            pnlFaceView.VisibleChanged += (s, e) =>
            {
                if (pnlFaceView.Visible) StartFaceCamera();
                else                     StopFaceCamera();
            };
            pnlRight.Controls.Add(pnlFaceView);

            int cx = 270;

            lblFaceTitle = Lbl("Face Recognition Login",
                new Font("Segoe UI", 18, FontStyle.Bold), TxtMain, cx, 55, true);
            lblFaceHint  = Lbl("Look at the camera — click Capture to authenticate",
                new Font("Segoe UI", 9), TxtSub, cx, 88, true);

            picFace = new PictureBox
            {
                Location    = new Point(95, 120),
                Size        = new Size(350, 262),
                BackColor   = Color.FromArgb(20, 18, 38),
                SizeMode    = PictureBoxSizeMode.StretchImage,
                BorderStyle = BorderStyle.None,
            };
            picFace.Paint += (s, e) =>
            {
                var g = e.Graphics;
                using (var pen = new Pen(Color.FromArgb(80, Accent), 2))
                    g.DrawPath(pen, RoundRect(new Rectangle(1, 1, picFace.Width - 2, picFace.Height - 2), 12));
                if (picFace.Image == null)
                    using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    using (var f = new Font("Segoe UI", 10))
                        g.DrawString("Starting camera…", f, new SolidBrush(TxtSub), picFace.ClientRectangle, sf);
            };

            // Scan-corner overlay (transparent label on top of picFace)
            var scanOverlay = new Label { Location = new Point(95, 120), Size = new Size(350, 262), BackColor = Color.Transparent };
            scanOverlay.Paint += (s, e) => DrawScanCorners(e.Graphics, scanOverlay.Size);

            lblFaceStatus = new Label
            {
                Text      = "Scanning…",
                Font      = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = TxtSub,
                BackColor = Color.Transparent,
                Location  = new Point(95, 390),
                Size      = new Size(350, 22),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            btnFaceCapture = RoundBtn("Capture & Authenticate", 95, 415, 350, 40, Accent, AccentHov, TxtMain);
            btnFaceCapture.Click  += async (s, e) => await AuthenticateWithFaceAsync();

            var btnEnrollFace = OutlineBtn("＋ Register My Face", 95, 462, 350, 36);
            btnEnrollFace.Click  += (s, e) =>
            {
                string email = ShowInputDialog("Enter your account email to register your face:", "Register Face");
                if (string.IsNullOrWhiteSpace(email)) return;
                EnrollFace(email.Trim());
            };

            btnFaceCancel = OutlineBtn("← Back to Login", 95, 505, 350, 36);
            btnFaceCancel.Click += (s, e) => ShowView("login");

            pnlFaceView.Controls.AddRange(new Control[] {
                lblFaceTitle, lblFaceHint, picFace, scanOverlay,
                lblFaceStatus, btnFaceCapture, btnEnrollFace, btnFaceCancel
            });
        }

        // ── View switching ───────────────────────────────────────────────────────

        void ShowView(string view)
        {
            pnlLoginView.Visible  = view == "login";
            pnlSignUpView.Visible = view == "signup";
            pnlFaceView.Visible   = view == "face";

            if (view == "login")  pnlLoginView.BringToFront();
            if (view == "signup") pnlSignUpView.BringToFront();
            if (view == "face")   pnlFaceView.BringToFront();
        }

        // ── Sign-In flow ─────────────────────────────────────────────────────────

        private async Task HandleSignInAsync()
        {
            string email    = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            lblLoginError.Text = "";

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                lblLoginError.Text = "Please enter your email and password.";
                return;
            }

            btnSignIn.Enabled = false;
            btnSignIn.Text    = "Signing in…";

            bool ok = await SignInAsync(email, password);

            btnSignIn.Enabled = true;
            btnSignIn.Text    = "Sign In";

            if (ok) { DialogResult = DialogResult.OK; Close(); }
        }

        // ── Sign-Up flow ─────────────────────────────────────────────────────────

        private async Task HandleSignUpAsync()
        {
            string name = txtSuName.Text.Trim();
            string email = txtSuEmail.Text.Trim();
            string pass  = txtSuPass.Text;
            string conf  = txtSuConf.Text;
            lblSignUpError.Text = "";

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass))
            {
                lblSignUpError.Text = "All fields are required.";
                return;
            }
            if (pass != conf)
            {
                lblSignUpError.Text = "Passwords do not match.";
                return;
            }
            if (pass.Length < 6)
            {
                lblSignUpError.Text = "Password must be at least 6 characters.";
                return;
            }

            btnCreate.Enabled = false;
            btnCreate.Text    = "Creating account…";

            bool ok = await SignUpAsync(email, pass, name);

            btnCreate.Enabled = true;
            btnCreate.Text    = "Create Account";

            if (!ok) return;

            // Optionally enroll face right after signup
            if (_cascade != null)
            {
                var res = MessageBox.Show(
                    $"Account created! Would you like to enroll your face for future logins?\n\n" +
                    $"(You can skip this — face login requires at least one email login with 'Remember me' first.)",
                    "Face Enrollment", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                    EnrollFace(email);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        // ── Firebase Auth: Sign In ────────────────────────────────────────────────

        private async Task<bool> SignInAsync(string email, string password)
        {
            try
            {
                string url  = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={FirebaseApiKey}";
                string body = JsonSerializer.Serialize(new { email, password, returnSecureToken = true });
                var resp    = await Http.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
                string json = await resp.Content.ReadAsStringAsync();

                if (!resp.IsSuccessStatusCode)
                {
                    using (var doc = JsonDocument.Parse(json))
                    {
                        string msg = doc.RootElement.GetProperty("error").GetProperty("message").GetString();
                        lblLoginError.Text = FriendlyError(msg);
                    }
                    return false;
                }

                using (var doc = JsonDocument.Parse(json))
                {
                    LoggedInEmail = doc.RootElement.GetProperty("email").GetString();
                    LoggedInName  = doc.RootElement.TryGetProperty("displayName", out var dn)
                                    && dn.ValueKind == JsonValueKind.String
                        ? dn.GetString()
                        : email.Split('@')[0];

                    if (chkRemember.Checked)
                    {
                        string rt = doc.RootElement.GetProperty("refreshToken").GetString();
                        SaveSession(email, LoggedInName, rt);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                lblLoginError.Text = "Connection error: " + ex.Message;
                return false;
            }
        }

        // ── Firebase Auth: Sign Up ────────────────────────────────────────────────

        private async Task<bool> SignUpAsync(string email, string password, string displayName)
        {
            try
            {
                // Create account
                string url  = $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={FirebaseApiKey}";
                string body = JsonSerializer.Serialize(new { email, password, returnSecureToken = true });
                var resp    = await Http.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
                string json = await resp.Content.ReadAsStringAsync();

                if (!resp.IsSuccessStatusCode)
                {
                    using (var doc = JsonDocument.Parse(json))
                    {
                        string msg = doc.RootElement.GetProperty("error").GetProperty("message").GetString();
                        lblSignUpError.Text = FriendlyError(msg);
                    }
                    return false;
                }

                string idToken, refreshToken;
                using (var doc = JsonDocument.Parse(json))
                {
                    idToken      = doc.RootElement.GetProperty("idToken").GetString();
                    refreshToken = doc.RootElement.GetProperty("refreshToken").GetString();
                }

                // Set display name
                string updUrl  = $"https://identitytoolkit.googleapis.com/v1/accounts:update?key={FirebaseApiKey}";
                string updBody = JsonSerializer.Serialize(new { idToken, displayName, returnSecureToken = false });
                await Http.PostAsync(updUrl, new StringContent(updBody, Encoding.UTF8, "application/json"));

                LoggedInEmail = email;
                LoggedInName  = displayName;
                SaveSession(email, displayName, refreshToken);
                return true;
            }
            catch (Exception ex)
            {
                lblSignUpError.Text = "Connection error: " + ex.Message;
                return false;
            }
        }

        // ── Firebase Auth: exchange refresh token (face login) ───────────────────

        private async Task<bool> SignInWithRefreshTokenAsync(string email, string displayName, string refreshToken)
        {
            try
            {
                string url  = $"https://securetoken.googleapis.com/v1/token?key={FirebaseApiKey}";
                string body = $"grant_type=refresh_token&refresh_token={Uri.EscapeDataString(refreshToken)}";
                var resp    = await Http.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded"));
                if (!resp.IsSuccessStatusCode) return false;

                string json = await resp.Content.ReadAsStringAsync();
                using (var doc = JsonDocument.Parse(json))
                {
                    string newRt = doc.RootElement.GetProperty("refresh_token").GetString();
                    SaveSession(email, displayName, newRt); // rotate stored token
                }
                LoggedInEmail = email;
                LoggedInName  = displayName;
                return true;
            }
            catch { return false; }
        }

        // ── Face camera ──────────────────────────────────────────────────────────

        private void StartFaceCamera()
        {
            if (_faceCapture != null || _cascade == null) return;
            try
            {
                _faceCapture = new VideoCapture(0);
                if (!_faceCapture.IsOpened)
                {
                    _faceCapture.Dispose();
                    _faceCapture = null;
                    lblFaceStatus.Text = "Could not open camera.";
                    return;
                }
                _faceTimer = new System.Windows.Forms.Timer { Interval = 100 };
                _faceTimer.Tick += FaceCameraTick;
                _faceTimer.Start();
                lblFaceStatus.Text = "Scanning… position your face in the frame.";
            }
            catch (Exception ex)
            {
                lblFaceStatus.Text = "Camera error: " + ex.Message;
            }
        }

        private void StopFaceCamera()
        {
            _faceTimer?.Stop();
            _faceTimer?.Dispose();
            _faceTimer = null;
            _faceCapture?.Dispose();
            _faceCapture = null;
            if (picFace?.Image != null) { picFace.Image.Dispose(); picFace.Image = null; }
        }

        private void FaceCameraTick(object sender, EventArgs e)
        {
            if (_faceCapture == null) return;

            var mat = new Mat();
            _faceCapture.Read(mat);
            if (mat.IsEmpty) { mat.Dispose(); return; }

            using (var colorImg = mat.ToImage<Bgr, byte>())
            using (var grayImg  = colorImg.Convert<Gray, byte>())
            {
                grayImg._EqualizeHist();
                var faces = _cascade.DetectMultiScale(grayImg, 1.1, 5, new Size(80, 80));

                foreach (var face in faces)
                    colorImg.Draw(face, new Bgr(255, 45, 120), 2);

                var display = colorImg.ToBitmap();
                mat.Dispose();

                if (picFace.Image != null) picFace.Image.Dispose();
                picFace.Image = display;

                if (faces.Length > 0)
                    lblFaceStatus.Text = "Face detected — click Capture & Authenticate.";
                else
                    lblFaceStatus.Text = "No face detected — look at the camera.";
            }
        }

        // ── Face authentication (on button click) ────────────────────────────────

        private async Task AuthenticateWithFaceAsync()
        {
            if (!File.Exists(FaceModelPath))
            {
                lblFaceStatus.Text = "No faces enrolled yet. Sign up first, then enroll your face.";
                return;
            }

            // Capture current frame
            if (_faceCapture == null) { lblFaceStatus.Text = "Camera not available."; return; }
            var snap = new Mat();
            _faceCapture.Read(snap);
            if (snap.IsEmpty) { snap.Dispose(); lblFaceStatus.Text = "Empty frame — try again."; return; }

            btnFaceCapture.Enabled = false;
            lblFaceStatus.Text     = "Recognizing…";

            string recognizedEmail = null;
            await Task.Run(() =>
            {
                try
                {
                    using (var colorImg = snap.ToImage<Bgr, byte>())
                    using (var grayImg  = colorImg.Convert<Gray, byte>())
                    {
                        grayImg._EqualizeHist();
                        var faces = _cascade.DetectMultiScale(grayImg, 1.1, 5, new Size(80, 80));
                        if (faces.Length == 0) return;

                        using (var faceRoi = grayImg.GetSubRect(faces[0]).Resize(100, 100, Inter.Linear))
                        {
                            var recognizer = new LBPHFaceRecognizer();
                            recognizer.Read(FaceModelPath);
                            var result = recognizer.Predict(faceRoi);

                            if (result.Distance < FaceThreshold)
                            {
                                var map = LoadFaceMap();
                                map.TryGetValue(result.Label.ToString(), out recognizedEmail);
                            }
                        }
                    }
                }
                catch { }
                finally { snap.Dispose(); }
            });

            btnFaceCapture.Enabled = true;

            if (recognizedEmail == null)
            {
                lblFaceStatus.Text = "Face not recognized. Try again or use email login.";
                return;
            }

            var sessions = LoadSessions();
            if (!sessions.TryGetValue(recognizedEmail, out var session))
            {
                lblFaceStatus.Text = "Recognized! Sign in with email + 'Remember me' once to enable face login.";
                return;
            }

            lblFaceStatus.Text = $"Recognized as {session.DisplayName}! Signing in…";
            bool ok = await SignInWithRefreshTokenAsync(recognizedEmail, session.DisplayName, session.RefreshToken);

            if (ok) { DialogResult = DialogResult.OK; Close(); }
            else     lblFaceStatus.Text = "Session expired — please sign in with email/password once.";
        }

        // ── Face enrollment ──────────────────────────────────────────────────────

        private void EnrollFace(string email)
        {
            if (_cascade == null) return;
            Directory.CreateDirectory(FaceLoginDir);

            int labelId = GetOrCreateLabelId(email);

            var dlg = new Form
            {
                Text            = "Face Enrollment — look at the camera",
                Size            = new Size(430, 370),
                StartPosition   = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox     = false,
                MinimizeBox     = false,
                BackColor       = Color.FromArgb(18, 16, 36),
            };

            var pic = new PictureBox
            {
                Bounds   = new Rectangle(10, 10, 400, 290),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Black,
            };
            var lbl = new Label
            {
                Text      = "Preparing camera…",
                Bounds    = new Rectangle(10, 308, 400, 22),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Font      = new Font("Segoe UI", 9),
            };
            dlg.Controls.Add(pic);
            dlg.Controls.Add(lbl);

            var samples  = new List<Mat>();
            var lblIds   = new List<int>();
            int captured = 0;
            const int Target = 10;
            bool done    = false;

            VideoCapture cap = null;
            System.Windows.Forms.Timer t = null;

            dlg.Shown += (s, e) =>
            {
                cap = new VideoCapture(0);
                t   = new System.Windows.Forms.Timer { Interval = 200 };
                t.Tick += (ts, te) =>
                {
                    if (done) return;
                    var mat = new Mat();
                    cap.Read(mat);
                    if (mat.IsEmpty) { mat.Dispose(); return; }

                    using (var colorImg = mat.ToImage<Bgr, byte>())
                    using (var grayImg  = colorImg.Convert<Gray, byte>())
                    {
                        grayImg._EqualizeHist();
                        var faces = _cascade.DetectMultiScale(grayImg, 1.1, 5, new Size(80, 80));
                        foreach (var f in faces) colorImg.Draw(f, new Bgr(255, 45, 120), 2);

                        pic.Image?.Dispose();
                        pic.Image = colorImg.ToBitmap();
                        mat.Dispose();

                        if (faces.Length > 0 && captured < Target)
                        {
                            using (var faceRoi = grayImg.GetSubRect(faces[0]).Resize(100, 100, Inter.Linear))
                            {
                                samples.Add(faceRoi.Mat.Clone());
                                lblIds.Add(labelId);
                            }
                            captured++;
                            lbl.Text = $"Capturing face samples… {captured}/{Target}";
                        }

                        if (captured >= Target && !done)
                        {
                            done = true;
                            t.Stop();
                            lbl.Text = "Training model — please wait…";
                            Application.DoEvents();

                            try
                            {
                                var recognizer = new LBPHFaceRecognizer();
                                var vecMat = new VectorOfMat(samples.ToArray());
                                var vecIds = new VectorOfInt(lblIds.ToArray());

                                if (File.Exists(FaceModelPath))
                                {
                                    recognizer.Read(FaceModelPath);
                                    recognizer.Update(vecMat, vecIds);
                                }
                                else
                                {
                                    recognizer.Train(vecMat, vecIds);
                                }
                                recognizer.Write(FaceModelPath);

                                var map = LoadFaceMap();
                                map[labelId.ToString()] = email;
                                File.WriteAllText(FaceMapPath, JsonSerializer.Serialize(map));
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Enrollment error: " + ex.Message, "Error");
                            }
                            finally
                            {
                                foreach (var m in samples) m.Dispose();
                                cap.Dispose();
                                cap = null;
                            }

                            pic.Image?.Dispose();
                            dlg.Close();
                            MessageBox.Show(
                                "Face enrolled successfully!\nYou can now log in with face recognition.",
                                "Enrollment Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                };
                t.Start();
            };

            dlg.FormClosed += (s, e) =>
            {
                t?.Stop(); t?.Dispose();
                cap?.Dispose();
                foreach (var m in samples) try { m.Dispose(); } catch { }
            };

            StopFaceCamera();
            dlg.ShowDialog(this);
            StartFaceCamera();
        }

        // ── Helpers: sessions + face map ─────────────────────────────────────────

        private int GetOrCreateLabelId(string email)
        {
            var map = LoadFaceMap();
            foreach (var kv in map)
                if (kv.Value == email && int.TryParse(kv.Key, out int id)) return id;
            return map.Count; // next available integer label
        }

        private Dictionary<string, string> LoadFaceMap()
        {
            if (!File.Exists(FaceMapPath)) return new Dictionary<string, string>();
            try   { return JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(FaceMapPath)); }
            catch { return new Dictionary<string, string>(); }
        }

        private void SaveSession(string email, string displayName, string refreshToken)
        {
            var sessions = LoadSessions();
            sessions[email] = new SessionEntry { DisplayName = displayName, RefreshToken = refreshToken };
            File.WriteAllText(SessionsPath, JsonSerializer.Serialize(sessions));
        }

        private Dictionary<string, SessionEntry> LoadSessions()
        {
            if (!File.Exists(SessionsPath)) return new Dictionary<string, SessionEntry>();
            try   { return JsonSerializer.Deserialize<Dictionary<string, SessionEntry>>(File.ReadAllText(SessionsPath)); }
            catch { return new Dictionary<string, SessionEntry>(); }
        }

        private class SessionEntry
        {
            public string DisplayName  { get; set; }
            public string RefreshToken { get; set; }
        }

        private string FriendlyError(string msg)
        {
            if (msg == null) return "Authentication failed.";
            if (msg.Contains("EMAIL_NOT_FOUND") || msg.Contains("INVALID_LOGIN_CREDENTIALS"))
                return "Invalid email or password.";
            if (msg.Contains("INVALID_PASSWORD"))   return "Incorrect password.";
            if (msg.Contains("EMAIL_EXISTS"))        return "An account with this email already exists.";
            if (msg.Contains("WEAK_PASSWORD"))       return "Password must be at least 6 characters.";
            if (msg.Contains("INVALID_EMAIL"))       return "Please enter a valid email address.";
            if (msg.Contains("TOO_MANY_ATTEMPTS"))   return "Too many attempts. Try again later.";
            if (msg.Contains("USER_DISABLED"))       return "This account has been disabled.";
            return msg;
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

            for (int r = 120; r > 0; r -= 8)
            {
                int alpha = (int)(18 * (1f - r / 120f));
                using (var br = new SolidBrush(Color.FromArgb(alpha, Accent)))
                    g.FillEllipse(br, 180 - r, 150 - r, r * 2, r * 2);
            }

            using (var br = new SolidBrush(Color.FromArgb(40, Accent)))
                g.FillEllipse(br, 120, 90, 120, 120);
            using (var pen = new Pen(Color.FromArgb(160, Accent), 2))
                g.DrawEllipse(pen, 122, 92, 116, 116);

            using (var f = new Font("Segoe UI", 22, FontStyle.Bold))
            using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                g.DrawString("MDI", f, new SolidBrush(TxtMain), new Rectangle(120, 90, 120, 120), sf);

            using (var f = new Font("Segoe UI", 16, FontStyle.Bold))
            using (var sf = new StringFormat { Alignment = StringAlignment.Center })
                g.DrawString("Neural Suite", f, new SolidBrush(TxtMain), new Rectangle(30, 232, 300, 30), sf);

            using (var f = new Font("Segoe UI", 9))
            using (var sf = new StringFormat { Alignment = StringAlignment.Center })
                g.DrawString("AI · Face Recognition · Object Detection", f,
                    new SolidBrush(TxtSub), new Rectangle(30, 264, 300, 22), sf);

            string[] features = { "Gemini AI Chatbot", "Face Enrollment", "YOLO Detection" };
            for (int i = 0; i < features.Length; i++)
            {
                int py = 340 + i * 50;
                using (var br = new SolidBrush(Color.FromArgb(35, Accent)))
                    g.FillPath(br, RoundRect(new Rectangle(60, py, 240, 34), 17));
                using (var pen = new Pen(Color.FromArgb(70, Accent), 1))
                    g.DrawPath(pen, RoundRect(new Rectangle(60, py, 240, 34), 17));
                using (var f = new Font("Segoe UI", 9))
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    g.DrawString(features[i], f, new SolidBrush(TxtMain), new Rectangle(60, py, 240, 34), sf);
            }

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

        // ── Control factories ────────────────────────────────────────────────────

        Label Lbl(string text, Font font, Color color, int x, int y, bool centered)
        {
            var l = new Label
            {
                Text      = text,
                Font      = font,
                ForeColor = color,
                BackColor = Color.Transparent,
                AutoSize  = !centered,
            };
            if (centered)
            {
                l.TextAlign = ContentAlignment.MiddleCenter;
                l.Size      = new Size(540, font.Height + 6);
                l.Location  = new Point(0, y);
            }
            else
            {
                l.Location = new Point(x, y);
            }
            return l;
        }

        Label ErrorLbl(int x, int y, int w) => new Label
        {
            Text      = "",
            Font      = new Font("Segoe UI", 8.5f),
            ForeColor = Color.FromArgb(255, 80, 100),
            BackColor = Color.Transparent,
            AutoSize  = false,
            Size      = new Size(w, 20),
            Location  = new Point(x, y),
            TextAlign = ContentAlignment.MiddleLeft,
        };

        Panel InputWrap(int x, int y, int w, out TextBox txt, string placeholder, bool isPassword)
        {
            var wrap = new Panel
            {
                Location  = new Point(x, y),
                Size      = new Size(w, 42),
                BackColor = InputBg,
            };
            wrap.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                bool focused = wrap.ContainsFocus;
                var pen = new Pen(focused ? Accent : InputBord, focused ? 1.5f : 1f);
                var gp = RoundRect(new Rectangle(0, 0, wrap.Width - 1, wrap.Height - 1), 8);
                g.FillPath(new SolidBrush(InputBg), gp);
                g.DrawPath(pen, gp);
                pen.Dispose();
            };

            int toggleWidth = isPassword ? 40 : 0;
            var innerTxt = new TextBox
            {
                Location              = new Point(14, 10),
                Size                  = new Size(w - 14 - toggleWidth - 6, 22),
                BackColor             = InputBg,
                ForeColor             = TxtMain,
                Font                  = new Font("Segoe UI", 10),
                BorderStyle           = BorderStyle.None,
                UseSystemPasswordChar = isPassword,
            };
            innerTxt.Enter += (s, e) => wrap.Invalidate();
            innerTxt.Leave += (s, e) => wrap.Invalidate();
            innerTxt.HandleCreated += (s, e) => SetCueBanner(innerTxt, placeholder);
            txt = innerTxt;

            wrap.Controls.Add(txt);

            if (isPassword)
            {
                var toggle = new Label
                {
                    Text      = "Show",
                    Font      = new Font("Segoe UI", 8f, FontStyle.Bold),
                    ForeColor = TxtSub,
                    BackColor = Color.Transparent,
                    Size      = new Size(toggleWidth - 4, 22),
                    Location  = new Point(w - toggleWidth + 2, 10),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Cursor    = Cursors.Hand,
                };
                toggle.Click += (s, e) =>
                {
                    bool hiding = innerTxt.UseSystemPasswordChar;
                    innerTxt.UseSystemPasswordChar = !hiding;
                    toggle.Text      = hiding ? "Hide" : "Show";
                    toggle.ForeColor = hiding ? Accent : TxtSub;
                };
                wrap.Controls.Add(toggle);
            }

            wrap.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, w, 42, 8, 8));
            return wrap;
        }

        Button RoundBtn(string text, int x, int y, int w, int h, Color fill, Color hover, Color fg)
        {
            var btn = new Button
            {
                Text      = text,
                Font      = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = fg,
                BackColor = fill,
                Location  = new Point(x, y),
                Size      = new Size(w, h),
                FlatStyle = FlatStyle.Flat,
                Cursor    = Cursors.Hand,
            };
            btn.FlatAppearance.BorderSize         = 0;
            btn.FlatAppearance.MouseOverBackColor = hover;
            btn.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, w, h, 10, 10));
            return btn;
        }

        Button OutlineBtn(string text, int x, int y, int w, int h)
        {
            var btn = new Button
            {
                Text      = text,
                Font      = new Font("Segoe UI", 9),
                ForeColor = TxtMain,
                BackColor = Color.Transparent,
                Location  = new Point(x, y),
                Size      = new Size(w, h),
                FlatStyle = FlatStyle.Flat,
                Cursor    = Cursors.Hand,
            };
            btn.FlatAppearance.BorderColor        = Color.FromArgb(90, Accent);
            btn.FlatAppearance.BorderSize         = 1;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(25, Accent);
            btn.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, w, h, 10, 10));
            return btn;
        }

        Button Chrome(string text, int x, int y, Color hover)
        {
            var btn = new Button
            {
                Text      = text,
                Font      = new Font("Segoe UI", 8),
                ForeColor = TxtSub,
                BackColor = Color.Transparent,
                Location  = new Point(x, y),
                Size      = new Size(26, 20),
                FlatStyle = FlatStyle.Flat,
                Cursor    = Cursors.Hand,
            };
            btn.FlatAppearance.BorderSize         = 0;
            btn.FlatAppearance.MouseOverBackColor = hover;
            return btn;
        }

        Label HRule(int x, int y, int w) => new Label
        {
            Location  = new Point(x, y + 6),
            Size      = new Size(w, 1),
            BackColor = Color.FromArgb(50, 45, 80),
        };

        void DrawScanCorners(Graphics g, Size sz)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int len = 20;
            using (var pen = new Pen(Accent, 2.5f))
            {
                g.DrawLine(pen, 0, len, 0, 0);             g.DrawLine(pen, 0, 0, len, 0);
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

        // ── Drag ─────────────────────────────────────────────────────────────────

        void DragStart(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) _dragStart = e.Location; }
        void DragMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                Location = new Point(Location.X + e.X - _dragStart.X, Location.Y + e.Y - _dragStart.Y);
        }

        // ── Input dialog ─────────────────────────────────────────────────────────

        private string ShowInputDialog(string prompt, string title)
        {
            var dlg = new Form
            {
                Text            = title,
                Size            = new Size(420, 150),
                StartPosition   = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox     = false,
                MinimizeBox     = false,
                BackColor       = Color.FromArgb(18, 16, 36),
            };
            var lbl = new Label { Text = prompt, ForeColor = TxtMain, Font = new Font("Segoe UI", 9),
                Location = new Point(14, 14), Size = new Size(380, 20), BackColor = Color.Transparent };
            var txt = new TextBox { Location = new Point(14, 40), Size = new Size(380, 24),
                BackColor = InputBg, ForeColor = TxtMain, Font = new Font("Segoe UI", 10), BorderStyle = BorderStyle.FixedSingle };
            var btnOk = new Button { Text = "OK", DialogResult = DialogResult.OK,
                Location = new Point(226, 74), Size = new Size(80, 30),
                BackColor = Accent, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            var btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel,
                Location = new Point(314, 74), Size = new Size(80, 30),
                BackColor = Color.FromArgb(50, 45, 80), ForeColor = TxtMain, FlatStyle = FlatStyle.Flat };
            btnOk.FlatAppearance.BorderSize = 0;
            btnCancel.FlatAppearance.BorderSize = 0;
            dlg.AcceptButton = btnOk;
            dlg.CancelButton = btnCancel;
            dlg.Controls.AddRange(new Control[] { lbl, txt, btnOk, btnCancel });
            return dlg.ShowDialog(this) == DialogResult.OK ? txt.Text : null;
        }

        // ── Win32 ────────────────────────────────────────────────────────────────

        [DllImport("Gdi32.dll")]
        static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int cx, int cy);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, string lParam);

        static void SetCueBanner(TextBox tb, string text)
        {
            const uint EM_SETCUEBANNER = 0x1501;
            SendMessage(tb.Handle, EM_SETCUEBANNER, IntPtr.Zero, text);
        }
    }
}
