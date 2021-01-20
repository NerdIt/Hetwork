using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hetwork
{
    public partial class ProjectSelectionForm : Form
    {

        NodeForm nf = null;
        bool initialSelection;
        bool selectedOption = false;
        public ProjectSelectionForm(NodeForm n, bool init)
        {
            InitializeComponent();
            nf = n;
            openBtn.Enabled = false;
            initialSelection = init;

            for (int i = 0; i < Program.projects.Length; i++)
            {
                projectPanel.Items.Add(Program.projects[i].Split('\\')[Program.projects[i].Split('\\').Length - 1]);
            }
        }

        private void openBtn_Click(object sender, EventArgs e)
        {
            selectedOption = true;
            Program.selectedProject = new Project(0, 0);
            Program.selectedProject.Load(Program.projects[projectPanel.SelectedIndex].Split('\\')[Program.projects[projectPanel.SelectedIndex].Split('\\').Length - 1]);
            if (File.Exists(Program.projects[projectPanel.SelectedIndex] + @"\savedata.data"))
            {
                nf.LoadData(Program.selectedProject, false);
            }
            else
            {
                Debug.WriteLine("File not found");
                Program.selectedProject = new Project(0, 0);
                Program.selectedProject.zoom = 1;
                Program.selectedProject.Load(Program.projects[projectPanel.SelectedIndex].Split('\\')[Program.projects[projectPanel.SelectedIndex].Split('\\').Length - 1]);
                nf.LoadData(Program.selectedProject, true);
            }


            Close();
        }

        private void projectPanel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(projectPanel.SelectedIndex != -1)
            {
                openBtn.Enabled = true;
            }
            else
            {
                openBtn.Enabled = false;
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            selectedOption = true;
            if (initialSelection)
                Environment.Exit(0);
            else
                Close();
        }

        private void newBtn_Click(object sender, EventArgs e)
        {
            var ib = Interaction.InputBox("New Project Name", "Create Project");
            if (ib != "")
            {
                selectedOption = true;
                if (!Directory.Exists(Program.projectPath + ib))
                    Directory.CreateDirectory(Program.projectPath + ib);
                string newPath = Program.projectPath + ib;
                Program.selectedProject = new Project(0, 0);
                Program.selectedProject.Load(newPath.Split('\\')[newPath.Split('\\').Length - 1]);
                nf.LoadData(Program.selectedProject, true);

                Close();
            }
        }

        private void ProjectSelectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!selectedOption)
                e.Cancel = true;
        }
    }
}
