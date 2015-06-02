using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ApkPopUp
{
    class Device
    {
        private string P_ID;
        private string P_product;
        private string P_SDKVersion;
        private string P_androidVersion;
        private string P_brand;
        private statusEnum P_status;
        
        
        public enum statusEnum
        {
            offline,
            online
        }
        public string ID
        {
            get { return P_ID; }
        }
        public string product
        {
            get { return P_product; }
        }
        public string SDKVersion
        {
            get { return P_SDKVersion; }
        }
        public string androidVersion
        {
            get { return P_androidVersion; }
        }
        public string brand
        {
            get { return P_brand; }
        }
        public statusEnum status
        {
            get { return P_status; }
        }
        public Device()
        {
            //ADB.setDeviceInfo(this);
        }
        public Device(string deviceID)
        {
            P_ID = deviceID;
            ADB.setDeviceInfo(this);
        }
        public void setAttributes(string product, string SDKVersion, string androidVersion, string brand)
        {
            this.P_product = product;
            this.P_SDKVersion = SDKVersion;
            this.P_androidVersion = androidVersion;
            this.P_brand = brand;
        }
        public void setState(string stat)
        {
            stat = stat.Trim();
            this.P_status = stat == "device" ? statusEnum.online : statusEnum.offline;
        }
    }
}

