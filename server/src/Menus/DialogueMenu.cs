using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menus
{
    public class DialogueMenu : Menu
    {
        //Set state to "exit" to exit dialogue
        public string state = "";

        public Creature creature;

        public DialogueMenu(Creature creature)
        {
            this.creature = creature;
        }

        public override void OnStart()
        {
            if(creature.talkStart != null)
            {
                creature.talkStart(session);
            }
        }

        public override Input[] GetInputs(ServerResponse response)
        {
            List<Input> inputs = new();

            if(creature.talkInputs != null)
            {
                inputs.AddRange(creature.talkInputs(session, this));
            }

            return inputs.ToArray();
        }

        public override void HandleInput(ClientAction action, ServerResponse response)
        {

            if (creature.talkHandler != null)
                creature.talkHandler(session, action, this);

            if (state.Equals("exit")) //Exit dialogue
                session.SetMenu(new LocationMenu(session));
        }
    }
}