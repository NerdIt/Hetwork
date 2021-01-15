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
        public bool isHoveringNewNode = false;
        public bool isEditingConnection = false;

        public Point offsetToCursor = new Point();

        public NodeConnection connection;
        public NodeGraph nodeGraph;

        public bool isMain = false;

        public List<NodeVisual> children = new List<NodeVisual>();

        public int id;

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

        public virtual bool IsWithinRect(Point mouse)
        {
            return false;
        }

        public virtual bool IsWithinHoverArea(Point mouse)
        {
            return false;
        }

        public virtual Point GetConnectionLocation(Point _point)
        {
            return new Point();
        }

        public Point GetRelativePosition()
        {
            Point offset = nodeGraph.graphOffset;
            return new Point(X + offset.X, Y + offset.Y);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.Collect();
        }

        public PercentageComplete GetPercentage(NodeVisual node)
        {
            float complete = 0;
            float incomplete = 0;
            float total = 0;
            if(node.GetType() == Type.GetType("Hetwork.FolderNode") && node.children.Count > 0)
            {

                foreach (NodeVisual nv in node.children)
                {
                    if (nv.GetType() != Type.GetType("Hetwork.FolderNode"))
                    {
                        total++;
                        if (nv.GetType() == Type.GetType("Hetwork.SingularTaskNode"))
                        {
                            if ((nv as SingularTaskNode).taskElement.completed)
                            {
                                complete++;
                            }
                            else
                            {
                                incomplete++;
                            }
                        }
                        else if (nv.GetType() == Type.GetType("Hetwork.ListTaskNode"))
                        {
                            if ((nv as ListTaskNode).taskElement.completed)
                            {
                                complete++;
                            }
                            else
                            {
                                incomplete++;
                            }
                        }
                    }
                    else
                    {
                        PercentageComplete pc = nv.GetPercentage(nv);
                        complete += pc.complete;
                        incomplete += pc.incomplete;
                        total += pc.total;
                    }
                }
                    
                
            }

            return new PercentageComplete(complete, incomplete, total);
        }


    }

    public class FolderNode : NodeVisual
    {
        public int percentage;
        
        
        

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

            if (isSelected)
            {
                if (connection != null)
                {
                    connection.color = Color.FromArgb(255, 153, 153, 153);
                }
            }
            else
            {
                if (connection != null)
                {
                    connection.color = Color.FromArgb(255, 63, 63, 63);
                }
            }


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
            Font stringFont = new Font("Andale Mono", 9 * nodeGraph.textSize * zoom, FontStyle.Bold);
            SizeF stringSize = new SizeF();
            stringSize = g.MeasureString(percText, stringFont);
            while (stringSize.Width > 55 && stringFont.Size > 1)
            {
                stringSize = g.MeasureString(percText, stringFont);
                stringFont = new Font("Andale Mono", stringFont.Size - 1 * nodeGraph.textSize);
            }
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            g.DrawString(percText, stringFont, new SolidBrush(Color.FromArgb(255, 195, 195, 195)), new PointF((int)((X + offset.X) * zoom), (int)((Y + offset.Y) * zoom)), sf);



            //  DRAW FULL NAME AND BORDER ON HOVER AREA
            if (isHover && !isSelected)
            {
                g.DrawEllipse(new Pen(Color.LightGray, 2 * zoom), (int)((X - Width / 2 + offset.X) * zoom), (int)((Y - Height / 2 + offset.Y) * zoom), (int)(Width * zoom), (int)(Height * zoom));

                string nameText = title;
                Font titleFont = new Font("Andale Mono", 7 * nodeGraph.textSize * zoom, FontStyle.Bold);
                stringSize = new SizeF();
                stringSize = g.MeasureString(nameText, stringFont);
                while (stringSize.Width > 140 && titleFont.Size > 1)
                {
                    stringSize = g.MeasureString(nameText, titleFont);
                    titleFont = new Font("Andale Mono", titleFont.Size - 1 * nodeGraph.textSize);
                }
                sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;
                g.DrawString(nameText, titleFont, new SolidBrush(Color.FromArgb(255, 123, 123, 123)), new PointF((int)((X + offset.X) * zoom), (int)((Y - Height / 2 - stringSize.Height / 2 + offset.Y) * zoom)), sf);
            }

            if(isSelected)
            {
                g.DrawEllipse(new Pen(Color.FromArgb(255, 63, 63, 63), 2 * zoom), (int)((X - Width / 2 + offset.X) * zoom), (int)((Y - Height / 2 + offset.Y) * zoom), (int)(Width * zoom), (int)(Height * zoom));

                string nameText = title;
                Font titleFont = new Font("Andale Mono", 7 * nodeGraph.textSize * zoom, FontStyle.Bold);
                stringSize = new SizeF();
                stringSize = g.MeasureString(nameText, stringFont);
                while (stringSize.Width > 140 && titleFont.Size > 1)
                {
                    stringSize = g.MeasureString(nameText, titleFont);
                    titleFont = new Font("Andale Mono", titleFont.Size - 1 * nodeGraph.textSize);
                }
                sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;
                g.DrawString(nameText, titleFont, new SolidBrush(Color.FromArgb(255, 123, 123, 123)), new PointF((int)((X + offset.X) * zoom), (int)((Y - Height / 2 - stringSize.Height / 2 + offset.Y) * zoom)), sf);
            }

            if (isHoverArea && !isHoveringNewNode && connection == null)
            {
                g.FillEllipse(new SolidBrush(Color.FromArgb(255, 63, 63, 63)), new Rectangle(X + offset.X - 3, Y + offset.Y + Height / 2 + 6, 6, 6));
            }
            //g.FillEllipse(new SolidBrush(Color.Red), new Rectangle(new Point(connectionGrabPoint.X - 2, connectionGrabPoint.Y - 2), new Size(12, 12)));                
            if (isHoveringNewNode && connection == null)
            {
                g.FillEllipse(new SolidBrush(Color.FromArgb(255, 63, 63, 63)), new Rectangle(X + offset.X - 4, Y + offset.Y + Height / 2 + 6, 8, 8));
            }

            if(isEditingConnection)
            {
                g.DrawEllipse(new Pen(Color.FromArgb(255, 150, 150, 150), 1.5f), new Rectangle(new Point((int)((X - Width / 2 + offset.X) * zoom), (int)((Y - Height / 2 + offset.Y) * zoom)), new Size((int)(Width * zoom), (int)(Height * zoom))));
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

        public override Point GetConnectionLocation(Point _point)
        {
            int R = Math.Max(Width, Height) / 2 + 10;
            //if (X > _point.X || Y > _point.Y)
            //{
            //    R += 5;
            //}
            

            double vX = _point.X - X + 2;
            double vY = _point.Y - Y + 2;
            double magV = Math.Sqrt(vX * vX + vY * vY);
            double aX = X + 2 + vX / magV * R;
            double aY = Y + 2 + vY / magV * R;

            return new Point((int)aX, (int)aY);
        }

    }

    public class SingularTaskNode : NodeVisual
    {

        public SingularTask taskElement;


        public SingularTaskNode(string name, int x, int y, int width, int height, NodeGraph graphControl)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            nodeGraph = graphControl;
            title = name;
        }

        public override void DrawShadow(Graphics g)
        {
            g.InterpolationMode = InterpolationMode.Low;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            float zoom = nodeGraph.graphZoom;
            Point offset = nodeGraph.graphOffset;
            g.FillRectangle(new SolidBrush(Color.FromArgb(100, 0, 0, 0)), new Rectangle(new Point((int)((X + 2 - Width / 2 + offset.X) * zoom), (int)((Y + 2 - Height / 2 + offset.Y) * zoom)), new Size((int)(Width * zoom), (int)(Height * zoom))));
        }

        public override void Draw(Graphics g)
        {
            g.InterpolationMode = InterpolationMode.Low;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Point offset = nodeGraph.graphOffset;
            float zoom = nodeGraph.graphZoom;

            if(isSelected)
            {
                if(connection != null)
                {
                    connection.color = Color.FromArgb(255, 153, 153, 153);
                }
            }
            else
            {
                if (connection != null)
                {
                    connection.color = Color.FromArgb(255, 63, 63, 63);
                }
            }

            //  DRAW BASE FILLED RECT
            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 252, 252, 252)), (int)((X - Width / 2 + offset.X) * zoom), (int)((Y - Height / 2 + offset.Y) * zoom), (int)(Width * zoom), (int)(Height * zoom));

            //  DRAW BORDER
            g.DrawRectangle(new Pen(Color.FromArgb(255, 215, 215, 215)), (int)((X - Width / 2 + offset.X) * zoom), (int)((Y - Height / 2 + offset.Y) * zoom), (int)(Width * zoom), (int)(Height * zoom));


            //  DRAW NAME TEXT
            string nText = title;
            Font stringFont = new Font("Andale Mono", 9 * zoom * nodeGraph.textSize, FontStyle.Bold);
            SizeF stringSize = new SizeF();
            stringSize = g.MeasureString(nText, stringFont);
            if (stringSize.Width > 180)
            {
                string newText = "";
                int inc = 0;
                Debug.WriteLine("Loop");
                for (int i = 0; i < nText.Length; i++)
                {
                    newText += nText[i];
                    if (i == inc + 10)
                    {
                        newText += "\n";
                        stringFont = new Font("Andale Mono", stringFont.Size - 1 * nodeGraph.textSize * zoom, FontStyle.Bold);
                        inc += 10;
                    }
                    
                }
                nText = newText;
            }

            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            g.DrawString(nText, stringFont, new SolidBrush(Color.FromArgb(255, 195, 195, 195)), new PointF((int)((X + offset.X) * zoom), (int)((Y + offset.Y) * zoom)), sf);



            //sf = new StringFormat();
            //sf.LineAlignment = StringAlignment.Center;
            //sf.Alignment = StringAlignment.Center;
            //g.DrawString(titleText, stringFont, new SolidBrush(Color.FromArgb(255, 195, 195, 195)), new PointF((int)((X + offset.X) * zoom), (int)((Y - 10 + offset.Y) * zoom)), sf);

            //  DRAW FULL NAME AND BORDER ON HOVER AREA
            if (isHover && !isSelected)
            {
                g.DrawRectangle(new Pen(Color.LightGray, 2 * zoom), (int)((X - Width / 2 + offset.X) * zoom), (int)((Y - Height / 2 + offset.Y) * zoom), (int)(Width * zoom), (int)(Height * zoom));

                string nameText = title;
                Font titleFont = new Font("Andale Mono", 7 * nodeGraph.textSize * zoom, FontStyle.Bold);
                stringSize = new SizeF();
                stringSize = g.MeasureString(nameText, stringFont);
                while (stringSize.Width > 140 && titleFont.Size > 1)
                {
                    stringSize = g.MeasureString(nameText, titleFont);
                    titleFont = new Font("Andale Mono", titleFont.Size - 1 * nodeGraph.textSize);
                }
                sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;
                g.DrawString(nameText, titleFont, new SolidBrush(Color.FromArgb(255, 123, 123, 123)), new PointF((int)((X + offset.X) * zoom), (int)((Y - Height / 2 - stringSize.Height / 2 + offset.Y) * zoom)), sf);
            }

            if (isSelected)
            {
                g.DrawRectangle(new Pen(Color.FromArgb(255, 63, 63, 63), 2 * zoom), (int)((X - Width / 2 + offset.X) * zoom), (int)((Y - Height / 2 + offset.Y) * zoom), (int)(Width * zoom), (int)(Height * zoom));

                string nameText = title;
                Font titleFont = new Font("Andale Mono", 7 * nodeGraph.textSize * zoom, FontStyle.Bold);
                stringSize = new SizeF();
                stringSize = g.MeasureString(nameText, stringFont);
                while (stringSize.Width > 140 && titleFont.Size > 1)
                {
                    stringSize = g.MeasureString(nameText, titleFont);
                    titleFont = new Font("Andale Mono", titleFont.Size - 1 * nodeGraph.textSize);
                }
                sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;
                g.DrawString(nameText, titleFont, new SolidBrush(Color.FromArgb(255, 123, 123, 123)), new PointF((int)((X + offset.X) * zoom), (int)((Y - Height / 2 - stringSize.Height / 2 + offset.Y) * zoom)), sf);
            }


            if (isHoverArea && !isHoveringNewNode && connection == null)
            {
                g.FillEllipse(new SolidBrush(Color.FromArgb(255, 63, 63, 63)), new Rectangle(X + offset.X - 3, Y + offset.Y + Height / 2 + 6, 6, 6));
            }
            //g.FillEllipse(new SolidBrush(Color.Red), new Rectangle(new Point(connectionGrabPoint.X - 2, connectionGrabPoint.Y - 2), new Size(12, 12)));                
            if (isHoveringNewNode && connection == null)
            {
                g.FillEllipse(new SolidBrush(Color.FromArgb(255, 63, 63, 63)), new Rectangle(X + offset.X - 4, Y + offset.Y + Height / 2 + 6, 8, 8));
            }

            if(isEditingConnection)
            {
                g.DrawRectangle(new Pen(Color.FromArgb(255, 150, 150, 150), 1.5f), (int)((X - Width / 2 + offset.X) * zoom), (int)((Y - Height / 2 + offset.Y) * zoom), (int)(Width * zoom), (int)(Height * zoom));
            }
        }


        public override bool IsWithinRect(Point mouse)
        {
            Point offset = nodeGraph.graphOffset;
            float zoom = nodeGraph.graphZoom;

            Rectangle r = new Rectangle((int)((X - Width / 2 + offset.X) * zoom), (int)((Y - Height / 2 + offset.Y) * zoom), (int)(Width * zoom), (int)(Height * zoom));
            return r.Contains(mouse);
        }

        public override bool IsWithinHoverArea(Point mouse)
        {
            Point offset = nodeGraph.graphOffset;
            float zoom = nodeGraph.graphZoom;

            Rectangle r = new Rectangle(X + offset.X - Width / 2, Y + offset.Y - Height + 20, Width, Height + 20);
            return r.Contains(mouse);
        }


        public int Clamp(int x, int lower, int upper)
        {
            if(x < lower)
            {
                x = lower;
            }
            if(x > upper)
            {
                x = upper;
            }

            return x;
        }

        public Point[] points
        { 
            get 
            {
                //return new Point[8] {   new Point(X, Y + Height / 2 + 10), new Point(X, Y - Height / 2 - 10),
                //                        new Point(X + Width / 2 + 10, Y), new Point(X - Width / 2 - 10, Y),
                //                        new Point(X + Width / 2 + 10, Y + Height / 2 + 10),
                //                        new Point(X + Width / 2 + 10, Y - Height / 2 - 10),
                //                        new Point(X - Width / 2 - 10, Y + Height / 2 + 10),
                //                        new Point(X - Width / 2 - 10, Y - Height / 2 - 10)};
                return new Point[4] {   new Point(X, Y + Height / 2 + 10), new Point(X, Y - Height / 2 - 10),
                                        new Point(X + Width / 2 + 10, Y), new Point(X - Width / 2 - 10, Y)};
            } 
        }

        public override Point GetConnectionLocation(Point _point)
        {
            List<float> d = new List<float>();
            for(int i = 0; i < points.Length; i++)
            {
                d.Add(GetDistance(_point, points[i]));
            }

            try
            {
                return points[d.IndexOf(d.Min())];
            }
            catch
            {
                return points[2];
            }

        }

        public float GetDistance(Point p1, Point p2)
        {
            float xDelta = p1.X - p2.X;
            float yDelta = p1.Y - p2.Y;

            return (float)Math.Sqrt(Math.Pow(xDelta, 2) + Math.Pow(yDelta, 2));
        }


    }

    public class ListTaskNode : NodeVisual
    {
        public ListTask taskElement;

        public ListTaskNode(string name, int x, int y, int width, int height, NodeGraph graphControl)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            nodeGraph = graphControl;
            title = name;
        }

        public override void DrawShadow(Graphics g)
        {
            g.InterpolationMode = InterpolationMode.Low;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            float zoom = nodeGraph.graphZoom;
            Point offset = nodeGraph.graphOffset;
            g.FillRectangle(new SolidBrush(Color.FromArgb(100, 0, 0, 0)), new Rectangle(new Point((int)((X + 2 - Width / 2 + offset.X) * zoom), (int)((Y + 2 - Height / 2 + offset.Y) * zoom)), new Size((int)(Width * zoom), (int)(Height * zoom))));
        }

        public override void Draw(Graphics g)
        {
            g.InterpolationMode = InterpolationMode.Low;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Point offset = nodeGraph.graphOffset;
            float zoom = nodeGraph.graphZoom;

            if (isSelected)
            {
                if (connection != null)
                {
                    connection.color = Color.FromArgb(255, 153, 153, 153);
                }
            }
            else
            {
                if (connection != null)
                {
                    connection.color = Color.FromArgb(255, 63, 63, 63);
                }
            }

            //  DRAW BASE FILLED RECT
            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 252, 252, 252)), (int)((X - Width / 2 + offset.X) * zoom), (int)((Y - Height / 2 + offset.Y) * zoom), (int)(Width * zoom), (int)(Height * zoom));

            //  DRAW BORDER
            g.DrawRectangle(new Pen(Color.FromArgb(255, 215, 215, 215)), (int)((X - Width / 2 + offset.X) * zoom), (int)((Y - Height / 2 + offset.Y) * zoom), (int)(Width * zoom), (int)(Height * zoom));


            //  DRAW NAME TEXT
            string nText = title;
            Font stringFont = new Font("Andale Mono", 9 * zoom * nodeGraph.textSize, FontStyle.Bold);
            SizeF stringSize = new SizeF();
            stringSize = g.MeasureString(nText, stringFont);
            if (stringSize.Width > 180)
            {
                string newText = "";
                int inc = 0;
                Debug.WriteLine("Loop");
                for (int i = 0; i < nText.Length; i++)
                {
                    newText += nText[i];
                    if (i == inc + 10)
                    {
                        newText += "\n";
                        stringFont = new Font("Andale Mono", stringFont.Size - 1 * nodeGraph.textSize * zoom, FontStyle.Bold);
                        inc += 10;
                    }

                }
                nText = newText;
            }

            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            g.DrawString(nText, stringFont, new SolidBrush(Color.FromArgb(255, 195, 195, 195)), new PointF((int)((X + offset.X) * zoom), (int)((Y + offset.Y) * zoom)), sf);



            //sf = new StringFormat();
            //sf.LineAlignment = StringAlignment.Center;
            //sf.Alignment = StringAlignment.Center;
            //g.DrawString(titleText, stringFont, new SolidBrush(Color.FromArgb(255, 195, 195, 195)), new PointF((int)((X + offset.X) * zoom), (int)((Y - 10 + offset.Y) * zoom)), sf);

            //  DRAW FULL NAME AND BORDER ON HOVER AREA
            if (isHover && !isSelected)
            {
                g.DrawRectangle(new Pen(Color.LightGray, 2 * zoom), (int)((X - Width / 2 + offset.X) * zoom), (int)((Y - Height / 2 + offset.Y) * zoom), (int)(Width * zoom), (int)(Height * zoom));

                string nameText = title;
                Font titleFont = new Font("Andale Mono", 7 * nodeGraph.textSize * zoom, FontStyle.Bold);
                stringSize = new SizeF();
                stringSize = g.MeasureString(nameText, stringFont);
                while (stringSize.Width > 140 && titleFont.Size > 1)
                {
                    stringSize = g.MeasureString(nameText, titleFont);
                    titleFont = new Font("Andale Mono", titleFont.Size - 1 * nodeGraph.textSize);
                }
                sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;
                g.DrawString(nameText, titleFont, new SolidBrush(Color.FromArgb(255, 123, 123, 123)), new PointF((int)((X + offset.X) * zoom), (int)((Y - Height / 2 - stringSize.Height / 2 + offset.Y) * zoom)), sf);
            }

            if (isSelected)
            {
                g.DrawRectangle(new Pen(Color.FromArgb(255, 63, 63, 63), 2 * zoom), (int)((X - Width / 2 + offset.X) * zoom), (int)((Y - Height / 2 + offset.Y) * zoom), (int)(Width * zoom), (int)(Height * zoom));

                string nameText = title;
                Font titleFont = new Font("Andale Mono", 7 * nodeGraph.textSize * zoom, FontStyle.Bold);
                stringSize = new SizeF();
                stringSize = g.MeasureString(nameText, stringFont);
                while (stringSize.Width > 140 && titleFont.Size > 1)
                {
                    stringSize = g.MeasureString(nameText, titleFont);
                    titleFont = new Font("Andale Mono", titleFont.Size - 1 * nodeGraph.textSize);
                }
                sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;
                g.DrawString(nameText, titleFont, new SolidBrush(Color.FromArgb(255, 123, 123, 123)), new PointF((int)((X + offset.X) * zoom), (int)((Y - Height / 2 - stringSize.Height / 2 + offset.Y) * zoom)), sf);
            }


            if (isHoverArea && !isHoveringNewNode && connection == null)
            {
                g.FillEllipse(new SolidBrush(Color.FromArgb(255, 63, 63, 63)), new Rectangle(X + offset.X - 3, Y + offset.Y + Height / 2 + 6, 6, 6));
            }
            //g.FillEllipse(new SolidBrush(Color.Red), new Rectangle(new Point(connectionGrabPoint.X - 2, connectionGrabPoint.Y - 2), new Size(12, 12)));                
            if (isHoveringNewNode && connection == null)
            {
                g.FillEllipse(new SolidBrush(Color.FromArgb(255, 63, 63, 63)), new Rectangle(X + offset.X - 4, Y + offset.Y + Height / 2 + 6, 8, 8));
            }

            if (isEditingConnection)
            {
                g.DrawRectangle(new Pen(Color.FromArgb(255, 150, 150, 150), 1.5f), (int)((X - Width / 2 + offset.X) * zoom), (int)((Y - Height / 2 + offset.Y) * zoom), (int)(Width * zoom), (int)(Height * zoom));
            }
        }


        public override bool IsWithinRect(Point mouse)
        {
            Point offset = nodeGraph.graphOffset;
            float zoom = nodeGraph.graphZoom;

            Rectangle r = new Rectangle((int)((X - Width / 2 + offset.X) * zoom), (int)((Y - Height / 2 + offset.Y) * zoom), (int)(Width * zoom), (int)(Height * zoom));
            return r.Contains(mouse);
        }

        public override bool IsWithinHoverArea(Point mouse)
        {
            Point offset = nodeGraph.graphOffset;
            float zoom = nodeGraph.graphZoom;

            Rectangle r = new Rectangle(X + offset.X - Width / 2, Y + offset.Y - Height + 20, Width, Height + 20);
            return r.Contains(mouse);
        }



        public int Clamp(int x, int lower, int upper)
        {
            if (x < lower)
            {
                x = lower;
            }
            if (x > upper)
            {
                x = upper;
            }

            return x;
        }

        public Point[] points
        {
            get
            {
                //return new Point[8] {   new Point(X, Y + Height / 2 + 10), new Point(X, Y - Height / 2 - 10),
                //                        new Point(X + Width / 2 + 10, Y), new Point(X - Width / 2 - 10, Y),
                //                        new Point(X + Width / 2 + 10, Y + Height / 2 + 10),
                //                        new Point(X + Width / 2 + 10, Y - Height / 2 - 10),
                //                        new Point(X - Width / 2 - 10, Y + Height / 2 + 10),
                //                        new Point(X - Width / 2 - 10, Y - Height / 2 - 10)};
                return new Point[4] {   new Point(X, Y + Height / 2 + 10), new Point(X, Y - Height / 2 - 10),
                                        new Point(X + Width / 2 + 10, Y), new Point(X - Width / 2 - 10, Y)};
            }
        }

        public override Point GetConnectionLocation(Point _point)
        {
            List<float> d = new List<float>();
            for (int i = 0; i < points.Length; i++)
            {
                d.Add(GetDistance(_point, points[i]));
            }

            try
            {
                return points[d.IndexOf(d.Min())];
            }
            catch
            {
                return points[2];
            }

        }

        public float GetDistance(Point p1, Point p2)
        {
            float xDelta = p1.X - p2.X;
            float yDelta = p1.Y - p2.Y;

            return (float)Math.Sqrt(Math.Pow(xDelta, 2) + Math.Pow(yDelta, 2));
        }


    }




}
