using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NodeIt
{
    public class GraphLog
    {
        public static void WriteToLog(object sender, string msg, Project p)
        {
            if (!Directory.Exists(p.path + @"logs\"))
                Directory.CreateDirectory(p.path + @"logs\");

            using (StreamWriter sw = File.AppendText(p.path + @"logs\graphLog.log"))
            {
                sw.WriteLine($"{sender.ToString()} {msg} \t{DateTime.Now}");
            }
        }

        public static void WriteToLog(string sender, string msg, Project p)
        {
            if (!Directory.Exists(p.path + @"logs\"))
                Directory.CreateDirectory(p.path + @"logs\");

            using (StreamWriter sw = File.AppendText(p.path + @"logs\graphLog.log"))
            {
                sw.WriteLine($"{sender} {msg} \t{DateTime.Now}");
            }
        }
    }
}
