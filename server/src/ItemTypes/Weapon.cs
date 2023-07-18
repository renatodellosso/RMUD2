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

        protected override bool EquipInHands => true;

        public Weapon(string id, string name, Die damage, DamageType damageType, string description = "No description provided", int weight = 0, int sellValue = 0, 
            AbilityScore? abilityScore = null, string color = "white")
            : base(id, name, weight, description, color)
        {
            //Make sure that key and id are the same!
            attacks.Add(id, new Attack(id, FormattedName, damage, damageType, staminaCost: 2, weapon: this, atkBonusAbilityScore: abilityScore, dmgAbilityScore: abilityScore));
            this.sellValue = sellValue;
        }

        public Weapon(string id, string name, Attack[] attacks, string description = "No description provided", int weight = 0, int sellValue = 0, string color = "white")
            : base(id, name, weight, description, color)
        {
            this.sellValue = sellValue;

            foreach (Attack attack in attacks)
            {
                attack.ApplyWeapon(this);

                this.attacks.Add(attack.id, attack);
            }
        }

        public Weapon(Attack[] attacks, int weight = 0, int sellValue = 0, string color = "white")
            : this("", "", attacks, "", weight, sellValue, color)
        { }

        public override string Overview(ItemHolder<Item> item, Creature? creature = null)
        {
            string msg = base.Overview(item, creature) + "<br>Attack Options:";
            
            foreach(KeyValuePair<string, Attack> attack in attacks)
            {
                msg += $"<br>-{attack.Value.Overview(creature, item)}";
            }

            return msg;
        }

        protected override bool CanEquip(Player player, ItemHolder<Weapon> item, EquipmentSlot slot)
        {
            return player.mainHand == null || player.mainHand != (object)item;//Weirdly, we can't compare ItemHolder<Armor> to ItemHolder<Item> directly,
                                                                              //so we have to cast one of them to object first
        }

        protected override void Equip(Player player, ItemHolder<Weapon> item, EquipmentSlot slot)
        {
            player.inventory.Remove(item);

            if (slot == EquipmentSlot.MainHand)
            {
                if (player.mainHand != null)
                    player.inventory.Add(player.mainHand);

                player.mainHand = item;
            }
            else
            {
                if (player.offHand != null)
                    player.inventory.Add(player.offHand);

                player.offHand = item;
            }
        }
    }
}