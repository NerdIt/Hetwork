using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            nodeGraph1.nodes.Add(new FolderNode(50, 50, 50, 50, 0, nodeGraph1));
            nodeGraph1.nodes.Add(new FolderNode(150, 150, 50, 50, 0, nodeGraph1));
            nodeGraph1.nodes.Add(new FolderNode(250, 250, 50, 50, 0, nodeGraph1));
            nodeGraph1.nodes.Add(new FolderNode(350, 350, 50, 50, 0, nodeGraph1));
            nodeGraph1.connections.Add(new NodeConnection(new Point(15, 30), new Point(150, 120), nodeGraph1));
        }
    }
}
