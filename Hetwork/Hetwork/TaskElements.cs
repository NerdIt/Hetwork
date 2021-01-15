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

        public SingularTask(string name, string body)
        {
            taskTitle = name;
            taskContent = body;
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

        public ListTask(string name, List<SingularTask> items)
        {
            taskTitle = name;
            elements.AddRange(items);
        }
    }


}
