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
    public partial class EditorForm : Form
    {
        private NodeForm parentForm;
        private NodeVisual node;
        private CheckListPro CheckList;
        public EditorForm(NodeForm parent, NodeVisual n, CheckListPro clp)
        {
            InitializeComponent();

            parentForm = parent;
            node = n;
            CheckList = clp;

            titleTextBox.Text = CheckList.Items[CheckList.selectetedItem].name;
            contentBox.Text = CheckList.Items[CheckList.selectetedItem].details;
        }

        private void applyBtn_Click(object sender, EventArgs e)
        {
            CheckList.Items[CheckList.selectetedItem].name = titleTextBox.Text;
            CheckList.Items[CheckList.selectetedItem].details = contentBox.Text;
            //parentForm.UpdateNodeValue(node, CheckList);
            CheckList.Invalidate();
            Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
