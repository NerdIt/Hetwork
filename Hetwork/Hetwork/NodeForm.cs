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
            nodeGraph1.nodes.Add(new FolderNode("Enemies", 50, 50, 45, 45, 0, nodeGraph1));
            nodeGraph1.nodes[0].isMain = true;
            nodeGraph1.nodes.Add(new FolderNode("Emulation Passes", 150, 150, 45, 45, 0, nodeGraph1));
            nodeGraph1.nodes.Add(new FolderNode("Player Actions", 250, 250, 45, 45, 0, nodeGraph1));
            nodeGraph1.nodes.Add(new FolderNode("Attack Animations", 350, 350, 45, 45, 0, nodeGraph1));
            nodeGraph1.nodes.Add(new SingularTaskNode("New Node Test", 450, 450, 100, 35, nodeGraph1));
            nodeGraph1.nodes.Add(new SingularTaskNode("Turd Burgler", 450, 450, 100, 35, nodeGraph1));

        }
    }
}
