using ItemTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Inventory : IEnumerable<ItemHolder<Item>> //IEnumerable allows us to use foreach loops
{

    public float maxWeight = -1; //If we need to manually set, use this
    public virtual float MaxWeight => maxWeight; //Set to -1 to disable

    List<ItemHolder<Item>> items = new();

    public int Count => items.Count;
    public float Weight => items.Sum(item => item.Weight);

    public object Current => throw new NotImplementedException();

    //Default constructor for deserialization
    //Not having this causes Mongo to throw an error, saying "no suitable constructor found"
    public Inventory() { }

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

    /// <returns>A list of the items that were added</returns>
    public List<ItemHolder<Item>> Add(List<ItemHolder<Item>> items)
    {
        //This will clear references to the items
        for (int i = 0; i < items.Count; i++)
        {
            items[i] = items[i].Clone();
        }

        //Utils.Log($"Adding {items.Count} item to inventory...");
        List<ItemHolder<Item>> added = new();

        foreach (ItemHolder<Item> item in items)
        {
            //Utils.Log($"Adding item {item.Item.name} to inventory...");
            ItemHolder<Item> toAdd = item.Clone();
            //Utils.Log("Cloned item");
            toAdd.amt = 0;

            //Add items to toAdd until we can't anymore
            while(item.amt > 0)
            {
                //Utils.Log($"Adding item {item.Item.name} to inventory... {item.amt} left");
                if(MaxWeight == -1 || Weight + toAdd.Item.Weight <= MaxWeight)
                {
                    toAdd.amt++;
                    item.amt--;
                }
                else
                {
                    //Utils.Log("Inventory full");
                    break;
                }
            }

            //Utils.Log("Added item x" + toAdd.amt);

            if (toAdd.amt > 0)
            {
                //Utils.Log("Actually adding toAdd...");
                added.Add(toAdd);

                //Don't add a new element in the list if we can just add to an existing one
                if(this.items.Any(i => i.id == toAdd.id))
                {
                    this.items.First(i => i.id == toAdd.id).amt += toAdd.amt;
                }
                else this.items.Add(toAdd);
            }
        }

        //Utils.Log("Done adding items. Added: " + added.Count + ", Items: " + added.First().amt);

        return added;
    }

    /// <returns>A list of the items that were added</returns>
    public List<ItemHolder<Item>> Add(ItemHolder<Item>[] items)
    {
        return Add(items.ToList());
    }


    /// <returns>The items that were added</returns>
    public ItemHolder<Item>? Add(ItemHolder<Item> item)
    {
        return Add(new ItemHolder<Item>[] { item }).FirstOrDefault();
    }


    /// <returns>The items that were removed</returns>
    public List<ItemHolder<Item>>? Remove(List<ItemHolder<Item>> items)
    {
        //This will clear references to the items
        for (int i = 0; i < items.Count; i++)
        {
            items[i] = items[i].Clone();
        }

        List<ItemHolder<Item>> removed = new();

        foreach(ItemHolder<Item> item in items)
        {
            ItemHolder<Item>? held = this.items.FirstOrDefault(i => i.id == item.id);
            
            if(held == null)
            {
                continue;
            }

            //Utils.Log("Held == Item: " + (held == item));
            //Utils.Log("Before: " + held.amt);
            int origAmt = held.amt;
            held.amt -= item.amt; //Remove the items
            //Utils.Log("After Subtraction: " + held.amt);
            held.amt = Math.Max(held.amt, 0); //Make sure we don't go below 0
            //Utils.Log("After Maxing: " + held.amt);
            int removedAmt = origAmt - held.amt; //Figure out how many items we actually removed
            //Utils.Log("Before Setting item.amt: " + held.amt);
            item.amt = removedAmt; //Track how many items we actually removed
            //Utils.Log("After: " + held.amt);

            if (item.amt > 0) removed.Add(item);
            if (held.amt == 0) this.items.Remove(held);
        }

        return removed;
    }

    public List<ItemHolder<Item>>? Remove(ItemHolder<Item>[] items)
    {
        return Remove(items.ToList());
    }

    public ItemHolder<Item>? Remove(ItemHolder<Item> item)
    {
        return Remove(new ItemHolder<Item>[] { item })?.FirstOrDefault();
    }

    /// <summary>
    /// Adds items to another inventory and removes those items from this inventory
    /// </summary>
    /// <param name="other">The inventory to add the items to</param>
    /// <param name="items">The items to transfer</param>
    /// <returns>The items that were transferred</returns>
    public List<ItemHolder<Item>> Transfer(Inventory other, List<ItemHolder<Item>> items)
    {
        List<ItemHolder<Item>> added = other.Add(items);

        List<ItemHolder<Item>> toRemove = new();
        foreach(ItemHolder<Item> item in added)
            toRemove.Add(item.Clone());
        Remove(toRemove);

        //Utils.Log($"Added " + added.Count + " items to inventory. # of Items: " + added.First().amt);
        return added;
    }

    public ItemHolder<Item>? Transfer(Inventory other, ItemHolder<Item> item)
    {
        return Transfer(other, new List<ItemHolder<Item>>() { item }).FirstOrDefault();
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