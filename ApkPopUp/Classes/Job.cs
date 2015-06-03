using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;

namespace ApkPopUp
{
    class Job
    {
        public Job(string jobTitle, Device device, ADB.Jobs job)
        {
            this.P_jobTitle = jobTitle;
            this.P_device = device;
            this.P_job = job;
        }
        private string P_jobTitle;
        private Device P_device;
        private ADB.Jobs P_job;


        public string jobTitle { get { return P_jobTitle; } }
        public Device device { get { return P_device; } }
        public ADB.Jobs job { get { return P_job; } }
        public ArrayList parameters = new ArrayList();
        public string AdbCmd;
        public string result;
        public bool done;
    }
}
