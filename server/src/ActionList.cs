using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionList
{

    public class SetToken : ServerAction<object>
    {
        public SetToken(string token) : base("setToken", token)
        {
        }
    }

        public class SetInput : ServerAction<object>
    {
        public SetInput(params Input[] inputs) : base("setInput", inputs)
        {
        }
    }

}