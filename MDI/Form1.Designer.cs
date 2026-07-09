namespace MDI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnChatbot = new System.Windows.Forms.Button();
            this.btnFaceRecognition = new System.Windows.Forms.Button();
            this.btnObjectRecognition = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.GhostWhite;
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnLogout);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(397, 678);
            this.panel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.btnChatbot);
            this.flowLayoutPanel1.Controls.Add(this.btnFaceRecognition);
            this.flowLayoutPanel1.Controls.Add(this.btnObjectRecognition);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(37, 254);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(321, 163);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // btnChatbot
            // 
            this.btnChatbot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChatbot.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepPink;
            this.btnChatbot.Font = new System.Drawing.Font("Rockwell", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChatbot.ForeColor = System.Drawing.Color.DeepPink;
            this.btnChatbot.Location = new System.Drawing.Point(3, 2);
            this.btnChatbot.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnChatbot.Name = "btnChatbot";
            this.btnChatbot.Size = new System.Drawing.Size(317, 50);
            this.btnChatbot.TabIndex = 4;
            this.btnChatbot.Text = "ChatBot";
            this.btnChatbot.UseVisualStyleBackColor = true;
            this.btnChatbot.Click += new System.EventHandler(this.btnChatbot_Click_1);
            // 
            // btnFaceRecognition
            // 
            this.btnFaceRecognition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFaceRecognition.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepPink;
            this.btnFaceRecognition.Font = new System.Drawing.Font("Rockwell", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFaceRecognition.ForeColor = System.Drawing.Color.DeepPink;
            this.btnFaceRecognition.Location = new System.Drawing.Point(3, 56);
            this.btnFaceRecognition.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnFaceRecognition.Name = "btnFaceRecognition";
            this.btnFaceRecognition.Size = new System.Drawing.Size(317, 50);
            this.btnFaceRecognition.TabIndex = 5;
            this.btnFaceRecognition.Text = "Face Recognition";
            this.btnFaceRecognition.UseVisualStyleBackColor = true;
            this.btnFaceRecognition.Click += new System.EventHandler(this.btnFaceRecognition_Click_1);
            // 
            // btnObjectRecognition
            // 
            this.btnObjectRecognition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnObjectRecognition.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepPink;
            this.btnObjectRecognition.Font = new System.Drawing.Font("Rockwell", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnObjectRecognition.ForeColor = System.Drawing.Color.DeepPink;
            this.btnObjectRecognition.Location = new System.Drawing.Point(3, 110);
            this.btnObjectRecognition.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnObjectRecognition.Name = "btnObjectRecognition";
            this.btnObjectRecognition.Size = new System.Drawing.Size(317, 50);
            this.btnObjectRecognition.TabIndex = 6;
            this.btnObjectRecognition.Text = "Object Recognition";
            this.btnObjectRecognition.UseVisualStyleBackColor = true;
            this.btnObjectRecognition.Click += new System.EventHandler(this.btnObjectRecognition_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Garamond", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.DeepPink;
            this.label4.Location = new System.Drawing.Point(117, 650);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(145, 19);
            this.label4.TabIndex = 11;
            this.label4.Text = "Jerenze Levi @ 2026";
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepPink;
            this.btnLogout.Font = new System.Drawing.Font("Rockwell", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.ForeColor = System.Drawing.Color.DeepPink;
            this.btnLogout.Location = new System.Drawing.Point(41, 583);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(317, 44);
            this.btnLogout.TabIndex = 10;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Garamond", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DeepPink;
            this.label3.Location = new System.Drawing.Point(131, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 19);
            this.label3.TabIndex = 9;
            this.label3.Text = "Your AI Boyfriend";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Rockwell", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DeepPink;
            this.label2.Location = new System.Drawing.Point(132, 165);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "Jerenze Levi";
            // 
            // pictureBox1
            // 
            this.pictureBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::MDI.Properties.Resources.AI_Boyfriend;
            this.pictureBox1.Location = new System.Drawing.Point(77, 18);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(241, 145);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.WaitOnLoad = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 678);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "[";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnChatbot;
        private System.Windows.Forms.Button btnObjectRecognition;
        private System.Windows.Forms.Button btnFaceRecognition;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}

