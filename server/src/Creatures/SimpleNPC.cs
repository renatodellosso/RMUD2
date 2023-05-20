using Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creatures
{
    public class SimpleNPC : Creature
    {

        public SimpleNPC(string id, string name, Func<Session, DialogueMenu, Input[]>? talkInputs = null, Action<Session, ClientAction, DialogueMenu>? talkHandler = null, 
            Action<Session>? talkStart = null)
        {
            baseId = id;
            this.name = name;
            attackable = false;

            this.talkInputs = talkInputs;
            this.talkHandler = talkHandler;
            this.talkStart = talkStart;
        }

    }
}
