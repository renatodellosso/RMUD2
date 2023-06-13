using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemTypes
{
    public class Weapon : Item
    {

        public Die damage;

        public AbilityScore abilityScore;

        public Weapon(string id, string name, AbilityScore abilityScore, Die damage) : base(id, name)
        {
            this.damage = damage;
            this.abilityScore = abilityScore;
        }

        public int RollDamage(Creature attacker, Creature target)
        {
            Die die = damage.Clone();

            die.modifier = () => attacker.abilityScores[abilityScore];

            return die.Roll();
        }

        public int AttackBonus(Creature attacker)
        {
            return attacker.abilityScores[abilityScore];
        }

    }
}