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
    public partial class CHECKLISTPRO_FORMTEST : Form
    {
        public CHECKLISTPRO_FORMTEST()
        {
            InitializeComponent();

            checkListPro1.Items.Add(new CheckedItemPro(false, "debug 1 abcdefghijklmnopqrstuvwxyz"));
            checkListPro1.Items.Add(new CheckedItemPro(true, "debug 2 abcdefghijklmnopqrstuvwxyz"));
            checkListPro1.Items.Add(new CheckedItemPro(true, "debug 1 abcdefghijklmnopqrstuvwxyz"));
            checkListPro1.Items.Add(new CheckedItemPro(false, "debug 2 abcdefghijklmnopqrstuvwxyz"));
            checkListPro1.Items.Add(new CheckedItemPro(false, "debug 1"));
            checkListPro1.Items.Add(new CheckedItemPro(false, "debug 2"));
            checkListPro1.Items.Add(new CheckedItemPro(false, "debug 1"));
            checkListPro1.Items.Add(new CheckedItemPro(false, "debug 2 abcdefghijklmnopqrstuvwxyz"));
            checkListPro1.Items.Add(new CheckedItemPro(true, "debug 1"));
            checkListPro1.Items.Add(new CheckedItemPro(false, "debug 2"));
            checkListPro1.Items.Add(new CheckedItemPro(false, "debug 1"));
            checkListPro1.Items.Add(new CheckedItemPro(false, "debug 2"));
            checkListPro1.Items.Add(new CheckedItemPro(false, "debug 1"));
            checkListPro1.Items.Add(new CheckedItemPro(false, "debug 2 abcdefghijklmnopqrstuvwxyz"));
        }
    }
}
