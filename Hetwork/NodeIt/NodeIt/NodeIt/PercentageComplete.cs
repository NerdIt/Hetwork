using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeIt
{
    public class PercentageComplete
    {
        public float complete = 0;
        public float incomplete = 0;
        public float total = 0;

        public PercentageComplete(float c, float i, float t)
        {
            complete = c;
            incomplete = i;
            total = t;
        }

        public float GetPercentage()
        {
            if(total == 0)
            {
                return 0;
            }

            return complete / total * 100;
        }
    }
}
