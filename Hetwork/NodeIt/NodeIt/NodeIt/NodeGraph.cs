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
//using NodeIt.Properties;
using System.Drawing.Imaging;
using System.Diagnostics;
using Microsoft.VisualBasic;

namespace NodeIt
{
    public partial class NodeGraph : UserControl
    {
        public Timer timer = new Timer();
        public bool needRepaint = true;
        public Point graphOffset = new Point(0, 0);
        public float graphZoom = 1f;
        public List<NodeVisual> nodes = new List<NodeVisual>();
        public float zoomFactor = 1;
        public List<NodeVisual> selectedNodes = new List<NodeVisual>();
        public string projectName = "";
        private bool focused = true;
        public float textSize = 1;
        public NodeVisual selectedNode;
        public Point CursorLocation = new Point(0, 0);


        public NodeGraph()
        {
            InitializeComponent();

            DoubleBuffered = true;
            timer.Interval = 30;
            timer.Tick += TimerOnTick;
            timer.Start();
            MouseWheel += ScrollWheelEvent;


            PopulateNewMenu();
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
            //MenuItem mi4 = new MenuItem();
            //mi4.Text = "Rename";
            //mi4.Click += new System.EventHandler(this.RenameNode);
            //cm2.MenuItems.Add(mi4);

            MenuItem mi5 = new MenuItem();
            mi5.Text = "Delete";
            mi5.Click += new System.EventHandler(this.DeleteNode);
            cm2.MenuItems.Add(mi5);


            NoneEmptyRightClick = cm2;
        }

        public void AddFolder(object sender, System.EventArgs e)
        {
            UndoManager.BackUp(nodes);
            nodes.Add(new FolderNode("New Folder", mouseDownPoint.X - graphOffset.X, mouseDownPoint.Y - graphOffset.Y, 45, 45, 0, Program.selectedProject.GetNodeId()));
            //UndoManager.BackUp(nodes);
        }

        public void AddTask(object sender, System.EventArgs e)
        {
            UndoManager.BackUp(nodes);
            var n = new SingularTaskNode("New Task", mouseDownPoint.X - graphOffset.X, mouseDownPoint.Y - graphOffset.Y, 100, 35, Program.selectedProject.GetNodeId());
            n.taskElement = new SingularTask(n.title, "", Program.selectedProject.GetTaskId());
            nodes.Add(n);
            //UndoManager.BackUp(nodes);
        }

        public void AddList(object sender, System.EventArgs e)
        {
            UndoManager.BackUp(nodes);
            var n = new ListTaskNode("New List", mouseDownPoint.X - graphOffset.X, mouseDownPoint.Y - graphOffset.Y, 100, 35, Program.selectedProject.GetNodeId());
            n.taskElement = new ListTask(n.title, new List<SingularTask>() { new SingularTask("New Task", "", Program.selectedProject.GetTaskId())}, Program.selectedProject.GetTaskId());
            nodes.Add(n);
            //UndoManager.BackUp(nodes);
        }

        public void RenameNode(object sender, System.EventArgs e)
        {
            var ib = Interaction.InputBox("New Node Name", "Rename", selectedNode.title);
            if (ib != "")
            {
                UndoManager.BackUp(nodes);
                selectedNode.title = ib;
                if (selectedNode.GetType() == Type.GetType("NodeIt.SingularTaskNode"))
                {
                    (selectedNode as SingularTaskNode).taskElement.taskTitle = ib;
                }
                else if (selectedNode.GetType() == Type.GetType("NodeIt.ListTaskNode"))
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
                UndoManager.BackUp(nodes);
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

            NodeSelected_Event(this, new MouseEventArgs(new MouseButtons(),0,CursorLocation.X, CursorLocation.Y, 0));


        }

        public void CompleteNode(object sender, System.EventArgs e)
        {
            if (selectedNode != null)
            {
                UndoManager.BackUp(nodes);
                if (selectedNode.GetType() == Type.GetType("NodeIt.SingularTaskNode"))
                {
                    (selectedNode as SingularTaskNode).taskElement.completed = !(selectedNode as SingularTaskNode).taskElement.completed;
                }
                else if (selectedNode.GetType() == Type.GetType("NodeIt.ListTaskNode"))
                {

                    for (int i = 0; i < (selectedNode as ListTaskNode).taskElement.elements.Count; i++)
                    {
                        (selectedNode as ListTaskNode).taskElement.elements[i].completed = !(selectedNode as ListTaskNode).taskElement.completed;
                    }
                    (selectedNode as ListTaskNode).taskElement.completed = !(selectedNode as ListTaskNode).taskElement.completed;
                }
                RecalculateNodePercentages();
                Invalidate();
                NodeEdited_Event(this, e);
                needRepaint = true;
            }


        }

        #endregion


        public void LoadData()
        {
            if (Program.selectedProject != null)
            {
                Project p = Program.selectedProject;
                nodes = p.nodes;
                projectName = p.title;
                zoomFactor = p.zoom;
                graphOffset = p.offset;
                needRepaint = true;
            }
        }


        public float scrollSensitivity = 0.1f;
        public void ScrollWheelEvent(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0 && zoomFactor < 1.5f)
            {
                zoomFactor += scrollSensitivity;
            }
            else if (e.Delta < 0 && zoomFactor > 0.5f)
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
                RecalculateNodePercentages();
                Invalidate();
            }
        }



