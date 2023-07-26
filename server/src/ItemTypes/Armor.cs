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
        public int Defense => GetDefense(null);

        Dictionary<DamageType, int> resistances = new();

        int sellValue = 0;
        public override int SellValue => sellValue;

        Dictionary<AbilityScore, int> abilityScoreBonuses = new();

        public Armor(string id, string name, float weight, int sellValue, int defense = 0, string description = "No description provided",
            Dictionary<DamageType, int>? resistances = null, string color = "white", int strength = 0, int dexterity = 0, int constitution = 0, int agility = 0, 
            int endurance = 0, int intelligence = 0, int wisdom = 0, int charisma = 0)
            : base(id, name, weight, description, color)
        {
            this.defense = defense;
            this.sellValue = sellValue;

            if(resistances != null)
                this.resistances = resistances;

            //If an ability score bonus is not 0, add it to the dictionary
            if(strength != 0)
                abilityScoreBonuses.Add(AbilityScore.Strength, strength);
            if(dexterity != 0)
                abilityScoreBonuses.Add(AbilityScore.Dexterity, dexterity);
            if(constitution != 0)
                abilityScoreBonuses.Add(AbilityScore.Constitution, constitution);
            if(agility != 0)
                abilityScoreBonuses.Add(AbilityScore.Agility, agility);
            if(endurance != 0)
                abilityScoreBonuses.Add(AbilityScore.Endurance, endurance);
            if(intelligence != 0)
                abilityScoreBonuses.Add(AbilityScore.Intelligence, intelligence);
            if(wisdom != 0)
                abilityScoreBonuses.Add(AbilityScore.Wisdom, wisdom);
            if(charisma != 0)
                abilityScoreBonuses.Add(AbilityScore.Charisma, charisma);
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

            if(abilityScoreBonuses.Any())
            {
                msg += "<br>Ability Score Bonuses:";
                foreach(KeyValuePair<AbilityScore, int> abilityScoreBonus in abilityScoreBonuses)
                {
                    msg += $"<br>-{Utils.Modifier(abilityScoreBonus.Value)} {abilityScoreBonus.Key}";
                }
            }

            Reforge reforge = Reforge.Get(item);
            if(reforge != null)
            {
                msg += $"<br>Reforge: {reforge.FormattedName}{reforge.Overview()}"; //Reforge overviews start with a line break
            }

            return msg;
        }

        public virtual int GetDefense(ItemHolder<Item>? item, DamageType? damageType = null)
        {
            if(damageType == null)
               return defense;

            resistances.TryGetValue(damageType.Value, out int resistance);
            return defense + resistance + (Reforge.Get(item)?.defense ?? 0);
        }

        public virtual int GetAbilityScoreBonus(AbilityScore abilityScore, ItemHolder<Item>? item)
        {
            abilityScoreBonuses.TryGetValue(abilityScore, out int bonus); //Bonus should default to 0 if the ability score isn't found
            return bonus + (Reforge.Get(item)?.abilityScores[abilityScore] ?? 0);
        }
    }
}
