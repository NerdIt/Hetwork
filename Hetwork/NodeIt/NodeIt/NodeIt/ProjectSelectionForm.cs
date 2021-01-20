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

namespace NodeIt
{
    public partial class ProjectSelectionForm : Form
    {
        List<string> projs;
        public ProjectSelectionForm()
        {
            InitializeComponent();

            openBtn.Enabled = false;

            LoadProjects();
        }

        void LoadProjects()
        {
            projs = ProjectManager.projects;
            for (int i = 0; i < projs.Count; i++)
            {
                projectPanel.Items.Add(projs[i].Split('\\')[projs[i].Split('\\').Length - 1]);
            }
        }

        private void openBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Project selectedProject = ProjectManager.GetProjectData(projectPanel.SelectedIndex);
            Program.SetSelectedProject(selectedProject);
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
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void newBtn_Click(object sender, EventArgs e)
        {
            //this.DialogResult = DialogResult.OK;
            var ib = Interaction.InputBox("New Project Name", "Create Project");
            if (ib != "")
            {
                ib = ib.Replace("\\", "-").Replace("/", "-").Replace(":", "-").Replace("*", "-").Replace("?", "-").Replace("\"", "-").Replace("<", "-").Replace(">", "-").Replace("|", "-");
                ProjectManager.CreateProject(ib);  
            }
            LoadProjects();
            //Close();
        }

        private void ProjectSelectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
