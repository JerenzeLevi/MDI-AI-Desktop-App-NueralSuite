namespace MDI
{
    partial class AIResponse
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AImessage = new System.Windows.Forms.Label();
            this.AIPicBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.AIPicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // AImessage
            // 
            this.AImessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AImessage.AutoSize = true;
            this.AImessage.BackColor = System.Drawing.Color.Pink;
            this.AImessage.Font = new System.Drawing.Font("Garamond", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AImessage.ForeColor = System.Drawing.Color.Crimson;
            this.AImessage.Location = new System.Drawing.Point(71, 31);
            this.AImessage.Name = "AImessage";
            this.AImessage.Size = new System.Drawing.Size(149, 27);
            this.AImessage.TabIndex = 3;
            this.AImessage.Text = "My Response";
            this.AImessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AIPicBox
            // 
            this.AIPicBox.Image = global::MDI.Properties.Resources.AI_Boyfriend;
            this.AIPicBox.Location = new System.Drawing.Point(3, 18);
            this.AIPicBox.Name = "AIPicBox";
            this.AIPicBox.Size = new System.Drawing.Size(62, 50);
            this.AIPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.AIPicBox.TabIndex = 4;
            this.AIPicBox.TabStop = false;
            // 
            // AIResponse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.AIPicBox);
            this.Controls.Add(this.AImessage);
            this.Name = "AIResponse";
            this.Size = new System.Drawing.Size(1271, 71);
            ((System.ComponentModel.ISupportInitialize)(this.AIPicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label AImessage;
        private System.Windows.Forms.PictureBox AIPicBox;
    }
}
