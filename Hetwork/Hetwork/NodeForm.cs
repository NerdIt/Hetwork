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
        public NodeForm()
        {
            InitializeComponent();
        }

        private void nodeGraph1_Load(object sender, EventArgs e)
        {
            mainGraph.nodes.Add(new FolderNode("Main", 50, 50, 45, 45, 0, mainGraph));
            mainGraph.nodes[0].isMain = true;


            
        }

        private void nodeGraph1_NodeSelected(object sender, EventArgs e)
        {
            if (mainGraph.selectedNode != null)
            {
                nodeTitleLabel.Text = mainGraph.selectedNode.title;
                if (mainGraph.selectedNode.GetType() == Type.GetType("Hetwork.SingularTaskNode"))
                {
                    try
                    {
                        SetDisplayType((mainGraph.selectedNode as SingularTaskNode).taskElement);
                        (contentDisplayPanel.Controls[0] as RichTextBox).Text = (mainGraph.selectedNode as SingularTaskNode).taskElement.taskContent;
                    }
                    catch
                    {
                        
                    }
                    
                }
                else if (mainGraph.selectedNode.GetType() == Type.GetType("Hetwork.ListTaskNode"))
                {
                    try
                    {
                        SetDisplayType((mainGraph.selectedNode as ListTaskNode).taskElement);
                        List<CheckedItemPro> items = new List<CheckedItemPro>();
                        ListTask lt = (mainGraph.selectedNode as ListTaskNode).taskElement;
                        for(int i = 0; i < lt.elements.Count; i++)
                        {
                            items.Add(new CheckedItemPro(lt.elements[i].completed, lt.elements[i].taskContent));
                        }

                        (contentDisplayPanel.Controls[0] as CheckListPro).Items.AddRange(items);
                    }
                    catch
                    {

                    }
                }
                else
                {
                    contentDisplayPanel.Controls.Clear();
                }
                

            }
            
        }

        private void nodeTitleLabel_TextChanged(object sender, EventArgs e)
        {
            scaleFont(sender as Label);
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

            lab.Font = new Font(lab.Font.FontFamily, newSize, lab.Font.Style);
        }


        public void SetDisplayType(object task)
        {
            if(task.GetType() == Type.GetType("Hetwork.SingularTask"))
            {
                if (contentDisplayPanel.Controls.Count > 0)
                    contentDisplayPanel.Controls.Clear();
                contentDisplayPanel.Controls.Add(TextContent());
            }
            else if(task.GetType() == Type.GetType("Hetwork.ListTask"))
            {
                if (contentDisplayPanel.Controls.Count > 0)
                    contentDisplayPanel.Controls.Clear();
                contentDisplayPanel.Controls.Add(ListContent());
            }
        }

        public RichTextBox TextContent()
        {
            RichTextBox rtb = new RichTextBox();

            rtb.Dock = DockStyle.Fill;
            rtb.BackColor = Color.FromArgb(255, 230, 230, 230);
            rtb.BorderStyle = BorderStyle.None;

            return rtb;
        }

        public CheckListPro ListContent()
        {
            CheckListPro clp = new CheckListPro();

            clp.Dock = DockStyle.Fill;
            clp.BackColor = Color.FromArgb(255, 230, 230, 230);
            clp.BorderStyle = BorderStyle.None;
            clp.ElementColor = Color.FromArgb(255, 230, 230, 230);
            clp.UseItemBorders = false;

            return clp;
        }

    }
}
