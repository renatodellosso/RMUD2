using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemTypes
{
    public class Weapon : Item
    {

        public Dictionary<string, Attack> attacks = new();
        public virtual Attack? Attack => attacks.Values.First();

        int sellValue = 0;
        public override int SellValue => sellValue;

        public Weapon(string id, string name, AbilityScore abilityScore, Die damage, string description = "No description provided", int weight = 0, int sellValue = 0)
            : base(id, name, weight, description)
        {
            attacks.Add(id, new Attack(id, FormattedName, damage, abilityScore, this)); //Make sure that key and id are the same!
            this.sellValue = sellValue;
        }

        public override List<Input> GetInputs(Session session, ItemHolder<Item> item)
        {
            Player player = session.Player ?? throw new Exception("Player is null!");
            List<Input> inputs = new();

            if(player.mainHand != item)
                inputs.Add(new(InputMode.Option, "equip", "Equip in main hand"));

            return inputs;
        }

        public override void HandleInput(Session session, ClientAction action, ItemHolder<Item> item, ref string state, ref bool addStateToPrev)
        {
            Player player = session.Player ?? throw new Exception("Player is null!");
            
            if(action.action == "equip" && (player.mainHand == null || player.mainHand != item))
            {
                ItemHolder<Item> clone = item.Clone();
                
                player.inventory.Remove(item);
                if (player.mainHand != null)
                    player.inventory.Add(player.mainHand);
                
                player.mainHand = clone;

                player.Update();

                state = "inventory";
                addStateToPrev = false;
            }
        }

    }
}