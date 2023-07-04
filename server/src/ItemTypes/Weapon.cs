using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemTypes
{
    public class Weapon : Equipable<Weapon>
    {

        public Dictionary<string, Attack> attacks = new();
        public virtual Attack? Attack => attacks.Values.First();

        int sellValue = 0;
        public override int SellValue => sellValue;

        protected override string SlotName => "main hand";

        public Weapon(string id, string name, AbilityScore abilityScore, Die damage, string description = "No description provided", int weight = 0, int sellValue = 0)
            : base(id, name, weight, description)
        {
            attacks.Add(id, new Attack(id, FormattedName, damage, abilityScore, this)); //Make sure that key and id are the same!
            this.sellValue = sellValue;
        }

        

        public override string Overview(Dictionary<string, object> data)
        {
            return base.Overview(data) + $"<br>Deals {Attack.damage} damage";
        }

        protected override bool CanEquip(Player player, ItemHolder<Weapon> item)
        {
            return player.mainHand == null || player.mainHand != (object)item;//Weirdly, we can't compare ItemHolder<Armor> to ItemHolder<Item> directly,
                                                                              //so we have to cast one of them to object first
        }

        protected override void Equip(Player player, ItemHolder<Weapon> item)
        {
            player.inventory.Remove(item);

            if (player.mainHand != null)
                player.inventory.Add(player.mainHand);

            player.mainHand = item;
        }
    }
}