using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ApkPopUp
{
    public partial class ApkPanel : UserControl
    {
        private APK p_apk;
        public APK apk
        {
            set
            {
                if (value != null)
                {
                    p_apk = value;
                    Image icon = Image.FromStream(new MemoryStream(value.details.HiResIcon));
                    int height = Math.Min(icon.Height, 180);
                    int width = Math.Min(icon.Width, 180); ;
                    IconBx.Size = new System.Drawing.Size(width, height);
                    Size = new System.Drawing.Size(width + 10, height + 20);
                    IconBx.Location = new Point(5, 5);
                    lblName.Location = new Point(5, height + 5);
                    IconBx.Image = icon;
                    pictureBox1.Visible = false;
                    lblName.Text = value.details.applicationLable + " " + value.details.versionName;
                }
            }
            get
            {
                return p_apk;
            }
        }
        public ApkPanel()
        {
            InitializeComponent();
        }
        public ApkPanel(APK apk)
        {
            InitializeComponent();
            this.apk = apk;
        }
    }
}
