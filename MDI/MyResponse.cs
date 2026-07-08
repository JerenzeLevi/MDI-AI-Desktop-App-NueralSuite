using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDI
{
    public partial class MyResponse : UserControl
    {
        public MyResponse()
        {
            InitializeComponent();

            // Setup the UserControl properties
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Margin = new Padding(0, 5, 10, 5);
            this.MinimumSize = new Size(0, 30);

            // Setup the Message Label properties
            message.AutoSize = true;
            message.MaximumSize = new Size(250, 0);
            message.AutoEllipsis = false;
            message.Padding = new Padding(10);
            message.BackColor = Color.LightBlue;

            // Anchor it to Top-Right so it behaves when resizing
            message.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // 🔥 CRITICAL FIX: Every time the Form resizes this control, 
            // recalculate the X position to flush the text box to the right wall.
            this.SizeChanged += MyResponse_SizeChanged;
        }

        private void MyResponse_SizeChanged(object sender, EventArgs e)
        {
            // Position the message label at the absolute right margin of the user control
            message.Location = new Point(this.Width - message.Width, 0);
        }

        public string Message
        {
            get { return message.Text; }
            set
            {
                message.Text = value;

                // Force layout updates so measurements take effect immediately
                message.PerformLayout();
                this.PerformLayout();
            }
        }
    }
}