using Hetwork.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hetwork
{
    public partial class NodeMenu : UserControl
    {
        Timer timer = new Timer();
        Timer scrolltimer = new Timer();

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
        RichTextBox ectb;
        RichTextBox ettb;

        public Font titleFont = new Font("Arial", 7, FontStyle.Bold);
        public Font contentFont = new Font("Arial", 7);

        int fontHeight = 1;

        public NodeMenu()
        {
            InitializeComponent();

            DoubleBuffered = true;
            timer.Interval = 30;
            timer.Tick += TimerOnTick;
            timer.Start();

            scrolltimer.Interval = 30;
            scrolltimer.Tick += ScrollTick;
            scrolltimer.Start();



            MouseWheel += Mouse_Scroll;
            tb = new RichTextBox();
            
            tb.Height = 20;
            tb.BorderStyle = BorderStyle.None;
            tb.Multiline = false;
            tb.MouseWheel += Mouse_Scroll;
            Controls.Add(tb);

            ectb = new RichTextBox();
            ectb.Height = 80;
            ectb.BorderStyle = BorderStyle.None;
            ectb.ScrollBars = RichTextBoxScrollBars.Vertical;
            Controls.Add(ectb);

            ettb = new RichTextBox();
            ettb.Height = 20;
            ettb.BorderStyle = BorderStyle.None;
            ettb.Multiline = false;
            Controls.Add(ettb);

        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            if (needRepaint)
            { 
                Invalidate();
            }
        }
        float scrollAmount;
        int scrollCount = 0;
        private void ScrollTick(object sender, EventArgs eventArgs)
        {
            if(scrollAmount != 0)
            {
                if(scrollAmount > 0)
                {
                    offset += 5 * scrollCount * 2;
                    scrollAmount -= 5;
                }
                else if(scrollAmount < 0)
                {
                    offset -= 5 * scrollCount * 2;
                    scrollAmount += 5;
                }
                scrollCount--;

                if (offset < 0)
                    offset = 0;
                needRepaint = true;
            }
        }



        public int selectedTask = -1;


        public void Draw(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            g.InterpolationMode = InterpolationMode.Low;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            fontHeight = (int)g.MeasureString("|", titleFont).Height;

            //  TITLE BOX
            tb.Location = new Point(2, 2 - offset);
            tb.Width = Width - 6;
            g.DrawRectangle(new Pen(Color.Black), 1, 1 - offset, Width - 5, 21);

            


            for (int i = 0; i < tasks.Count; i++)
            {
                DrawTask(tasks[i], g, i);
            }

            Rectangle addRect = new Rectangle(1, ((int)g.MeasureString("|", titleFont).Height + 20 + 3) * tasks.Count + 25 - offset, Width - 5, 40);
            if (newTaskBtnHover)
            {
                g.FillRectangle(new SolidBrush(Color.LightGray), addRect);
            }

            g.DrawRectangle(new Pen(Color.Black), addRect);
            

            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            g.DrawString("+", new Font("Arial", 15, FontStyle.Bold), new SolidBrush(Color.Gray), new Point(1 + addRect.Width / 2, ((int)g.MeasureString("|", titleFont).Height + 20 + 3) * tasks.Count + 25 - offset + addRect.Height / 2), sf);

            GC.Collect();
            needRepaint = false;

            if (selectedTask != -1)
            {
                //  ITEM CONTENT BOX
                ectb.Visible = true;
                ectb.Enabled = true;
                ectb.Location = new Point(2, Height - ectb.Height - 4);
                ectb.Width = Width - 6;
                g.DrawRectangle(new Pen(Color.Black), 1, Height - ectb.Height - 5, Width - 5, ectb.Height + 1);

                ettb.Visible = true;
                ettb.Enabled = true;
                ettb.Location = new Point(2, Height - ectb.Height - ettb.Height - 8);
                ettb.Width = Width - 6;
                g.DrawRectangle(new Pen(Color.Black), 1, Height - ectb.Height - ettb.Height - 9, Width - 5, ettb.Height + 1);
            }
            else
            {
                ectb.Visible = false;
                ectb.Enabled = false;

                ettb.Visible = false;
                ettb.Enabled = false;
            }
        }


        public void DrawTask(SingularTask t, Graphics g, int index)
        {
            g.InterpolationMode = InterpolationMode.Low;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            //int contentHeight = (int)g.MeasureString(t.taskContent, contentFont).Height;
            

            //  DRAW BG
            Rectangle rect = new Rectangle(1, ((int)g.MeasureString("|", titleFont).Height + 20 + 3) * index + 25 - offset, Width - 5, (int)g.MeasureString("|", titleFont).Height + 20);
            if (index != hoverTask && index != selectedTask)
                g.FillRectangle(new SolidBrush(Color.LightGray), rect);
            else if (index != selectedTask)
                g.FillRectangle(new SolidBrush(Color.Gray), rect);
            else
                g.FillRectangle(new SolidBrush(Color.LightBlue), rect);
            g.DrawRectangle(new Pen(Color.Black), rect);

            //  DRAW CHECK
            Rectangle checkRect = new Rectangle(3, ((int)g.MeasureString("|", titleFont).Height + 20 + 3) * index + 27 - offset, (int)g.MeasureString("|", titleFont).Height, (int)g.MeasureString("|", titleFont).Height);
            g.FillRectangle(new SolidBrush(Color.White), checkRect);
            g.DrawRectangle(new Pen(Color.Black), checkRect);

            //  DRAW TITLE
            string titleText = "";
            SizeF stringSize = g.MeasureString(titleText, titleFont);
            int titleIndex = 0;
            while(stringSize.Width + 9 < Width - 5 - (checkRect.X + checkRect.Width + 4) && titleIndex < t.taskTitle.Length)
            {
                titleText += t.taskTitle[titleIndex];
                stringSize = g.MeasureString(titleText, titleFont);
                titleIndex++;
            }
            if(stringSize.Width + 9 >= Width - 5 - (checkRect.X + checkRect.Width + 4) && !titleText.Equals(t.taskTitle))
                titleText += "..";

            g.DrawString(titleText, titleFont, new SolidBrush(Color.Black), new Point(checkRect.X + checkRect.Width + 4, checkRect.Y));

            //  DRAW CONTENT
            string contentText = "";
            stringSize = g.MeasureString(titleText, titleFont);
            int contentIndex = 0;
            while (stringSize.Width + 9 < Width - 5 - (checkRect.X) && contentIndex < t.taskContent.Length)
            {
                contentText += t.taskContent[contentIndex];
                stringSize = g.MeasureString(contentText, titleFont);
                contentIndex++;
            }
            if (stringSize.Width + 9 >= Width - 5 - (checkRect.X) && !contentText.Equals(t.taskContent))
                contentText += "..";
            g.DrawString(contentText, contentFont, new SolidBrush(Color.Black), new Point(checkRect.X, checkRect.Y + checkRect.Height + 5));

        }

        public int hoverTask = -1;
        public bool newTaskBtnHover = false;

        private void NodeMenu_MouseMove(object sender, MouseEventArgs e)
        {
            newTaskBtnHover = false;
            bool foundHovertask = false;
            for (int i = 0; i < tasks.Count; i++)
            {
                Rectangle rect = new Rectangle(1, (fontHeight + 20 + 3) * i + 25 - offset, Width - 5, fontHeight + 20);
                if(rect.Contains(e.Location))
                {
                    hoverTask = i;
                    foundHovertask = true;
                    break;
                }
            }
            if (!foundHovertask)
            {
                hoverTask = -1;
                if(new Rectangle(1, ((int)fontHeight + 20 + 3) * tasks.Count + 25 - offset, Width - 5, 40).Contains(e.Location))
                {
                    newTaskBtnHover = true;
                }

            }


            needRepaint = true;
        }

        
        void Mouse_Scroll(object sender, MouseEventArgs e)
        {
            //Debug.WriteLine($"{e.Delta}");

            scrollCount++;
            if (e.Delta > 0)
            {
                scrollAmount -= 5;
            }
            else if (e.Delta < 0)
            {
                scrollAmount += 5;
            }


            
            

            needRepaint = true;
        }

        private void NodeMenu_Resize(object sender, EventArgs e)
        {
            Invalidate();
            needRepaint = true;
        }

        private void NodeMenu_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                bool foundHovertask = false;
                for (int i = 0; i < tasks.Count; i++)
                {
                    Rectangle rect = new Rectangle(1, (fontHeight + 20 + 3) * i + 25 - offset, Width - 5, fontHeight + 20);
                    if (rect.Contains(e.Location))
                    {
                        selectedTask = i;
                        foundHovertask = true;
                        break;
                    }
                }
                if (!foundHovertask)
                {
                    selectedTask = -1;
                    if (new Rectangle(1, ((int)fontHeight + 20 + 3) * tasks.Count + 25 - offset, Width - 5, 40).Contains(e.Location))
                    {
                        tasks.Add(new SingularTask("New Item", "", 0));
                    }
                    
                }
            }
        }
    }
}
