using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;

namespace Hetwork
{
    class Program
    {
        public static string exePath { get { return AppDomain.CurrentDomain.BaseDirectory; } }
        public static string projectPath { get { if (!Directory.Exists(exePath + @"\projects\")) Directory.CreateDirectory(exePath + @"\projects\"); return exePath + @"\projects\"; } }

        public static string[] projects { get { return Directory.GetDirectories(projectPath); } }

        public static Project selectedProject = null;


        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;

        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);

            //Application.Run(new VisualDesign());

            //string path = projectPath;

            //for (int i = 0; i < projects.Length; i++)
            //{
            //    Console.WriteLine($"{i} " + projects[i].Split('\\')[projects[i].Split('\\').Length - 1]);
            //}
            //Console.Write("Select Project: ");
            //int s = int.Parse(Console.ReadLine());
            //if (s > projects.Length)
            //    Environment.Exit(0);


            //if (s != -1)
            //{
            //    Console.WriteLine($"You Selected {projects[s]}");

            //    selectedProject = new Project(0, 0);
            //    selectedProject.Load(projects[s].Split('\\')[projects[s].Split('\\').Length - 1]);
            //}
            //else
            //{
            //    var ib = Interaction.InputBox("New Project Name", "Create Project");
            //    if (ib != "")
            //    {
            //        if (!Directory.Exists(projectPath + ib))
            //            Directory.CreateDirectory(projectPath + ib);
            //        string newPath = projectPath + ib;
            //        selectedProject = new Project(0, 0);
            //        selectedProject.Load(newPath.Split('\\')[newPath.Split('\\').Length - 1]);
            //    }
            //}

            //NodeForm nf = new NodeForm();
            //if (s != -1)
            //{
            //    nf.LoadData(selectedProject, false);
            //}
            //else
            //{
            //    nf.LoadData(selectedProject, true);
            //}


            NodeForm nf = new NodeForm(true);
            Application.Run(nf);
            
            
            //ps.ShowDialog();
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

        public string title = "";

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
