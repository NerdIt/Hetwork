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

            nodeMenu1.canAdd = false;
            nodeMenu1.Enabled = false;
        }

        public void LoadData(Project p, bool newProj)
        {
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
            nodeMenu1.tasks.Clear();
            nodeMenu1.canAdd = false;
            nodeMenu1.selectedTask = -1;
            nodeMenu1.tb.Text = "";
            nodeMenu1.hoverTask = -1;
            nodeMenu1.Enabled = false;

            if (mainGraph.selectedNode != null)
            {
                selectedNode = mainGraph.selectedNode;
                if(selectedNode.GetType() == Type.GetType("Hetwork.SingularTaskNode"))
                {
                    SingularTaskNode node = selectedNode as SingularTaskNode;
                    nodeMenu1.Enabled = true;
                    nodeMenu1.canAdd = false;
                    nodeMenu1.tb.Text = node.title;
                    nodeMenu1.tasks.Add(node.taskElement);
                }
                else if(selectedNode.GetType() == Type.GetType("Hetwork.ListTaskNode"))
                {
                    ListTaskNode node = selectedNode as ListTaskNode;
                    nodeMenu1.Enabled = true;
                    nodeMenu1.canAdd = true;
                    nodeMenu1.tb.Text = node.title;
                    for(int i = 0; i < node.taskElement.elements.Count; i++)
                    {
                        nodeMenu1.tasks.Add(node.taskElement.elements[i]);
                    }    
                }
            }
            nodeMenu1.Invalidate();
        }


        private void primaryTable_Paint(object sender, PaintEventArgs e)
        {

        }

        private void mainGraph_NodeEdited(object sender, EventArgs e)
        {
            //GraphLog.WriteToLog(this, "Node data updated");
        }
        

    }
}
