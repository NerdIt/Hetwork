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

        public NodeConnection(NodeVisual node1, NodeVisual node2, NodeGraph ng)
        {
            n1 = node1;
            n2 = node2;
            nodeGraph = ng;
        }

        public void Update()
        {
            Point offset = nodeGraph.graphOffset;
            float zoom = nodeGraph.graphZoom;



            point1 = ClosestCircleNodePoint(n2, n1, 30);
            point2 = ClosestCircleNodePoint(n1, n2, 30);

            if (Distance(point2, new Point(n2.X, n2.Y)) < Distance(point1, new Point(n2.X, n2.Y)))
            {
                point1 = Midpoint(new Point(n1.X, n1.Y), new Point(n2.X, n2.Y));
                point2 = Midpoint(new Point(n1.X, n1.Y), new Point(n2.X, n2.Y));
            }

            //if(n2.IsWithinCircle(new Point(n2.X, n2.Y), point2, 30))
            //{
            //    point2 = point1;
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

        public Point ClosestCircleNodePoint(NodeVisual point, NodeVisual center, int R)
        {
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

            try
            {
                g.FillEllipse(new SolidBrush(Color.FromArgb(255, 63, 63, 63)), new Rectangle(new Point((int)((point1.X - 2.5f + offset.X) * zoom), (int)((point1.Y - 2.5f + offset.Y) * zoom)), new Size((int)(5 * zoom), (int)(5 * zoom))));

                g.FillEllipse(new SolidBrush(Color.FromArgb(255, 63, 63, 63)), new Rectangle(new Point((int)((point2.X - 2.5f + offset.X) * zoom), (int)((point2.Y - 2.5f + offset.Y) * zoom)), new Size((int)(5 * zoom), (int)(5 * zoom))));
            }
            catch
            {

            }
            g.DrawLine(new Pen(Color.FromArgb(255, 63, 63, 63), 2 * zoom), new Point((int)((point1.X+ offset.X) * zoom), (int)((point1.Y + offset.Y) * zoom)), new Point((int)((point2.X + offset.X) * zoom), (int)((point2.Y + offset.Y) * zoom)));

        }
    }
}
