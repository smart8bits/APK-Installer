using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ApkPopUp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            args = new string[1];
            args[0] = @"C:\Users\smart8\Desktop\app\com.dama.hardwareinfo-1\base.apk";
            if (args.Length > 0)
            {
                if (File.Exists(args[0]))
                {
                    FileInfo inputInfo = new FileInfo(args[0]);
                    if (inputInfo.Extension == ".apk")
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Form1(args[0]));
                    }
                }
            }

        }
    }
}
