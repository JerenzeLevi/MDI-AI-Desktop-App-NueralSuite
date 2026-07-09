using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace MDI
{
    public partial class Form1 : Form
    {
        // Track which button is currently hovered by the mouse
        private Button hoveredButton = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 1. Force MDI Container mode immediately before formatting layout layers
            this.IsMdiContainer = true;

            // 2. THE NO-PANEL OVERLAP FIX: Calculate exactly where your menu items end on screen.
            int menuRightBoundary = btnChatbot.Left + btnChatbot.Width + 20;

            // 3. Force the internal MDI client workspace to shift past your sidebar items safely
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is MdiClient)
                {
                    ctrl.BackColor = Color.White;

                    // Moves the window canvas right past your menu layout so nothing ever overlaps!
                    ctrl.Left = menuRightBoundary;
                    ctrl.Width = this.ClientSize.Width - menuRightBoundary;
                }
            }

            // 4. Match your array with the exact names of your designer buttons
            Button[] sidebarButtons = { btnChatbot, btnFaceRecognition, btnObjectRecognition, btnLogout };

            foreach (Button btn in sidebarButtons)
            {
                // Apply your beautiful rounded corner shapes
                SetRoundedCorners(btn, 15);

                // Disable ALL built-in Windows flat hover highlights so they stop making things blocky
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = Color.White;
                btn.FlatAppearance.MouseDownBackColor = Color.White;

                // Attach our custom, clean mouse events to manage the state
                btn.MouseEnter += (s, a) => { hoveredButton = btn; btn.Invalidate(); };
                btn.MouseLeave += (s, a) => { hoveredButton = null; btn.Invalidate(); };

                // Hijack the paint event to cleanly draw our own pink hover background and white text
                btn.Paint += Button_Paint;
            }
        }

        // --- THE FLAWLESS HOVER PAINT FIX ---
        private void Button_Paint(object sender, PaintEventArgs e)
        {
            Button btn = (Button)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (btn == hoveredButton)
            {
                using (SolidBrush bgBrush = new SolidBrush(Color.HotPink))
                {
                    g.FillRectangle(bgBrush, btn.ClientRectangle);
                }

                using (SolidBrush textBrush = new SolidBrush(Color.White))
                {
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString(btn.Text, btn.Font, textBrush, btn.ClientRectangle, sf);
                }
            }
        }

        // --- FIXED CHILD FORM LOADER WINDOW FACTORY ---
        private void OpenChildForm(Type formType)
        {
            Form existingForm = this.MdiChildren.FirstOrDefault(f => f.GetType() == formType);

            // Hide all other open child forms first so they don't stack behind each other
            foreach (Form frm in this.MdiChildren)
            {
                frm.Hide();
            }

            if (existingForm != null)
            {
                existingForm.WindowState = FormWindowState.Maximized;
                existingForm.Show();
                existingForm.BringToFront();
            }
            else
            {
                // 1. Create the instance
                Form childForm = (Form)Activator.CreateInstance(formType);

                // 2. CRITICAL SEQUENCE: Set borderless and docking rules FIRST
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;

                // 3. Assign the parent container layout
                childForm.MdiParent = this;

                // 4. Force a maximized state override so it consumes the entire workspace
                childForm.WindowState = FormWindowState.Maximized;

                childForm.Show();
            }
        }

        // --- HELPER METHOD FOR CLEAN ROUNDED CORNERS ---
        private void SetRoundedCorners(Control control, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddArc(new Rectangle(control.Width - radius, 0, radius, radius), 270, 90);
            path.AddArc(new Rectangle(control.Width - radius, control.Height - radius, radius, radius), 0, 90);
            path.AddArc(new Rectangle(0, control.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();
            control.Region = new Region(path);
        }

        // --- FIXED BUTTON CLICK EVENTS WITH YOUR CORRECT REGISTERED NAMES ---
        private void btnChatbot_Click_1(object sender, EventArgs e)
        {
            OpenChildForm(typeof(ChatBotForm));
        }

        private void btnFaceRecognition_Click_1(object sender, EventArgs e)
        {
            OpenChildForm(typeof(FaceRecognitionForm));
        }

        private void btnObjectRecognition_Click(object sender, EventArgs e)
        {
            OpenChildForm(typeof(ObjectOrientationForm));
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Left empty intentionally
        }
    }
}