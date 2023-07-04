using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemTypes
{
    public abstract class Equipable<T> : Item where T : Item
    {

        protected abstract string SlotName { get; } //This is weird, but the internet says we have to

        protected Equipable(string id, string name, float weight, string description = "No description provided") : base(id, name, weight, description)
        {

        }

        public override List<Input> GetInputs(Session session, ItemHolder<Item> item)
        {
            Player player = session.Player ?? throw new Exception("Player is null!");
            List<Input> inputs = new();

            if (player.mainHand != item)
                inputs.Add(new(InputMode.Option, "equip", $"Equip in {SlotName} slot"));

            return inputs;
        }

        public override void HandleInput(Session session, ClientAction action, ItemHolder<Item> item, ref string state, ref bool addStateToPrev)
        {
            Player player = session.Player ?? throw new Exception("Player is null!");
            
            if (action.action == "equip" && CanEquip(session.Player, item as ItemHolder<T>)) //As lets us cast the generic parameter
            {
                try
                {
                    ItemHolder<T> casted = item.Clone<T>();
                    casted.amt = 1;
                    //ItemHolder<T>? casted = item as ItemHolder<T> ?? throw new Exception("ItemHolder<T> is null!");

                    Equip(session.Player, casted);

                    player.Update();

                    state = "inventory";
                    addStateToPrev = false;
                }
                catch (Exception e)
                {
                    Utils.Log($"Caught error equiping Equipable of type: {GetType()}. Error: {e.Message}\n{e.StackTrace}");
                }
            }
        }

        protected abstract bool CanEquip(Player player, ItemHolder<T> item);
        protected abstract void Equip(Player player, ItemHolder<T> item);
    }
}
