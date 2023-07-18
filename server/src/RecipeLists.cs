using ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class RecipeLists
{
    public static class All
    {
        public static readonly Recipe IRON_BAR = new(new ItemHolder<Item>[] { new("ironore", 1), new("coal", 1) }, "ironbar", 1, 5);
        public static readonly Recipe BREASTPLATE = new("ironbar", 4, "breastplate", 1, 20);
        public static readonly Recipe CHAINMAIL = new("ironbar", 6, "chainmail", 1, 30);
        public static readonly Recipe COOKED_MEAT = new(new ItemHolder<Item>[] { new("meat", 1), new("coal", 1) }, "cookedmeat", 1, 5);
        public static readonly Recipe GRILLED_MUSHROOM = new(new ItemHolder<Item>[] { new("mushroom", 1), new("coal", 1) }, "grilledmushroom", 1, 5);
        public static readonly Recipe COAL = new("log", 1, "coal", 1, 5); //If we add multiple ways to get coal, maybe change this to COAL_FROM_LOG
        public static readonly Recipe SPEAR = new(new ItemHolder<Item>[] { new("log", 1), new("ironbar", 1) }, "spear", 1, 10);
        public static readonly Recipe AXE = new(new ItemHolder<Item>[] { new("log", 1), new("ironbar", 3) }, "axe", 1, 20);
        public static readonly Recipe LONGSWORD = new(new ItemHolder<Item>[] { new("log", 1), new("ironbar", 5) }, "longsword", 1, 40);
        public static readonly Recipe MACE = new("ironbar", 5, "mace", 1, 45);
        public static readonly Recipe PICKAXE = new(new ItemHolder<Item>[] { new("log", 1), new("ironbar", 3) }, "pickaxe", 1, 20);
        public static readonly Recipe LESSER_HEALING_POTION = 
            new(new ItemHolder<Item>[] { new("bottle", 1), new("taintedflesh", 1), new("vampiricdust", 1) }, "lesserhealingpotion", 1, 40);
        public static readonly Recipe HEALING_POTION =
            new(new ItemHolder<Item>[] { new("bottle", 1), new("holyblood", 2), new("vampiricdust", 2) }, "healingpotion", 1, 70);
        public static readonly Recipe LESSER_STAMINA_POTION =
            new(new ItemHolder<Item>[] { new("bottle", 1), new("taintedflesh", 1), new("aberrantchunk", 1) }, "lesserstaminapotion", 1, 60);
        public static readonly Recipe DARK_CLOAK =
            new(new ItemHolder<Item>[] { new("cloth", 10), new("shadowessence", 3) }, "darkcloak", 1, 100);
        public static readonly Recipe BONE_CLUB = new("bone", 15, "boneclub", 1, 80);
        public static readonly Recipe UNHOLY_KNIFE =
            new(new ItemHolder<Item>[] { new("ironbar", 2), new("vampiricdust", 10), new("shadowessence", 4) }, "unholyknife", 1, 120);
        public static readonly Recipe DARK_STEEL =
            new(new ItemHolder<Item>[] { new("ironbar", 1), new("shadowessence", 1) }, "darksteel", 1, 20);
        public static readonly Recipe DARK_STEEL_BREASTPLATE =
            new(new ItemHolder<Item>[] { new("darksteel", 4), new("shadowessence", 2), new("cloth", 2) }, "darksteelbreastplate", 1, 150);
        public static readonly Recipe HERETIC_SWORD =
            new(new ItemHolder<Item>[] { new("darksteel", 6), new("shadowessence", 2), new("vampiricdust", 3), new("cloth", 2) }, "hereticsword", 1, 175);
        public static readonly Recipe DRUID_AXE =
            new(new ItemHolder<Item>[] { new("log", 10), new("ironbar", 4), new("spore", 5) }, "druidaxe", 1, 175);
        public static readonly Recipe CLERIC_ARMOR =
            new(new ItemHolder<Item>[] { new("ironbar", 10), new("holyblood", 4), new("cloth", 5) }, "clericarmor", 1, 225);
        public static readonly Recipe FUNGAL_ARMOR =
            new(new ItemHolder<Item>[] { new("log", 10), new("spore", 5), new("holyblood", 1) }, "fungalarmor", 1, 225);
        public static readonly Recipe MIND_BREAKER =
            new(new ItemHolder<Item>[] { new("darksteel", 5), new("aberrantcluster", 5), new("holyblood", 1), new("spore", 1) }, "mindbreaker", 1, 225);
    }

    public static readonly Recipe[] FORGE = new Recipe[]
    {
        All.COAL,
        All.IRON_BAR,
        All.BREASTPLATE,
        All.CHAINMAIL,
        All.SPEAR,
        All.AXE,
        All.PICKAXE,
        All.LONGSWORD,
        All.MACE,
        All.DARK_STEEL,
        All.BONE_CLUB,
        All.UNHOLY_KNIFE,
        All.DARK_STEEL_BREASTPLATE,
        All.HERETIC_SWORD,
        All.CLERIC_ARMOR,
        All.MIND_BREAKER
    };

    public static readonly Recipe[] CAMPFIRE = new Recipe[]
    {
        All.COOKED_MEAT,
        All.GRILLED_MUSHROOM,
        All.COAL,
        All.LESSER_HEALING_POTION,
        All.HEALING_POTION,
        All.LESSER_STAMINA_POTION,
        All.DRUID_AXE,
        All.FUNGAL_ARMOR
    };

    public static readonly Recipe[] LOOM = new Recipe[]
    {
        All.DARK_CLOAK
    };
}