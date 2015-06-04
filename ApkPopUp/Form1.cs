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
            showIconThread.IsBackground = true;
            showIconThread.Start();
            ADB.newLogAdd += new ADB.newLogAdded(AnalyseLog);
            ADB.Start();
            //Device selectedDevice;
            if(Properties.Settings.Default.SelectedDevice == "")
            {
                ADB.refreshDevicesList();
            }
            else
            {
                ComBxDevices.Text = Properties.Settings.Default.SelectedDevice;
            }
                
        }
        private void showIconMethod(string ApkPath)
        {
            apkPanel1.apk = new APK(ApkPath);
        }

        public void BtnInstall_Click(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.SelectedDevice == "")
                ADB.install(apkPanel1.apk, true, ADB.devicesList[ComBxDevices.SelectedIndex]);
            else
                ADB.install(apkPanel1.apk, true, new Device(Properties.Settings.Default.SelectedDevice));
            if (ComBxDevices.Text != Properties.Settings.Default.SelectedDevice)
            {
                Properties.Settings.Default.SelectedDevice = ComBxDevices.Text;
                Properties.Settings.Default.Save();
            }
            //ADB.install(apkPanel1.apk, true, new Device("BH90MHT50D"));
        }

        private void AnalyseLog(ADB.log log, EventArgs e)
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
            if (log.jobTitle == "Get State" && !GetDevices)
            {
                this.Invoke((MethodInvoker)delegate()
                    {
                        string lastDeviceSelected = ComBxDevices.Text;
                        ComBxDevices.Items.Clear();
                        foreach (Device device in ADB.devicesList)
                        {

                            ComBxDevices.Items.Add(device.ID);
                        }
                        if (ComBxDevices.Text == "")
                            ComBxDevices.SelectedIndex = 0;
                        else
                            ComBxDevices.SelectedValue = lastDeviceSelected;
                    });
                GetDevices = true;
            }
        }

        bool GetDevices = false;
        private void ComBxDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void ComBxDevices_MouseClick(object sender, MouseEventArgs e)
        {
            if (!GetDevices)
            {
                ADB.refreshDevicesList();
            }
        }
    }
}

