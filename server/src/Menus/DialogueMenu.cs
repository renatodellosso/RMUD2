using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menus
{
    public class DialogueMenu : Menu
    {

        public string state = "";

        public Creature creature;

        public DialogueMenu(Creature creature)
        {
            this.creature = creature;
        }

        public override Input[] GetInputs(ServerResponse response)
        {
            List<Input> inputs = new();

            if(creature.talkInputs != null)
            {
                inputs.AddRange(creature.talkInputs(session.Player, state));
            }

            return inputs.ToArray();
        }

        public override void HandleInput(ClientAction action, ServerResponse response)
        {
            if (creature.talkHandler != null)
                creature.talkHandler(session.Player, action, state);
        }
    }
}