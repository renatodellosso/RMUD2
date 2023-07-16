using ItemTypes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Items
{
    public static class ItemList
    {
        //We use ConcurrentDictionary instead of Dictionary because it is thread-safe
        //https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2?view=net-7.0
        static readonly ConcurrentDictionary<string, Item> ITEMS = new(new Dictionary<string, Item>()
        {
            { "coin", new SimpleItem("coin", "Gold Coin", 0.05f, "A shiny gold coin.", sellValue : 1, color: "yellow") },

            //Weapons
            { "spear", new Weapon("spear", "Spear", new Attack[]
                {
                    new("stab", "Stab", 8, AbilityScore.Strength, 2),
                    new("power", "Power Thrust", 10, AbilityScore.Strength, 3)
                }, "A stick with a pointy end.", 5, sellValue: 20) },
            { "axe", new Weapon("axe", "Axe", new Attack[]
                {
                    new("chop", "Chop", 8, AbilityScore.Strength, 2),
                    new("swing", "Swing", new(4, 3), AbilityScore.Strength, 4)
                }, "Good for chopping, both logs and necks.", 8, sellValue: 25) },
            { "pickaxe", new Weapon("pickaxe", "Pickaxe", AbilityScore.Strength, new(4), "A useful mining tool that could serve as a weapon in a pinch.",
                8, sellValue: 20) },

            //Armors
            { "peasantclothes", new Armor("peasantclothes", "Peasant Clothes", 5, 10, 0, "A ragged set of clothes.") },
            { "breastplate", new Armor("breastplate", "Breastplate", 15, 100, 3, "A protective metal breastplate.") },
            { "chainmail", new Armor("chainmail", "Chainmail", 20, 125, 5, "A chain shirt.") },

            //Consumables
            { "ale", new SimpleConsumable("ale", "Mug of Ale", 0.5f, (session) =>
                {
                    session?.Player?.Heal(2);
                }, "Drink", "A frothy mug of bitter ale.", sellValue : 2)
            },
            { "mushroom", new SimpleConsumable("mushroom", "Mushroom", 0.1f, (session) =>
                {
                    session?.Player?.Heal(1);
                }, "Eat", "A small grey mushroom. Possibly edible.", sellValue : 1)
            },
            { "cookedmeat", new SimpleConsumable("cookedmeat", "Cooked Meat", 0.6f, (session) =>
                {
                    session?.Player?.Heal(3);
                }, "Eat", "A grilled hunk of meat.", sellValue : 8)
            },
            { "grilledmushroom", new SimpleConsumable("grilledmushroom", "Grilled Mushroom", 0.1f, (session) =>
                {
                    session?.Player?.Heal(2);
                }, "Eat", "An aromatic grilled mushroom. Probably not dangerous.", sellValue : 6)
            },
            { "lesserhealingpotion", new SimpleConsumable("lesserhealingpotion", "Lesser Healing Potion", 0.5f, (session) =>
                {
                    session?.Player?.Heal(10);
                }, "Drink", "A blood-red potion. A faint sparkling can be seen inside.", sellValue : 25)
            },

            //Misc Items
            { "rottenflesh", new SimpleItem("rottenflesh", "Rotten Flesh", 0.5f, "Rotten, decaying flesh, crawling with maggots. You probably shouldn't touch it.", sellValue: 1) },
            { "bone", new SimpleItem("bone", "Bone", 1, "A durable, white (actually more grey) bone.", sellValue: 2) },
            { "slime", new SimpleItem("slime", "Slime", 0.25f, "A glob of greenish-grey goo. Icky.", sellValue: 1) },
            { "meat", new SimpleItem("meat", "Meat", 1, "Mostly edible carcass. Where it's from, no one knows (it's best not to care).", sellValue : 3) },
            { "ironore", new SimpleItem("ironore", "Iron Ore", 1, "A chunk of raw iron.", sellValue : 3) },
            { "coal", new SimpleItem("coal", "Coal", 1, "A chunk of coal, good for smelting.", sellValue : 4) },
            { "taintedflesh", new SimpleItem("taintedflesh", "Tainted Flesh", 1, "A hunk of corrupted meat.", sellValue : 7) },
            { "ironbar", new SimpleItem("ironbar", "Iron Bar", 1, "A rectangular bar of iron, ready for use.", sellValue : 10) },
            { "log", new SimpleItem("log", "Log", 2, "A cylindrical piece of wood.", sellValue : 4) },
            { "vampiricdust", new SimpleItem("vampiricdust", "Vampiric Dust", 0.15f, "A sprinkling of shimmery red dust.", sellValue: 8) },
            { "bottle", new SimpleItem("bottle", "Empty Bottle", 0.2f, "An empty glass bottle.", sellValue: 4) },
        });

        public static T? Get<T>(string id) where T : Item
        {
            ITEMS.TryGetValue(id, out Item? item);
            return item as T;
        }

        public static Item? Get(string id)
        {
            return Get<Item>(id);
        }
    }
}