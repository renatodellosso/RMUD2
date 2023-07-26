using ItemTypes;
using Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class RecipeLists
{
    public static class All
    {
        //If a recipe is infinitely repeatable (log to coal for example), it can't give XP

        public static readonly Recipe IRON_BAR = new(new ItemHolder<Item>[] { new("ironore", 1), new("coal", 1) }, "ironbar", 1, 5);
        public static readonly Recipe BREASTPLATE = new("ironbar", 4, "breastplate", 1, 20);
        public static readonly Recipe CHAINMAIL = new("ironbar", 6, "chainmail", 1, 30);
        public static readonly Recipe COOKED_MEAT = new(new ItemHolder<Item>[] { new("meat", 1), new("coal", 1) }, "cookedmeat", 1, 5);
        public static readonly Recipe GRILLED_MUSHROOM = new(new ItemHolder<Item>[] { new("mushroom", 1), new("coal", 1) }, "grilledmushroom", 1, 5);
        public static readonly Recipe COAL = new("log", 1, "coal", 1); //If we add multiple ways to get coal, maybe change this to COAL_FROM_LOG. This used to infinitely repeatable, so it can't give XP
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
            new(new ItemHolder<Item>[] { new("bottle", 1), new("taintedflesh", 1), new("aberrantcluster", 1) }, "lesserstaminapotion", 1, 60);
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
        public static readonly Recipe SPORE = new("mushroom", 2, "spore", 1, 25);
        public static readonly Recipe LIVING_STONE_ARMOR =
            new(new ItemHolder<Item>[] { new("livingstone", 8), new("ironbar", 15), new("cloth", 10) }, "livingstonearmor", 1, 325);
        public static readonly Recipe PYROMANCER_CLOAK =
            new(new ItemHolder<Item>[] { new("ember", 10), new("firesteel", 3), new("cloth", 25) }, "pyromancercloak", 1, 275);
        public static readonly Recipe FIRE_STEEL =
            new(new ItemHolder<Item>[] { new("darksteel", 1), new("ember", 3) }, "firesteel", 1, 40);
        public static readonly Recipe FLAME_TRIDENT =
            new(new ItemHolder<Item>[] { new("ember", 10), new("firesteel", 5), new("cloth", 3), new("spear") }, "flametrident", 1, 275);
        public static readonly Recipe GLACIER_ARMOR =
            new(new ItemHolder<Item>[] { new("livingstone", 3), new("ice", 10), new("frostshard", 8) }, "glacierarmor", 1, 275);
        public static readonly Recipe FROSTBITE =
            new(new ItemHolder<Item>[] { new("darksteel", 12), new("ice", 10), new("frostshard", 12) }, "frostbite", 1, 275);
        public static readonly Recipe BLOODRIPPER =
            new(new ItemHolder<Item>[] { new("darksteel", 16), new("holyblood", 10), new("taintedflesh", 12) }, "bloodripper", 1, 325);
        public static readonly Recipe ENDBRINGER =
            new(new ItemHolder<Item>[] { new("unholyknife"), new("hereticsword"), new("mindbreaker"), new("druidaxe"), new("flametrident"), new("landslide"), new("boneclub"),
            new("frostbite"), new("mindreaper"), new("bloodripper") }, "endbringer", 1, 1000);
        public static readonly Recipe APPRENTICE_ROBE =
            new("cloth", 15, "apprenticerobe", 1, 125);
        public static readonly Recipe JOURNEYMAN_ROBE =
            new(new ItemHolder<Item>[] { new("apprenticerobe", 1), new("spore", 8), new("vampiricdust", 4) }, "journeymanrobe", 1, 250);
        public static readonly Recipe SAGE_ROBE =
            new(new ItemHolder<Item>[] { new("journeymanrobe", 1), new("aberrantcluster", 8), new("holyblood", 4), new("frostshard", 4), new("shadowessence", 4), 
                new("livingstone") }, "sagerobe", 1, 500);
        public static readonly Recipe HELLSTEEL =
            new(new ItemHolder<Item>[] { new("firesteel", 1), new("darksteel", 2), new("holyblood", 1), new("ember", 3), new("brimstone") }, "hellsteel", 1, 80);
        public static readonly Recipe TRUE_PYROMANCER_CLOAK =
            new(new ItemHolder<Item>[] { new("pyromancercloack", 1), new("hellsteel", 5), new("holyblood", 10), new("brimstone", 5), new("vampiricdust", 5), new("coal", 10) }, 
                "truepyromancercloak", 1, 1200);
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
        All.MIND_BREAKER,
        All.FIRE_STEEL,
        All.FLAME_TRIDENT,
    };

    public static readonly Recipe[] CAMPFIRE = new Recipe[]
    {
        All.COOKED_MEAT,
        All.GRILLED_MUSHROOM,
        All.COAL,
        All.LESSER_HEALING_POTION,
        All.HEALING_POTION,
        All.LESSER_STAMINA_POTION,
    };

    public static readonly Recipe[] LOOM = new Recipe[]
    {
        All.DARK_CLOAK,
        All.PYROMANCER_CLOAK,
        All.APPRENTICE_ROBE,
        All.JOURNEYMAN_ROBE,
        All.SAGE_ROBE
    };

    public static readonly Recipe[] GROVE = new Recipe[]
    {
        All.SPORE,
        All.DRUID_AXE,
        All.FUNGAL_ARMOR,
        All.LIVING_STONE_ARMOR,
        All.GLACIER_ARMOR,
        All.FROSTBITE,
        All.BLOODRIPPER,
    };

    public static readonly Recipe[] UNHOLY_ALTAR = new Recipe[]
    {
        All.ENDBRINGER
    };

    public static readonly Recipe[] DEMON_STATUE = new Recipe[]
    {
        All.FIRE_STEEL,
        All.FLAME_TRIDENT,
        All.PYROMANCER_CLOAK,
        All.BLOODRIPPER,
        All.HELLSTEEL,
        All.TRUE_PYROMANCER_CLOAK
    };

    static readonly Recipe[] MYSTERIOUS_TRADER = new Recipe[]
    {
        new("returnscroll", 1.5f),
        new("healingpotion", 2),
        new("aberrantcluster", 3),
        new("vampiricdust", 2),
        new("spore", 2),
        new("darksteel", 2),
        new("hereticsword", 5),
        new("unholyknife", 5),
        new("clericarmor", 5),
        new("fungalarmor", 5),
        new("mindbreaker", 6),
        new("druidaxe", 5),
        new("ale", 1.2f),
        new(DungeonTeleportationScroll.GetId(0)),
        new(DungeonTeleportationScroll.GetId(1)),
        new(DungeonTeleportationScroll.GetId(2)),
        new(DungeonTeleportationScroll.GetId(3)),
        new(DungeonTeleportationScroll.GetId(4)),
        new(DungeonTeleportationScroll.GetId(5)),
        new(DungeonTeleportationScroll.GetId(6)),
    };

    public static Recipe[] GenMysteriousTraderInventory()
    {
        HashSet<Recipe> shop = new(); //Use a hashset to prevent duplicates
        int offerCount = Utils.RandInt(Config.Gameplay.MYSTERIOUS_TRADER_MIN_OFFERS, Config.Gameplay.MYSTERIOUS_TRADER_MAX_OFFERS+1);

        int tries = 0;
        while(shop.Count < offerCount && tries < Config.Gameplay.MYSTERIOUS_TRADER_MAX_OFFERS * 2)
        {
            shop.Add(MYSTERIOUS_TRADER[Utils.RandInt(MYSTERIOUS_TRADER.Length)]);
            tries++;
        }

        return shop.ToArray();
    }

}