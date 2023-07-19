using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menus
{
    public class CharacterMenu : Menu
    {

        public override bool ShowSidebar => true;

        public override void OnStart()
        {
            session?.Log(session.Player!.GetCharacterText());
        }

        public override Input[] GetInputs(ServerResponse response)
        {
            List<Input> inputs = new()
            {
                back,
                new(InputMode.Option, "mainHand", "Main Hand"),
                new(InputMode.Option, "offHand", "Off Hand"),
                new(InputMode.Option, "armor", "Armor"),
            };

            return inputs.ToArray();
        }

        public override void HandleInput(ClientAction action, ServerResponse response)
        {
            if (action.action == "back")
                session?.SetMenu(new LocationMenu(session));
            else if(action.action == "mainHand")
            {
                Player? player = session?.Player;

                if(player == null || player.mainHand == null)
                    session?.Log("You have no weapon equipped.");
                else session?.Log(player.mainHand.Overview(player));
            }
            else if (action.action == "offHand")
            {
                Player? player = session?.Player;

                if (player == null || player.offHand == null)
                    session?.Log("You have no weapon equipped.");
                else session?.Log(player.offHand.Overview(player));
            }
            else if(action.action == "armor")
            {
                Player? player = session?.Player;

                if (player == null || player.armor == null)
                    session?.Log("You have no armor equipped.");
                else session?.Log(player.armor.Overview(player));
            }
        }
    }
}
