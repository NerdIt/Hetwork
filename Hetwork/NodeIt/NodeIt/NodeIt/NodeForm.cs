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
    public partial class NodeForm : Form
    {
        //private Project currentProject = null;

        public NodeForm()
        {
            InitializeComponent();
        }


        public void LoadProject()
        {
            mainGraph.Enabled = false;
            mainGraph.LoadData();
            mainGraph.Enabled = true;
        }


        NodeVisual selectedNode;

        private void nodeGraph1_Load(object sender, EventArgs e)
        {
            
        }

        private void scaleFont(Label lab)
        {
            Image fakeImage = new Bitmap(1, 1);
            Graphics graphics = Graphics.FromImage(fakeImage);

            SizeF extent = graphics.MeasureString(lab.Text, lab.Font);

            float hRatio = lab.Height / extent.Height;
            float wRatio = lab.Width / extent.Width;
            float ratio = (hRatio < wRatio) ? hRatio : wRatio;

            float newSize = lab.Font.Size * ratio;
            if (newSize <= 0)
                newSize = 0.5f;
            else if (newSize > 50)
            {
                newSize = 50;
            }

            lab.Font = new Font(lab.Font.FontFamily, newSize, lab.Font.Style);
        }

        private void nodeGraph1_NodeSelected(object sender, EventArgs e)
        {
            nodeMenu1.Wipe();
            

            if (mainGraph.selectedNode != null)
            {
                nodeMenu1.selectedNode = mainGraph.selectedNode;
                selectedNode = mainGraph.selectedNode;
                //Debug.WriteLine(selectedNode.id);
                if (selectedNode.GetType() == Type.GetType("NodeIt.SingularTaskNode"))
                {
                    SingularTaskNode node = selectedNode as SingularTaskNode;
                    nodeMenu1.Enabled = true;
                    nodeMenu1.canAdd = false;
                    nodeMenu1.tb.Text = node.title;
                    nodeMenu1.tasks.Add(node.taskElement);
                }
                else if (selectedNode.GetType() == Type.GetType("NodeIt.ListTaskNode"))
                {
                    ListTaskNode node = selectedNode as ListTaskNode;
                    nodeMenu1.Enabled = true;
                    nodeMenu1.canAdd = true;
                    nodeMenu1.tb.Text = node.title;
                    for (int i = 0; i < node.taskElement.elements.Count; i++)
                    {
                        nodeMenu1.tasks.Add(node.taskElement.elements[i]);
                    }
                }
                else if (selectedNode.GetType() == Type.GetType("NodeIt.FolderNode"))
                {
                    FolderNode node = selectedNode as FolderNode;
                    nodeMenu1.Enabled = true;
                    nodeMenu1.canAdd = false;
                    nodeMenu1.tb.Text = node.title;
                }
            }
            nodeMenu1.Invalidate();
        }


        private void primaryTable_Paint(object sender, PaintEventArgs e)
        {

        }

        private void mainGraph_NodeEdited(object sender, EventArgs e)
        {
            
            nodeMenu1.Invalidate();
            //GraphLog.WriteToLog(this, "Node data updated");
        }

        private void MenuUpdated(object sender, EventArgs e)
        {
            
        }

        private void FileMenu_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == ts0)
            {
                var ib = Interaction.InputBox("New Project Name", "Create Project");
                if (ib != "")
                {
                    //if (!Directory.Exists(Program.projectPath + ib))
                    //    Directory.CreateDirectory(Program.projectPath + ib);
                    mainGraph.UpdateSelectedProject();
                    ProjectManager.SaveSelectedProject();
                    ib = ib.Replace("\\", "-").Replace("/", "-").Replace(":", "-").Replace("*", "-").Replace("?", "-").Replace("\"", "-").Replace("<", "-").Replace(">", "-").Replace("|", "-");
                    ProjectManager.CreateProject(ib);
                    Project selectedProject = ProjectManager.GetProjectData(ProjectManager.GetProjectIndexByName(ib));
                    Program.SetSelectedProject(selectedProject);
                    LoadProject();

                }
            }
            else if (e.ClickedItem == ts1)
            {
                //mainGraph.UpdateSelectedProject();
                //ProjectManager.SaveSelectedProject();

                OpenProject();
            }
            else if (e.ClickedItem == ts2)
            {
                mainGraph.UpdateSelectedProject();
                ProjectManager.SaveSelectedProject();
            }
            else if (e.ClickedItem == ts3)
            {
                Environment.Exit(0);
            }
        }

        private void NodeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveProject();
        }

        private void mainGraph_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.S && ModifierKeys == Keys.Control)
            {
                SaveProject();
            }
            else if (e.KeyCode == Keys.O && ModifierKeys == Keys.Control)
            {
                OpenProject();
            }
            else if (e.KeyCode == Keys.N && ModifierKeys == Keys.Control)
            {
                NewProject();
            }
        }

        private void nodeMenu1_MenuKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && ModifierKeys == Keys.Control)
            {
                SaveProject();
            }
            else if (e.KeyCode == Keys.O && ModifierKeys == Keys.Control)
            {
                OpenProject();
            }
            else if (e.KeyCode == Keys.N && ModifierKeys == Keys.Control)
            {
                NewProject();
            }
        }

        void SaveProject()
        {
            mainGraph.UpdateSelectedProject();
            ProjectManager.SaveSelectedProject();
            Debug.WriteLine("SAVING");
        }

        void OpenProject()
        {
            mainGraph.UpdateSelectedProject();
            ProjectManager.SaveSelectedProject();

            ProjectSelectionForm psf = new ProjectSelectionForm();
            psf.ShowInTaskbar = false;
            DialogResult dr = psf.ShowDialog();
            if (dr == DialogResult.OK)
            {
                LoadProject();
            }
            else if (dr == DialogResult.Cancel)
            {

            }
        }

        void NewProject()
        {
            var ib = Interaction.InputBox("New Project Name", "Create Project");
            if (ib != "")
            {
                //if (!Directory.Exists(Program.projectPath + ib))
                //    Directory.CreateDirectory(Program.projectPath + ib);
                mainGraph.UpdateSelectedProject();
                ProjectManager.SaveSelectedProject();
                ib = ib.Replace("\\", "-").Replace("/", "-").Replace(":", "-").Replace("*", "-").Replace("?", "-").Replace("\"", "-").Replace("<", "-").Replace(">", "-").Replace("|", "-");
                ProjectManager.CreateProject(ib);
                Project selectedProject = ProjectManager.GetProjectData(ProjectManager.GetProjectIndexByName(ib));
                Program.SetSelectedProject(selectedProject);
                LoadProject();

            }
        }

        private void nodeMenu1_ControlUpdated(object sender, EventArgs e)
        {
            if (mainGraph.selectedNode != null)
            {
                if (mainGraph.selectedNode.GetType() == Type.GetType("NodeIt.SingularTaskNode"))
                {
                    SingularTaskNode node = mainGraph.selectedNode as SingularTaskNode;
                    node.title = nodeMenu1.tb.Text;
                    mainGraph.RecalculateNodePercentages();
                    mainGraph.Invalidate();
                }
                else if (mainGraph.selectedNode.GetType() == Type.GetType("NodeIt.ListTaskNode"))
                {
                    ListTaskNode node = mainGraph.selectedNode as ListTaskNode;
                    node.title = nodeMenu1.tb.Text;
                    bool completed = true;
                    for (int i = 0; i < node.taskElement.elements.Count; i++)
                    {
                        if (!node.taskElement.elements[i].completed)
                        {
                            completed = false;
                            break;
                        }
                    }
                    node.taskElement.completed = completed;
                    mainGraph.RecalculateNodePercentages();
                    mainGraph.Invalidate();
                }
                else if (mainGraph.selectedNode.GetType() == Type.GetType("NodeIt.FolderNode"))
                {
                    FolderNode node = mainGraph.selectedNode as FolderNode;
                    if (!node.isMain)
                    {
                        node.title = nodeMenu1.tb.Text;
                    }
                    mainGraph.Invalidate();
                }
            }
        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://1saacdev.github.io/NodeIt/Help.html");
        }
    }
}
