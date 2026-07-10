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
    public partial class AIResponse : UserControl
    {
        public AIResponse()
        {
            InitializeComponent();

            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            this.Margin = new Padding(10, 5, 0, 5);
            this.MaximumSize = new Size(600, 0);

            // 🔥 Avatar
            AIPicBox.Size = new Size(35, 35);
            AIPicBox.SizeMode = PictureBoxSizeMode.StretchImage;
            AIPicBox.Location = new Point(0, 5);

            // 🔥 Message bubble
            AImessage.AutoSize = true;
            AImessage.MaximumSize = new Size(250, 0);
            AImessage.AutoEllipsis = false;
            AImessage.BackColor = Color.LightGray;
            AImessage.Padding = new Padding(10);

            // 🔥 Position message BESIDE avatar
            AImessage.Location = new Point(45, 5);
        }

        public string Message
        {
            get => AImessage.Text;
            set => AImessage.Text = value;
        }
    }
}