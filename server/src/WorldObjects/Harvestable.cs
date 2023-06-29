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

        public Harvestable(string id, string name, string location, string desc, string verb, string itemId, int minItems, int maxItems) : base(id, name, location)
        {
            this.desc = desc;
            this.verb = verb;
            this.itemId = itemId;
            this.minItems = minItems;
            this.maxItems = maxItems;
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
                int amtPicked = Utils.RandInt(minItems, maxItems + 1);
                ItemHolder<Item>? items = session.Player?.inventory.Add(new ItemHolder<Item>(itemId, amtPicked));

                if (items != null)
                {
                    session.Log($"You took {items?.amt} {items?.FormattedName}.");
                    session.Player?.Update();
                }

                Delete(ref state, ref addStateToPrev);
            }
        }
    }
}
