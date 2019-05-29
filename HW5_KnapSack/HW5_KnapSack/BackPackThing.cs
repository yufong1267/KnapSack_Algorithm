using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW5_KnapSack
{
    class BackPackThing
    {
        public int thing_num = 0;
        public int thing_weight = 0;
        public int thing_value = 0;

        public BackPackThing(int num)
        {
            this.thing_num = num;
        }
        public BackPackThing(int num , int weight)
        {
            this.thing_num = num;
            this.thing_weight = weight;
        }
        public BackPackThing(int num, int weight , int value)
        {
            this.thing_num = num;
            this.thing_weight = weight;
            this.thing_value = value;
        }

        public void store_weight(int weight)
        {
            this.thing_weight = weight;
        }
        public void store_value(int value)
        {
            this.thing_value = value;
        }
    }
}
