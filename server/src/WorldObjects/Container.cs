using ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldObjects
{
    public class Container : WorldObject
    {

        Inventory inventory = new();

        public Container(string id, string name, ItemHolder[] items) : base(id, name)
        {
            inventory.Add(items);
        }

        public Container(string id, string name) : this(id, name, Array.Empty<ItemHolder>())
        {

        }

        public override List<Input> GetInputs(Player player, string state)
        {
            string[] args = state.Split('.');
            List<Input> inputs = new();

            if (CanAccess(player))
            {
                //If no item specifier is present
                if(args.Length == 2)
                    foreach(ItemHolder item in inventory)
                        inputs.Add(new(InputMode.Option, item.uid, item.FormattedName));
            }

            return inputs;
        }

        public override void HandleInput(Session session, ClientAction action, ref string state)
        {
            string[] args = state.Split('.');

            if(CanAccess(session.Player))
            {
                if (args.Length == 2)
                {
                    //Item not specified

                }
            }
            else
            {
                session.Log($"You cannot access {FormattedName}");
                action.action = "back"; //Automatically go back a state
            }
        }

        protected virtual bool CanAccess(Creature creature)
        {
            return true;
        }
    }
}
