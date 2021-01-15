using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hetwork
{
    class Serializer
    {
        public void SaveProject(Project p)
        {
            List<NodeVisual> nodes = p.nodes;

            List<SingularTask> singleTasks = new List<SingularTask>();
            List<ListTask> listTasks = new List<ListTask>();
            List<FolderNode> folders = new List<FolderNode>();
            List<SingularTaskNode> singleNode = new List<SingularTaskNode>();
            List<ListTaskNode> listNode = new List<ListTaskNode>();


            List<string> dataString = new List<string>();


            foreach (NodeVisual nv in nodes)
            {
                if (nv.GetType() == Type.GetType("Hetwork.FolderNode"))
                {
                    folders.Add(nv as FolderNode);
                }
                else if (nv.GetType() == Type.GetType("Hetwork.SingularTaskNode"))
                {
                    singleNode.Add(nv as SingularTaskNode);
                    singleTasks.Add((nv as SingularTaskNode).taskElement);
                }
                else if (nv.GetType() == Type.GetType("Hetwork.ListTaskNode"))
                {
                    listNode.Add(nv as ListTaskNode);
                    listTasks.Add((nv as ListTaskNode).taskElement);
                    foreach (SingularTask st in (nv as ListTaskNode).taskElement.elements)
                    {
                        singleTasks.Add(st);
                    }
                }
            }

            dataString.Add($"[preset] {p.offset.X},{p.offset.Y}");
            dataString.Add($"[preset] {p.zoom}");
            for (int i = 0; i < folders.Count; i++)
            {
                string parentId = folders[i].connection != null ? folders[i].connection.n2.id.ToString() : "null";
                string childId = "";
                for (int j = 0; j < folders[i].children.Count; j++)
                {
                    if (j != 0)
                    {
                        childId += ",";
                    }
                    childId += folders[i].children[j].id;
                }
                if (childId == "") { childId = "null"; }

                dataString.Add($"[node] folder {folders[i].id} {folders[i].X},{folders[i].Y} {folders[i].Width},{folders[i].Height} {parentId} {childId} {folders[i].isMain} \"{folders[i].title}\"");
            }

            for (int i = 0; i < singleNode.Count; i++)
            {
                string parentId = singleNode[i].connection != null ? singleNode[i].connection.n2.id.ToString() : "null";

                dataString.Add($"[node] single {singleNode[i].id} {singleNode[i].X},{singleNode[i].Y} {singleNode[i].Width},{singleNode[i].Height} {parentId} {singleNode[i].taskElement.id} {singleNode[i].title}");
            }

            for (int i = 0; i < listNode.Count; i++)
            {
                string parentId = listNode[i].connection != null ? listNode[i].connection.n2.id.ToString() : "null";

                dataString.Add($"[node] list {listNode[i].id} {listNode[i].X},{listNode[i].Y} {listNode[i].Width},{listNode[i].Height} {parentId} {listNode[i].taskElement.id} {listNode[i].title}");
            }

            for (int i = 0; i < singleTasks.Count; i++)
            {
                dataString.Add($"[task] single {singleTasks[i].id} {singleTasks[i].completed} {singleTasks[i].taskTitle} {singleTasks[i].taskContent.Replace("\n", "\\n")}");
            }

            for (int i = 0; i < listTasks.Count; i++)
            {
                string childId = "";
                for (int j = 0; j < listTasks[i].elements.Count; j++)
                {
                    if (j != 0)
                    {
                        childId += ",";
                    }
                    childId += listTasks[i].elements[j].id;
                }
                if (childId == "") { childId = "null"; }

                dataString.Add($"[task] list {listTasks[i].id} {listTasks[i].completed} {listTasks[i].taskTitle} {childId}");
            }
        }
    }
}
