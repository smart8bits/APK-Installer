using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace ApkPopUp
{
    class ADB
    {
        #region JobManagement
        private static Queue<Job> jobs = new Queue<Job>();
        
        private static void addJob(Job job)
        {
            AnalyseJob(job);
            if(manageJobThread == null || !manageJobThread.IsAlive)
            {
                manageJobThread = new Thread(manageJobThreadStart);
                manageJobThread.IsBackground = true;
                manageJobThread.Start();
            }
        }
        static ThreadStart manageJobThreadStart = new ThreadStart(manageJobs);
        static Thread manageJobThread;
        private static void manageJobs()
        {
            while (true)
            {
                if (jobs.Count > 0)
                {
                    Do(jobs.Dequeue());
                }
                else
                {
                    Thread.CurrentThread.Abort();
                }
            }
        }
        
        #endregion

        #region JobsEnum
        public enum Jobs
        {
            Install,
            Uninstall,
            Devices,
            GetState,
            GetDeviceInfo
        }
        #endregion

        private static Process adb = new Process();
        private static string ADBPath = @"D:\_Programming\Android\Bank\ADB\adb.exe";
        private static StringBuilder outPutStr = new StringBuilder();

        #region ADB Start
        public static void Start()
        {
            adb.StartInfo = new ProcessStartInfo
            {
                FileName = ADBPath,//@"D:\ADB\adb.exe",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            adb.Start();
            adb.WaitForExit();
            adb.OutputDataReceived += new DataReceivedEventHandler(outPutRecived);
        }
        private static void outPutRecived(object sender, DataReceivedEventArgs e)
        {
            outPutStr.AppendLine(e.Data);
        }
        #endregion

        #region ADB Do Command
        public enum logType
        {
            Error,
            Warning,
            Info
        }
        public struct log
        {
            public string jobTitle;
            public logType type;
            public string message;
        }
        public static ArrayList logs = new ArrayList();
        public delegate void newLogAdded(log l, EventArgs e);
        public static event newLogAdded newLogAdd;
        private static void AnalyseJob(Job job)
        {
            StringBuilder AdbCmdBuilder = new StringBuilder();
            switch (job.job)
            {
                case Jobs.Devices:
                    AdbCmdBuilder.Append("devices");
                    break;
                case Jobs.GetState:
                    AdbCmdBuilder.Append("get-state");
                    break;
                case Jobs.GetDeviceInfo:
                    AdbCmdBuilder.Append("shell getprop");
                    break;
                case Jobs.Install:
                    AdbCmdBuilder.Append("install ");
                    if(job.parameters.Count > 1)
                    {
                        AdbCmdBuilder.Append(job.parameters[1]);
                        AdbCmdBuilder.Append(" ");
                    }
                    AdbCmdBuilder.Append(String.Format("\"{0}\"", job.parameters[0]));
                    break;
                case Jobs.Uninstall:
                    break;
            }
            job.AdbCmd = AdbCmdBuilder.ToString();
            jobs.Enqueue(job);
        }
        
        private static void Do(Job jobToDo)
        {
            string Cmd;
            if (jobToDo.device.ID != null)
            {
                adb.StartInfo.Arguments = string.Format("-s \"{0}\" get-state", jobToDo.device.ID);
                Cmd = string.Format("-s \"{0}\" {1}", jobToDo.device.ID, jobToDo.AdbCmd);
            }
            else
            {
                adb.StartInfo.Arguments = string.Format("get-state");
                Cmd = jobToDo.AdbCmd;
            }
            adb.Start();
            adb.BeginOutputReadLine();
            adb.WaitForExit();
            jobToDo.device.setState(outPutStr.ToString());
            outPutStr.Clear();
            adb.CancelOutputRead();
            if (jobToDo.device.status == Device.statusEnum.online)
            {
                adb.StartInfo.Arguments = Cmd;
                adb.Start();
                adb.BeginOutputReadLine();
                adb.WaitForExit();
                jobToDo.result = outPutStr.ToString();
                outPutStr.Clear();
                adb.CancelOutputRead();
            }
            else
            {
                jobToDo.result = "Device is Offline";
            }
            jobToDo.done = true;
            AnalyseResult(jobToDo);
        }

        private static void AnalyseResult(Job job)
        {
            log log;
                    
            switch (job.job)
            {
                case Jobs.Devices:
                    log.jobTitle = job.jobTitle;
                    log.type = logType.Info;
                    log.message = job.result;
                    logs.Add(log);
                    newLogAdd(log, new EventArgs());
                    
                    int start = 0;
                    int end = job.result.IndexOf("\n");
                    devicesList.Clear();
                    while (end != -1)
                    {
                        start = end + 1;
                        end = job.result.IndexOf("\n", start);
                        if (end - start < 2)
                            continue;
                        string device = job.result.Substring(start, end - start);
                        int IDEnd = device.IndexOf("\t");
                        int endOfLine = device.Length - 2;
                        string ID = device.Substring(0, IDEnd);
                        string status = device.Substring(IDEnd + 1, endOfLine - IDEnd);
                        Device newDevice = new Device(ID);
                        devicesList.Add(newDevice);
                    }
                    break;

                case Jobs.GetState:
                    log.jobTitle = job.jobTitle;
                    log.type = logType.Info;
                    log.message = job.result;
                    logs.Add(log);
                    newLogAdd(log, new EventArgs());

                    job.device.setState(job.result.Trim());
                    break;

                case Jobs.GetDeviceInfo:
                    if (job.result != "Device is Offline")
                    {
                        log.jobTitle = job.jobTitle;
                        log.type = logType.Info;
                        log.message = job.result;
                        logs.Add(log);
                        newLogAdd(log, new EventArgs());

                        job.device.setAttributes(
                        getProperty(job.result, "[ro.build.product]"),
                        getProperty(job.result, "[ro.build.version.sdk]"),
                        getProperty(job.result, "[ro.build.version.release]"),
                        getProperty(job.result, "[ro.product.brand]")
                        );
                    }
                    else
                    {
                        log.jobTitle = job.jobTitle;
                        log.type = logType.Error;
                        log.message = job.result;
                        logs.Add(log);
                        newLogAdd(log, new EventArgs());
                    }

                    break;
                case Jobs.Install:
                    if (job.result != "Device is Offline")
                    {
                        log.jobTitle = job.jobTitle;

                        int failIndex = job.result.IndexOf("Failure");
                        if (failIndex >= 0)
                        {
                            log.type = logType.Error;
                            log.message = job.result.Substring(failIndex + 9, job.result.IndexOf("\r", failIndex) - 1 - (failIndex + 9));
                        }
                        else
                        {
                            log.type = logType.Info;
                            log.message = job.result;
                        }
                        logs.Add(log);
                        newLogAdd(log, new EventArgs());
                    }
                    else
                    {
                        log.jobTitle = job.jobTitle;
                        log.type = logType.Error;
                        log.message = job.result;
                        logs.Add(log);
                        newLogAdd(log, new EventArgs());
                    }
                    break;
                case Jobs.Uninstall:
                    if (job.result != "Device is Offline")
                    {
                    }
                    else
                    {
                        log.jobTitle = job.jobTitle;
                        log.type = logType.Error;
                        log.message = job.result;
                        logs.Add(log);
                        newLogAdd(log, new EventArgs());
                    }
                    break;
            }
        }
        #endregion

        #region Get Conected Devices
        public static List<Device> devicesList = new List<Device>();
        private static void refreshDevicesList()
        {
            Job getDevices = new Job("Get Devices", null, Jobs.Devices);
            addJob(getDevices);
        }

        #region Set Device Info
        public static void setDeviceInfo(Device device)
        {
            Job getDeviceState = new Job("Get State", device, Jobs.GetState);
            addJob(getDeviceState);
            Job getDeviceInfo = new Job("Get Device Info", device, Jobs.GetDeviceInfo);
            addJob(getDeviceInfo);
        }
        private static string getProperty(string deviceProperties, string propertyName)
        {
            int start = deviceProperties.IndexOf(propertyName) + propertyName.Length + 3;
            int end = deviceProperties.IndexOf("\n", start) - 2;
            return (deviceProperties.Substring(start, end - start));
        }
        #endregion

        #endregion

        #region Public Methods
        public static void install(APK app, bool Reinstall, Device device)
        {
            string appPath = app.details.filePath;
            Job installJob = new Job(string.Format("Install {0} {1}", app.details.applicationLable, app.details.versionName), device, Jobs.Install);
            installJob.parameters.Add(appPath);
            if (Reinstall)
            {
                installJob.parameters.Add("-r");
            }
            addJob(installJob);
        }

        public static void uninstall(APK app, bool keepData, Device device)
        {
            Job uninstallJob = new Job(string.Format("Uninstall {0} {1}", app.details.applicationLable, app.details.versionName), device, Jobs.Uninstall);
            if (keepData)
            {
                //uninstallJob.AdbCmd = string.Format("shell pm uninstall -k -k \"{0}\"", packageName);
            }
            else
            {
                //uninstallJob.AdbCmd = string.Format("uninstall \"{0}\"", packageName);
            }
            addJob(uninstallJob);
            while (!uninstallJob.done) ;
            if (uninstallJob.result.IndexOf("Failure") >= 0)
            {
                throw new ApplicationException("Failure");
            }
        }
        #endregion
    }
}