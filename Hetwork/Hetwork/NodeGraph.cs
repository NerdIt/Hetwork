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
using Hetwork.Properties;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace Hetwork
{
    public partial class NodeGraph : UserControl
    {
        public Timer timer = new Timer();
        bool needRepaint = true;

        public Point graphOffset = new Point(0,0);
        public float graphZoom = 1f;
        public List<NodeVisual> nodes = new List<NodeVisual>();
        public List<NodeConnection> connections = new List<NodeConnection>();

        public List<NodeVisual> selectedNodes = new List<NodeVisual>();

        public NodeGraph()
        {
            InitializeComponent();
            
            DoubleBuffered = true;
            timer.Interval = 30;
            timer.Tick += TimerOnTick;
            timer.Start();
            DoubleBuffered = true;
        }


        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            if (DesignMode) return;
            if (needRepaint)
            {
                Invalidate();
            }
        }


        

        public void PaintControl(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.Low;
            e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;





            //  DRAW SHADOWS
            foreach (NodeVisual nv in nodes)
            {
                nv.DrawShadow(e.Graphics);
            }

            foreach(NodeConnection nc in connections)
            {
                nc.Draw(e.Graphics);
            }

            //  DRAW NODES
            foreach (NodeVisual nv in nodes)
            {
                nv.Draw(e.Graphics);
            }

            //  GRAPH OFFSET STRING
            needRepaint = false;
            if (middleMouseDown)
            {
                e.Graphics.DrawString($"{graphOffset.X},{graphOffset.Y}", new Font("Arial", 7), new SolidBrush(Color.FromArgb(255, 195, 195, 195)), 0, 0);
            }

            //  GRAPH ZOOM STRING
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Far;
            e.Graphics.DrawString($"{string.Format("{0:0.0}", (double)graphZoom)}", new Font("Arial", 7), new SolidBrush(Color.FromArgb(255, 195, 195, 195)), Width - 1, 0, sf);
        }

        bool middleMouseDown = false;
        bool leftMouseDown = false;
        Point cursorOffset = new Point();
        public List<NodeVisual> draggingNodes = new List<NodeVisual>();
        Point mouseDownPoint = new Point();
        Point mouseUpPoint = new Point();

        private void NodeGraph_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownPoint = e.Location;
            if(e.Button == MouseButtons.Middle)
            {
                middleMouseDown = true;
            }
            else if(e.Button == MouseButtons.Left)
            {
                leftMouseDown = true;

                NodeVisual nv = nodes.LastOrDefault(x => x.IsWithinCircle(new Point(x.X, x.Y), e.Location, 45 / 2) && x.GetType() == Type.GetType("Hetwork.FolderNode"));
                if (nv == null)
                    nv = nodes.LastOrDefault(x => x.IsWithinRect(e.Location));

                if (nv != null && !draggingNodes.Contains(nv))
                {
                    draggingNodes.Add(nv);
                }

                if(nv != null)
                {
                    ArrangeNodeToTop(nv);

                    if (!selectedNodes.Contains(nv))
                    {
                        if (ModifierKeys != Keys.Shift)
                        {
                            for (int i = 0; i < selectedNodes.Count; i++)
                            {
                                selectedNodes[i].isSelected = false;
                            }
                            selectedNodes.Clear();
                        }
                        selectedNodes.Add(nv);
                    }
                    else if(ModifierKeys != Keys.Shift)
                    {
                        for (int i = 0; i < selectedNodes.Count; i++)
                        {
                            selectedNodes[i].isSelected = false;
                        }
                        selectedNodes.Clear();
                        nv.isSelected = true;
                        selectedNodes.Add(nv);
                    }

                    foreach (NodeVisual n in selectedNodes)
                    {
                        n.isSelected = true;
                    }
                }
                else if(ModifierKeys != Keys.Shift)
                {
                    for (int i = 0; i < selectedNodes.Count; i++)
                    {
                        selectedNodes[i].isSelected = false;
                    }
                    selectedNodes.Clear();
                }


                //for (int i = 0; i < selectedNodes.Count; i++)
                //{
                //    selectedNodes[i].offsetToCursor = new Point(selectedNodes[i].X - draggingNodes[0].X, selectedNodes[i].Y - draggingNodes[0].Y);
                //}

            }
            cursorOffset = PointToScreen(e.Location);
            needRepaint = true;
        }

        private void NodeGraph_MouseUp(object sender, MouseEventArgs e)
        {
            mouseUpPoint = e.Location;
            if (e.Button == MouseButtons.Middle)
            {
                middleMouseDown = false;
            }
            if(e.Button == MouseButtons.Left)
            {
                leftMouseDown = false;
                draggingNodes.Clear();

                //NodeVisual nv = nodes.FirstOrDefault(x => x.IsWithinCircle(new Point(x.X, x.Y), e.Location, 17.5));
                //if (nv == null)
                //    nv = nodes.FirstOrDefault(x => x.IsWithinRect(new Rectangle(x.X, x.Y, x.Width, x.Height), e.Location));

                
            }

            needRepaint = true;
        }

        public float Distance(Point p1, Point p2)
        {
            return ((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        private void NodeGraph_MouseMove(object sender, MouseEventArgs e)
        {
            var em = PointToScreen(e.Location);
            Point offset = new Point(em.X - cursorOffset.X, em.Y - cursorOffset.Y);

            foreach (NodeVisual n in nodes)
            {
                n.isHover = false;

            }

            NodeVisual n1 = nodes.LastOrDefault(x => x.IsWithinCircle(new Point(x.X, x.Y), e.Location, 45 / 2) && x.GetType() == Type.GetType("Hetwork.FolderNode"));
            if (n1 == null)
                n1 = nodes.LastOrDefault(x => x.IsWithinRect(e.Location));

            if (n1 != null)
            {
                n1.isHover = true;
            }


            if (middleMouseDown)
            {
                graphOffset = new Point(graphOffset.X + offset.X, graphOffset.Y + offset.Y);
                
            }
            else if(leftMouseDown && draggingNodes.Count > 0)
            {
                foreach(NodeVisual nv in draggingNodes)
                {
                    if (ModifierKeys != Keys.Shift)
                    {

                        nv.X += (int)(offset.X);
                        nv.Y += (int)(offset.Y);
                        
                    }
                    else
                    {
                        if (graphZoom == 1)
                        {
                            foreach (NodeVisual nodeVis in selectedNodes)
                            {
                                nodeVis.X += (int)(offset.X);
                                nodeVis.Y += (int)(offset.Y);
                            }
                        }
                    }
                }
            }


            cursorOffset = em;
            needRepaint = true;
        }

        float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }

        private void NodeGraph_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == '+')
            //{
            //    if (graphZoom < 1.5)
            //    {
            //        graphZoom += 0.1f;
            //        if(graphZoom > 1.5f)
            //        {
            //            graphZoom = 1.5f;
                        
            //        }
            //        needRepaint = true;
            //        graphZoom = (float)Math.Round((double)graphZoom, 1);
            //    }
            //}
            //else if(e.KeyChar == '_')
            //{
            //    if (graphZoom > 0.5)
            //    {
            //        graphZoom -= 0.1f;
            //        if (graphZoom < 0.5f)
            //        {
            //            graphZoom = 0.5f;
            //        }
            //        graphZoom = (float)Math.Round((double)graphZoom, 1);
            //    }
            //    needRepaint = true;
            //}
            
            

        }

        public void ArrangeNodeToTop(NodeVisual n)
        {
            int i = nodes.IndexOf(n);
            if(i != -1)
            {
                nodes.Insert(nodes.Count, n);
                nodes.RemoveAt(i);
            }
        }
    
    }




    public class DraggingNode
    {
        public Point offset;
        public NodeVisual node;

        public DraggingNode(Point o, NodeVisual n)
        {
            offset = o;
            node = n;
        }

        public static bool ContainByNode(List<DraggingNode> d, NodeVisual node)
        {
            foreach(DraggingNode dn in d)
            {
                if (dn.node == node)
                    return true;
            }
            return false;
        }
    }
}
