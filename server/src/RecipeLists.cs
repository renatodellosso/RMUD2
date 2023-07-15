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
        public static readonly Recipe PICKAXE = new(new ItemHolder<Item>[] { new("log", 1), new("ironbar", 3) }, "pickaxe", 1, 20);
        public static readonly Recipe LESSER_HEALING_POTION = 
            new(new ItemHolder<Item>[] { new("bottle", 1), new("taintedflesh", 1), new("vampiricdust", 1) }, "lesserhealingpotion", 1, 40);
    }

    public static readonly Recipe[] FORGE = new Recipe[]
    {
        All.COAL,
        All.IRON_BAR,
        All.BREASTPLATE,
        All.CHAINMAIL,
        All.SPEAR,
        All.AXE,
        All.PICKAXE
    };

    public static readonly Recipe[] CAMPFIRE = new Recipe[]
    {
        All.COOKED_MEAT,
        All.GRILLED_MUSHROOM,
        All.COAL,
        All.LESSER_HEALING_POTION
    };
}