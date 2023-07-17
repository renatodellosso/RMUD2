using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemTypes
{
    public class Armor : Equipable<Armor>
    {

        protected override string SlotName => "armor";

        public int defense = 0;

        int sellValue = 0;
        public override int SellValue => sellValue;

        public Armor(string id, string name, float weight, int sellValue, int defense = 0, string description = "No description provided") : base(id, name, weight, description)
        {
            this.defense = defense;
            this.sellValue = sellValue;
        }

        protected override bool CanEquip(Player player, ItemHolder<Armor> item)
        {
            return player.armor == null || player.armor != item; 
        }

        protected override void Equip(Player player, ItemHolder<Armor> item)
        {
            player.inventory.Remove(item);

            if (player.armor != null)
                player.inventory.Add(player.armor, true);

            player.armor = item;
        }

        public override string Overview(ItemHolder<Item> item, Creature? creature = null)
        {
            return base.Overview(item, creature) + $"<br>Defense: {defense}";
        }
    }
}
