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
                        for (int i = 0; i < lt.elements.Count; i++)
                        {
                            items.Add(new CheckedItemPro(lt.elements[i].completed, lt.elements[i].taskTitle, lt.elements[i].taskContent));
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
            else if (mainGraph.selectedNode == null)
            {
                nodeTitleLabel.Text = "";
                contentDisplayPanel.Controls.Clear();
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
            if (newSize <= 0)
                newSize = 0.5f;
            else if (newSize > 50)
            {
                newSize = 50;
            }

            lab.Font = new Font(lab.Font.FontFamily, newSize, lab.Font.Style);
        }


        public void SetDisplayType(object task)
        {
            if (task.GetType() == Type.GetType("Hetwork.SingularTask"))
            {
                if (contentDisplayPanel.Controls.Count > 0)
                    contentDisplayPanel.Controls.Clear();
                contentDisplayPanel.Controls.Add(TextContent());
            }
            else if (task.GetType() == Type.GetType("Hetwork.ListTask"))
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
            rtb.MouseDown += RichTextMouseDown;
            rtb.MouseUp += RichTextMouseUp;
            rtb.TextChanged += RichTextTextChange;
            rtb.Font = new Font("Courier New", 8);

            return rtb;
        }

        public CheckListPro ListContent()
        {
            CheckListPro clp = new CheckListPro(this);

            clp.Dock = DockStyle.Fill;
            clp.BackColor = Color.FromArgb(255, 230, 230, 230);
            clp.BorderStyle = BorderStyle.None;
            clp.ElementColor = Color.FromArgb(255, 230, 230, 230);
            clp.UseItemBorders = false;
            clp.MouseDown += ChecklistMouseDown;
            clp.MouseUp += ChecklistMouseUp;
            clp.MouseDoubleClick += ChecklistDoubleClick;
            clp.TextFont = new Font("Courier New", 8);
            clp.KeyDown += CheckListKeyDown;

            return clp;
        }

        public void RichTextMouseDown(object sender, MouseEventArgs e)
        {

            if (mainGraph.selectedNode != null)
            {
                UpdateNodeValue(mainGraph.selectedNode, sender);
            }
        }

        public void RichTextTextChange(object sender, EventArgs e)
        {
            if (mainGraph.selectedNode != null)
            {
                UpdateNodeValue(mainGraph.selectedNode, sender);
            }
        }
        public void RichTextMouseUp(object sender, MouseEventArgs e)
        {
            if (mainGraph.selectedNode != null)
            {
                UpdateNodeValue(mainGraph.selectedNode, sender);
            }
        }

        public void ChecklistMouseDown(object sender, MouseEventArgs e)
        {
            if (mainGraph.selectedNode != null)
            {
                UpdateNodeValue(mainGraph.selectedNode, sender);
            }
        }

        public void CheckListKeyDown(object sender, KeyEventArgs e)
        {
            if(mainGraph.selectedNode != null)
            {
                UpdateNodeValue(mainGraph.selectedNode, sender);
            }
        }

        private EditorForm elementEditor = null;

        public void ChecklistDoubleClick(object sender, MouseEventArgs e)
        {
            if ((sender as CheckListPro).selectetedItem != -1)
            {
                if (elementEditor == null || elementEditor.Text != "Editor")
                {
                    elementEditor = new EditorForm(this, mainGraph.selectedNode, sender as CheckListPro);
                    elementEditor.StartPosition = FormStartPosition.CenterScreen;
                    elementEditor.Show();
                }
                else
                {
                    elementEditor.Close();
                    elementEditor = new EditorForm(this, mainGraph.selectedNode, sender as CheckListPro);
                    elementEditor.StartPosition = FormStartPosition.CenterScreen;
                    elementEditor.Show();
                }

            }
        }

        public void ChecklistMouseUp(object sender, MouseEventArgs e)
        {
            if (mainGraph.selectedNode != null)
            {
                UpdateNodeValue(mainGraph.selectedNode, sender);
            }
        }

        public void UpdateNodeValue(NodeVisual node, object contentDisplay)
        {
            mainGraph.recalculatePercentage = true;
            if (contentDisplay.GetType() == Type.GetType("Hetwork.CheckListPro"))
            {
                
                CheckListPro c = contentDisplay as CheckListPro;
                ListTaskNode lt = node as ListTaskNode;
                lt.taskElement.elements.Clear();
                foreach(CheckedItemPro item in c.Items)
                {
                    lt.taskElement.elements.Add(new SingularTask(item.name, item.details, item.check));
                }
            }
            else if(contentDisplay.GetType().ToString().Contains("RichTextBox"))
            {
                RichTextBox r = contentDisplay as RichTextBox;
                SingularTaskNode st = node as SingularTaskNode;
                st.taskElement.taskContent = r.Text;
            }
        }

        private void primaryTable_Paint(object sender, PaintEventArgs e)
        {

        }

        private void mainGraph_NodeEdited(object sender, EventArgs e)
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
                        for (int i = 0; i < lt.elements.Count; i++)
                        {
                            items.Add(new CheckedItemPro(lt.elements[i].completed, lt.elements[i].taskTitle, lt.elements[i].taskContent));
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
            else if (mainGraph.selectedNode == null)
            {
                nodeTitleLabel.Text = "";
                contentDisplayPanel.Controls.Clear();
            }
        }
    }
}
