using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class Reforge
{

    public string id = "ID NOT SET!", name = "NAME NOT SET!", color = "";
    public string FormattedName => color != "" ? Utils.Style(name, color) : name;

    public int atkBonus = 0, dmgBonus = 0, critTheshold = 0, defense = 0, stamina = 0;
    public float critMult = 0, lifesteal = 0;

    public Dictionary<AbilityScore, int> abilityScores = new()
    {
        { AbilityScore.Strength, 0 },
        { AbilityScore.Dexterity, 0 },
        { AbilityScore.Constitution, 0 },
        { AbilityScore.Agility, 0 },
        { AbilityScore.Endurance, 0 },
        { AbilityScore.Intelligence, 0 },
        { AbilityScore.Wisdom, 0 },
        { AbilityScore.Charisma, 0}
    };

    public Reforge(string id, string name, string color = "", int atkBonus = 0, int dmgBonus = 0, int critThreshold = 0, int defense = 0, float critMult = 0, int stamina = 0, float lifesteal = 0,
        int strength = 0, int dexterity = 0, int constitution = 0, int agility = 0, int endurance = 0, int intelligence = 0, int wisdom = 0, int charisma = 0)
    {
        this.id = id;
        this.name = name;
        this.color = color;

        this.atkBonus = atkBonus;
        this.dmgBonus = dmgBonus;
        this.critTheshold = critThreshold;
        this.defense = defense;
        this.critMult = critMult;
        this.stamina = stamina;
        this.lifesteal = lifesteal;

        abilityScores[AbilityScore.Strength] = strength;
        abilityScores[AbilityScore.Dexterity] = dexterity;
        abilityScores[AbilityScore.Constitution] = constitution;
        abilityScores[AbilityScore.Agility] = agility;
        abilityScores[AbilityScore.Endurance] = endurance;
        abilityScores[AbilityScore.Intelligence] = intelligence;
        abilityScores[AbilityScore.Wisdom] = wisdom;
        abilityScores[AbilityScore.Charisma] = charisma;
    }

    //In here since it's shorter than ReforgeList
    public static Reforge? Get(string id)
    {
        ReforgeList.REFORGES.TryGetValue(id, out Reforge? reforge);
        return reforge;
    }

    public static Reforge? Get(ItemHolder<ItemTypes.Item>? item)
    {
        if(item?.data.TryGetValue("reforge", out object? reforge) ?? false)
        {
            return Get((string)reforge);
        }
        return null;
    }

    public string Overview()
    {
        string msg = "";

        if (atkBonus != 0)
            msg += $"<br>-Attack Bonus: {Utils.Modifier(atkBonus)}";
        if (dmgBonus != 0)
            msg += $"<br>-Damage Bonus: {Utils.Modifier(dmgBonus)}";
        if (critTheshold != 0)
            msg += $"<br>-Critical Hit Threshold: {Utils.Modifier(critTheshold)}";
        if (critMult != 0)
            msg += $"<br>-Critical Hit Multiplier: {Utils.PercentModifer(critMult)}";
        if (stamina != 0)
            msg += $"<br>-Stamin Cost: {Utils.Modifier(stamina)}";
        if (lifesteal != 0)
            msg += $"<br>-Lifesteal: {Utils.PercentModifer(lifesteal)}";

        if (defense != 0)
            msg += $"<br>-Defense: {Utils.Modifier(defense)}";

        foreach (KeyValuePair<AbilityScore, int> abilityScore in abilityScores)
        {
            if(abilityScore.Value != 0)
                msg += $"<br>-{abilityScore.Key}: {Utils.Modifier(abilityScore.Value)}";
        }

        return msg;
    }

}
