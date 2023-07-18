using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemTypes
{
    public class Armor : Equipable<Armor>
    {

        protected override bool EquipInArmor => true;

        int defense = 0;
        public int Defense => GetDefense();

        Dictionary<DamageType, int> resistances = new();

        int sellValue = 0;
        public override int SellValue => sellValue;

        public Armor(string id, string name, float weight, int sellValue, int defense = 0, string description = "No description provided",
            Dictionary<DamageType, int>? resistances = null, string color = "white") : base(id, name, weight, description, color)
        {
            this.defense = defense;
            this.sellValue = sellValue;

            if(resistances != null)
                this.resistances = resistances;
        }

        protected override bool CanEquip(Player player, ItemHolder<Armor> item, EquipmentSlot slot)
        {
            return player.armor == null || player.armor != item; 
        }

        protected override void Equip(Player player, ItemHolder<Armor> item, EquipmentSlot slot)
        {
            player.inventory.Remove(item);

            if (player.armor != null)
                player.inventory.Add(player.armor, true);

            player.armor = item;
        }

        public override string Overview(ItemHolder<Item> item, Creature? creature = null)
        {
            string msg = base.Overview(item, creature) + $"<br>Defense: {Defense}";

            if(resistances.Any())
            {
                foreach(KeyValuePair<DamageType, int> resistance in resistances)
                {
                    msg += $"<br>-{resistance.Key}: {Utils.Modifier(resistance.Value)} defense";
                }
            }

            return msg;
        }

        public virtual int GetDefense(DamageType? damageType = null)
        {
            if(damageType == null)
               return defense;

            resistances.TryGetValue(damageType.Value, out int resistance);
            return defense + resistance;
        }
    }
}
