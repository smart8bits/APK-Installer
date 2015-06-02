using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace ApkPopUp
{
    public partial class Form1 : Form
    {
        public Form1(string ApkPath)
        {
            InitializeComponent();
            //ThreadStart showIcon = new ThreadStart(showIconMethod(apk));
            Thread showIconThread = new Thread(() => showIconMethod(ApkPath));
            showIconThread.Start();
            ADB.newLogAdd += new ADB.newLogAdded(installError);
            ADB.Start();

        }
        private void showIconMethod(string ApkPath)
        {
            apkPanel1.apk = new APK(ApkPath);
        }

        public void BtnInstall_Click(object sender, EventArgs e)
        {
            ADB.install(apkPanel1.apk, true, new Device("BH90MHT50D"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            APK apk = new APK(@"E:\Android\APP\Org Apps\AirDroid\AirDroid 2.1.0.apk");
            //ADB.newLogAdd += new ADB.newLogAdded(installError);
            ADB.install(apk, false, new Device());
        }
        private void installError(ADB.log log, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate()
            {
                FlowLayoutPanel fl = new FlowLayoutPanel();
                Label title = new Label();
                title.Text = log.jobTitle;
                title.Width = fl.Width;
                Label Type = new Label();
                Type.Text = log.type.ToString();
                Type.Width = fl.Width;
                Label Message = new Label();
                Message.Text = log.message;
                Message.Width = fl.Width;
                fl.Controls.Add(title);
                fl.Controls.Add(Type);
                fl.Controls.Add(Message);
                fl.Width = flowLayoutPanel1.Width - 25;
                fl.Height = 75;
                fl.BorderStyle = BorderStyle.FixedSingle;
                fl.BackColor = Color.White;
                fl.FlowDirection = FlowDirection.TopDown;
                flowLayoutPanel1.Controls.Add(fl);
            });
            if (log.type == ADB.logType.Error)
            {
                MessageBox.Show(log.message, log.jobTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

