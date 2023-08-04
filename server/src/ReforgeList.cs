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
    };

    public static readonly Table<string> WEAPON_REFORGES = new
    (
        new(1, "sharp"),
        new(1, "precise"),
        new(1, "brutal"),
        new(1, "devastating"),
        new(0.5f, "razeredged"),
        new(0.5f, "exacting"),
        new(0.5f, "finisher"),
        new(0.5f, "destroyer"),
        new(0.25f, "ripper"),
        new(0.25f, "elegant"),
        new(0.25f, "obliterating"),
        new(0.25f, "groundshaking")
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

}