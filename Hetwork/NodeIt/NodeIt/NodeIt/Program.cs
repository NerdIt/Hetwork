using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NodeIt
{

    static class Program
    {
        public static string exePath { get { return AppDomain.CurrentDomain.BaseDirectory; } }
        public static string projectPath { get { if (!Directory.Exists(exePath + @"\projects\")) Directory.CreateDirectory(exePath + @"\projects\"); return exePath + @"\projects\"; } }

        

        public static Project selectedProject = null;

        public static NodeForm mainForm;

        static void Main(string[] args)
        {
            ProjectManager.Initialize();
            ProjectManager.LoadProjects();

            mainForm = new NodeForm();
            ProjectSelectionForm ps = new ProjectSelectionForm();
            ps.ShowInTaskbar = true;
            DialogResult dr = ps.ShowDialog();
            if(dr == DialogResult.OK)
            {
                Console.WriteLine("Project Selected");
                mainForm.StartPosition = FormStartPosition.CenterScreen;
                mainForm.LoadProject();
                Application.Run(mainForm);
            }
            else if(dr == DialogResult.Cancel)
            {
                Console.WriteLine("No Project Selected");
                Environment.Exit(0);
            }


            //selectedProject = new Project(0,0);
            //selectedProject.Load("Test For Now");

            
        }

        public static void SetSelectedProject(Project p)
        {
            selectedProject = p;
        }
    }

    public class Project
    {
        public int nodeGlobalId = 0;
        public int taskGlobalId = 0;
        public List<NodeVisual> nodes = new List<NodeVisual>();
        public float zoom = 1;
        public Point offset = new Point(0,0);
        public string title = "";

        public Size graphSize = new Size(50, 50);

        public string path { get { return Program.projectPath + title + @"\"; } }

        public Project(int ngi, int tgi)
        {
            nodeGlobalId = ngi;
            taskGlobalId = tgi;
        }

        public void Load(string t)
        {
            title = t;
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


    public static class ListExtensions
    {

        public static void Rearrange<T>(this List<T> list, int index, int targetIndex)
        {
            try
            {
                T v = list[index];
                list.RemoveAt(index);
                list.Insert(targetIndex, v);
            }
            catch
            {

            }
        }

        public static void SendToTop<T>(this List<T> list, int index, int targetIndex)
        {
            T v = list[index];
            list.RemoveAt(index);
            list.Insert(0, v);
        }

        public static void SendToBottom<T>(this List<T> list, int index, int targetIndex)
        {
            T v = list[index];
            list.RemoveAt(index);
            list.Insert(list.Count, v);
        }
    }
}
