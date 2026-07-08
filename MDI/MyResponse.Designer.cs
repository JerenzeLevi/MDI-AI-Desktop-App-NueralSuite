namespace MDI
{
    partial class MyResponse
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
            this.message = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // message
            // 
            this.message.AutoSize = true;
            this.message.BackColor = System.Drawing.Color.Pink;
            this.message.Dock = System.Windows.Forms.DockStyle.Right;
            this.message.Font = new System.Drawing.Font("Garamond", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.message.ForeColor = System.Drawing.Color.Crimson;
            this.message.Location = new System.Drawing.Point(1113, 0);
            this.message.MaximumSize = new System.Drawing.Size(250, 0);
            this.message.Name = "message";
            this.message.Size = new System.Drawing.Size(149, 27);
            this.message.TabIndex = 2;
            this.message.Text = "My Response";
            this.message.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MyResponse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.message);
            this.Name = "MyResponse";
            this.Size = new System.Drawing.Size(1262, 63);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label message;
    }
}
