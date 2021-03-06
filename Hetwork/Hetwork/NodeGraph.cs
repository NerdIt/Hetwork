﻿using System;
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
using Microsoft.VisualBasic;

namespace Hetwork
{
    public partial class NodeGraph : UserControl
    {
        public Timer timer = new Timer();
        bool needRepaint = true;

        public Point graphOffset = new Point(0,0);
        public float graphZoom = 1f;
        public List<NodeVisual> nodes = new List<NodeVisual>();

        public float zoomFactor = 1;

        public bool recalculatePercentage = false;


        public List<NodeVisual> selectedNodes = new List<NodeVisual>();

        public string projectName = "";
        private bool focused = true;
        public float textSize = 1;

        public NodeVisual selectedNode;

        public Point CursorLocation = new Point(0,0);

        public Project graphProject;

        public NodeGraph()
        {
            InitializeComponent();
            
            DoubleBuffered = true;
            timer.Interval = 30;
            timer.Tick += TimerOnTick;
            timer.Start();
            DoubleBuffered = true;
            MouseWheel += ScrollWheelEvent;

            PopulateNewMenu();
        }

        public void InitGraph()
        {
            nodes.Add(new FolderNode(graphProject.title, 50, 50, 45, 45, 0, this, graphProject.GetNodeId()));
            nodes[0].isMain = true;
            projectName = graphProject.title;
        }

        private ContextMenu EmptyRigthClick = null;
        private ContextMenu NoneEmptyRightClick = null;


        #region RigthClickMenu

        public void PopulateNewMenu()
        {
            ContextMenu cm1 = new ContextMenu();
            MenuItem mi1 = new MenuItem();
            mi1.Text = "Add Folder";
            mi1.Click += new System.EventHandler(this.AddFolder);
            cm1.MenuItems.Add(mi1);

            MenuItem mi2 = new MenuItem();
            mi2.Text = "Add Task";
            mi2.Click += new System.EventHandler(this.AddTask);
            cm1.MenuItems.Add(mi2);
            
            MenuItem mi3 = new MenuItem();
            mi3.Text = "Add List";
            mi3.Click += new System.EventHandler(this.AddList);
            cm1.MenuItems.Add(mi3);

            EmptyRigthClick = cm1;


            ContextMenu cm2 = new ContextMenu();
            MenuItem mi4 = new MenuItem();
            mi4.Text = "Rename";
            mi4.Click += new System.EventHandler(this.RenameNode);
            cm2.MenuItems.Add(mi4);

            MenuItem mi5 = new MenuItem();
            mi5.Text = "Delete";
            mi5.Click += new System.EventHandler(this.DeleteNode);
            cm2.MenuItems.Add(mi5);


            NoneEmptyRightClick = cm2;
        }

        public void AddFolder(object sender, System.EventArgs e)
        {
            nodes.Add(new FolderNode("New Folder", mouseDownPoint.X - graphOffset.X, mouseDownPoint.Y - graphOffset.Y, 45, 45, 0, this, graphProject.GetNodeId()));
        }

        public void AddTask(object sender, System.EventArgs e)
        {
            var n = new SingularTaskNode("New Task", mouseDownPoint.X - graphOffset.X, mouseDownPoint.Y - graphOffset.Y, 100, 35, this, graphProject.GetNodeId());
            n.taskElement = new SingularTask(n.title, "", graphProject.GetTaskId());
            nodes.Add(n);
        }

        public void AddList(object sender, System.EventArgs e)
        {
            var n = new ListTaskNode("New List", mouseDownPoint.X - graphOffset.X, mouseDownPoint.Y - graphOffset.Y, 100, 35, this, graphProject.GetNodeId());
            n.taskElement = new ListTask(n.title, new List<SingularTask>(), graphProject.GetTaskId());
            nodes.Add(n);
        }