        public NodeConnection connection;


        string toolTipContent = "";

        public void RecalculateNodePercentages()
        {
            foreach (NodeVisual nv in nodes)
            {
                if (nv.GetType() == Type.GetType("NodeIt.FolderNode"))
                {
                    PercentageComplete pc = nv.GetPercentage(nv);
                    (nv as FolderNode).percentage = (int)pc.GetPercentage();
                    //Debug.WriteLine($"{pc.complete} {pc.incomplete} {pc.total} {pc.GetPercentage()}%");
                }
            }
        }
        public Rectangle selectionBox = new Rectangle();

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





            #region DrawTitle
            Point averagePoint;
            int aX = 0;
            int aY = 0;
            for (int i = 0; i < nodes.Count; i++)
            {
                aX += nodes[i].GetRelativePosition(graphOffset).X;
                aY += nodes[i].GetRelativePosition(graphOffset).Y;
            }
            if (nodes.Count > 0)
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
                nv.DrawShadow(e.Graphics, graphOffset);
            }
            
            foreach (NodeVisual nc in nodes)
            {
                if (nc.connection != null)
                {

                    nc.connection.Draw(e.Graphics, graphOffset);
                }
            }


            int countNode = 0;
            //  DRAW NODES
            foreach (NodeVisual nv in nodes)
            {
                nv.Draw(e.Graphics, graphOffset);




                if (nv.isHover && nv.GetType() == Type.GetType("NodeIt.FolderNode"))
                {
                    PercentageComplete pc = nv.GetPercentage(nv);
                    if (toolTipContent != $"{pc.complete} Complete\n{pc.total} Total\n{(nv as FolderNode).percentage}%")
                    {
                        toolTip1.ToolTipTitle = nv.title;
                        toolTip1.SetToolTip(this, $"{pc.complete} Complete\n{pc.total} Total\n{(nv as FolderNode).percentage}%");
                    }
                    countNode++;
                }
                else if (nv.isHover && nv.GetType() == Type.GetType("NodeIt.SingularTaskNode"))
                {
                    if (toolTipContent != $"Singular Task\nIs Completed: {(nv as SingularTaskNode).taskElement.completed}")
                    {
                        toolTip1.ToolTipTitle = nv.title;
                        toolTip1.SetToolTip(this, $"Singular Task\nIs Completed: {(nv as SingularTaskNode).taskElement.completed}");
                    }
                    countNode++;
                }
                else if (nv.isHover && nv.GetType() == Type.GetType("NodeIt.ListTaskNode"))
                {
                    if (toolTipContent != $"List Task\nIs Completed: {(nv as ListTaskNode).taskElement.completed}\nTasks: {(nv as ListTaskNode).taskElement.elements.Count}")
                    {
                        toolTip1.ToolTipTitle = nv.title;
                        toolTip1.SetToolTip(this, $"List Task\nIs Completed: {(nv as ListTaskNode).taskElement.completed}\nTasks: {(nv as ListTaskNode).taskElement.elements.Count}");
                    }
                    countNode++;
                }
            }
            if(countNode == 0)
            {
                toolTip1.ToolTipTitle = "";
                toolTip1.SetToolTip(this,"");
                toolTip1.Hide(this);
            }

            #endregion


            if(leftMouseDown && draggingNodes.Count <= 0 && dragConnection == null && editingNodeConnection == null)
            {
                Point startPoint = new Point(mouseDownPoint.X, mouseDownPoint.Y);
                Size dragBoxSize = new Size(CursorLocation.X - mouseDownPoint.X, CursorLocation.Y - mouseDownPoint.Y);
                if (CursorLocation.X < mouseDownPoint.X)
                {
                    int oldWidth = dragBoxSize.Width;
                    startPoint.X = CursorLocation.X;
                    dragBoxSize.Width = mouseDownPoint.X - CursorLocation.X;
                }
                if (CursorLocation.Y < mouseDownPoint.Y)
                {
                    int oldHeight = dragBoxSize.Height;
                    startPoint.Y = CursorLocation.Y;
                    dragBoxSize.Height = mouseDownPoint.Y - CursorLocation.Y;
                }
                selectionBox = new Rectangle(startPoint, dragBoxSize);

                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(35, 0, 0, 0)), new Rectangle(startPoint, dragBoxSize));
            }
            else
            {
                selectionBox = new Rectangle();
            }



        }



        public float NearestPointOnLineDistance(Point linePnt, PointF lineDir, Point pnt)
        {
            Normalize(lineDir); //this needs to be a unit vector
            var v = new Point(pnt.X - linePnt.X, pnt.Y - linePnt.Y);
            var d = DotProduct(new decimal[2] { v.X, v.Y }, new decimal[2] { (decimal)lineDir.X, (decimal)lineDir.Y });

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
            toolTip1.Hide(this);

            CursorLocation = new Point((int)(PointToClient(Cursor.Position).X / zoomFactor), (int)(PointToClient(Cursor.Position).Y / zoomFactor));
            mouseDownPoint = CursorLocation;
            if (e.Button == MouseButtons.Middle)
            {
                middleMouseDown = true;
            }
            else if (e.Button == MouseButtons.Left)
            {


                #region NodeMoveAndSelectionManagement_MouseDown

                leftMouseDown = true;

                NodeVisual nv = nodes.LastOrDefault(x => x.IsWithinCircle(new Point(x.X, x.Y), CursorLocation, 45 / 2, graphOffset) && x.GetType() == Type.GetType("NodeIt.FolderNode"));
                if (nv == null)
                    nv = nodes.LastOrDefault(x => x.IsWithinRect(CursorLocation, graphOffset));

                if (nv != null && !draggingNodes.Contains(nv))
                {
                    draggingNodes.Add(nv);
                }

                if (nv != null)
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
                    else if (ModifierKeys != Keys.Shift)
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
                else if (ModifierKeys != Keys.Shift)
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
                    if (shouldClear)
                        selectedNodes.Clear();
                }

                if (selectedNodes.Count > 0)
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


                        if (c1.GetClosestPoint(new Point(CursorLocation.X, CursorLocation.Y), graphOffset) == c1.n1)
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
            else if (e.Button == MouseButtons.Right)
            {


                NodeVisual nv = nodes.LastOrDefault(x => x.IsWithinCircle(new Point(x.X, x.Y), CursorLocation, 45 / 2, graphOffset) && x.GetType() == Type.GetType("NodeIt.FolderNode"));
                if (nv == null)
                    nv = nodes.LastOrDefault(x => x.IsWithinRect(CursorLocation, graphOffset));

                editingNodeConnection = null;
                dragConnection = null;

                if (nv == null)
                {
                    EmptyRigthClick.Show(this, e.Location);
                }
                else
                {
                    if (!nv.isMain)
                    {
                        selectedNode = nv;

                        if (nv.GetType() != Type.GetType("NodeIt.FolderNode"))
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
            if (e.Button == MouseButtons.Left)
            {
                if (editingNodeConnection != null && editingNodeConnection.connection != null)
                {
                    editingNodeConnection.connection.isSelected = false;
                }

                if(draggingNodes.Count > 0)
                {
                    UndoManager.BackUp(nodes);
                }


                var foundInSelection = nodes.FindAll(x => selectionBox.Contains(new Point(x.X + graphOffset.X, x.Y + graphOffset.Y)));

                

                if (foundInSelection.Count > 0)
                {
                    if (ModifierKeys != Keys.Shift)
                    {
                        foreach (NodeVisual nodeV in selectedNodes)
                        {
                            nodeV.isSelected = false;
                        }
                        selectedNodes.Clear();
                    }
                    for (int i = 0; i < foundInSelection.Count; i++)
                    {
                        foundInSelection[i].isSelected = true;
                        if(!selectedNodes.Contains(foundInSelection[i]))
                            selectedNodes.Add(foundInSelection[i]);
                    }
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
                    RecalculateNodePercentages();
                    if (dragConnection != null)
                    {
                        NodeVisual nv = nodes.LastOrDefault(x => x.IsWithinRect(CursorLocation, graphOffset) || x.IsWithinCircle(new Point(x.X, x.Y), CursorLocation, 22.5f, graphOffset));

                        if (nv != null && nv != editingNodeConnection)
                        {

                            if (processingConnection != null && processingConnection.n1 != editingNodeConnection)
                            {


                                if (editingNodeConnection.GetType() == Type.GetType("NodeIt.FolderNode") && nv.GetType() == Type.GetType("NodeIt.FolderNode"))
                                {
                                    //Debug.WriteLine("Folder To Folder if processing further connection");
                                    if (editingNodeConnection.connection != null)
                                    {
                                        //Debug.WriteLine("Processing Node Has Connection");
                                        if (nv.connection == null)
                                        {
                                            //Debug.WriteLine("Found Node Has No Connection");
                                            //bool canLink = true;
                                            //NodeVisual processingNode = editingNodeConnection;
                                            //for (int i = 0; i < nodes.Count; i++)
                                            //{
                                            //    if (processingNode.connection != null)
                                            //        processingNode = processingNode.connection.n2;
                                            //    else
                                            //        break;

                                            //    if (processingNode.connection.n2 == editingNodeConnection)
                                            //    {
                                            //        canLink = false;
                                            //        break;
                                            //    }

                                            //}
                                            if (ConnectionSafeGuard(nv, editingNodeConnection))
                                                nv.connection = new NodeConnection(nv, editingNodeConnection);
                                        }
                                        else
                                        {
                                            //Debug.WriteLine("Found Node Has Connection");
                                            //bool canLink = true;
                                            //NodeVisual processingNode = nv;
                                            //for (int i = 0; i < nodes.Count; i++)
                                            //{
                                            //    if (processingNode.connection != null)
                                            //        processingNode = processingNode.connection.n2;
                                            //    else
                                            //        break;

                                            //    if (processingNode.connection.n2 == editingNodeConnection)
                                            //    {
                                            //        canLink = false;
                                            //        break;
                                            //    }

                                            //}

                                            if (ConnectionSafeGuard(nv, editingNodeConnection))
                                            {
                                                nv.connection.RemoveChild();
                                                nv.connection = new NodeConnection(nv, editingNodeConnection);
                                            }
                                        }
                                    }
                                    else if (!editingNodeConnection.isMain)
                                    {
                                        //Debug.WriteLine("Processing Node Has No Connection and is not main");
                                        //bool canLink = true;
                                        //NodeVisual processingNode = editingNodeConnection;
                                        //for (int i = 0; i < nodes.Count; i++)
                                        //{
                                        //    if (processingNode.connection != null)
                                        //        processingNode = processingNode.connection.n2;
                                        //    else
                                        //        break;

                                        //    if (processingNode.connection.n2 == editingNodeConnection)
                                        //    {
                                        //        canLink = false;
                                        //        break;
                                        //    }

                                        //}
                                        //Debug.WriteLine(editingNodeConnection.title);
                                        if (ConnectionSafeGuard(nv, editingNodeConnection))
                                            editingNodeConnection.connection = new NodeConnection(editingNodeConnection, nv);
                                    }
                                    else
                                    {
                                        //Debug.WriteLine("Processing Node Has No Connection and is main");
                                        //bool canLink = true;
                                        //NodeVisual processingNode = nv;
                                        //for (int i = 0; i < nodes.Count; i++)
                                        //{
                                        //    if (processingNode.connection != null)
                                        //        processingNode = processingNode.connection.n2;
                                        //    else
                                        //        break;

                                        //    if (processingNode.connection.n2 == editingNodeConnection)
                                        //    {
                                        //        canLink = false;
                                        //        break;
                                        //    }

                                        //}
                                        if (ConnectionSafeGuard(nv, editingNodeConnection))
                                        {
                                            if (nv.connection != null)
                                                nv.connection.RemoveChild();
                                            nv.connection = new NodeConnection(nv, editingNodeConnection);
                                        }
                                    }
                                }
                                else if (editingNodeConnection.GetType() == Type.GetType("NodeIt.FolderNode") && nv.GetType() != Type.GetType("NodeIt.FolderNode"))
                                {
                                    //Debug.WriteLine("Folder to Task if processing Furthor Connection");
                                    if (nv.connection != null)
                                    {
                                        nv.connection.RemoveChild();
                                        nv.connection = new NodeConnection(nv, editingNodeConnection);
                                    }
                                    else
                                    {
                                        nv.connection = new NodeConnection(nv, editingNodeConnection);
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
                                if (editingNodeConnection.GetType() == Type.GetType("NodeIt.FolderNode") && nv.GetType() != Type.GetType("NodeIt.FolderNode"))
                                {
                                    //Debug.WriteLine("Folder to Task 2");
                                    if (nv.connection == null)
                                    {
                                        nv.connection = new NodeConnection(nv, editingNodeConnection);
                                    }
                                    else
                                    {
                                        nv.connection.RemoveChild();
                                        nv.connection = new NodeConnection(nv, editingNodeConnection);
                                    }
                                }
                                else if (editingNodeConnection.GetType() != Type.GetType("NodeIt.FolderNode") && nv.GetType() == Type.GetType("NodeIt.FolderNode"))
                                {
                                    //Debug.WriteLine("Task to Folder");
                                    editingNodeConnection.connection = new NodeConnection(editingNodeConnection, nv);
                                }
                                else if (editingNodeConnection.GetType() == Type.GetType("NodeIt.FolderNode") && nv.GetType() == Type.GetType("NodeIt.FolderNode") && !editingNodeConnection.isMain)
                                {

                                    //bool canLink = true;
                                    //NodeVisual processingNode = nv;
                                    //for (int i = 0; i < nodes.Count; i++)
                                    //{
                                    //    if (processingNode.connection != null)
                                    //        processingNode = processingNode.connection.n2;
                                    //    else
                                    //        break;

                                    //    if (processingNode?.connection?.n2 == editingNodeConnection)
                                    //    {
                                    //        canLink = false;
                                    //        break;
                                    //    }

                                    //}
                                    //Debug.WriteLine("Folder to Folder");
                                    if (ConnectionSafeGuard(nv, editingNodeConnection))
                                        editingNodeConnection.connection = new NodeConnection(editingNodeConnection, nv);
                                }
                                else if (editingNodeConnection.isMain && nv.connection == null)
                                {
                                    //Debug.WriteLine("Main To Node");
                                    nv.connection = new NodeConnection(nv, editingNodeConnection);
                                }
                                else
                                {

                                }
                            }
                            
                            //editingNodeConnection.connection = new NodeConnection(editingNodeConnection, nv, this);
                        }
                        else if (nv == null && editingNodeConnection != null && editingNodeConnection.connection != null)
                        {
                            //processingConnection.n1.connection.RemoveChild();
                            //editingNodeConnection.connection = null;
                        }
                        UndoManager.BackUp(nodes);
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


        bool ConnectionSafeGuard(NodeVisual secondaryNode, NodeVisual originalNode)
        {
            if (secondaryNode.connection == null)
                return true;
            //if (secondaryNode.connection.n2 == null)
            //    return true;
            if (secondaryNode.connection.n2.isMain)
                return true;
            if (secondaryNode.connection.n2 == originalNode)
                return false;

            if (ConnectionSafeGuard(secondaryNode.connection.n2, originalNode))
                return true;

            return false;
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

            NodeVisual n1 = nodes.LastOrDefault(x => x.IsWithinCircle(new Point(x.X, x.Y), CursorLocation, 45 / 2, graphOffset) && x.GetType() == Type.GetType("NodeIt.FolderNode"));
            if (n1 == null)
                n1 = nodes.LastOrDefault(x => x.IsWithinRect(CursorLocation, graphOffset));

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
            else if (leftMouseDown && draggingNodes.Count > 0)
            {
                if(draggingNodes.Count > 0)
                {
                    if (ModifierKeys != Keys.Shift)
                    {
                        foreach (NodeVisual nv in draggingNodes)
                        {
                            nv.X += (int)(offset.X);
                            nv.Y += (int)(offset.Y);
                        }

                    }
                    else
                    {

                        foreach (NodeVisual nodeVis in selectedNodes)
                        {
                            nodeVis.X += (int)(offset.X);
                            nodeVis.Y += (int)(offset.Y);
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

            NodeVisual n2 = nodes.LastOrDefault(x => x.IsWithinHoverArea(CursorLocation, graphOffset) && x.GetType() != Type.GetType("NodeIt.FolderNode"));
            if (n2 != null)
            {
                n2.isHoverArea = true;

                if (IsWithinRect(new Rectangle(n2.X + graphOffset.X - 5, n2.Y + graphOffset.Y + n2.Height / 2 + 7, 10, 10), CursorLocation))
                {

                    n2.isHoveringNewNode = true;
                }
            }

            n2 = nodes.LastOrDefault(x => x.IsWithinCircle(new Point(x.X, x.Y), CursorLocation, 45 / 2 + 18, graphOffset) && x.GetType() == Type.GetType("NodeIt.FolderNode"));
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

            foreach (NodeVisual v in nodes)
            {
                if (v.connection != null)
                {
                    ncs.Add(v.connection);
                }
            }

            for (int i = 0; i < ncs.Count; i++)
            {
                if (ncs[i].IsHoveringWithinConnectionPoint(CursorLocation, graphOffset))
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
            if (i != -1)
            {
                nodes.Insert(nodes.Count, n);
                nodes.RemoveAt(i);
            }
        }

        private void NodeGraph_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Delete && focused)
            {
                UndoManager.BackUp(nodes);
                for (int i = 0; i < selectedNodes.Count; i++)
                {
                    if (!selectedNodes[i].isMain)
                    {
                        if (selectedNodes[i].connection != null)
                        {
                            selectedNodes[i].connection.RemoveChild();
                        }
                        if (selectedNodes[i].GetType() == Type.GetType("NodeIt.FolderNode"))
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
                        

                        
                        NodeEdited_Event(this, e);
                    }

                }

                if(selectedNode != null && !selectedNode.isMain)
                    nodes.Remove(selectedNode);

                for(int i = 0; i < selectedNodes.Count; i++)
                {
                    if (selectedNodes[i] != null)
                        selectedNodes[i].isSelected = false;
                }
                selectedNodes.Clear();
                selectedNode = null;
                needRepaint = true;
                NodeSelected_Event(this, new MouseEventArgs(new MouseButtons(), 0, CursorLocation.X, CursorLocation.Y, 0));
            }
            else if (ModifierKeys == Keys.Control && e.KeyCode == Keys.S)
            {
                //Debug.WriteLine("Save");
                //UpdateSelectedProject();
                //ProjectManager.SaveSelectedProject();
            }
            else if (e.KeyCode == Keys.Z && ModifierKeys == Keys.Control)
            {
                UndoManager.Undo(this);
            }
            else if (e.KeyCode == Keys.Y && ModifierKeys == Keys.Control)
            {
                UndoManager.Redo(this);
            }


        }

        public void UpdateSelectedProject()
        {
            Program.selectedProject.nodes = nodes;
            Program.selectedProject.zoom = zoomFactor;
            Program.selectedProject.offset = graphOffset;
            Program.selectedProject.graphSize = new Size(Width, Height);
        }


        #region Events
        [Browsable(true)]
        [Category("NodeGraph Action")]
        [Description("Invoked when node selection changes")]
        public event EventHandler NodeSelected;
        public void NodeSelected_Event(object sender, MouseEventArgs e)
        {
            UndoManager.BackUp(nodes);
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
            UndoManager.BackUp(nodes);
            RecalculateNodePercentages();
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
            foreach (DraggingNode dn in d)
            {
                if (dn.node == node)
                    return true;
            }
            return false;
        }
    }
}