using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Drawing;
using System.IO;
using System;
using Ionic.Zip;

namespace ApkPopUp
{
    public class APK
    {

        public struct Details
        {
            public string filePath;
            public string packageName;
            public string sdkVersion;
            public string targetSdkVersion;
            public string versionCode;
            public string versionName;
            public ArrayList userPermisions;
            public string applicationLable;
            public string applicationIconPath;
            public string applicationHiResIconPath;
            public byte[] icon;
            public byte[] HiResIcon;
            public DateTime insertDate;
            public long size;
            public bool Organized;
        }
        public Details details;
        
        public APK()
        {
            this.details = new Details();
            this.details.userPermisions = new ArrayList();
        }
        private string installAapt()
        {
            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ApkPopUp";
            if (!Directory.Exists(appDataDir))
                Directory.CreateDirectory(appDataDir);
            if (!File.Exists(appDataDir + "//aapt.exe"))
                File.WriteAllBytes(appDataDir + "//aapt.exe", Properties.Resources.aapt);
            return appDataDir + "//aapt.exe";
        }
        public APK(string filePath)
        {
            this.details = new Details();
            //this.details.userPermisions = new ArrayList();
            this.details.filePath = filePath;
            this.details.Organized = false;

            Process aapt = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = installAapt(),
                    Arguments = "d badging \"" + filePath + "\"",
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };
            aapt.Start();
            aapt.BeginOutputReadLine();

            aapt.OutputDataReceived += new DataReceivedEventHandler(dateRecived);
            aapt.WaitForExit();
            //--------------------------------------------DEBUG: MAKE AN ICON FOR NO ICONS APKS-------------------------------------
            Stream iconReader = new MemoryStream(); 
            if (details.applicationIconPath != null && details.applicationIconPath.Length > 4)
            {
                using (ZipFile zip = ZipFile.Read(details.filePath))
                {
                    ZipEntry apkIcon = zip[details.applicationIconPath];
                    apkIcon.Extract(iconReader);
                    iconReader.Position = 0;
                    details.icon = new byte[iconReader.Length];
                    iconReader.Read(details.icon, 0, Convert.ToInt32(iconReader.Length));
                    //ReadHiResIcon
                    apkIcon = zip[details.applicationHiResIconPath];
                    iconReader = new MemoryStream();
                    apkIcon.Extract(iconReader);
                    iconReader.Position = 0;
                    details.HiResIcon = new byte[iconReader.Length];
                    iconReader.Read(details.HiResIcon, 0, Convert.ToInt32(iconReader.Length));
                }
            }
            else
            {
                MemoryStream ms = new MemoryStream();
                Properties.Resources.pingouin_brice_de_nice_98.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                
                details.icon = ms.ToArray();
            }
            

            FileInfo ApkInfo = new FileInfo(filePath);
            details.size = ApkInfo.Length;
        }

        private void dateRecived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)// && 
                //e.Data.IndexOf("application-label") < 0 && 
                //e.Data.IndexOf("application-icon") < 0)// &&
                //e.Data.IndexOf("launchable-activity") < 0 &&
                //e.Data.IndexOf("uses-feature-not-required") < 0 &&
                //e.Data.IndexOf("uses-gl-es") < 0 &&
                //e.Data.IndexOf("uses-feature") < 0)
                getInfosFrom(e.Data);
        }

        private void getInfosFrom(string aaptOutputLine)
        {
            if(aaptOutputLine.IndexOf("package") == 0)
            {
                this.details.packageName = getValue(aaptOutputLine, "name");
                this.details.versionCode = getValue(aaptOutputLine, "versionCode");
                this.details.versionName = getValue(aaptOutputLine, "versionName");
            }

            if(aaptOutputLine.IndexOf("sdkVersion") >= 0)
            {
                this.details.sdkVersion = getValue(aaptOutputLine);
            }

            if (aaptOutputLine.IndexOf("targetSdkVersion") >= 0)
            {
                this.details.targetSdkVersion = getValue(aaptOutputLine);
            }

            //if (aaptOutputLine.IndexOf("uses-permission") >= 0)
            //{
            //    this.details.userPermisions.Add(getValue(aaptOutputLine));
            //}

            if (aaptOutputLine.IndexOf("application:") >= 0)
            {
                this.details.applicationLable = getValue(aaptOutputLine, "label");
                this.details.applicationIconPath = getValue(aaptOutputLine, "icon");
            }
            if(aaptOutputLine.IndexOf("application-icon-120") >= 0||
                aaptOutputLine.IndexOf("application-icon-160") >= 0 ||
                aaptOutputLine.IndexOf("application-icon-240") >= 0||
                aaptOutputLine.IndexOf("application-icon-320") >= 0||
                aaptOutputLine.IndexOf("application-icon-480") >= 0||
                aaptOutputLine.IndexOf("application-icon-640") >= 0)
            {
                this.details.applicationHiResIconPath = getValue(aaptOutputLine);
            }
        }


        private string getValue(string line, string key)
        {
            int valueStartIndex = line.IndexOf('\'', line.IndexOf(key)) + 1;
            return (line.Substring(valueStartIndex, line.IndexOf('\'', valueStartIndex) - valueStartIndex));
        }
        private string getValue(string line)
        {
            int valueStartIndex = line.IndexOf('\'') + 1;
            return (line.Substring(valueStartIndex, line.IndexOf('\'', valueStartIndex) - valueStartIndex));
        }
    }
}
