using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemTypes
{
    public interface IConsumable
    {

        //Forces derived classes to implement this method
        string Verb();

        Input GetInput()
        {
            return new Input(InputMode.Option, "use", Verb());
        }

        void Use(Session session, ref string state, ref bool addStateToPrev, Dictionary<string, object> data);

    }
}
