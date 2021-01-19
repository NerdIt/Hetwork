using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hetwork
{
    public partial class NodeForm : Form
    {
        private Project currentProject = null;

        public NodeForm()
        {
            InitializeComponent();

            (nodeFormMenu.Items[0] as ToolStripDropDownButton).ShowDropDownArrow = false;

            nodeMenu1.canAdd = false;
            nodeMenu1.Enabled = false;
            nodeMenu1.ControlUpdated += MenuUpdated;
        }

        public void LoadData(Project p, bool newProj)
        {
            mainGraph.nodes.Clear();
            nodeMenu1.Wipe();
            nodeMenu1.Enabled = false;
            currentProject = p;
            mainGraph.graphProject = p;
            if (newProj)
            {
                mainGraph.InitGraph();
            }
            else
            {
                Serializer.LoadProject(currentProject, mainGraph);
                mainGraph.nodes = p.nodes;
                mainGraph.graphOffset = p.offset;
                mainGraph.zoomFactor = p.zoom;

                GraphLog.WriteToLog(this, "Load and Project data paired");
            }
            mainGraph.recalculatePercentage = true;
            nodeMenu1.Enabled = true;
            mainGraph.Invalidate();
            //mainGraph.InitGraph();
        }

        NodeVisual selectedNode;

        private void nodeGraph1_Load(object sender, EventArgs e)
        {
            
        }

        

        

        
        private void scaleFont(Label lab)
        {
            Image fakeImage = new Bitmap(1, 1); //As we cannot use CreateGraphics() in a class library, so the fake image is used to load the Graphics.
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
                if (selectedNode.GetType() == Type.GetType("Hetwork.SingularTaskNode"))
                {
                    SingularTaskNode node = selectedNode as SingularTaskNode;
                    nodeMenu1.Enabled = true;
                    nodeMenu1.canAdd = false;
                    nodeMenu1.tb.Text = node.title;
                    nodeMenu1.tasks.Add(node.taskElement);
                }
                else if (selectedNode.GetType() == Type.GetType("Hetwork.ListTaskNode"))
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
                else if (selectedNode.GetType() == Type.GetType("Hetwork.FolderNode"))
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
                if (mainGraph.selectedNode.GetType() == Type.GetType("Hetwork.SingularTaskNode"))
                {
                    SingularTaskNode node = mainGraph.selectedNode as SingularTaskNode;
                    node.title = nodeMenu1.tb.Text;
                    mainGraph.recalculatePercentage = true;
                    mainGraph.Invalidate();
                }
                else if (mainGraph.selectedNode.GetType() == Type.GetType("Hetwork.ListTaskNode"))
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
                    mainGraph.recalculatePercentage = true;
                    mainGraph.Invalidate();
                }
                else if (mainGraph.selectedNode.GetType() == Type.GetType("Hetwork.FolderNode"))
                {
                    FolderNode node = mainGraph.selectedNode as FolderNode;
                    node.title = nodeMenu1.tb.Text;
                    mainGraph.Invalidate();
                }
            }
        }

        private void FileMenu_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(e.ClickedItem == ts0)
            {

            }
            else if(e.ClickedItem == ts1)
            {
                ProjectSelectionForm psf = new ProjectSelectionForm(this);
                psf.ShowDialog();
                //Hide();
                
            }
            else if (e.ClickedItem == ts2)
            {
                if (currentProject != null)
                    Serializer.SaveProject(currentProject);
            }
            else if (e.ClickedItem == ts3)
            {
                if(currentProject != null)
                    Serializer.SaveProject(currentProject);
                Environment.Exit(0);
            }
        }

        private void NodeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (currentProject != null)
                Serializer.SaveProject(currentProject);
        }
    }
}
