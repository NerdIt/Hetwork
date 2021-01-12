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
            if(mainGraph.selectedNode != null)
                nodeTitleLabel.Text = mainGraph.selectedNode.title;
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
                contentDisplayPanel.Controls
            }
            else if(task.GetType() == Type.GetType("Hetwork.ListTask"))
            {

            }
        }

        public RichTextBox TextContent()
        {
            
        }

    }
}
