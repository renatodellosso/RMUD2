using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menus
{
    public class CharacterMenu : Menu
    {

        public override void OnStart()
        {
            session?.Log(session.Player!.GetCharacterText());
        }

        public override Input[] GetInputs(ServerResponse response)
        {
            List<Input> inputs = new()
            {
                back
            };

            return inputs.ToArray();
        }

        public override void HandleInput(ClientAction action, ServerResponse response)
        {
            if (action.action == "back")
                session?.SetMenu(new LocationMenu(session));
        }
    }
}
