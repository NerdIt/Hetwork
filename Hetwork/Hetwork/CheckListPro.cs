using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace Hetwork
{
    public partial class CheckListPro : UserControl
    {
        public List<CheckedItemPro> Items = new List<CheckedItemPro>();
        public Timer timer = new Timer();
        bool needRepaint = true;

        private Color elementColor = Color.Gray;



        private int yOffset = 0;

        private int minimumScroll = 0;

        private int elementDistance = 3;

        private int elementHeight = 10;

        private bool useBorder = true;
        private Color borderColor = Color.Black;
        private float borderWidth = 0.5f;


        private int maximumScroll
        {
            get
            {
                return Items.Count * (elementHeight + elementDistance) - Height;
            }
        }

        private int scrollSensitivity = 5;


        public CheckListPro()
        {
            InitializeComponent();

            DoubleBuffered = true;
            timer.Interval = 30;
            timer.Tick += TimerOnTick;
            timer.Start();


            MouseWheel += Control_MouseWheel;
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            if (needRepaint)
            {
                Invalidate();
            }
        }


        public void Paint_Object(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.InterpolationMode = InterpolationMode.Low;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            for (int i = 0; i < Items.Count; i++)
            {
                if(useBorder)
                    g.DrawRectangle(new Pen(borderColor, borderWidth),new Rectangle(new Point(0, (elementHeight + elementDistance) * i - yOffset), new Size(Width - 1, 11)));

                g.FillRectangle(new SolidBrush(elementColor), new Rectangle(new Point(0, (elementHeight + elementDistance) * i - yOffset), new Size(Width, 11)));
            }


            needRepaint = false;
        }

        void Control_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                yOffset -= scrollSensitivity;
            }
            else if (e.Delta < 0)
            {
                yOffset += scrollSensitivity;
            }

            if (yOffset > maximumScroll)
            {
                yOffset = maximumScroll;
            }
            if (yOffset < minimumScroll)
            {
                yOffset = minimumScroll;
            }
            needRepaint = true;


        }











        #region Properties


        [Browsable(true), Category("Check List Pro"), Description("Item background color")]
        public Color ElementColor
        {
            get
            {
                return elementColor;
            }
            set
            {
                elementColor = value;
                needRepaint = true;
            }
        }

        [Browsable(true), Category("Check List Pro"), Description("Distance between each item")]
        public int ElementDistance
        {
            get
            {
                return elementDistance;
            }
            set
            {
                elementDistance = value;
                needRepaint = true;
            }
        }

        [Browsable(true), Category("Check List Pro"), Description("Height of item element")]
        public int ElementHeight
        {
            get
            {
                return elementHeight;
            }
            set
            {
                elementHeight = value;
                needRepaint = true;
            }
        }

        [Browsable(true), Category("Check List Pro"), Description("Amount the list offsets when scrolling")]
        public int ScrollSensitivity
        {
            get
            {
                return scrollSensitivity;
            }
            set
            {
                scrollSensitivity = value;
            }
        }

        [Browsable(true), Category("Check List Pro"), Description("Choose whether to display element borders")]
        public bool UseItemBorders
        {
            get
            {
                return useBorder;
            }
            set
            {
                useBorder = value;
            }
        }

        [Browsable(true), Category("Check List Pro"), Description("Element border rendering color")]
        public Color BorderColor
        {
            get
            {
                return borderColor;
            }
            set
            {
                borderColor = value;
            }
        }

        [Browsable(true), Category("Check List Pro"), Description("Element border rendering width")]
        public float BorderWidth
        {
            get
            {
                return borderWidth;
            }
            set
            {
                borderWidth = value;
            }
        }

        #endregion

    }


    public class CheckedItemPro
    {
        public bool check = false;
        public string name = "";
        public CheckedItemPro(bool checkStatus, string nameDetail)
        {
            check = checkStatus;
            name = nameDetail;
        }
    }
}