        public void RenameNode(object sender, System.EventArgs e)
        {
            var ib = Interaction.InputBox("New Node Name", "Rename", selectedNode.title);
            if (ib != "")
            {
                selectedNode.title = ib;
                if(selectedNode.GetType() == Type.GetType("Hetwork.SingularTaskNode"))
                {
                    (selectedNode as SingularTaskNode).taskElement.taskTitle = ib;
                }
                else if (selectedNode.GetType() == Type.GetType("Hetwork.ListTaskNode"))
                {
                    (selectedNode as ListTaskNode).taskElement.taskTitle = ib;
                }

                NodeEdited_Event(this, e);
            }

            
        }

        public void DeleteNode(object sender, System.EventArgs e)
        {
            if (selectedNode != null)
            {
                int ind = nodes.IndexOf(selectedNode);
                if (ind != -1)
                {
                    if (nodes[ind].connection != null)
                    {
                        nodes[ind].connection.RemoveChild();
                        nodes[ind].connection.Dispose();
                    }
                    nodes[ind].Dispose();
                    nodes.Remove(selectedNode);
                    if (selectedNodes.Contains(selectedNode))
                        selectedNodes.Remove(selectedNode);
                    selectedNode = null;
                    NodeEdited_Event(this, e);
                }
            }


        }
        
        public void CompleteNode(object sender, System.EventArgs e)
        {
            if (selectedNode != null)
            {
                if(selectedNode.GetType() == Type.GetType("Hetwork.SingularTaskNode"))
                {
                    (selectedNode as SingularTaskNode).taskElement.completed = !(selectedNode as SingularTaskNode).taskElement.completed;
                }
                else if (selectedNode.GetType() == Type.GetType("Hetwork.ListTaskNode"))
                {
                    
                    for(int i = 0; i < (selectedNode as ListTaskNode).taskElement.elements.Count; i++)
                    {
                        (selectedNode as ListTaskNode).taskElement.elements[i].completed = !(selectedNode as ListTaskNode).taskElement.completed;
                    }
                }
                recalculatePercentage = true;
                Invalidate();
                NodeEdited_Event(this, e);
                needRepaint = true;
            }
            

        }

        #endregion


        public float scrollSensitivity = 0.1f;
        public void ScrollWheelEvent(object sender, MouseEventArgs e)
        {
            if(e.Delta > 0 && zoomFactor < 1.5f)
            {
                zoomFactor += scrollSensitivity;
            }
            else if(e.Delta < 0 && zoomFactor > 0.5f)
            {
                zoomFactor -= scrollSensitivity;
            }

            needRepaint = true;
                
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            if (DesignMode) return;
            if (needRepaint)
            {
                recalculatePercentage = true;
                Invalidate();
            }
        }



        public NodeConnection connection;

