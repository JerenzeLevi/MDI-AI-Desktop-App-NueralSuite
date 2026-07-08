namespace MDI
{
    partial class ObjectOrientationForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlHeader        = new System.Windows.Forms.Panel();
            this.lblTitle         = new System.Windows.Forms.Label();
            this.tabControl       = new System.Windows.Forms.TabControl();
            this.tabTemplates     = new System.Windows.Forms.TabPage();
            this.tabDetect        = new System.Windows.Forms.TabPage();

            // Templates tab
            this.pnlTemplateSide  = new System.Windows.Forms.Panel();
            this.lblTmplTitle     = new System.Windows.Forms.Label();
            this.lstTemplates     = new System.Windows.Forms.ListBox();
            this.pnlNameRow       = new System.Windows.Forms.Panel();
            this.txtObjectName    = new System.Windows.Forms.TextBox();
            this.btnAddTemplate   = new Guna.UI2.WinForms.Guna2Button();
            this.btnRemoveTemplate= new Guna.UI2.WinForms.Guna2Button();
            this.pnlPhotoButtons  = new System.Windows.Forms.Panel();
            this.btnUploadPhotos  = new Guna.UI2.WinForms.Guna2Button();
            this.btnTakePhoto     = new Guna.UI2.WinForms.Guna2Button();
            this.lblPhotoCount    = new System.Windows.Forms.Label();
            this.lblPhotoHint     = new System.Windows.Forms.Label();
            this.flwPreview       = new System.Windows.Forms.FlowLayoutPanel();

            // Detection tab
            this.pnlDetectSide    = new System.Windows.Forms.Panel();
            this.lblDetectStatus  = new System.Windows.Forms.Label();
            this.btnStartDetect   = new Guna.UI2.WinForms.Guna2Button();
            this.lblConfidence    = new System.Windows.Forms.Label();
            this.trkConfidence    = new System.Windows.Forms.TrackBar();
            this.lblDetectedTitle = new System.Windows.Forms.Label();
            this.lstDetected      = new System.Windows.Forms.ListBox();
            this.picCamera        = new System.Windows.Forms.PictureBox();

            this.pnlHeader.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabTemplates.SuspendLayout();
            this.tabDetect.SuspendLayout();
            this.pnlTemplateSide.SuspendLayout();
            this.pnlNameRow.SuspendLayout();
            this.pnlPhotoButtons.SuspendLayout();
            this.pnlDetectSide.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkConfidence)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCamera)).BeginInit();
            this.SuspendLayout();

            // ── pnlHeader ─────────────────────────────────────────────────────
            this.pnlHeader.BackColor = System.Drawing.Color.HotPink;
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock     = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Size     = new System.Drawing.Size(900, 40);
            this.pnlHeader.Name     = "pnlHeader";

            this.lblTitle.AutoSize  = true;
            this.lblTitle.Font      = new System.Drawing.Font("Rockwell", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblTitle.Location  = new System.Drawing.Point(12, 10);
            this.lblTitle.Text      = "Object Recognition  —  Template Matching (No Model Required)";

            // ── tabControl ────────────────────────────────────────────────────
            this.tabControl.Controls.Add(this.tabTemplates);
            this.tabControl.Controls.Add(this.tabDetect);
            this.tabControl.Dock     = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font     = new System.Drawing.Font("Segoe UI", 9.5F);
            this.tabControl.Name     = "tabControl";
            this.tabControl.TabIndex = 1;

            // ── tabTemplates ──────────────────────────────────────────────────
            this.tabTemplates.Text = "  My Objects  ";
            this.tabTemplates.Name = "tabTemplates";
            this.tabTemplates.UseVisualStyleBackColor = true;
            this.tabTemplates.Controls.Add(this.flwPreview);
            this.tabTemplates.Controls.Add(this.pnlTemplateSide);

            // ── pnlTemplateSide (left, 220px) ─────────────────────────────────
            this.pnlTemplateSide.BackColor   = System.Drawing.Color.FromArgb(245, 245, 250);
            this.pnlTemplateSide.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTemplateSide.Dock        = System.Windows.Forms.DockStyle.Left;
            this.pnlTemplateSide.Width       = 220;
            this.pnlTemplateSide.Name        = "pnlTemplateSide";
            this.pnlTemplateSide.Padding     = new System.Windows.Forms.Padding(8, 8, 8, 8);

            this.pnlTemplateSide.Controls.Add(this.lblPhotoHint);
            this.pnlTemplateSide.Controls.Add(this.lblPhotoCount);
            this.pnlTemplateSide.Controls.Add(this.pnlPhotoButtons);
            this.pnlTemplateSide.Controls.Add(this.pnlNameRow);
            this.pnlTemplateSide.Controls.Add(this.lstTemplates);
            this.pnlTemplateSide.Controls.Add(this.lblTmplTitle);

            int sy = 8;
            this.lblTmplTitle.Font      = new System.Drawing.Font("Rockwell", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTmplTitle.ForeColor = System.Drawing.Color.HotPink;
            this.lblTmplTitle.Location  = new System.Drawing.Point(8, sy);
            this.lblTmplTitle.Size      = new System.Drawing.Size(200, 20);
            this.lblTmplTitle.Text      = "Your Objects";
            this.lblTmplTitle.Name      = "lblTmplTitle";

            sy += 22;
            this.lstTemplates.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstTemplates.Font        = new System.Drawing.Font("Segoe UI", 9F);
            this.lstTemplates.Location    = new System.Drawing.Point(8, sy);
            this.lstTemplates.Size        = new System.Drawing.Size(200, 160);
            this.lstTemplates.Name        = "lstTemplates";
            this.lstTemplates.SelectedIndexChanged += new System.EventHandler(this.lstTemplates_SelectedIndexChanged);

            sy += 164;
            this.pnlNameRow.Location = new System.Drawing.Point(8, sy);
            this.pnlNameRow.Size     = new System.Drawing.Size(200, 30);
            this.pnlNameRow.Name     = "pnlNameRow";

            this.txtObjectName.Font      = new System.Drawing.Font("Segoe UI", 9F);
            this.txtObjectName.Location  = new System.Drawing.Point(0, 2);
            this.txtObjectName.Size      = new System.Drawing.Size(108, 26);
            this.txtObjectName.Name      = "txtObjectName";
            this.txtObjectName.KeyDown  += new System.Windows.Forms.KeyEventHandler(this.txtObjectName_KeyDown);

            this.btnAddTemplate.BorderRadius = 5;
            this.btnAddTemplate.FillColor    = System.Drawing.Color.HotPink;
            this.btnAddTemplate.ForeColor    = System.Drawing.Color.White;
            this.btnAddTemplate.Font         = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnAddTemplate.Location     = new System.Drawing.Point(112, 2);
            this.btnAddTemplate.Size         = new System.Drawing.Size(42, 26);
            this.btnAddTemplate.Text         = "Add";
            this.btnAddTemplate.Name         = "btnAddTemplate";
            this.btnAddTemplate.Click       += new System.EventHandler(this.btnAddTemplate_Click);

            this.btnRemoveTemplate.BorderRadius = 5;
            this.btnRemoveTemplate.FillColor    = System.Drawing.Color.FromArgb(200, 200, 205);
            this.btnRemoveTemplate.ForeColor    = System.Drawing.Color.White;
            this.btnRemoveTemplate.Font         = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnRemoveTemplate.Location     = new System.Drawing.Point(158, 2);
            this.btnRemoveTemplate.Size         = new System.Drawing.Size(42, 26);
            this.btnRemoveTemplate.Text         = "Del";
            this.btnRemoveTemplate.Enabled      = false;
            this.btnRemoveTemplate.Name         = "btnRemoveTemplate";
            this.btnRemoveTemplate.Click       += new System.EventHandler(this.btnRemoveTemplate_Click);

            this.pnlNameRow.Controls.Add(this.txtObjectName);
            this.pnlNameRow.Controls.Add(this.btnAddTemplate);
            this.pnlNameRow.Controls.Add(this.btnRemoveTemplate);

            sy += 36;
            var sep = new System.Windows.Forms.Panel
            {
                BackColor = System.Drawing.Color.FromArgb(210, 210, 215),
                Location  = new System.Drawing.Point(8, sy),
                Size      = new System.Drawing.Size(200, 1),
                Name      = "pnlSep"
            };
            this.pnlTemplateSide.Controls.Add(sep);

            sy += 8;
            this.pnlPhotoButtons.Location = new System.Drawing.Point(8, sy);
            this.pnlPhotoButtons.Size     = new System.Drawing.Size(200, 68);
            this.pnlPhotoButtons.Name     = "pnlPhotoButtons";

            this.btnUploadPhotos.BorderRadius = 6;
            this.btnUploadPhotos.FillColor    = System.Drawing.Color.HotPink;
            this.btnUploadPhotos.ForeColor    = System.Drawing.Color.White;
            this.btnUploadPhotos.Font         = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnUploadPhotos.Location     = new System.Drawing.Point(0, 0);
            this.btnUploadPhotos.Size         = new System.Drawing.Size(200, 30);
            this.btnUploadPhotos.Text         = "+ Upload Photos (1–5)";
            this.btnUploadPhotos.Enabled      = false;
            this.btnUploadPhotos.Name         = "btnUploadPhotos";
            this.btnUploadPhotos.Click       += new System.EventHandler(this.btnUploadPhotos_Click);

            this.btnTakePhoto.BorderRadius = 6;
            this.btnTakePhoto.FillColor    = System.Drawing.Color.MediumOrchid;
            this.btnTakePhoto.ForeColor    = System.Drawing.Color.White;
            this.btnTakePhoto.Font         = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnTakePhoto.Location     = new System.Drawing.Point(0, 34);
            this.btnTakePhoto.Size         = new System.Drawing.Size(200, 30);
            this.btnTakePhoto.Text         = "Take Photo with Webcam";
            this.btnTakePhoto.Enabled      = false;
            this.btnTakePhoto.Name         = "btnTakePhoto";
            this.btnTakePhoto.Click       += new System.EventHandler(this.btnTakePhoto_Click);

            this.pnlPhotoButtons.Controls.Add(this.btnUploadPhotos);
            this.pnlPhotoButtons.Controls.Add(this.btnTakePhoto);

            sy += 74;
            this.lblPhotoCount.Font      = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPhotoCount.ForeColor = System.Drawing.Color.DeepPink;
            this.lblPhotoCount.Location  = new System.Drawing.Point(8, sy);
            this.lblPhotoCount.Size      = new System.Drawing.Size(200, 16);
            this.lblPhotoCount.Text      = "";
            this.lblPhotoCount.Name      = "lblPhotoCount";

            sy += 22;
            this.lblPhotoHint.Font      = new System.Drawing.Font("Segoe UI", 8F);
            this.lblPhotoHint.ForeColor = System.Drawing.Color.Gray;
            this.lblPhotoHint.Location  = new System.Drawing.Point(8, sy);
            this.lblPhotoHint.Size      = new System.Drawing.Size(200, 60);
            this.lblPhotoHint.Text      = "Tip: Use 2–3 clear photos of\nthe object from slightly different\nangles for best results.";
            this.lblPhotoHint.Name      = "lblPhotoHint";

            // ── flwPreview (fill, photo thumbnails) ────────────────────────────
            this.flwPreview.AutoScroll  = true;
            this.flwPreview.BackColor   = System.Drawing.Color.FromArgb(28, 28, 28);
            this.flwPreview.Dock        = System.Windows.Forms.DockStyle.Fill;
            this.flwPreview.Padding     = new System.Windows.Forms.Padding(8);
            this.flwPreview.Name        = "flwPreview";

            // ── tabDetect ─────────────────────────────────────────────────────
            this.tabDetect.Text = "  Live Detection  ";
            this.tabDetect.Name = "tabDetect";
            this.tabDetect.UseVisualStyleBackColor = true;
            this.tabDetect.Controls.Add(this.picCamera);
            this.tabDetect.Controls.Add(this.pnlDetectSide);

            // ── pnlDetectSide (right, 240px) ──────────────────────────────────
            this.pnlDetectSide.BackColor   = System.Drawing.Color.FromArgb(245, 245, 250);
            this.pnlDetectSide.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDetectSide.Dock        = System.Windows.Forms.DockStyle.Right;
            this.pnlDetectSide.Width       = 240;
            this.pnlDetectSide.Name        = "pnlDetectSide";

            this.pnlDetectSide.Controls.Add(this.lstDetected);
            this.pnlDetectSide.Controls.Add(this.lblDetectedTitle);
            this.pnlDetectSide.Controls.Add(this.trkConfidence);
            this.pnlDetectSide.Controls.Add(this.lblConfidence);
            this.pnlDetectSide.Controls.Add(this.btnStartDetect);
            this.pnlDetectSide.Controls.Add(this.lblDetectStatus);

            int dy = 14;
            this.lblDetectStatus.Font      = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblDetectStatus.ForeColor = System.Drawing.Color.DeepPink;
            this.lblDetectStatus.Location  = new System.Drawing.Point(10, dy);
            this.lblDetectStatus.Size      = new System.Drawing.Size(218, 18);
            this.lblDetectStatus.Text      = "No objects yet — add templates first.";
            this.lblDetectStatus.Name      = "lblDetectStatus";

            dy += 26;
            this.btnStartDetect.BorderRadius = 8;
            this.btnStartDetect.FillColor    = System.Drawing.Color.HotPink;
            this.btnStartDetect.ForeColor    = System.Drawing.Color.White;
            this.btnStartDetect.Font         = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold);
            this.btnStartDetect.Location     = new System.Drawing.Point(10, dy);
            this.btnStartDetect.Size         = new System.Drawing.Size(218, 38);
            this.btnStartDetect.Text         = "Start Detection";
            this.btnStartDetect.Name         = "btnStartDetect";
            this.btnStartDetect.Click       += new System.EventHandler(this.btnStartDetect_Click);

            dy += 46;
            this.lblConfidence.Font      = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblConfidence.ForeColor = System.Drawing.Color.DeepPink;
            this.lblConfidence.Location  = new System.Drawing.Point(10, dy);
            this.lblConfidence.Size      = new System.Drawing.Size(218, 16);
            this.lblConfidence.Text      = "Match threshold: 60%";
            this.lblConfidence.Name      = "lblConfidence";

            dy += 18;
            this.trkConfidence.Minimum      = 30;
            this.trkConfidence.Maximum      = 95;
            this.trkConfidence.Value        = 60;
            this.trkConfidence.TickFrequency= 5;
            this.trkConfidence.SmallChange  = 1;
            this.trkConfidence.LargeChange  = 5;
            this.trkConfidence.Location     = new System.Drawing.Point(6, dy);
            this.trkConfidence.Size         = new System.Drawing.Size(222, 36);
            this.trkConfidence.Name         = "trkConfidence";
            this.trkConfidence.Scroll      += new System.EventHandler(this.trkConfidence_Scroll);

            dy += 40;
            this.lblDetectedTitle.Font      = new System.Drawing.Font("Rockwell", 9F, System.Drawing.FontStyle.Bold);
            this.lblDetectedTitle.ForeColor = System.Drawing.Color.HotPink;
            this.lblDetectedTitle.Location  = new System.Drawing.Point(10, dy);
            this.lblDetectedTitle.Size      = new System.Drawing.Size(218, 18);
            this.lblDetectedTitle.Text      = "Detected Objects:";
            this.lblDetectedTitle.Name      = "lblDetectedTitle";

            dy += 22;
            this.lstDetected.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstDetected.Font        = new System.Drawing.Font("Segoe UI", 9F);
            this.lstDetected.ForeColor   = System.Drawing.Color.DeepPink;
            this.lstDetected.Location    = new System.Drawing.Point(10, dy);
            this.lstDetected.Size        = new System.Drawing.Size(218, 380);
            this.lstDetected.Anchor      = System.Windows.Forms.AnchorStyles.Top
                                         | System.Windows.Forms.AnchorStyles.Left
                                         | System.Windows.Forms.AnchorStyles.Bottom
                                         | System.Windows.Forms.AnchorStyles.Right;
            this.lstDetected.Name        = "lstDetected";

            this.picCamera.BackColor = System.Drawing.Color.Black;
            this.picCamera.Dock      = System.Windows.Forms.DockStyle.Fill;
            this.picCamera.SizeMode  = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCamera.Name      = "picCamera";

            // ── Form ──────────────────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor           = System.Drawing.Color.WhiteSmoke;
            this.ClientSize          = new System.Drawing.Size(900, 686);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlHeader);
            this.Name  = "ObjectOrientationForm";
            this.Text  = "Object Recognition";
            this.Load += new System.EventHandler(this.ObjectOrientationForm_Load);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabTemplates.ResumeLayout(false);
            this.tabDetect.ResumeLayout(false);
            this.pnlTemplateSide.ResumeLayout(false);
            this.pnlNameRow.ResumeLayout(false);
            this.pnlNameRow.PerformLayout();
            this.pnlPhotoButtons.ResumeLayout(false);
            this.pnlDetectSide.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trkConfidence)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCamera)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel              pnlHeader;
        private System.Windows.Forms.Label              lblTitle;
        private System.Windows.Forms.TabControl         tabControl;
        private System.Windows.Forms.TabPage            tabTemplates;
        private System.Windows.Forms.TabPage            tabDetect;
        private System.Windows.Forms.Panel              pnlTemplateSide;
        private System.Windows.Forms.Label              lblTmplTitle;
        private System.Windows.Forms.ListBox            lstTemplates;
        private System.Windows.Forms.Panel              pnlNameRow;
        private System.Windows.Forms.TextBox            txtObjectName;
        private Guna.UI2.WinForms.Guna2Button           btnAddTemplate;
        private Guna.UI2.WinForms.Guna2Button           btnRemoveTemplate;
        private System.Windows.Forms.Panel              pnlPhotoButtons;
        private Guna.UI2.WinForms.Guna2Button           btnUploadPhotos;
        private Guna.UI2.WinForms.Guna2Button           btnTakePhoto;
        private System.Windows.Forms.Label              lblPhotoCount;
        private System.Windows.Forms.Label              lblPhotoHint;
        private System.Windows.Forms.FlowLayoutPanel    flwPreview;
        private System.Windows.Forms.Panel              pnlDetectSide;
        private System.Windows.Forms.Label              lblDetectStatus;
        private Guna.UI2.WinForms.Guna2Button           btnStartDetect;
        private System.Windows.Forms.Label              lblConfidence;
        private System.Windows.Forms.TrackBar           trkConfidence;
        private System.Windows.Forms.Label              lblDetectedTitle;
        private System.Windows.Forms.ListBox            lstDetected;
        private System.Windows.Forms.PictureBox         picCamera;
    }
}
