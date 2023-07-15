using Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldObjects;

namespace WorldObjects
{
    class CraftingStation : WorldObject
    {

        Recipe[] recipes;

        public CraftingStation(string id, string name, string location, Recipe[] recipes) : base(id, name, location)
        {
            this.recipes = recipes;
        }

        public override List<Input> GetInputs(Player player, string state)
        {
            return new List<Input>()
            {
                new(InputMode.Option, "craft", "Craft"),
            };
        }

        public override void HandleInput(Session session, ClientAction action, ref string state, ref bool addStateToPrev)
        {
            if(action.action == "back")
            {
                state = "interact";
                addStateToPrev = false;
            }
            else if(action.action == "craft")
            {
                session.SetMenu(new CraftingMenu(name, recipes));
            }
        }
    }
}