        public void PaintControl(object sender, PaintEventArgs e)
        {

            //  GRAPH OFFSET STRING
            needRepaint = false;
            if (middleMouseDown)
            {
                e.Graphics.DrawString($"{graphOffset.X},{graphOffset.Y}", new Font("Arial", 7 * textSize), new SolidBrush(Color.FromArgb(255, 195, 195, 195)), 0, 0);
            }

            


            //  GRAPH ZOOM STRING
            StringFormat tsf = new StringFormat();
            tsf.Alignment = StringAlignment.Far;
            e.Graphics.DrawString($"{string.Format("{0:0.0}", (double)zoomFactor)}", new Font("Arial", 7 * textSize), new SolidBrush(Color.FromArgb(255, 195, 195, 195)), Width - 1, 0, tsf);


            e.Graphics.InterpolationMode = InterpolationMode.Low;
            e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
            e.Graphics.ScaleTransform(zoomFactor, zoomFactor);

            if (editingNodeConnection != null)
                dragConnection.Draw(e.Graphics);


            if (recalculatePercentage)
            {
                foreach(NodeVisual nv in nodes)
                {
                    if(nv.GetType() == Type.GetType("Hetwork.FolderNode"))
                    {
                        PercentageComplete pc = nv.GetPercentage(nv);
                        (nv as FolderNode).percentage = (int)pc.GetPercentage();
                        //Debug.WriteLine($"{pc.complete} {pc.incomplete} {pc.total} {pc.GetPercentage()}%");
                    }
                }
                recalculatePercentage = false;
            }


            #region DrawTitle
            Point averagePoint;
            int aX = 0;
            int aY = 0;
            for (int i = 0; i < nodes.Count; i++)
            {
                aX += nodes[i].GetRelativePosition().X;
                aY += nodes[i].GetRelativePosition().Y;
            }
            if(nodes.Count > 0)
            { 
                aX /= nodes.Count;
                aY /= nodes.Count;
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
            CursorLocation = new Point((int)(PointToClient(Cursor.Position).X / zoomFactor), (int)(PointToClient(Cursor.Position).Y / zoomFactor));
            mouseDownPoint = CursorLocation;
            if(e.Button == MouseButtons.Middle)
            {
                middleMouseDown = true;
            }
            else if(e.Button == MouseButtons.Left)
            {


                #region NodeMoveAndSelectionManagement_MouseDown

                leftMouseDown = true;

                NodeVisual nv = nodes.LastOrDefault(x => x.IsWithinCircle(new Point(x.X, x.Y), CursorLocation, 45 / 2) && x.GetType() == Type.GetType("Hetwork.FolderNode"));
                if (nv == null)
                    nv = nodes.LastOrDefault(x => x.IsWithinRect(CursorLocation));

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

                if(selectedNodes.Count > 0)
                {
                    selectedNode = selectedNodes[0];
                }
                else
                {
                    selectedNode = null;
                }
                NodeSelected_Event(this, e);

                #endregion


                #region CreateOrEditNodeConnection_MouseDown


                editingNodeConnection = nodes.LastOrDefault(x => x.isHoveringNewNode);
                if (editingNodeConnection != null)
                {
                    dragConnection = new DragConnection(editingNodeConnection.GetConnectionLocation(new Point(CursorLocation.X - graphOffset.X, CursorLocation.Y - graphOffset.Y)), new Point(CursorLocation.X - graphOffset.X, CursorLocation.Y - graphOffset.Y), this);
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
                        
                        
                        if(c1.GetClosestPoint(new Point(CursorLocation.X, CursorLocation.Y)) == c1.n1)
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
                        

                        dragConnection = new DragConnection(editingNodeConnection.GetConnectionLocation(new Point(CursorLocation.X - graphOffset.X, CursorLocation.Y - graphOffset.Y)), new Point(CursorLocation.X - graphOffset.X, CursorLocation.Y - graphOffset.Y), this);

                        c1.isSelected = true;
                        //editingNodeConnection.connection.RemoveChild();
                        //editingNodeConnection.connection = null;
                    }
                }

                #endregion








            }
            else if(e.Button == MouseButtons.Right)
            {


                NodeVisual nv = nodes.LastOrDefault(x => x.IsWithinCircle(new Point(x.X, x.Y), CursorLocation, 45 / 2) && x.GetType() == Type.GetType("Hetwork.FolderNode"));
                if (nv == null)
                    nv = nodes.LastOrDefault(x => x.IsWithinRect(CursorLocation));


                if (nv == null)
                {
                    EmptyRigthClick.Show(this, e.Location);
                }
                else
                {
                    if (!nv.isMain)
                    {
                        selectedNode = nv;

                        if(nv.GetType() != Type.GetType("Hetwork.FolderNode"))
                        {
                            MenuItem mi5 = new MenuItem();
                            mi5.Text = "Toggle Completion";
                            mi5.Click += new System.EventHandler(this.CompleteNode);
                            NoneEmptyRightClick.MenuItems.Add(mi5);
                        }

                        NoneEmptyRightClick.Show(this, e.Location);
                        if (NoneEmptyRightClick.MenuItems[NoneEmptyRightClick.MenuItems.Count - 1].Text == "Toggle Completion")
                        {
                            NoneEmptyRightClick.MenuItems.RemoveAt(NoneEmptyRightClick.MenuItems.Count - 1);
                        }
                    }
                }
            }
            cursorOffset = PointToScreen(CursorLocation);
            needRepaint = true;
        }

