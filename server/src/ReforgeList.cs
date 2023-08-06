using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ReforgeList
{

    public static readonly Dictionary<string, Reforge> REFORGES = new()
    {
        //Weapon Reforges
        { "sharp", new("sharp", "Sharp", dmgBonus: 1) },
        { "precise", new("precise", "Precise", atkBonus: 1) },
        { "brutal", new("brutal", "Brutal", critMult: 1) },
        { "devastating", new("devastating", "Devastating", critThreshold: -1) },
        { "draining", new("draining", "Draining", lifesteal: .1f) },
        { "razeredged", new("razeredged", "Razer-Edged", dmgBonus: 2) },
        { "exacting", new("exacting", "Exacting", atkBonus: 2) },
        { "finisher", new("finisher", "Finisher", critMult: 2) },
        { "destroyer", new("destroyer", "Destroyer", critThreshold: -2) },
        { "vampiric", new("vampiric", "Vampiric", lifesteal: .2f) },
        { "ripper", new("ripper", "Ripper", color: "aquamarine", dmgBonus: 3) },
        { "elegant", new("elegant", "Elegant", color: "aquamarine", atkBonus: 3) },
        { "obliterating", new("obliterating", "Obliterating", color: "aquamarine", critMult: 3) },
        { "groundshaking", new("groundshaking", "Groundshaking", color: "aquamarine", critThreshold: -3) },
        { "parasitic", new("parasitic", "Parasitic", color: "aquamarine", lifesteal: .3f) },

        //Armor Reforges
        { "sturdy", new("sturdy", "Sturdy", defense: 1) },
        { "shielding", new("shielding", "Shielding", defense: 2) },
        { "warden", new("warden", "Warden", color: "aquamarine", defense: 3) },

        { "muscled", new("muscled", "Muscled", strength: 1) },
        { "brawny", new("brawny", "Brawny", strength: 3) },
        { "giantstrength", new("giantstrength", "Giant Strength", color: "aquamarine", strength: 6) },
        { "quickfingered", new("quickfingered", "Quickfingered", dexterity: 1) },
        { "slim", new("slim", "Slim", dexterity: 3) },
        { "exact", new("exact", "Exact", color: "aquamarine", strength: 6) },
        { "beefy", new("beefy", "Beefy", constitution: 1) },
        { "reliable", new("reliable", "Reliable", constitution: 3) },
        { "enduring", new("enduring", "Enduring", color: "aquamarine", constitution: 6) },
        { "nimble", new("nimble", "Nimble", agility: 1) },
        { "fleeting", new("fleeting", "Fleeting", agility: 3) },
        { "catlike", new("catlike", "Catlike", color: "aquamarine", agility: 6) },
        { "fresh", new("fresh", "Fresh", endurance: 1) },
        { "outlasting", new("outlasting", "Outlasting", endurance: 3) },
        { "everlasting", new("everlasting", "Everlasting", color: "aquamarine", endurance: 6) },
        //Add intelligence reforges once there's a use for intelligence
        { "alert", new("alert", "Alert", wisdom: 1) },
        { "sentinel", new("sentinel", "Sentinel", wisdom: 3) },
        { "allseeing", new("allseeing", "All-Seeing", color: "aquamarine", wisdom: 6) },
        { "fine", new("fine", "Fine", charisma: 1) },
        { "ornate", new("ornate", "Ornate", charisma: 3) },
        { "exquisite", new("exquisite", "Exquisite", color: "aquamarine", charisma: 6) },

        //Soul Weapon Reforges
        { "infusedweapon", new("infusedweapon", "Infused", color: "darkred", atkBonus: 2, dmgBonus: 2, critThreshold: -1, critMult: .5f) },
        { "rending", new("rending", "Rending", color: "darkred", atkBonus: 2, dmgBonus: 3, critThreshold: -2, critMult: 1f) },
        { "soulrend", new("soulrend", "Soulrend", color: "darkmagenta", atkBonus: 2, dmgBonus: 3, critThreshold: -3, critMult: 2f) },
        { "soulbreaker", new("soulbreaker", "Soulbreaker", color: "darkblue", atkBonus: 2, dmgBonus: 4, critThreshold: -4, critMult: 3f) },
        { "leeching", new("leeching", "Leeching", color : "darkred", atkBonus : 2, dmgBonus : 2, critThreshold : -1, critMult : .5f, lifesteal: .2f) },
        { "soulsnatcher", new("soulsnatcher", "Soul Snatcher", color : "darkmagenta", atkBonus : 3, dmgBonus : 2, critThreshold : -1, critMult : .5f, lifesteal: .3f) },
        { "soulstealer", new("soulstealer", "Soul Stealer", color : "darkblue", atkBonus : 4, dmgBonus : 2, critThreshold : -1, critMult : .5f, lifesteal: .4f) },
        { "seeking", new("seeking", "Seeking", color: "darkred", atkBonus: 4, dmgBonus: 2, critThreshold: -2, critMult: .5f) },
        { "forseeing", new("forseeing", "Forseeing", color: "darkmagenta", atkBonus: 7, dmgBonus: 2, critThreshold: -3, critMult: .5f) },
        { "inevitable", new("inevitable", "Inevitable", color: "darkblue", atkBonus: 10, dmgBonus: 2, critThreshold: -5, critMult: .5f) },

        //Soul Armor Reforges
        { "infusedarmor", new("infusedarmor", "Infused", color: "darkred", defense: 2, strength: 2, dexterity: 2, constitution: 2, agility: 2, endurance: 2, intelligence: 2, wisdom: 2,
            charisma: 2) },
        { "potent", new("potent", "Potent", color: "darkmagenta", defense: 4, strength: 3, dexterity: 3, constitution: 3, agility: 3, endurance: 3, intelligence: 3, wisdom: 3,
            charisma: 3) },
        { "pure", new("pure", "Pure", color: "darkblue", defense: 5, strength: 4, dexterity: 4, constitution: 4, agility: 4, endurance: 4, intelligence: 4, wisdom: 4,
            charisma: 4) },
        { "soulshield", new("soulshield", "Soulshield", color: "darkred", defense: 3, strength: 2, dexterity: 2, constitution: 3, agility: 3, endurance: 2, intelligence: 2, wisdom: 2,
            charisma: 2) },
        { "soulguard", new("soulguard", "Soulguard", color: "darkmagenta", defense: 5, strength: 2, dexterity: 2, constitution: 5, agility: 5, endurance: 2, intelligence: 2, wisdom: 2,
            charisma: 2) },
        { "soullock", new("soullock", "Soullock", color: "darkblue", defense: 7, strength: 2, dexterity: 2, constitution: 7, agility: 7, endurance: 2, intelligence: 2, wisdom: 2,
            charisma: 2) },
        { "vastmind", new("vastmind", "Vastmind", color: "darkred", defense: 2, strength: 2, dexterity: 2, constitution: 2, agility: 2, endurance: 2, intelligence: 3, wisdom: 3,
            charisma: 3) },
        { "truemind", new("truemind", "Truemind", color: "darkmagenta", defense: 2, strength: 2, dexterity: 2, constitution: 2, agility: 2, endurance: 2, intelligence: 5, wisdom: 5,
            charisma: 5) },
        { "endlessmind", new("endlessmind", "Endlessmind", color: "darkblue", defense: 2, strength: 2, dexterity: 2, constitution: 2, agility: 2, endurance: 2, intelligence: 7, wisdom: 7,
            charisma: 7) },
        { "rampage", new("rampage", "Rampage", color: "darkred", defense: 3, strength: 3, dexterity: 3, constitution: 3, agility: 3, endurance: 3, intelligence: 2, wisdom: 2,
            charisma: 2) },
        { "unstoppable", new("unstoppable", "Unstoppable", color: "darkmagenta", defense: 4, strength: 4, dexterity: 4, constitution: 4, agility: 4, endurance: 4, intelligence: 2, wisdom: 2,
            charisma: 2) },
        { "bloodbathed", new("bloodbathed", "Bloodbathed", color: "darkblue", defense: 5, strength: 5, dexterity: 5, constitution: 5, agility: 5, endurance: 5, intelligence: 2, wisdom: 2,
            charisma: 2) },
    };

    public static readonly Table<string> WEAPON_REFORGES = new
    (
        new(1, "sharp"),
        new(1, "precise"),
        new(1, "brutal"),
        new(1, "devastating"),
        new(1, "draining"),
        new(0.5f, "razeredged"),
        new(0.5f, "exacting"),
        new(0.5f, "finisher"),
        new(0.5f, "destroyer"),
        new(0.5f, "vampiric"),
        new(0.25f, "ripper"),
        new(0.25f, "elegant"),
        new(0.25f, "obliterating"),
        new(0.25f, "groundshaking"),
        new(0.25f, "parasitic")
    );

    public static readonly Table<string> ARMOR_REFORGES = new
    (
        new(1, "sturdy"),
        new(0.5f, "shielding"),
        new(0.25f, "warden"),
        new(1, "muscled"),
        new(0.5f, "brawny"),
        new(0.25f, "giantstrength"),
        new(1, "quickfingered"),
        new(0.5f, "slim"),
        new(0.25f, "exact"),
        new(1, "beefy"),
        new(0.5f, "reliable"),
        new(0.25f, "enduring"),
        new(1, "nimble"),
        new(0.5f, "fleeting"),
        new(0.25f, "catlike"),
        new(1, "fresh"),
        new(0.5f, "outlasting"),
        new(0.25f, "everlasting"),
        new(1, "alert"),
        new(0.5f, "sentinel"),
        new(0.25f, "allseeing"),
        new(1, "fine"),
        new(0.5f, "ornate"),
        new(0.25f, "exquisite")
    );

    public static readonly Table<string> SOUL_WEAPON_REFORGES = new
    (
        new(1, "infusedweapon"),
        new(1f, "leeching"),
        new(0.4f, "soulsnatcher"),
        new(0.15f, "soulstealer"),
        new(1f, "rending"),
        new(0.4f, "soulrend"),
        new(0.15f, "soulbreaker"),
        new(1f, "seeking"),
        new(0.4f, "soulseeker"),
        new(0.15f, "soulfinder")
    );

    public static readonly Table<string> SOUL_ARMOR_REFORGES = new
    (
        new(1, "infusedarmor"),
        new(0.4f, "potent"),
        new(0.15f, "pure"),
        new(1f, "soulshield"),
        new(0.4f, "soulguard"),
        new(0.15f, "soullock"),
        new(1f, "vastmind"),
        new(0.4f, "truemind"),
        new(0.15f, "endlessmind")
    );

}