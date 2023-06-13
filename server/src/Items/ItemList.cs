using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Items
{
    public static class ItemList
    {
        static readonly Dictionary<string, ItemTypes.Item> ITEMS = new();

        public static T? Get<T>(string id) where T : ItemTypes.Item
        {
            ITEMS.TryGetValue(id, out ItemTypes.Item? item);
            return item as T;
        }
    }
}