        private void NodeGraph_MouseUp(object sender, MouseEventArgs e)
        {
            mouseUpPoint = CursorLocation;
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
                    recalculatePercentage = true;
                    if (dragConnection != null)
                    {
                        NodeVisual nv = nodes.LastOrDefault(x => x.IsWithinRect(CursorLocation) || x.IsWithinCircle(new Point(x.X, x.Y), CursorLocation, 22.5f));

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
                                            bool canLink = true;
                                            NodeVisual processingNode = editingNodeConnection;
                                            for (int i = 0; i < nodes.Count; i++)
                                            {
                                                if (processingNode.connection != null)
                                                    processingNode = processingNode.connection.n2;
                                                else
                                                    break;

                                                if (processingNode.connection.n2 == editingNodeConnection)
                                                {
                                                    canLink = false;
                                                    break;
                                                }

                                            }
                                            if(canLink)
                                                nv.connection = new NodeConnection(nv, editingNodeConnection, this);
                                        }
                                        else
                                        {
                                            bool canLink = true;
                                            NodeVisual processingNode = nv;
                                            for (int i = 0; i < nodes.Count; i++)
                                            {
                                                if (processingNode.connection != null)
                                                    processingNode = processingNode.connection.n2;
                                                else
                                                    break;

                                                if (processingNode.connection.n2 == editingNodeConnection)
                                                {
                                                    canLink = false;
                                                    break;
                                                }

                                            }

                                            if (canLink)
                                            {
                                                nv.connection.RemoveChild();
                                                nv.connection = new NodeConnection(nv, editingNodeConnection, this);
                                            }
                                        }
                                    }
                                    else if(!editingNodeConnection.isMain)
                                    {
                                        bool canLink = true;
                                        NodeVisual processingNode = editingNodeConnection;
                                        for (int i = 0; i < nodes.Count; i++)
                                        {
                                            if (processingNode.connection != null)
                                                processingNode = processingNode.connection.n2;
                                            else
                                                break;

                                            if (processingNode.connection.n2 == editingNodeConnection)
                                            {
                                                canLink = false;
                                                break;
                                            }

                                        }

                                        if(canLink)
                                            editingNodeConnection.connection = new NodeConnection(editingNodeConnection, nv, this);
                                    }
                                    else
                                    {
                                        bool canLink = true;
                                        NodeVisual processingNode = nv;
                                        for (int i = 0; i < nodes.Count; i++)
                                        {
                                            if (processingNode.connection != null)
                                                processingNode = processingNode.connection.n2;
                                            else
                                                break;

                                            if (processingNode.connection.n2 == editingNodeConnection)
                                            {
                                                canLink = false;
                                                break;
                                            }

                                        }
                                        if (canLink)
                                        {
                                            if (nv.connection != null)
                                                nv.connection.RemoveChild();
                                            nv.connection = new NodeConnection(nv, editingNodeConnection, this);
                                        }
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
                                    bool canLink = true;
                                    NodeVisual processingNode = nv;
                                    for (int i = 0; i < nodes.Count; i++)
                                    {
                                        if (processingNode.connection != null)
                                            processingNode = processingNode.connection.n2;
                                        else
                                            break;

                                        if (processingNode?.connection?.n2 == editingNodeConnection)
                                        {
                                            canLink = false;
                                            break;
                                        }

                                    }

                                    if(canLink)
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
                            //processingConnection.n1.connection.RemoveChild();
                            //editingNodeConnection.connection = null;
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
            CursorLocation = new Point((int)(PointToClient(Cursor.Position).X / zoomFactor), (int)(PointToClient(Cursor.Position).Y / zoomFactor));


            var em = PointToScreen(CursorLocation);
            Point offset = new Point(em.X - cursorOffset.X, em.Y - cursorOffset.Y);

            foreach (NodeVisual n in nodes)
            {
                n.isHover = false;
                n.isHoverArea = false;
                n.isHoveringNewNode = false;
            }


            #region NodeHover_MouseMove

            NodeVisual n1 = nodes.LastOrDefault(x => x.IsWithinCircle(new Point(x.X, x.Y), CursorLocation, 45 / 2) && x.GetType() == Type.GetType("Hetwork.FolderNode"));
            if (n1 == null)
                n1 = nodes.LastOrDefault(x => x.IsWithinRect(CursorLocation));

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
                
                dragConnection.point1 = editingNodeConnection.GetConnectionLocation(new Point(CursorLocation.X - graphOffset.X, CursorLocation.Y - graphOffset.Y));
                dragConnection.point2 = new Point(CursorLocation.X - graphOffset.X, CursorLocation.Y - graphOffset.Y);
            }

