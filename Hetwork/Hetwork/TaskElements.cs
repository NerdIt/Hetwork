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
    }

    public class ListTask
    {
        public bool completed = false;
        public string taskTitle = "";
        public List<SingularTask> elements = new List<SingularTask>();
    }


}
