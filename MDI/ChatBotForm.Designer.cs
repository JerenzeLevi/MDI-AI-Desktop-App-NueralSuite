namespace MDI
{
    partial class ChatBotForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.SendButton = new Guna.UI2.WinForms.Guna2Button();
            this.MyCutieText = new System.Windows.Forms.TextBox();
            this.scrollableDisplayPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.HistoryPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.NewChatBtn = new Guna.UI2.WinForms.Guna2Button();
            this.ClearHistoryBtn = new Guna.UI2.WinForms.Guna2Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.HotPink;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1091, 37);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Rockwell", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(36, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Chat with Jerenze Levi";
            // 
            // SendButton
            // 
            this.SendButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.SendButton.BorderRadius = 12;
            this.SendButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.SendButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.SendButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.SendButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.SendButton.FillColor = System.Drawing.Color.HotPink;
            this.SendButton.Font = new System.Drawing.Font("Garamond", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SendButton.ForeColor = System.Drawing.Color.White;
            this.SendButton.Location = new System.Drawing.Point(707, 563);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(108, 40);
            this.SendButton.TabIndex = 1;
            this.SendButton.Text = "Send";
            this.SendButton.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // MyCutieText
            // 
            this.MyCutieText.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.MyCutieText.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.MyCutieText.Font = new System.Drawing.Font("Garamond", 18F);
            this.MyCutieText.ForeColor = System.Drawing.Color.HotPink;
            this.MyCutieText.Location = new System.Drawing.Point(280, 562);
            this.MyCutieText.Name = "MyCutieText";
            this.MyCutieText.Size = new System.Drawing.Size(397, 41);
            this.MyCutieText.TabIndex = 2;
            // 
            // scrollableDisplayPanel
            // 
            this.scrollableDisplayPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scrollableDisplayPanel.BackColor = System.Drawing.Color.GhostWhite;
            this.scrollableDisplayPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.scrollableDisplayPanel.Location = new System.Drawing.Point(191, 54);
            this.scrollableDisplayPanel.Name = "scrollableDisplayPanel";
            this.scrollableDisplayPanel.Size = new System.Drawing.Size(705, 487);
            this.scrollableDisplayPanel.TabIndex = 3;
            this.scrollableDisplayPanel.WrapContents = false;
            // 
            // HistoryPanel
            // 
            this.HistoryPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.HistoryPanel.AutoScroll = true;
            this.HistoryPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.HistoryPanel.Location = new System.Drawing.Point(12, 168);
            this.HistoryPanel.Name = "HistoryPanel";
            this.HistoryPanel.Size = new System.Drawing.Size(173, 373);
            this.HistoryPanel.TabIndex = 4;
            // 
            // NewChatBtn
            // 
            this.NewChatBtn.BorderRadius = 12;
            this.NewChatBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.NewChatBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.NewChatBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.NewChatBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.NewChatBtn.FillColor = System.Drawing.Color.HotPink;
            this.NewChatBtn.Font = new System.Drawing.Font("Garamond", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewChatBtn.ForeColor = System.Drawing.Color.White;
            this.NewChatBtn.Location = new System.Drawing.Point(12, 54);
            this.NewChatBtn.Name = "NewChatBtn";
            this.NewChatBtn.Size = new System.Drawing.Size(173, 60);
            this.NewChatBtn.TabIndex = 5;
            this.NewChatBtn.Text = "New Chat";
            this.NewChatBtn.Click += new System.EventHandler(this.NewChatBtn_Click);
            //
            // ClearHistoryBtn
            //
            this.ClearHistoryBtn.BorderRadius = 12;
            this.ClearHistoryBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.ClearHistoryBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.ClearHistoryBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.ClearHistoryBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.ClearHistoryBtn.FillColor = System.Drawing.Color.Tomato;
            this.ClearHistoryBtn.Font = new System.Drawing.Font("Garamond", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClearHistoryBtn.ForeColor = System.Drawing.Color.White;
            this.ClearHistoryBtn.Location = new System.Drawing.Point(12, 122);
            this.ClearHistoryBtn.Name = "ClearHistoryBtn";
            this.ClearHistoryBtn.Size = new System.Drawing.Size(173, 38);
            this.ClearHistoryBtn.TabIndex = 6;
            this.ClearHistoryBtn.Text = "Clear History";
            this.ClearHistoryBtn.Click += new System.EventHandler(this.ClearHistoryBtn_Click);
            //
            // ChatBotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(1091, 731);
            this.ControlBox = false;
            this.Controls.Add(this.ClearHistoryBtn);
            this.Controls.Add(this.NewChatBtn);
            this.Controls.Add(this.HistoryPanel);
            this.Controls.Add(this.scrollableDisplayPanel);
            this.Controls.Add(this.MyCutieText);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ChatBotForm";
            this.Text = "ChatBotForm";
            this.Load += new System.EventHandler(this.ChatBotForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Button SendButton;
        private System.Windows.Forms.TextBox MyCutieText;
        private System.Windows.Forms.FlowLayoutPanel scrollableDisplayPanel;
        private System.Windows.Forms.FlowLayoutPanel HistoryPanel;
        private Guna.UI2.WinForms.Guna2Button NewChatBtn;
        private Guna.UI2.WinForms.Guna2Button ClearHistoryBtn;
    }
}