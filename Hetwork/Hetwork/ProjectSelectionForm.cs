using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public ProjectSelectionForm(NodeForm n)
        {
            InitializeComponent();
            nf = n;
            openBtn.Enabled = false;

            for (int i = 0; i < Program.projects.Length; i++)
            {
                projectPanel.Items.Add(Program.projects[i].Split('\\')[Program.projects[i].Split('\\').Length - 1]);
            }
        }

        private void openBtn_Click(object sender, EventArgs e)
        {
            Program.selectedProject = new Project(0, 0);
            Program.selectedProject.Load(Program.projects[projectPanel.SelectedIndex].Split('\\')[Program.projects[projectPanel.SelectedIndex].Split('\\').Length - 1]);
            nf.LoadData(Program.selectedProject, false);
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
            Close();
        }

        private void newBtn_Click(object sender, EventArgs e)
        {
            var ib = Interaction.InputBox("New Project Name", "Create Project");
            if (ib != "")
            {
                if (!Directory.Exists(Program.projectPath + ib))
                    Directory.CreateDirectory(Program.projectPath + ib);
                string newPath = Program.projectPath + ib;
                Program.selectedProject = new Project(0, 0);
                Program.selectedProject.Load(newPath.Split('\\')[newPath.Split('\\').Length - 1]);
                nf.LoadData(Program.selectedProject, true);
                Close();
            }
        }
    }
}
