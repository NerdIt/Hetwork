using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hetwork
{
    public partial class NodeMenu : UserControl
    {
        public Timer timer = new Timer();
        bool needRepaint = true;
        bool contentPanelRepaint = true;

        public NodeMenu()
        {
            InitializeComponent();

            DoubleBuffered = true;
            timer.Interval = 30;
            timer.Tick += TimerOnTick;
            timer.Start();

            contentPanel.BackColor = BackColor;
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            

            if (needRepaint)
            {

                Invalidate();

            }
        }

        public void Draw(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.InterpolationMode = InterpolationMode.Low;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            needRepaint = false;
        }

        public bool DrawText = true;

        public void DrawContentBox(object sender, PaintEventArgs e)
        {
            if(contentPanelRepaint)
            {
                contentPanel.Invalidate();
            }    

            Graphics g = e.Graphics;
            g.InterpolationMode = InterpolationMode.Low;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if(DrawText)
            {
                int controlWidth = contentPanel.Width - 3;
                int controlHeight = contentPanel.Height - 3;
                //  BORDER
                Rectangle borderSize = new Rectangle(0, 0, controlWidth, controlHeight);
                g.DrawRectangle(new Pen(Color.Red, 1), borderSize);

            }
            else
            {

            }

            contentPanelRepaint = false;

        }

        private void NodeMenu_Resize(object sender, EventArgs e)
        {
            needRepaint = true;
            contentPanelRepaint = true;

        }


    }
}