            #region NewConnectionHover_MouseMove

            NodeVisual n2 = nodes.LastOrDefault(x => x.IsWithinHoverArea(CursorLocation) && x.GetType() != Type.GetType("Hetwork.FolderNode"));
            if(n2 != null)
            {
                n2.isHoverArea = true;
                
                if(IsWithinRect(new Rectangle(n2.X + graphOffset.X - 5, n2.Y + graphOffset.Y + n2.Height / 2 + 7, 10, 10), CursorLocation))
                {
                    
                    n2.isHoveringNewNode = true;
                }
            }

            n2 = nodes.LastOrDefault(x => x.IsWithinCircle(new Point(x.X, x.Y), CursorLocation, 45 / 2 + 18) && x.GetType() == Type.GetType("Hetwork.FolderNode"));
            if (n2 != null)
            {
                n2.isHoverArea = true;

                if (IsWithinRect(new Rectangle(n2.X + graphOffset.X - 5, n2.Y + graphOffset.Y + n2.Height / 2 + 7, 10, 10), CursorLocation))
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
                if (ncs[i].IsHoveringWithinConnectionPoint(CursorLocation))
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

            //if (e.KeyChar == '+')
            //{
            //    zoomFactor += 0.1f;
            //}
            //else if (e.KeyChar == '_')
            //{
            //    zoomFactor -= 0.1f;
            //}

            needRepaint = true;



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

            if (e.KeyCode == Keys.Delete && focused)
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
                                    nodes[j].connection.RemoveChild();
                                    nodes[j].connection.Dispose();
                                    nodes[j].connection = null;
                                }
                            }
                        }
                        selectedNodes[i].Dispose();
                        nodes.Remove(selectedNodes[i]);
                        nodes.Remove(selectedNode);
                        
                        
                        NodeEdited_Event(this, e);
                    }
                }
                selectedNodes.Clear();
                selectedNode = null;
                needRepaint = true;
            }
            else if(ModifierKeys == Keys.Control && e.KeyCode == Keys.S)
            {
                Debug.WriteLine("Save");
                graphProject.nodes = nodes;
                graphProject.zoom = zoomFactor;
                graphProject.offset = graphOffset;
                Serializer.SaveProject(graphProject);
            }
            else if(e.KeyCode == Keys.N)
            {
                
                
            }


        }


        #region Events
        [Browsable(true)]
        [Category("NodeGraph Action")]
        [Description("Invoked when node selection changes")]
        public event EventHandler NodeSelected;
        public void NodeSelected_Event(object sender, MouseEventArgs e)
        {
            if (NodeSelected != null)
            {
                NodeSelected(this, e);
            }
        }

        [Browsable(true)]
        [Category("NodeGraph Action")]
        [Description("Invoked when node is edited")]
        public event EventHandler NodeEdited;
        public void NodeEdited_Event(object sender, EventArgs e)
        {
            recalculatePercentage = true;
            if (NodeEdited != null)
            {
                NodeEdited(this, e);
            }
        }
        #endregion

        private void NodeGraph_Enter(object sender, EventArgs e)
        {
            focused = true;
        }

        private void NodeGraph_Leave(object sender, EventArgs e)
        {
            focused = false;
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