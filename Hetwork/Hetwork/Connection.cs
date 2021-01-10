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
    public class NodeConnection
    {
        public Point point1;
        public Point point2;

        public int PointWidth;
        public int PointHeight;
        public NodeGraph nodeGraph;
        public NodeVisual n1;
        public NodeVisual n2;

        public Color color = Color.FromArgb(255, 63, 63, 63);

        public bool isHoverArea = false;

        public NodeConnection(NodeVisual node1, NodeVisual node2, NodeGraph ng)
        {
            n1 = node1;
            n2 = node2;
            
            nodeGraph = ng;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Update()
        {
            Point offset = nodeGraph.graphOffset;
            float zoom = nodeGraph.graphZoom;


            

            point1 = n1.GetConnectionLocation(new Point(n2.X, n2.Y));



            point2 = n2.GetConnectionLocation(new Point(n1.X, n1.Y));




            //if (n1.GetType() == Type.GetType("Hetwork.FolderNode") && n2.GetType() == Type.GetType("Hetwork.FolderNode"))
            //{
            //    if (Distance(point2, new Point(p2.X, p2.Y)) > Distance(point1, new Point(p2.X, p2.Y)))
            //    {

            //        point1 = Midpoint(new Point(p1.X, p1.Y), new Point(p2.X, p2.Y));
            //        point2 = Midpoint(new Point(p1.X, p1.Y), new Point(p2.X, p2.Y));
            //    }
            //}






        }

        public float Distance(Point p1, Point p2)
        {
            return ((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        Point Midpoint(Point a, Point b)
        {
            Point ret = new Point();
            ret.X = (a.X + b.X) / 2;
            ret.Y = (a.Y + b.Y) / 2;
            return ret;
        }

        public Point ClosestCircleNodePoint(Point point, Point center, int R)
        {
            if(center.X > point.X || center.Y > point.Y)
            {
                R += 5;
            }
            
            double vX = center.X - point.X;
            double vY = center.Y - point.Y;
            double magV = Math.Sqrt(vX * vX + vY * vY);
            double aX = point.X + vX / magV * R;
            double aY = point.Y + vY / magV * R;

            return new Point((int)Math.Floor(aX), (int)Math.Floor(aY));
        }





        public void Draw(Graphics g)
        {
            g.InterpolationMode = InterpolationMode.Default;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Update();

            Point offset = nodeGraph.graphOffset;
            float zoom = nodeGraph.graphZoom;

            if (!isHoverArea)
            {

                try
                {
                    g.FillEllipse(new SolidBrush(color), new Rectangle(new Point((int)((point1.X - 2.5f + offset.X) * zoom), (int)((point1.Y - 2.5f + offset.Y) * zoom)), new Size((int)(5 * zoom), (int)(5 * zoom))));

                    g.FillEllipse(new SolidBrush(color), new Rectangle(new Point((int)((point2.X - 2.5f + offset.X) * zoom), (int)((point2.Y - 2.5f + offset.Y) * zoom)), new Size((int)(5 * zoom), (int)(5 * zoom))));
                }
                catch
                {

                }
                g.DrawLine(new Pen(color, 2.5f * zoom), new Point((int)((point1.X + offset.X) * zoom), (int)((point1.Y + offset.Y) * zoom)), new Point((int)((point2.X + offset.X) * zoom), (int)((point2.Y + offset.Y) * zoom)));
            }
            else
            {
                try
                {
                    g.FillEllipse(new SolidBrush(Color.Red), new Rectangle(new Point((int)((point1.X - 2.5f + offset.X) * zoom), (int)((point1.Y - 2.5f + offset.Y) * zoom)), new Size((int)(5 * zoom), (int)(5 * zoom))));

                    g.FillEllipse(new SolidBrush(Color.Red), new Rectangle(new Point((int)((point2.X - 2.5f + offset.X) * zoom), (int)((point2.Y - 2.5f + offset.Y) * zoom)), new Size((int)(5 * zoom), (int)(5 * zoom))));
                }
                catch
                {

                }
                g.DrawLine(new Pen(Color.Red, 2.5f * zoom), new Point((int)((point1.X + offset.X) * zoom), (int)((point1.Y + offset.Y) * zoom)), new Point((int)((point2.X + offset.X) * zoom), (int)((point2.Y + offset.Y) * zoom)));
            }
        }
    }


    public class DragConnection
    {
        public Point point1;
        public Point point2;
        public NodeGraph nodeGraph;

        public DragConnection(Point p1, Point p2, NodeGraph ng)
        {
            point1 = p1;
            point2 = p2;
            nodeGraph = ng;
        }


        public void Draw(Graphics g)
        {
            g.InterpolationMode = InterpolationMode.Default;
            g.SmoothingMode = SmoothingMode.AntiAlias;



            Point offset = nodeGraph.graphOffset;
            float zoom = nodeGraph.graphZoom;

            try
            {
                g.FillEllipse(new SolidBrush(Color.FromArgb(255, 63, 63, 63)), new Rectangle(new Point((int)((point1.X - 2.5f + offset.X) * zoom), (int)((point1.Y - 2.5f + offset.Y) * zoom)), new Size((int)(5 * zoom), (int)(5 * zoom))));

                g.FillEllipse(new SolidBrush(Color.FromArgb(255, 63, 63, 63)), new Rectangle(new Point((int)((point2.X - 2.5f + offset.X) * zoom), (int)((point2.Y - 2.5f + offset.Y) * zoom)), new Size((int)(5 * zoom), (int)(5 * zoom))));
            }
            catch
            {

            }
            g.DrawLine(new Pen(Color.FromArgb(255, 63, 63, 63), 2.5f * zoom), new Point((int)((point1.X + offset.X) * zoom), (int)((point1.Y + offset.Y) * zoom)), new Point((int)((point2.X + offset.X) * zoom), (int)((point2.Y + offset.Y) * zoom)));

        }
    }
}
