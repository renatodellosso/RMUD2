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

        public Weapon(string id, string name, AbilityScore abilityScore, Die damage, string description = "No description provided", int weight = 0)
            : base(id, name, weight, description)
        {
            attacks.Add(id, new Attack(id, FormattedName, damage, abilityScore, this)); //Make sure that key and id are the same!
        }

    }
}