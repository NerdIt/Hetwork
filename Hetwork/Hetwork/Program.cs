using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace Hetwork
{
    class Program
    {
        public static string exePath { get { return AppDomain.CurrentDomain.BaseDirectory; } }
        public static string projectPath { get { if (!Directory.Exists(exePath + @"\projects\")) Directory.CreateDirectory(exePath + @"\projects\"); return exePath + @"\projects\"; } }

        public static string[] projects { get { return Directory.GetDirectories(projectPath); } }

        public static Project selectedProject = null;

        static void Main(string[] args)
        {
            //string s = Console.ReadLine();
            //while(!projects.Contains(s))
            //{
            //    Console.WriteLine("Project Doesn't Exist");
            //    s = Console.ReadLine();
            //}
            //selectedProject = new Project(0);



            Application.Run(new NodeForm());
            //Application.Run(new CHECKLISTPRO_FORMTEST());
            //Application.Run(new EditorForm());
        }
    }


    public class Project
    {
        public int nodeGlobalId = 0;
        public int taskGlobalId = 0;
        public List<NodeVisual> nodes = new List<NodeVisual>();
        public float zoom;
        public Point offset;

        public string title;

        public Project(int ngi, int tgi)
        {
            nodeGlobalId = ngi;
            taskGlobalId = tgi;
        }


        public int GetNodeId()
        {
            nodeGlobalId++;
            return nodeGlobalId - 1;
        }

        public int GetTaskId()
        {
            taskGlobalId++;
            return taskGlobalId - 1;
        }
        
    }
}
