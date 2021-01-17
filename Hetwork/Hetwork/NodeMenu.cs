using Hetwork.Properties;
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
        Timer timer = new Timer();
        bool needRepaint = true;
        public List<SingularTask> tasks = new List<SingularTask>();

        int maxOffset
        {
            get
            {
                return 43 * tasks.Count + 24 - Height;
            }
        }

        int offset = 0;

        RichTextBox tb;

        public NodeMenu()
        {
            InitializeComponent();

            DoubleBuffered = true;
            timer.Interval = 30;
            timer.Tick += TimerOnTick;
            timer.Start();

            MouseWheel += Mouse_Scroll;
            tb = new RichTextBox();
            tb.Width = Width - 5;
            tb.Height = 20;
            tb.BorderStyle = BorderStyle.None;
            tb.Multiline = false;
            tb.MouseWheel += Mouse_Scroll;
            Controls.Add(tb);

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

            tb.Location = new Point(1, 1 - offset);
            g.DrawRectangle(new Pen(Color.Black), 0, -offset, Width - 5 + 1, 21);

            for (int i = 0; i < tasks.Count; i++)
            {
                DrawTask(tasks[i], g, i);
            }

           

            needRepaint = false;
        }

        public void DrawTask(SingularTask t, Graphics g, int index)
        {
            g.InterpolationMode = InterpolationMode.Low;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            //  DRAW BG
            Rectangle rect = new Rectangle(1, 43 * index + 24 - offset, Width - 5, 40);
            g.FillRectangle(new SolidBrush(Color.LightGray), rect);
            g.DrawRectangle(new Pen(Color.Black), rect);

            //  DRAW CHECK
            Rectangle checkRect = new Rectangle(3, 43 * index + 26 - offset, 11, 11);
            g.FillRectangle(new SolidBrush(Color.White), checkRect);
            g.DrawRectangle(new Pen(Color.Black), checkRect);
            

        }

        private void NodeMenu_MouseMove(object sender, MouseEventArgs e)
        {
            needRepaint = true;
        }

        void Mouse_Scroll(object sender, MouseEventArgs e)
        {
            if(e.Delta > 0)
            {
                offset -= 5;
            }
            else if (e.Delta < 0)
            {
                offset += 5;
            }

            if (offset < 0)
                offset = 0;
            

            needRepaint = true;
        }
    }
}
