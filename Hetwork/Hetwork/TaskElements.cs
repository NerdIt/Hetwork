using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hetwork
{
    public class SingularTask
    {
        public bool completed = false;
        public string taskTitle = "";
        public string taskContent = "";
        public int id;

        public SingularTask(string name, string body, int ID)
        {
            taskTitle = name;
            taskContent = body;
            id = ID;
        }

        public SingularTask(string name, string body, bool completionStatus)
        {
            taskTitle = name;
            taskContent = body;
            completed = completionStatus;
        }
    }

    public class ListTask
    {
        public bool completed = false;
        public string taskTitle = "";
        public List<SingularTask> elements = new List<SingularTask>();
        public int id;

        public ListTask(string name, List<SingularTask> items, int ID)
        {
            taskTitle = name;
            elements.AddRange(items);
            id = ID;
        }
    }


}
