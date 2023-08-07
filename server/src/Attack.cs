using ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Attack
{
    public string id, name;

    public Die damage;

    public AbilityScore dmgAbilityScore, atkBonusAbilityScore;

    public Weapon? weapon;

    public int staminaCost;

    public DamageType damageType;

    int atkBonus, critThreshold;

    float critMult, lifesteal;

    public virtual Action<Creature, Creature, ItemHolder<Weapon>?> execute => Execute;
    public virtual Func<Creature, List<Creature>> getTargets => GetTargets;

    public Attack(string id, string name, Die damage, DamageType damageType, int staminaCost = 2, AbilityScore? dmgAbilityScore = AbilityScore.Strength, 
        AbilityScore? atkBonusAbilityScore = AbilityScore.Dexterity, Weapon? weapon = null, int atkBonus = 0, float critMult = 2, int critThreshold = 20, float lifesteal = 0f)
    {
        atkBonusAbilityScore ??= AbilityScore.Dexterity;
        dmgAbilityScore ??= AbilityScore.Strength;

        this.id = id;
        this.name = name;
        this.damage = damage;
        this.damageType = damageType;
        this.dmgAbilityScore = dmgAbilityScore.Value; //We use .Value because we know it's not null
        this.atkBonusAbilityScore = atkBonusAbilityScore.Value;
        this.weapon = weapon;
        this.staminaCost = staminaCost;
        this.atkBonus = atkBonus;
        this.critThreshold = critThreshold;
        this.critMult = critMult;
        this.lifesteal = lifesteal;
    }

    public int GetStaminaCost(Creature? attacker, ItemHolder<Weapon>? item)
    {
        return staminaCost + (Reforge.Get(item)?.stamina ?? 0);
    }

    Die GetDamage(Creature? attacker, ItemHolder<Weapon>? item)
    {
        Die die = damage.Clone();

        die.modifier += attacker?.GetAbilityScore(dmgAbilityScore) ?? 0;
        die.modifier += Reforge.Get(item)?.dmgBonus ?? 0;

        return die;
    }

    int GetCritThreshold(Creature? attacker, ItemHolder<Weapon>? item)
    {
        return critThreshold + (Reforge.Get(item)?.critTheshold ?? 0);
    }

    float GetCritMult(Creature? attacker, ItemHolder<Weapon>? item)
    {
        return critMult + (Reforge.Get(item)?.critMult ?? 0);
    }

    float GetLifesteal(Creature? attacker, ItemHolder<Weapon>? item)
    {
        return lifesteal + (Reforge.Get(item)?.lifesteal ?? 0);
    }

    public int RollDamage(Creature attacker, Creature target, ItemHolder<Weapon>? item)
    {
        Die die = GetDamage(attacker, item);
        return die.Roll();
    }

    public int AttackBonus(Creature attacker, ItemHolder<Item>? item)
    {
        return attacker.GetAbilityScore(dmgAbilityScore) + atkBonus + (Reforge.Get(item)?.atkBonus ?? 0);
    }

    void Execute(Creature attacker, Creature target, ItemHolder<Weapon>? item)
    {
        Utils.Log($"Executing attack {id} on {target.baseId} from {attacker.baseId}!");

        attacker.stamina -= GetStaminaCost(attacker, item);

        int atkBonus = AttackBonus(attacker, item);
        int baseRoll = Utils.RandInt(20) + 1; //We add 1, since RandInt(20) returns a number from 0 to 19, and we want 1 to 20
        int roll = baseRoll + atkBonus;
        int critThreshold = GetCritThreshold(attacker, item);
        float critMult = GetCritMult(attacker, item);
        float lifesteal = GetLifesteal(attacker, item);

        if (attacker is Player player)
            player.session?.Log($"{roll - atkBonus} + {atkBonus} = {roll} {(roll >= target.DodgeThreshold || baseRoll >= critThreshold ? "Hit!" : "Miss!")}");

        bool crit = baseRoll >= critThreshold;
        if (roll >= target.DodgeThreshold || crit)
        {
            //Hit
            //We calculate the damage so can log how much was dealt, then actually deal the damage. This ensures players who die from the attack still get the log message
            int rolledDmg = RollDamage(attacker, target, item);

            if(crit)
                rolledDmg = (int)Math.Round(rolledDmg * critMult);

            int damage = target.CalculateDamage(rolledDmg, damageType);
            attacker.Location?.Log($"{(crit ? Utils.Style("CRITICAL!", "orangered", true) + " " : "")}{attacker.FormattedName} hit {target.FormattedName} for {damage} " +
                $"({rolledDmg} - {target.GetDefense(damageType)}) {damageType} damage with {name}!");
            target.TakeDamage(damage, damageType, attacker);

            if(lifesteal > 0)
            {
                int heal = (int)Math.Round(damage * lifesteal);
                heal = attacker.Heal(heal);
                attacker.Location?.Log($"{attacker.FormattedName} healed for {heal} health from {name}!");
            }
        }
        else
        {
            //Miss
            attacker.Location?.Log($"{attacker.FormattedName} missed {target.FormattedName} with {name}!");
        }
    }

    List<Creature> GetTargets(Creature attacker)
    {
        List<Creature> targets = new();

        if (attacker.health > 0)
        {
            bool pvp = !attacker.Location?.safe ?? false; //Used to be: bool pvp = attacker.Location?.safe (dumbest line of coded I ever wrote)

            HashSet<Creature> creatures = attacker.Location?.creatures ?? new();
            IEnumerable<Creature>? attackable = creatures?.Where(c => c.attackable) ?? null;

            if (attackable != null)
            {
                if (!pvp)
                    attackable = attackable.Where(c => c is not Player);

                foreach (Creature creature in attackable)
                    if (creature != attacker && creature.attackable && creature.location == attacker.location)
                        targets.Add(creature);
            }
        }

        return targets;
    }

    public bool CanUse(Creature creature, ItemHolder<Weapon>? item)
    {
        return creature.stamina >= GetStaminaCost(creature, item);
    }

    public virtual string Overview(Creature? creature = null, ItemHolder<Weapon>? item = null)
    {
        string msg = name + ":";

        msg += $" {(creature != null ? Utils.Modifier(AttackBonus(creature, item)) : $"+{atkBonusAbilityScore}{Utils.Modifier(atkBonus)}")} to hit.";

        Die damage = this.damage.Clone();
        damage.modifier += creature?.GetAbilityScore(dmgAbilityScore) ?? 0;
        msg += $" Deals {GetDamage(creature, item)}{(creature != null ? "" : $"+{dmgAbilityScore}")} {damageType} damage.";
        msg += $" Costs {GetStaminaCost(creature, item)} stamina.";
        msg += $" Crits on a roll of {GetCritThreshold(creature, item)}+ for {Math.Round(GetCritMult(creature, item), 1)}x damage.";
        
        if(lifesteal > 0)
            msg += $" {Utils.Percent(GetLifesteal(creature, item))} lifesteal.";

        return msg;
    }

    public void ApplyWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        id = weapon.id + "." + id;
        if(weapon.name != "") name += $" ({weapon.FormattedName})";
    }

}
