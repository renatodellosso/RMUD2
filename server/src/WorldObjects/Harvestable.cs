using ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldObjects
{
    public class Harvestable : WorldObject
    {

        string desc, verb, itemId;
        string ActionId => verb.ToLower().Replace(" ", "");
        int minItems, maxItems;

        Func<Player, int[]> getAmtRange; //0 is min, 1 is max

        public Harvestable(string id, string name, string location, string desc, string verb, string itemId, int minItems = 1, int maxItems = 1, 
            Func<Player, int[]>? getAmtRange = null)
            : base(id, name, location)
        {
            this.desc = desc;
            this.verb = verb;
            this.itemId = itemId;
            this.minItems = minItems;
            this.maxItems = maxItems;

            this.getAmtRange = getAmtRange ?? ((player) => new int[] { minItems, maxItems });
        }

        public override string GetOverview(Player player)
        {
            return desc;
        }

        public override List<Input> GetInputs(Player player, string state)
        {
            return new List<Input>()
            {
                new(InputMode.Option, ActionId, verb)
            };
        }

        public override void HandleInput(Session session, ClientAction action, ref string state, ref bool addStateToPrev)
        {
            if (action.action == ActionId)
            {
                int[] amtRange = getAmtRange(session.Player!);
                int amtPicked = Utils.RandInt(amtRange[0], amtRange[1] + 1);
                ItemHolder<Item>? items = session.Player?.inventory.Add(new ItemHolder<Item>(itemId, amtPicked));

                if (items != null)
                {
                    session.Log($"You took {items?.amt} {items?.FormattedName}.");
                    session.Player?.Update();

                    state = "interact";
                    addStateToPrev = false;
                }

                Delete(ref state, ref addStateToPrev);
            }
        }
    }
}
