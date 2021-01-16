using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Hetwork
{
    public class GraphLog
    {
        public static void WriteToLog(object sender, string msg)
        {
            if (!Directory.Exists(Program.selectedProject.path + @"logs\"))
                Directory.CreateDirectory(Program.selectedProject.path + @"logs\");

            using (StreamWriter sw = File.AppendText(Program.selectedProject.path + @"logs\graphLog.log"))
            {
                sw.WriteLine($"{sender.ToString()} {msg} \t{DateTime.Now}");
            }
        }

        public static void WriteToLog(string sender, string msg)
        {
            if (!Directory.Exists(Program.selectedProject.path + @"logs\"))
                Directory.CreateDirectory(Program.selectedProject.path + @"logs\");

            using (StreamWriter sw = File.AppendText(Program.selectedProject.path + @"logs\graphLog.log"))
            {
                sw.WriteLine($"{sender} {msg} \t{DateTime.Now}");
            }
        }
    }
}
