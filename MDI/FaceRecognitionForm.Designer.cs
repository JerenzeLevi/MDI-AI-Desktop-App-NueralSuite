namespace MDI
{
    partial class FaceRecognitionForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlDataset = new System.Windows.Forms.Panel();
            this.btnOpenFolder = new Guna.UI2.WinForms.Guna2Button();
            this.btnClearDataset = new Guna.UI2.WinForms.Guna2Button();
            this.btnTrainRecognizer = new Guna.UI2.WinForms.Guna2Button();
            this.lblEnrolled = new System.Windows.Forms.Label();
            this.listFaces = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnUploadImage = new Guna.UI2.WinForms.Guna2Button();
            this.btnCaptureFace = new Guna.UI2.WinForms.Guna2Button();
            this.txtPersonName = new System.Windows.Forms.TextBox();
            this.lblNamePrompt = new System.Windows.Forms.Label();
            this.lblDatasetTitle = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.picCameraView = new System.Windows.Forms.PictureBox();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnStartScanning = new Guna.UI2.WinForms.Guna2Button();
            this.pnlHeader.SuspendLayout();
            this.pnlDataset.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCameraView)).BeginInit();
            this.pnlStatus.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlHeader
            //
            this.pnlHeader.BackColor = System.Drawing.Color.HotPink;
            this.pnlHeader.Controls.Add(this.label1);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(900, 40);
            this.pnlHeader.TabIndex = 0;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Rockwell", 10F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Face Recognition";
            //
            // pnlDataset
            //
            this.pnlDataset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.pnlDataset.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDataset.Controls.Add(this.btnOpenFolder);
            this.pnlDataset.Controls.Add(this.btnClearDataset);
            this.pnlDataset.Controls.Add(this.btnTrainRecognizer);
            this.pnlDataset.Controls.Add(this.lblEnrolled);
            this.pnlDataset.Controls.Add(this.listFaces);
            this.pnlDataset.Controls.Add(this.btnUploadImage);
            this.pnlDataset.Controls.Add(this.btnCaptureFace);
            this.pnlDataset.Controls.Add(this.txtPersonName);
            this.pnlDataset.Controls.Add(this.lblNamePrompt);
            this.pnlDataset.Controls.Add(this.lblDatasetTitle);
            this.pnlDataset.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlDataset.Location = new System.Drawing.Point(640, 40);
            this.pnlDataset.Name = "pnlDataset";
            this.pnlDataset.Padding = new System.Windows.Forms.Padding(10);
            this.pnlDataset.Size = new System.Drawing.Size(260, 646);
            this.pnlDataset.TabIndex = 1;
            //
            // btnOpenFolder
            //
            this.btnOpenFolder.BorderRadius = 8;
            this.btnOpenFolder.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnOpenFolder.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnOpenFolder.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnOpenFolder.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnOpenFolder.FillColor = System.Drawing.Color.HotPink;
            this.btnOpenFolder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnOpenFolder.ForeColor = System.Drawing.Color.White;
            this.btnOpenFolder.Location = new System.Drawing.Point(10, 455);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(236, 34);
            this.btnOpenFolder.TabIndex = 9;
            this.btnOpenFolder.Text = "Open Captures Folder";
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            //
            // btnClearDataset
            //
            this.btnClearDataset.BorderRadius = 8;
            this.btnClearDataset.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnClearDataset.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnClearDataset.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnClearDataset.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnClearDataset.FillColor = System.Drawing.Color.Tomato;
            this.btnClearDataset.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnClearDataset.ForeColor = System.Drawing.Color.White;
            this.btnClearDataset.Location = new System.Drawing.Point(10, 412);
            this.btnClearDataset.Name = "btnClearDataset";
            this.btnClearDataset.Size = new System.Drawing.Size(236, 34);
            this.btnClearDataset.TabIndex = 8;
            this.btnClearDataset.Text = "Clear All Faces";
            this.btnClearDataset.Click += new System.EventHandler(this.btnClearDataset_Click);
            //
            // btnTrainRecognizer
            //
            this.btnTrainRecognizer.BorderRadius = 8;
            this.btnTrainRecognizer.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTrainRecognizer.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTrainRecognizer.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTrainRecognizer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnTrainRecognizer.FillColor = System.Drawing.Color.SeaGreen;
            this.btnTrainRecognizer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTrainRecognizer.ForeColor = System.Drawing.Color.White;
            this.btnTrainRecognizer.Location = new System.Drawing.Point(10, 370);
            this.btnTrainRecognizer.Name = "btnTrainRecognizer";
            this.btnTrainRecognizer.Size = new System.Drawing.Size(236, 34);
            this.btnTrainRecognizer.TabIndex = 7;
            this.btnTrainRecognizer.Text = "Train Recognizer";
            this.btnTrainRecognizer.Click += new System.EventHandler(this.btnTrainRecognizer_Click);
            //
            // lblEnrolled
            //
            this.lblEnrolled.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblEnrolled.ForeColor = System.Drawing.Color.Gray;
            this.lblEnrolled.Location = new System.Drawing.Point(10, 142);
            this.lblEnrolled.Name = "lblEnrolled";
            this.lblEnrolled.Size = new System.Drawing.Size(236, 18);
            this.lblEnrolled.TabIndex = 5;
            this.lblEnrolled.Text = "Enrolled:";
            //
            // listFaces
            //
            this.listFaces.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colCount});
            this.listFaces.FullRowSelect = true;
            this.listFaces.GridLines = true;
            this.listFaces.HideSelection = false;
            this.listFaces.Location = new System.Drawing.Point(10, 162);
            this.listFaces.Name = "listFaces";
            this.listFaces.Size = new System.Drawing.Size(236, 200);
            this.listFaces.TabIndex = 6;
            this.listFaces.UseCompatibleStateImageBehavior = false;
            this.listFaces.View = System.Windows.Forms.View.Details;
            //
            // colName
            //
            this.colName.Text = "Name";
            this.colName.Width = 160;
            //
            // colCount
            //
            this.colCount.Text = "Faces";
            this.colCount.Width = 72;
            //
            // btnUploadImage
            //
            this.btnUploadImage.BorderRadius = 8;
            this.btnUploadImage.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnUploadImage.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnUploadImage.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnUploadImage.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnUploadImage.FillColor = System.Drawing.Color.MediumOrchid;
            this.btnUploadImage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnUploadImage.ForeColor = System.Drawing.Color.White;
            this.btnUploadImage.Location = new System.Drawing.Point(132, 100);
            this.btnUploadImage.Name = "btnUploadImage";
            this.btnUploadImage.Size = new System.Drawing.Size(114, 34);
            this.btnUploadImage.TabIndex = 4;
            this.btnUploadImage.Text = "Upload Image";
            this.btnUploadImage.Click += new System.EventHandler(this.btnUploadImage_Click);
            //
            // btnCaptureFace
            //
            this.btnCaptureFace.BorderRadius = 8;
            this.btnCaptureFace.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCaptureFace.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCaptureFace.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCaptureFace.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCaptureFace.FillColor = System.Drawing.Color.HotPink;
            this.btnCaptureFace.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCaptureFace.ForeColor = System.Drawing.Color.White;
            this.btnCaptureFace.Location = new System.Drawing.Point(10, 100);
            this.btnCaptureFace.Name = "btnCaptureFace";
            this.btnCaptureFace.Size = new System.Drawing.Size(114, 34);
            this.btnCaptureFace.TabIndex = 3;
            this.btnCaptureFace.Text = "Capture Face";
            this.btnCaptureFace.Click += new System.EventHandler(this.btnCaptureFace_Click);
            //
            // txtPersonName
            //
            this.txtPersonName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPersonName.Location = new System.Drawing.Point(10, 64);
            this.txtPersonName.Name = "txtPersonName";
            this.txtPersonName.Size = new System.Drawing.Size(236, 30);
            this.txtPersonName.TabIndex = 2;
            //
            // lblNamePrompt
            //
            this.lblNamePrompt.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblNamePrompt.ForeColor = System.Drawing.Color.Gray;
            this.lblNamePrompt.Location = new System.Drawing.Point(10, 44);
            this.lblNamePrompt.Name = "lblNamePrompt";
            this.lblNamePrompt.Size = new System.Drawing.Size(236, 18);
            this.lblNamePrompt.TabIndex = 1;
            this.lblNamePrompt.Text = "Person name:";
            //
            // lblDatasetTitle
            //
            this.lblDatasetTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDatasetTitle.Font = new System.Drawing.Font("Rockwell", 10F, System.Drawing.FontStyle.Bold);
            this.lblDatasetTitle.ForeColor = System.Drawing.Color.HotPink;
            this.lblDatasetTitle.Location = new System.Drawing.Point(10, 10);
            this.lblDatasetTitle.Name = "lblDatasetTitle";
            this.lblDatasetTitle.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblDatasetTitle.Size = new System.Drawing.Size(238, 36);
            this.lblDatasetTitle.TabIndex = 0;
            this.lblDatasetTitle.Text = "Face Dataset";
            this.lblDatasetTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            //
            // pnlMain
            //
            this.pnlMain.Controls.Add(this.picCameraView);
            this.pnlMain.Controls.Add(this.pnlStatus);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 40);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(640, 646);
            this.pnlMain.TabIndex = 2;
            //
            // picCameraView
            //
            this.picCameraView.BackColor = System.Drawing.Color.Black;
            this.picCameraView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picCameraView.Location = new System.Drawing.Point(0, 0);
            this.picCameraView.Name = "picCameraView";
            this.picCameraView.Size = new System.Drawing.Size(640, 586);
            this.picCameraView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCameraView.TabIndex = 1;
            this.picCameraView.TabStop = false;
            this.picCameraView.Click += new System.EventHandler(this.picCameraView_Click);
            //
            // pnlStatus
            //
            this.pnlStatus.Controls.Add(this.lblStatus);
            this.pnlStatus.Controls.Add(this.btnStartScanning);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStatus.Location = new System.Drawing.Point(0, 586);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Padding = new System.Windows.Forms.Padding(8);
            this.pnlStatus.Size = new System.Drawing.Size(640, 60);
            this.pnlStatus.TabIndex = 0;
            //
            // lblStatus
            //
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStatus.Font = new System.Drawing.Font("Rockwell", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.DeepPink;
            this.lblStatus.Location = new System.Drawing.Point(8, 8);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(624, 20);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Status: IDLE";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // btnStartScanning
            //
            this.btnStartScanning.BorderRadius = 12;
            this.btnStartScanning.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnStartScanning.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnStartScanning.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnStartScanning.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnStartScanning.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnStartScanning.FillColor = System.Drawing.Color.HotPink;
            this.btnStartScanning.Font = new System.Drawing.Font("Garamond", 11F, System.Drawing.FontStyle.Bold);
            this.btnStartScanning.ForeColor = System.Drawing.Color.White;
            this.btnStartScanning.Location = new System.Drawing.Point(8, 30);
            this.btnStartScanning.Name = "btnStartScanning";
            this.btnStartScanning.Size = new System.Drawing.Size(624, 22);
            this.btnStartScanning.TabIndex = 1;
            this.btnStartScanning.Text = "Start Scanning";
            this.btnStartScanning.Click += new System.EventHandler(this.btnStartScanning_Click);
            //
            // FaceRecognitionForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(900, 686);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlDataset);
            this.Controls.Add(this.pnlHeader);
            this.Name = "FaceRecognitionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Face Recognition";
            this.Load += new System.EventHandler(this.FaceRecognitionForm_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlDataset.ResumeLayout(false);
            this.pnlDataset.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCameraView)).EndInit();
            this.pnlStatus.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlDataset;
        private System.Windows.Forms.Label lblDatasetTitle;
        private System.Windows.Forms.Label lblNamePrompt;
        private System.Windows.Forms.TextBox txtPersonName;
        private Guna.UI2.WinForms.Guna2Button btnCaptureFace;
        private Guna.UI2.WinForms.Guna2Button btnUploadImage;
        private System.Windows.Forms.Label lblEnrolled;
        private System.Windows.Forms.ListView listFaces;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colCount;
        private Guna.UI2.WinForms.Guna2Button btnTrainRecognizer;
        private Guna.UI2.WinForms.Guna2Button btnClearDataset;
        private Guna.UI2.WinForms.Guna2Button btnOpenFolder;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblStatus;
        private Guna.UI2.WinForms.Guna2Button btnStartScanning;
        private System.Windows.Forms.PictureBox picCameraView;
    }
}
