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
            { "spear", new Weapon("spear", "Spear", AbilityScore.Strength, new(8), "A stick with a pointy end.", 5) }
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