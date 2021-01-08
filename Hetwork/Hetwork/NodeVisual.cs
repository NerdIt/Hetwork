using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hetwork
{
    public class NodeVisual
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public string title;


        public bool isHover = false;
        public bool isHoverArea = false;
        public bool isDragging = false;
        public bool isSelected = false;

        public Point offsetToCursor = new Point();

        public virtual void DrawShadow(Graphics g)
        {

        }

        public virtual void Draw(Graphics g)
        {
            
        }


        public virtual bool IsWithinCircle(Point center, Point mouse, double radius)
        {
            return false;
        }

        public virtual bool IsWithinRect(Rectangle rect, Point mouse)
        {
            return false;
        }
    }

    public class FolderNode : NodeVisual
    {
        public int percentage;
        public NodeGraph nodeGraph;
        
        

        public FolderNode(string name, int x, int y, int width, int height, int perc, NodeGraph graphControl)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            percentage = perc;
            nodeGraph = graphControl;
            title = name;
        }

        public override void DrawShadow(Graphics g)
        {
            g.InterpolationMode = InterpolationMode.Low;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            float zoom = nodeGraph.graphZoom;
            Point offset = nodeGraph.graphOffset;
            g.FillEllipse(new SolidBrush(Color.FromArgb(100, 0, 0, 0)), new Rectangle(new Point((int)((X + 2 - Width / 2 + offset.X) * zoom), (int)((Y + 2 - Height / 2 + offset.Y) * zoom)), new Size((int)(Width * zoom), (int)(Height * zoom))));
        }

        public override void Draw(Graphics g)
        {
            g.InterpolationMode = InterpolationMode.Low;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Point offset = nodeGraph.graphOffset;
            float zoom = nodeGraph.graphZoom;

            //  CREATE GRADIANT
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse((int)((X - Width / 2 + offset.X) * zoom), (int)((Y - Height / 2 + offset.Y) * zoom), (int)(Width * zoom), (int)(Height * zoom));
            PathGradientBrush pthGrBrush = new PathGradientBrush(path);
            pthGrBrush.CenterColor = Color.FromArgb(255, 255, 255, 255);
            Color[] colors = { Color.FromArgb(255, 240, 240, 240) };
            pthGrBrush.SurroundColors = colors;

            //  DRAW BASE FILLED CIRCLE
            g.FillEllipse(pthGrBrush, (int)((X - Width / 2 + offset.X) * zoom), (int)((Y - Height / 2 + offset.Y) * zoom), (int)(Width * zoom), (int)(Height * zoom));

            //  DRAW BORDER
            g.DrawEllipse(new Pen(Color.FromArgb(255,215,215,215)), new Rectangle(new Point((int)((X - Width / 2 + offset.X) * zoom), (int)((Y - Height / 2 + offset.Y) * zoom)), new Size((int)(Width * zoom), (int)(Height * zoom))));


            //  DRAW PERCENTAGE TEXT
            string percText = percentage + "%";
            Font stringFont = new Font("Arial", 7 * zoom, FontStyle.Bold);
            SizeF stringSize = new SizeF();
            while(stringSize.Width > 45 && stringFont.Size > 1)
            {
                stringSize = g.MeasureString(percText, stringFont);
                stringFont = new Font("Arial", stringFont.Size - 1);
            }
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            g.DrawString(percText, stringFont, new SolidBrush(Color.FromArgb(255, 195, 195, 195)), new PointF((int)((X + offset.X) * zoom), (int)((Y + 10 + offset.Y) * zoom)), sf);

            //  DRAW FULL NAME AND BORDER ON HOVER AREA
            if(isHover && !isSelected)
            {
                g.DrawEllipse(new Pen(Color.LightGray, 2 * zoom), (int)((X - Width / 2 + offset.X) * zoom), (int)((Y - Height / 2 + offset.Y) * zoom), (int)(Width * zoom), (int)(Height * zoom));

                string nameText = title;
                Font titleFont = new Font("Arial", 7 * zoom, FontStyle.Bold);
                stringSize = new SizeF();
                while (stringSize.Width > 60 && titleFont.Size > 1)
                {
                    stringSize = g.MeasureString(nameText, titleFont);
                    titleFont = new Font("Arial", titleFont.Size - 1);
                }
                sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;
                g.DrawString(nameText, titleFont, new SolidBrush(Color.FromArgb(190, 195, 195, 195)), new PointF((int)((X + offset.X) * zoom), (int)((Y - 22 + offset.Y) * zoom)), sf);
            }

            if(isSelected)
            {
                g.DrawEllipse(new Pen(Color.FromArgb(255, 63, 63, 63), 2 * zoom), (int)((X - Width / 2 + offset.X) * zoom), (int)((Y - Height / 2 + offset.Y) * zoom), (int)(Width * zoom), (int)(Height * zoom));

                string nameText = title;
                Font titleFont = new Font("Arial", 7 * zoom, FontStyle.Bold);
                stringSize = new SizeF();
                while (stringSize.Width > 60 && titleFont.Size > 1)
                {
                    stringSize = g.MeasureString(nameText, titleFont);
                    titleFont = new Font("Arial", titleFont.Size - 1);
                }
                sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;
                g.DrawString(nameText, titleFont, new SolidBrush(Color.FromArgb(190, 195, 195, 195)), new PointF((int)((X + offset.X) * zoom), (int)((Y - 22 + offset.Y) * zoom)), sf);
            }
        }




        public override bool IsWithinCircle(Point center, Point mouse, double radius)
        {
            Point offset = nodeGraph.graphOffset;
            float zoom = nodeGraph.graphZoom;
            radius = radius * zoom;
            int diffX = (int)((center.X + offset.X) * zoom - (mouse.X));
            int diffY = (int)((center.Y + offset.Y) * zoom - (mouse.Y));
            return (diffX * diffX + diffY * diffY) <= radius * radius;
        }
    }


    
}
