using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemTypes
{
    public abstract class Equipable<T> : Item where T : Item
    {

        protected virtual bool EquipInHands => false;
        protected virtual bool EquipInArmor => false;

        protected Equipable(string id, string name, float weight, string description = "No description provided", string color = "white")
            : base(id, name, weight, description, color)
        {

        }

        public override List<Input> GetInputs(Session session, ItemHolder<Item> item, string state)
        {
            Player player = session.Player ?? throw new Exception("Player is null!");
            List<Input> inputs = base.GetInputs(session, item, state);

            if (EquipInHands && player.mainHand != item && player.offHand != item)
            {
                inputs.Add(new(InputMode.Option, "equipMainHand", $"Equip in main hand"));
                inputs.Add(new(InputMode.Option, "equipOffHand", $"Equip in off hand"));
            }
            else if (EquipInArmor && (ItemHolder<Item>)player.armor != item)
            {
                inputs.Add(new(InputMode.Option, "equipArmor", $"Equip in armor slot"));
            }

            return inputs;
        }

        public override void HandleInput(Session session, ClientAction action, ItemHolder<Item> item, ref string state, ref bool addStateToPrev)
        {
            Player player = session.Player ?? throw new Exception("Player is null!");
            
            EquipmentSlot slot = (EquipmentSlot)Enum.Parse(typeof(EquipmentSlot), action.action["equip".Length..]);

            if (action.action.StartsWith("equip") && CanEquip(session.Player, item as ItemHolder<T>, slot)) //As lets us cast the generic parameter
            {
                try
                {
                    ItemHolder<T> casted = item.Clone<T>();
                    casted.amt = 1;
                    //ItemHolder<T>? casted = item as ItemHolder<T> ?? throw new Exception("ItemHolder<T> is null!");

                    Equip(session.Player, casted, slot);

                    player.Update();

                    session.Log($"Equipped {item.FormattedName}");

                    state = "inventory";
                    addStateToPrev = false;
                }
                catch (Exception e)
                {
                    Utils.Log($"Caught error equiping Equipable of type: {GetType()}. Error: {e.Message}\n{e.StackTrace}");
                }
            }
            else base.HandleInput(session, action, item, ref state, ref addStateToPrev);
        }

        protected abstract bool CanEquip(Player player, ItemHolder<T> item, EquipmentSlot slot);
        protected abstract void Equip(Player player, ItemHolder<T> item, EquipmentSlot slot);
    }
}

public enum EquipmentSlot
{
    MainHand,
    OffHand,
    Armor
}