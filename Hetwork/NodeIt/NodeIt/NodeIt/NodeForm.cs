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
                    for(int i = 0; i < node.taskElement.elements.Count; i++)
                    {
                        if(!node.taskElement.elements[i].completed)
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

        private void FileMenu_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //if(e.ClickedItem == ts0)
            //{
            //    var ib = Interaction.InputBox("New Project Name", "Create Project");
            //    if (ib != "")
            //    {
            //        if (!Directory.Exists(Program.projectPath + ib))
            //            Directory.CreateDirectory(Program.projectPath + ib);
            //        string newPath = Program.projectPath + ib;
            //        Program.selectedProject = new Project(0, 0);
            //        Program.selectedProject.Load(newPath.Split('\\')[newPath.Split('\\').Length - 1]);
            //        LoadData(Program.selectedProject, true);

            //    }
            //}
            //else if(e.ClickedItem == ts1)
            //{
            //    ProjectSelectionForm psf = new ProjectSelectionForm(this, false);
            //    psf.ShowDialog();
            //    //Hide();
                
            //}
            //else if (e.ClickedItem == ts2)
            //{
            //    if (currentProject != null)
            //        Serializer.SaveProject(currentProject);
            //}
            //else if (e.ClickedItem == ts3)
            //{
            //    if(currentProject != null)
            //        Serializer.SaveProject(currentProject);
            //    Environment.Exit(0);
            //}
        }

        private void NodeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (currentProject != null)
            //{
            //    Debug.WriteLine(currentProject.zoom);
            //    Serializer.SaveProject(currentProject);
            //}
        }
    }
}
