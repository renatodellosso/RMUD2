using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menus
{
    public class HelpMenu : Menu
    {

        class HelpText
        {
            public string id, name, text;

            public HelpText(string name, string text)
            {
                id = name.ToLower().Replace(" ", "");
                this.name = name;
                this.text = text;
            }
        }

        public override string Status => session.Player.Location.status;
        public override bool ShowSidebar => true;

        readonly HelpText[] HELP_TEXTS = new HelpText[]
        {
            new("General", Utils.Style("Welcome to RMUD2!", bold: true) +
                "<br>Thanks for playing my game. The core loop you'll need to do is explore the dungeon, kill monsters and loot their corpses, then use your loot to craft better items." +
                "<br>You are heavily encouraged to poke around in the various menus and locations. See what you can find!"),
            new("Combat", "To fight monsters, you'll need to select an attack with your sidebar (on the right), then click an enemy name to attack (below the attack list)." +
                "<br>Attacking requires stamina, with the amount required listed in parentheses after the attack name."),
            new("Bestiary", "Accessed through Character > Bestiary, the bestiary tracks every monster you've killed. It can be used to view the stats, attacks, and drops of those monsters."),
            new("Ability Scores", "Each ability score provides a different bonus:<br>" +
                $"STR: +{Config.Gameplay.CARRY_WEIGHT_PER_STR} lbs. max carry weight and +1 damage<br>" +
                $"DEX: +1 attack bonus<br>" +
                $"CON: +{Config.Gameplay.HP_PER_CON} HP<br>" +
                $"AGI: Alternates +1 dodge threshold and +{Utils.Percent(Config.Gameplay.STAMINA_REGEN_PER_EVERY_OTHER_AGI)} stamina regen<br>" +
                $"END: +{Config.Gameplay.STAMINA_PER_END} max stamina<br>" +
                $"WIS: +{Utils.Percent(Config.Gameplay.XP_PER_WIS)} XP<br>" +
                $"CHA: +{Config.Gameplay.SELL_CUT_PER_CHA * 100}% sell cut"),
        };

        public HelpMenu(Session session)
        {
            this.session = session;
        }

        public override void OnStart()
        {
            session?.Log("Select what you want help with:");
        }

        public override Input[] GetInputs(ServerResponse response)
        {
            List<Input> inputs = new()
            {
                back
            };

            foreach(HelpText text in HELP_TEXTS)
                inputs.Add(new Input(text.id, text.name));

            return inputs.ToArray();
        }

        public override void HandleInput(ClientAction action, ServerResponse response)
        {
            if(action.action == "back")
            {
                session?.SetMenu(new LocationMenu(session));
            }
            else
            {
                foreach(HelpText text in HELP_TEXTS)
                {
                    if(action.action == text.id)
                    {
                        session?.Log(text.text);
                        return;
                    }
                }
            }
        }

    }
}
