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
            nodeGraph1.nodes.Add(new FolderNode("Node 1", 50, 50, 35, 35, 0, nodeGraph1));
            nodeGraph1.nodes.Add(new FolderNode("Node 2", 150, 150, 35, 35, 0, nodeGraph1));
            nodeGraph1.nodes.Add(new FolderNode("Node 3", 250, 250, 35, 35, 0, nodeGraph1));
            nodeGraph1.nodes.Add(new FolderNode("Node 4", 350, 350, 35, 35, 0, nodeGraph1));
            nodeGraph1.connections.Add(new NodeConnection(nodeGraph1.nodes[0], nodeGraph1.nodes[1], nodeGraph1));
        }
    }
}
