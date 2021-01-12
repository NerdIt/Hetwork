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


        public List<NodeVisual> selectedNodes = new List<NodeVisual>();

        public string projectName = "Project Name";

        public float textSize = 1;

        public NodeGraph()
        {
            InitializeComponent();
            
            DoubleBuffered = true;
            timer.Interval = 30;
            timer.Tick += TimerOnTick;
            timer.Start();
            DoubleBuffered = true;


            PopulateNewMenu();
        }

        #region RigthClickMenu

        public void PopulateNewMenu()
        {
            ContextMenu cm = new ContextMenu();
            MenuItem mi1 = new MenuItem();
            mi1.Text = "Add Folder";
            mi1.Click += new System.EventHandler(this.AddFolder);
            cm.MenuItems.Add(mi1);

            MenuItem mi2 = new MenuItem();
            mi2.Text = "Add Task";
            mi2.Click += new System.EventHandler(this.AddTask);
            cm.MenuItems.Add(mi2);
            
            MenuItem mi3 = new MenuItem();
            mi3.Text = "Add List";
            mi3.Click += new System.EventHandler(this.AddList);
            cm.MenuItems.Add(mi3);

            ContextMenu = cm;
        }

        public void AddFolder(object sender, System.EventArgs e)
        {
            nodes.Add(new FolderNode("New Folder", mouseDownPoint.X - graphOffset.X, mouseDownPoint.Y - graphOffset.Y, 45, 45, 0, this));
        }

        public void AddTask(object sender, System.EventArgs e)
        {
            nodes.Add(new SingularTaskNode("New Task", mouseDownPoint.X - graphOffset.X, mouseDownPoint.Y - graphOffset.Y, 100, 35, this));
        }

        public void AddList(object sender, System.EventArgs e)
        {
            nodes.Add(new ListTaskNode("New List", mouseDownPoint.X - graphOffset.X, mouseDownPoint.Y - graphOffset.Y, 100, 35, this));
        }
        #endregion

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            if (DesignMode) return;
            if (needRepaint)
            {
                Invalidate();
            }
        }



        public NodeConnection connection;

        public void PaintControl(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.Low;
            e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;



            #region DrawTitle
            Point averagePoint;
            int aX = 0;
            int aY = 0;
            for (int i = 0; i < nodes.Count; i++)
            {
                aX += nodes[i].GetRelativePosition().X;
                aY += nodes[i].GetRelativePosition().Y;
            }
            try
            {
                aX /= nodes.Count;
                aY /= nodes.Count;
            }
            catch
            {

            }
            averagePoint = new Point(aX, aY);
            //Debug.WriteLine(averagePoint);
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(projectName, new Font("Arial", 50 * textSize, FontStyle.Bold), new SolidBrush(Color.FromArgb(10, 0, 0, 0)), averagePoint, sf);
            #endregion

            #region DrawNodes
            //  DRAW SHADOWS
            foreach (NodeVisual nv in nodes)
            {
                nv.DrawShadow(e.Graphics);
            }

            foreach(NodeVisual nc in nodes)
            {
                if (nc.connection != null)
                {

                    nc.connection.Draw(e.Graphics);
                }
            }

            //  DRAW NODES
            foreach (NodeVisual nv in nodes)
            {
                nv.Draw(e.Graphics);
            }

            #endregion


            //  GRAPH OFFSET STRING
            needRepaint = false;
            if (middleMouseDown)
            {
                e.Graphics.DrawString($"{graphOffset.X},{graphOffset.Y}", new Font("Arial", 7 * textSize), new SolidBrush(Color.FromArgb(255, 195, 195, 195)), 0, 0);
            }
            
            if(editingNodeConnection != null)
                dragConnection.Draw(e.Graphics);


            ////  GRAPH ZOOM STRING
            //sf = new StringFormat();
            //sf.Alignment = StringAlignment.Far;
            //e.Graphics.DrawString($"{string.Format("{0:0.0}", (double)graphZoom)}", new Font("Arial", 7 * textSize), new SolidBrush(Color.FromArgb(255, 195, 195, 195)), Width - 1, 0, sf);
        }



        public float NearestPointOnLineDistance(Point linePnt, PointF lineDir, Point pnt)
        {
            Normalize(lineDir); //this needs to be a unit vector
            var v = new Point(pnt.X - linePnt.X, pnt.Y - linePnt.Y);
            var d = DotProduct(new decimal[2]{ v.X, v.Y }, new decimal[2] { (decimal)lineDir.X, (decimal)lineDir.Y });
            
            PointF p = new PointF(linePnt.X + lineDir.X * (float)d, linePnt.Y + lineDir.Y * (float)d);
            return Distance(new Point((int)p.X, (int)p.Y), pnt);
        }

        private decimal DotProduct(decimal[] vec1, decimal[] vec2)
        {
            if (vec1 == null)
                return 0;

            if (vec2 == null)
                return 0;

            if (vec1.Length != vec2.Length)
                return 0;

            decimal tVal = 0;
            for (int x = 0; x < vec1.Length; x++)
            {
                tVal += vec1[x] * vec2[x];
            }

            return tVal;
        }


        public PointF Normalize(PointF A)
        {
            double length = Math.Sqrt(A.X * A.X + A.Y * A.Y);
            return new PointF((float)(A.X / length), (float)(A.Y / length));
        }


        bool middleMouseDown = false;
        bool leftMouseDown = false;
        Point cursorOffset = new Point();
        public List<NodeVisual> draggingNodes = new List<NodeVisual>();
        Point mouseDownPoint = new Point();
        Point mouseUpPoint = new Point();

        NodeVisual editingNodeConnection = null;
        DragConnection dragConnection = null;
        NodeConnection processingConnection = null;

        private void NodeGraph_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownPoint = e.Location;
            if(e.Button == MouseButtons.Middle)
            {
                middleMouseDown = true;
            }
            else if(e.Button == MouseButtons.Left)
            {


                #region NodeMoveAndSelectionManagement_MouseDown

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
                    bool shouldClear = true;
                    for (int i = 0; i < selectedNodes.Count; i++)
                    {
                        if (!selectedNodes[i].isHoverArea)
                        {
                            selectedNodes[i].isSelected = false;
                        }
                        else
                        {
                            shouldClear = false;
                        }

                    }
                    if(shouldClear)
                        selectedNodes.Clear();
                }

                #endregion


                #region CreateOrEditNodeConnection_MouseDown


                editingNodeConnection = nodes.LastOrDefault(x => x.isHoveringNewNode);
                if (editingNodeConnection != null)
                {
                    dragConnection = new DragConnection(editingNodeConnection.GetConnectionLocation(new Point(e.Location.X - graphOffset.X, e.Location.Y - graphOffset.Y)), new Point(e.Location.X - graphOffset.X, e.Location.Y - graphOffset.Y), this);
                    if (editingNodeConnection.connection != null)
                    {
                        editingNodeConnection.connection.RemoveChild();
                        editingNodeConnection.connection = null;
                    }
                }
                else
                {
                    List<NodeConnection> ncs = new List<NodeConnection>();

                    foreach (NodeVisual v in nodes)
                    {

                        if (v.connection != null)
                        {
                            v.connection.isSelected = false;
                            ncs.Add(v.connection);
                        }
                    }

                    var c1 = ncs.LastOrDefault(x => x.isHoverArea);
                    
                    
                    if (c1 != null)
                    {
                        //NodeVisual editFrom = c1.GetClosestPoint(e.Location);
                        
                        
                        if(c1.GetClosestPoint(new Point(e.Location.X, e.Location.Y)) == c1.n1)
                        {
                            //  N1
                            //Debug.WriteLine("Closest is N1");
                            editingNodeConnection = c1.n2;
                            processingConnection = c1;
                        }
                        else
                        {
                            //  N2
                            //Debug.WriteLine("Closest is N2");
                            editingNodeConnection = c1.n1;
                            processingConnection = c1;
                        }
                        editingNodeConnection.isEditingConnection = true;
                        

                        dragConnection = new DragConnection(editingNodeConnection.GetConnectionLocation(new Point(e.Location.X - graphOffset.X, e.Location.Y - graphOffset.Y)), new Point(e.Location.X - graphOffset.X, e.Location.Y - graphOffset.Y), this);

                        c1.isSelected = true;
                        //editingNodeConnection.connection.RemoveChild();
                        //editingNodeConnection.connection = null;
                    }
                }

                #endregion








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
                if(editingNodeConnection != null && editingNodeConnection.connection != null)
                {
                    editingNodeConnection.connection.isSelected = false;
                }


                leftMouseDown = false;
                draggingNodes.Clear();

                #region EditNodeConnections_MouseUp
                if (processingConnection != null)
                {
                    processingConnection.n1.connection.RemoveChild();
                    processingConnection.n1.isEditingConnection = false;
                    processingConnection.n1.connection = null;
                }

                if (editingNodeConnection != null)
                {

                    if (dragConnection != null)
                    {
                        NodeVisual nv = nodes.LastOrDefault(x => x.IsWithinRect(e.Location) || x.IsWithinCircle(new Point(x.X, x.Y), e.Location, 22.5f));

                        if (nv != null && nv != editingNodeConnection)
                        {

                            if (processingConnection != null && processingConnection.n1 != editingNodeConnection)
                            {

                                
                                if (editingNodeConnection.GetType() == Type.GetType("Hetwork.FolderNode") && nv.GetType() == Type.GetType("Hetwork.FolderNode"))
                                {
                                    if (editingNodeConnection.connection != null)
                                    {
                                        if (nv.connection == null)
                                        {
                                            nv.connection = new NodeConnection(nv, editingNodeConnection, this);
                                        }
                                        else
                                        {
                                            nv.connection.RemoveChild();
                                            nv.connection = new NodeConnection(nv, editingNodeConnection, this);
                                        }
                                    }
                                    else if(!editingNodeConnection.isMain)
                                    {
                                        editingNodeConnection.connection = new NodeConnection(editingNodeConnection, nv, this);
                                    }
                                    else
                                    {
                                        if(nv.connection != null)
                                            nv.connection.RemoveChild();
                                        nv.connection = new NodeConnection(nv, editingNodeConnection, this);
                                    }
                                }
                                else if(editingNodeConnection.GetType() == Type.GetType("Hetwork.FolderNode") && nv.GetType() != Type.GetType("Hetwork.FolderNode"))
                                {
                                    
                                    if (nv.connection != null)
                                    {
                                        nv.connection.RemoveChild();
                                        nv.connection = new NodeConnection(nv, editingNodeConnection, this);
                                    }
                                    else
                                    {
                                        nv.connection = new NodeConnection(nv, editingNodeConnection, this);
                                    }
                                }
                            }
                            else
                            {
                                if (editingNodeConnection.connection != null)
                                {
                                    editingNodeConnection.connection.RemoveChild();
                                    editingNodeConnection.connection = null;
                                }
                                

                                //Node Senarios
                                if (editingNodeConnection.GetType() == Type.GetType("Hetwork.FolderNode") && nv.GetType() != Type.GetType("Hetwork.FolderNode"))
                                {
                                    
                                    if (nv.connection == null)
                                    {
                                        nv.connection = new NodeConnection(nv, editingNodeConnection, this);
                                    }
                                    else
                                    {
                                        nv.connection.RemoveChild();
                                        nv.connection = new NodeConnection(nv, editingNodeConnection, this);
                                    }
                                }
                                else if (editingNodeConnection.GetType() != Type.GetType("Hetwork.FolderNode") && nv.GetType() == Type.GetType("Hetwork.FolderNode"))
                                {
                                    
                                    editingNodeConnection.connection = new NodeConnection(editingNodeConnection, nv, this);
                                }
                                else if (editingNodeConnection.GetType() == Type.GetType("Hetwork.FolderNode") && nv.GetType() == Type.GetType("Hetwork.FolderNode") && !editingNodeConnection.isMain && !editingNodeConnection.isMain)
                                {
                                    editingNodeConnection.connection = new NodeConnection(editingNodeConnection, nv, this);
                                }
                                else if (editingNodeConnection.isMain && nv.connection == null)
                                {
                                    nv.connection = new NodeConnection(nv, editingNodeConnection, this);
                                }
                                else
                                {
                                    
                                }
                            }

                            //editingNodeConnection.connection = new NodeConnection(editingNodeConnection, nv, this);
                        }
                        else if(nv == null && editingNodeConnection != null && editingNodeConnection.connection != null)
                        {
                            editingNodeConnection.connection.RemoveChild();
                            editingNodeConnection.connection = null;
                        }
                        editingNodeConnection.isEditingConnection = false;
                        dragConnection = null;
                        editingNodeConnection = null;
                        processingConnection = null;
                    }
                }

                editingNodeConnection = null;
                dragConnection = null;
                #endregion


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
                n.isHoverArea = false;
                n.isHoveringNewNode = false;
            }


            #region NodeHover_MouseMove

            NodeVisual n1 = nodes.LastOrDefault(x => x.IsWithinCircle(new Point(x.X, x.Y), e.Location, 45 / 2) && x.GetType() == Type.GetType("Hetwork.FolderNode"));
            if (n1 == null)
                n1 = nodes.LastOrDefault(x => x.IsWithinRect(e.Location));

            if (n1 != null)
            {
                n1.isHover = true;
            }

            #endregion

            #region OffsetGraph_Or_MoveNode(s)_MouseMove
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
            #endregion



            if (editingNodeConnection != null)
            {
                
                dragConnection.point1 = editingNodeConnection.GetConnectionLocation(new Point(e.Location.X - graphOffset.X, e.Location.Y - graphOffset.Y));
                dragConnection.point2 = new Point(e.Location.X - graphOffset.X, e.Location.Y - graphOffset.Y);
            }

            #region NewConnectionHover_MouseMove

            NodeVisual n2 = nodes.LastOrDefault(x => x.IsWithinHoverArea(e.Location) && x.GetType() != Type.GetType("Hetwork.FolderNode"));
            if(n2 != null)
            {
                n2.isHoverArea = true;
                
                if(IsWithinRect(new Rectangle(n2.X + graphOffset.X - 5, n2.Y + graphOffset.Y + n2.Height / 2 + 7, 10, 10), e.Location))
                {
                    
                    n2.isHoveringNewNode = true;
                }
            }

            n2 = nodes.LastOrDefault(x => x.IsWithinCircle(new Point(x.X, x.Y), e.Location, 45 / 2 + 18) && x.GetType() == Type.GetType("Hetwork.FolderNode"));
            if (n2 != null)
            {
                n2.isHoverArea = true;

                if (IsWithinRect(new Rectangle(n2.X + graphOffset.X - 5, n2.Y + graphOffset.Y + n2.Height / 2 + 7, 10, 10), e.Location))
                {

                    n2.isHoveringNewNode = true;
                }


                //if (n2.IsWithinCircle(new Point(n2.connectionGrabPoint.X - 3, n2.connectionGrabPoint.Y - 3), e.Location, 500))
                //{
                //    Debug.WriteLine("Debug");
                //    n2.isSelectingNewNode = true;
                //}
            }
            #endregion


            #region NodeConnection_HoverArea_MouseMove

            List<NodeConnection> ncs = new List<NodeConnection>();

            foreach(NodeVisual v in nodes)
            {
                if(v.connection != null)
                {
                    ncs.Add(v.connection);
                }
            }

            for (int i = 0; i < ncs.Count; i++)
            {
                if (ncs[i].IsHoveringWithinConnectionPoint(e.Location))
                {
                    ncs[i].isHoverArea = true;
                }
                else
                {
                    ncs[i].isHoverArea = false;
                }
            }
            #endregion

            cursorOffset = em;
            needRepaint = true;
        }

        bool IsWithinRect(Rectangle rect, Point mouse)
        { 
            return rect.Contains(mouse);
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

        private void NodeGraph_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Delete)
            {
                for (int i = 0; i < selectedNodes.Count; i++)
                {
                    if (!selectedNodes[i].isMain)
                    {
                        if (selectedNodes[i].connection != null)
                        {
                            selectedNodes[i].connection.RemoveChild();
                        }
                        if (selectedNodes[i].GetType() == Type.GetType("Hetwork.FolderNode"))
                        {
                            for (int j = 0; j < nodes.Count; j++)
                            {
                                if (nodes[j].connection != null && nodes[j].connection.n2 == selectedNodes[i])
                                {
                                    nodes[j].connection.Dispose();
                                    nodes[j].connection = null;
                                }
                            }
                        }
                        selectedNodes[i].Dispose();
                        nodes.Remove(selectedNodes[i]);
                    }
                }
                selectedNodes.Clear();
                needRepaint = true;
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
