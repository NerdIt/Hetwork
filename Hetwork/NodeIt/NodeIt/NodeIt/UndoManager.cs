using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeIt
{
    public static class UndoManager
    {
        private static List<List<NodeVisual>> nodeHistory = new List<List<NodeVisual>>();

        private static int historyIndex = 0;

        public static void Clear()
        {
            nodeHistory.Clear();
        }

        public static List<NodeVisual> LoadHistory(int index)
        {
            return nodeHistory[index];
        }

        public static void Undo(NodeGraph ng)
        {
            if (nodeHistory.Count > 0)
            {

                if (historyIndex + 1 < nodeHistory.Count)
                {
                    historyIndex++;
                    Program.selectedProject.nodes = LoadHistory(historyIndex);
                    ng.nodes = LoadHistory(historyIndex);
                    ng.needRepaint = true;
                    ng.selectedNodes.Clear();
                    ng.selectedNode = null;
                    
                }
            }
        }

        public static void Redo(NodeGraph ng)
        {

            //if (nodeHistory.Count > 0)
            //{

            //    if (historyIndex - 1 > -1)
            //    {
            //        historyIndex--;
            //        Program.selectedProject.nodes = LoadHistory(historyIndex);
            //        ng.nodes = LoadHistory(historyIndex);
            //        ng.needRepaint = true;
            //    }
            //}
        }

        public static void BackUp(List<NodeVisual> n)
        {

            NodeVisual[] newNodes = new NodeVisual[n.Count];
            for (int i = 0; i < newNodes.Length; i++)
            {
                if (n[i].GetType() == Type.GetType("NodeIt.FolderNode"))
                {
                    FolderNode fn = new FolderNode(n[i].title, n[i].X, n[i].Y, n[i].Width, n[i].Height, 0, n[i].id);
                    fn.isSelected = n[i].isSelected;
                    fn.isMain = n[i].isMain;
                    //fn.connection = new NodeConnection(fn, n[i]);

                    newNodes[i] = fn;
                }
                else if (n[i].GetType() == Type.GetType("NodeIt.SingularTaskNode"))
                {
                    SingularTaskNode sn = new SingularTaskNode(n[i].title, n[i].X, n[i].Y, n[i].Width, n[i].Height, n[i].id);
                    sn.isSelected = n[i].isSelected;
                    //sn.connection = new NodeConnection(sn, n[i]);

                    sn.taskElement = (n[i] as SingularTaskNode).taskElement;
                    newNodes[i] = sn;
                }
                else if (n[i].GetType() == Type.GetType("NodeIt.ListTaskNode"))
                {
                    ListTaskNode ln = new ListTaskNode(n[i].title, n[i].X, n[i].Y, n[i].Width, n[i].Height, n[i].id);
                    ln.isSelected = n[i].isSelected;
                    //ln.connection = new NodeConnection(ln, n[i]);

                    ln.taskElement = (n[i] as ListTaskNode).taskElement;
                    newNodes[i] = ln;
                }
            }

            for (int i = 0; i < newNodes.Length; i++)
            {
                if(n[i].connection != null)
                    newNodes[i].connection = new NodeConnection(newNodes[i], newNodes[n.IndexOf(n[i].connection.n2)]);
                newNodes[i].isSelected = false;
            }

            nodeHistory.Insert(0, newNodes.ToList());
            if (nodeHistory.Count > 50)
            {

                nodeHistory.RemoveAt(nodeHistory.Count - 1);
            }
        }

    }
}
