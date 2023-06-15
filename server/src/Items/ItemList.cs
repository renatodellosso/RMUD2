using ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Items
{
    public static class ItemList
    {
        static readonly Dictionary<string, ItemTypes.Item> ITEMS = new()
        {
            { "spear", new ItemTypes.Weapon("spear", "Spear", AbilityScore.Strength, new(8)) }
        };

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