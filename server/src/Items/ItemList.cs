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
            { "spear", new Weapon("spear", "Spear", AbilityScore.Strength, new(8), "A stick with a pointy end.", 5, sellValue: 20) },
            { "rottenflesh", new SimpleItem("rottenflesh", "Rotten Flesh", 0.5f, "Rotten, decaying flesh, crawling with maggots. You probably shouldn't touch it.", sellValue: 1) },
            { "bone", new SimpleItem("bone", "Bone", 1, "A durable, white (actually more grey) bone.", sellValue: 2) },
            { "slime", new SimpleItem("slime", "Slime", 0.25f, "A glob of greenish-grey goo. Icky.", sellValue: 1) },
            { "meat", new SimpleItem("meat", "Meat", 1, "Mostly edible carcass. Where it's from, no one knows (it's best not to care).", sellValue : 3) },
            { "coin", new SimpleItem("coin", "Gold Coin", 0.2f, "A shiny gold coin.", sellValue : 1, color: "yellow") },
            { "ale", new SimpleItem("ale", "Mug of Ale", 0.5f, "A frothy mug of bitter ale.", sellValue : 2) },
        });

        public static T? Get<T>(string id) where T : ItemTypes.Item
        {
            ITEMS.TryGetValue(id, out ItemTypes.Item? item);
            return item as T;
        }

        public static Item? Get(string id)
        {
            return Get<Item>(id);
        }
    }
}