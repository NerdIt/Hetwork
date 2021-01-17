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
    public partial class VisualDesign : Form
    {
        public VisualDesign()
        {
            InitializeComponent();
            nodeMenu1.tasks.Add(new SingularTask("Debug Task", "DEBUG TEXT", 0));
            nodeMenu1.tasks.Add(new SingularTask("Debug Task 2", "1 2 3 4 5 6 7 8 9 0", 0));
            nodeMenu1.tasks.Add(new SingularTask("Debug Task 3", "a b c d e f g h i j k l m n o p q r s t u v w x y z", 0));
            nodeMenu1.tasks.Add(new SingularTask("Debug Task 4", "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z", 0));
        }

        private void tableLayoutPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }
    }
}
