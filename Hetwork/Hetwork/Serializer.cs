using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hetwork
{
    class Serializer
    {
        public static void SaveProject(Project p)
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

            dataString.Add($"[meta] nodeIDValue {p.nodeGlobalId}");
            dataString.Add($"[meta] taskIDValue {p.taskGlobalId}");

            dataString.Add($"[preset] offset {p.offset.X},{p.offset.Y}");
            dataString.Add($"[preset] zoom {p.zoom}");

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

                dataString.Add($"[node] folder {folders[i].id} {folders[i].X},{folders[i].Y} {folders[i].Width},{folders[i].Height} {parentId} {childId} {folders[i].isMain} {folders[i].title.Replace(" ", "§0xs000").Replace("\n", "§0xn001").Replace("\t", "§0xt002")}");
            }

            for (int i = 0; i < singleNode.Count; i++)
            {
                string parentId = singleNode[i].connection != null ? singleNode[i].connection.n2.id.ToString() : "null";

                dataString.Add($"[node] single {singleNode[i].id} {singleNode[i].X},{singleNode[i].Y} {singleNode[i].Width},{singleNode[i].Height} {parentId} {singleNode[i].taskElement.id} {singleNode[i].title.Replace(" ", "§0xs000").Replace("\n", "§0xn001").Replace("\t", "§0xt002")}");
            }

            for (int i = 0; i < listNode.Count; i++)
            {
                string parentId = listNode[i].connection != null ? listNode[i].connection.n2.id.ToString() : "null";

                dataString.Add($"[node] list {listNode[i].id} {listNode[i].X},{listNode[i].Y} {listNode[i].Width},{listNode[i].Height} {parentId} {listNode[i].taskElement.id} {listNode[i].title.Replace(" ", "§0xs000").Replace("\n", "§0xn001").Replace("\t", "§0xt002")}");
            }

            for (int i = 0; i < singleTasks.Count; i++)
            {
                dataString.Add($"[task] single {singleTasks[i].id} {singleTasks[i].completed} {singleTasks[i].taskTitle.Replace(" ", "§0xs000").Replace("\n", "§0xn001").Replace("\t", "§0xt002")} {singleTasks[i].taskContent.Replace(" ", "§0xs000").Replace("\n", "§0xn001").Replace("\t", "§0xt002")}");
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

                dataString.Add($"[task] list {listTasks[i].id} {listTasks[i].completed} {listTasks[i].taskTitle.Replace(" ", "§0xs000").Replace("\n", "§0xn001").Replace("\t", "§0xt002")} {childId}");
            }


            

            using (StreamWriter sw = new StreamWriter(p.path + "savedata.data"))
            {
                for (int i = 0; i < dataString.Count; i++)
                {
                    //sw.WriteLine(StringToBinary(dataString[i]));
                    
                    sw.WriteLine(dataString[i]);
                }
            }
        }

        public static string StringToBinary(string data)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in data.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }

        public static string BinaryToString(string data)
        {
            List<Byte> byteList = new List<Byte>();

            for (int i = 0; i < data.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }
            return Encoding.UTF7.GetString(byteList.ToArray());
        }

        public static void LoadProject(Project p, NodeGraph graph)
        {
            List<string> data = new List<string>();
            List<object> tasks = new List<object>();


            using (StreamReader sr = new StreamReader(p.path + "savedata.data"))
            {
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    //data.Add(BinaryToString(line));

                    data.Add(line);
                    //Console.WriteLine(data[data.Count - 1]);
                }
            }

            for(int i = 0; i < data.Count; i++)
            {
                string[] lineSplit = data[i].Split(' ');
                try
                {
                    switch (lineSplit[0])
                    {
                        case "[meta]":
                            switch (lineSplit[1])
                            {
                                case "nodeIDValue":
                                    p.nodeGlobalId = int.Parse(lineSplit[2]);
                                    break;
                                case "taskIDValue":
                                    p.taskGlobalId = int.Parse(lineSplit[2]);
                                    break;
                                default:
                                    Console.WriteLine($"Corrupted Meta Save {i}");
                                    break;
                            }
                            break;
                        case "[preset]":
                            switch (lineSplit[1])
                            {
                                case "offset":
                                    string[] splValue = lineSplit[2].Split(',');
                                    p.offset = new Point(int.Parse(splValue[0]), int.Parse(splValue[1]));
                                    break;
                                case "zoom":
                                    p.zoom = float.Parse(lineSplit[2]);
                                    break;
                                default:
                                    Console.WriteLine($"Corrupted Preset Save {i}");
                                    break;
                            }
                            break;
                        case "[node]":
                            switch (lineSplit[1])
                            {
                                case "folder":
                                    FolderNode fnode = new FolderNode(lineSplit[8].Replace("§0xs000", " ").Replace("§0xn001", "\n").Replace("§0xt002", "\t"), int.Parse(lineSplit[3].Split(',')[0]), int.Parse(lineSplit[3].Split(',')[1]), int.Parse(lineSplit[4].Split(',')[0]), int.Parse(lineSplit[4].Split(',')[0]), 0, graph, int.Parse(lineSplit[2]));
                                    if (lineSplit[5] != "null")
                                    {
                                        fnode.cachedParentID = int.Parse(lineSplit[5]);
                                    }
                                    else
                                    {
                                        fnode.cachedParentID = -1;
                                    }
                                    if (lineSplit[6] != "")
                                    {
                                        string[] splValue = lineSplit[6].Split(',');
                                        for (int j = 0; j < splValue.Length; j++)
                                        {
                                            fnode.cachedChildrenIDs.Add(int.Parse(splValue[j]));
                                        }
                                    }
                                    fnode.isMain = bool.Parse(lineSplit[7]);
                                    p.nodes.Add(fnode);
                                    break;
                                case "single":
                                    SingularTaskNode snode = new SingularTaskNode(lineSplit[7].Replace("§0xs000", " ").Replace("§0xn001", "\n").Replace("§0xt002", "\t"), int.Parse(lineSplit[3].Split(',')[0]), int.Parse(lineSplit[3].Split(',')[1]), int.Parse(lineSplit[4].Split(',')[0]), int.Parse(lineSplit[4].Split(',')[1]), graph, int.Parse(lineSplit[2]));
                                    if (lineSplit[5] != "null")
                                    {
                                        snode.cachedParentID = int.Parse(lineSplit[5]);
                                    }
                                    else
                                    {
                                        snode.cachedParentID = -1;
                                    }
                                    snode.cachedTaskID = int.Parse(lineSplit[6]);
                                    p.nodes.Add(snode);
                                    break;
                                case "list":
                                    ListTaskNode lnode = new ListTaskNode(lineSplit[7].Replace("§0xs000", " ").Replace("§0xn001", "\n").Replace("§0xt002", "\t"), int.Parse(lineSplit[3].Split(',')[0]), int.Parse(lineSplit[3].Split(',')[1]), int.Parse(lineSplit[4].Split(',')[0]), int.Parse(lineSplit[4].Split(',')[1]), graph, int.Parse(lineSplit[2]));
                                    if (lineSplit[5] != "null")
                                    {
                                        lnode.cachedParentID = int.Parse(lineSplit[5]);
                                    }
                                    else
                                    {
                                        lnode.cachedParentID = -1;
                                    }
                                    if (lineSplit[6] != "")
                                    {
                                        lnode.cachedListID = int.Parse(lineSplit[6]);
                                    }
                                    p.nodes.Add(lnode);
                                    break;
                                default:
                                    Console.WriteLine($"Corrupted Node Save {i}");
                                    break;
                            }
                            break;
                        case "[task]":
                            switch (lineSplit[1])
                            {
                                case "single":
                                    SingularTask stask = new SingularTask(lineSplit[4].Replace("§0xs000", " ").Replace("§0xn001", "\n").Replace("§0xt002", "\t"), lineSplit[5].Replace("§0xs000", " ").Replace("§0xn001", "\n").Replace("§0xt002", "\t"), bool.Parse(lineSplit[3]), int.Parse(lineSplit[2]));
                                    tasks.Add(stask);
                                    break;
                                case "list":
                                    ListTask ltask = new ListTask(lineSplit[4].Replace("§0xs000", " ").Replace("§0xn001", "\n").Replace("§0xt002", "\t"), new List<SingularTask>(), int.Parse(lineSplit[2]));
                                    ltask.completed = bool.Parse(lineSplit[3]);
                                    if (lineSplit[5] != "")
                                    {
                                        string[] splValue = lineSplit[5].Split(',');
                                        for (int j = 0; j < splValue.Length; j++)
                                        {
                                            ltask.cachedSingleIDs.Add(int.Parse(splValue[j]));
                                        }
                                    }
                                    tasks.Add(ltask);
                                    break;
                                default:
                                    Console.WriteLine($"Corrupted Task Save {i}");
                                    break;
                            }
                            break;
                        default:
                            Console.WriteLine($"Corrupted Tag Save {i}");
                            break;
                    }
                }
                catch(Exception e)
                {
                    var st = new StackTrace(e, true);
                    // Get the top stack frame
                    var frame = st.GetFrame(0);
                    // Get the line number from the stack frame
                    var line = frame.GetFileLineNumber();
                    Console.WriteLine($"Corrupted Save {i} '{e.Message}' on line {line}");
                }
            }

            for(int i = 0; i < tasks.Count; i++)
            {
                if(tasks[i].GetType() == Type.GetType("Hetwork.SingularTask"))
                {
                    
                }
                else if (tasks[i].GetType() == Type.GetType("Hetwork.ListTask"))
                {
                    for(int j = 0; j < (tasks[i] as ListTask).cachedSingleIDs.Count; j++)
                    {
                        object task = GetTaskByID(tasks, (tasks[i] as ListTask).cachedSingleIDs[j]);
                        if(task != null)
                        {
                            (tasks[i] as ListTask).elements.Add(task as SingularTask);
                        }
                    }
                }
            }


            for(int i = 0; i < p.nodes.Count; i++)
            {
                if(p.nodes[i].GetType() == Type.GetType("Hetwork.SingularTaskNode"))
                {
                    NodeVisual parent = GetNodeByID(p.nodes, p.nodes[i].cachedParentID);
                    if(parent != null)
                    {
                        p.nodes[i].connection = new NodeConnection(p.nodes[i], parent, graph);
                    }
                    object task = GetTaskByID(tasks, (p.nodes[i] as SingularTaskNode).cachedTaskID);
                    if(task.GetType() == Type.GetType("Hetwork.SingularTask"))
                    {
                        (p.nodes[i] as SingularTaskNode).taskElement = task as SingularTask;
                    }
                    else if (task.GetType() == Type.GetType("Hetwork.ListTask"))
                    {
                        Console.WriteLine("Corrupted IDs");
                    }
                }
                else if (p.nodes[i].GetType() == Type.GetType("Hetwork.ListTaskNode"))
                {
                    NodeVisual parent = GetNodeByID(p.nodes, p.nodes[i].cachedParentID);
                    if (parent != null)
                    {
                        p.nodes[i].connection = new NodeConnection(p.nodes[i], parent, graph);
                    }
                    object task = GetTaskByID(tasks, (p.nodes[i] as ListTaskNode).cachedListID);
                    
                    if (task != null && task.GetType() == Type.GetType("Hetwork.SingularTask"))
                    {
                        Console.WriteLine("Corrupted IDs");
                    }
                    else if (task != null && task.GetType() == Type.GetType("Hetwork.ListTask"))
                    {
                        (p.nodes[i] as ListTaskNode).taskElement = task as ListTask;
                    }
                }
                else if (p.nodes[i].GetType() == Type.GetType("Hetwork.FolderNode"))
                {
                    NodeVisual parent = GetNodeByID(p.nodes, p.nodes[i].cachedParentID);
                    if (parent != null)
                    {
                        p.nodes[i].connection = new NodeConnection(p.nodes[i], parent, graph);
                    }

                    //for(int j = 0; j < p.nodes[i].cachedChildrenIDs.Count; i++)
                    //{
                        
                    //}
                }
            }
        }

        public static NodeVisual GetNodeByID(List<NodeVisual> nodes, int ID)
        {
            for(int i = 0; i < nodes.Count; i++)
            {
                if(ID == nodes[i].id)
                {
                    return nodes[i];
                }
            }

            return null; 
        }

        public static object GetTaskByID(List<object> tasks, int ID)
        {
            for(int i = 0; i < tasks.Count; i++)
            {
                if(tasks[i].GetType() == Type.GetType("Hetwork.SingularTask"))
                {
                    if((tasks[i] as SingularTask).id == ID)
                    {
                        return tasks[i];
                    }
                }
                else if (tasks[i].GetType() == Type.GetType("Hetwork.ListTask"))
                {
                    if ((tasks[i] as ListTask).id == ID)
                    {
                        return tasks[i];
                    }
                }
            }

            return null;
        }
        
    }
}
