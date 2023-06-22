using ItemTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Inventory : IEnumerable<ItemHolder<Item>> //IEnumerable allows us to use foreach loops
{

    float maxWeight = -1;
    public virtual float MaxWeight => maxWeight; //Set to -1 to disable

    readonly List<ItemHolder<Item>> items = new();

    public int Count => items.Count;
    public float Weight => items.Sum(item => item.Weight);

    public object Current => throw new NotImplementedException();

    public Inventory(List<ItemHolder<Item>> items, float maxWeight = -1)
    {
        this.maxWeight = maxWeight;
        this.items.AddRange(items);
    }

    public Inventory(float maxWeight = -1) : this(new List<ItemHolder<Item>>(), maxWeight) { } //We use : this to call the other constructor
    public Inventory(ItemHolder<Item>[] items, float maxWeight = -1) : this(items.ToList(), maxWeight) { }

    public IEnumerator<ItemHolder<Item>> GetEnumerator()
    {
        return items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public ItemHolder<Item> this[int index]
    {
        get => items[index];
        set => items[index] = value;
    }


    /// <returns>A list of the items that could not be added</returns>
    public List<ItemHolder<Item>> Add(List<ItemHolder<Item>> items)
    {
        Utils.Log($"Adding {items.Count} item to inventory...");
        List<ItemHolder<Item>> rejected = new();

        foreach (ItemHolder<Item> item in items)
        {
            Utils.Log($"Adding item {item.Item.name} to inventory...");
            ItemHolder<Item> toAdd = item.Clone();
            Utils.Log("Cloned item");
            toAdd.amt = 0;

            //Add items to toAdd until we can't anymore
            while(item.amt > 0)
            {
                Utils.Log($"Adding item {item.Item.name} to inventory... {item.amt} left");
                if(MaxWeight == -1 || Weight + toAdd.Item.Weight <= MaxWeight)
                {
                    toAdd.amt++;
                    item.amt--;
                }
                else
                {
                    if(rejected.Contains(item)) rejected[rejected.IndexOf(item)].amt += item.amt;
                    else rejected.Add(item);
                    break;
                }
            }

            Utils.Log("Added item");

            if(toAdd.amt > 0) this.items.Add(toAdd);
            rejected.Add(item);
        }

        return rejected;
    }

    /// <returns>A list of the items that could not be added</returns>
    public List<ItemHolder<Item>> Add(ItemHolder<Item>[] items)
    {
        return Add(items.ToList());
    }


    /// <returns>Returns null if the entire stack was added, otherwise it returns what couldn't be added</returns>
    public ItemHolder<Item>? Add(ItemHolder<Item> item)
    {
        ItemHolder<Item> toAdd = item.Clone();
        toAdd.amt = 0;

        //Add items to toAdd until we can't anymore
        while (item.amt > 0)
        {
            if (MaxWeight == -1 || Weight + toAdd.Item.Weight <= MaxWeight)
            {
                toAdd.amt++;
                item.amt--;
            }
            else
            {
                break;
            }
        }

        if (toAdd.amt > 0) items.Add(toAdd);

        return item.amt == 0 ? null : item;
    }

    public new bool Contains(string id)
    {
        return items.Any(item => item.id == id);
    }

    public bool Contains(string id, Dictionary<string, object> data)
    {
        IEnumerable<ItemHolder<Item>> found = items.Where(item => item.id == id);

        //Don't bother checking data if an item with the same id isn't in the inventory
        if (!found.Any()) return false;
        
        //Check if the data matches
        foreach(ItemHolder<Item> item in found)
        {
            foreach(KeyValuePair<string, object> pair in data)
            {
                if (!item.data.ContainsKey(pair.Key) || item.data[pair.Key] != pair.Value)
                {
                    goto endOfInnerLoop; //See the label below. This lets use effectively continue the outer loop
                };
            }

            endOfInnerLoop:; //This is a label for goto

            return true;
        }

        return false;
    }
}