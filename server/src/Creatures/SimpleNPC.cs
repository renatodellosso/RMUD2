using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creatures
{
    public class SimpleNPC : Creature
    {

        public SimpleNPC(string name, Func<Player, string, Input[]>? talkInputs = null, Action<Player, ClientAction, string>? talkHandler = null)
        {
            this.name = name;
            attackable = false;

            this.talkInputs = talkInputs;
            this.talkHandler = talkHandler;
        }

    }
}
