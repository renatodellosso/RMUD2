﻿using ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Attack
{
    public string id, name;

    public Die damage;

    public AbilityScore abilityScore;

    public Weapon weapon;

    public virtual Action<Creature, Creature> execute => Execute;
    public virtual Func<Creature, List<Creature>> getTargets => GetTargets;

    public Attack(string id, string name, Die damage, AbilityScore abilityScore, Weapon weapon)
    {
        this.id = id;
        this.name = name;
        this.damage = damage;
        this.abilityScore = abilityScore;
        this.weapon = weapon;
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

    void Execute(Creature attacker, Creature target)
    {
        Utils.Log($"Executing attack {id} on {target.baseId} from {attacker.baseId}!");

        int atkBonus = AttackBonus(attacker);        
        int roll = Utils.RandInt(20) + 1 + atkBonus; //We add 1, since RandInt(20) returns a number from 0 to 19, and we want 1 to 20

        if (attacker is Player)
            ((Player)attacker).session?.Log($"{(roll - atkBonus)} + {atkBonus} = {roll} {(roll >= target.DodgeThreshold ? "Hit!" : "Miss!")}");

        if (roll >= target.DodgeThreshold)
        {

            //Hit
            //We calculate the damage so can log how much was dealt, then actually deal the damage. This ensures players who die from the attack still get the log message
            int rolledDmg = RollDamage(attacker, target);
            int damage = target.CalculateDamage(rolledDmg);
            attacker.Location?.Log($"{attacker.FormattedName} hit {target.FormattedName} for {damage} ({rolledDmg} - {rolledDmg-damage}) damage with {weapon.FormattedName}!");
            target.TakeDamage(damage, attacker);
        }
        else
        {
            //Miss
            attacker.Location?.Log($"{attacker.FormattedName} missed {target.FormattedName} with {weapon.FormattedName}!");
        }
    }

    List<Creature> GetTargets(Creature attacker)
    {
        List<Creature> targets = new();

        bool pvp = attacker.Location?.safe ?? false;

        List<Creature> creatures = attacker.Location?.creatures ?? new();
        IEnumerable<Creature>? attackable = creatures?.Where(c => c.attackable) ?? null;

        if (attackable != null)
        {
            if(!pvp)
                attackable = attackable.Where(c => c is not Player);

            foreach (Creature creature in attackable)
                if (creature != attacker && creature.attackable)
                    targets.Add(creature);
        }

        return targets;
    }

}
