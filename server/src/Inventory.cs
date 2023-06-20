using ItemTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Inventory : IEnumerable<ItemHolder> //IEnumerable allows us to use foreach loops
{

    float maxWeight = -1;
    public virtual float MaxWeight => maxWeight; //Set to -1 to disable

    readonly List<ItemHolder> items = new();

    public int Count => items.Count;
    public float Weight => items.Sum(item => item.Weight);

    public object Current => throw new NotImplementedException();

    public Inventory(List<ItemHolder> items, float maxWeight = -1)
    {
        this.maxWeight = maxWeight;
        this.items.AddRange(items);
    }

    public Inventory(float maxWeight = -1) : this(new List<ItemHolder>(), maxWeight) { } //We use : this to call the other constructor
    public Inventory(ItemHolder[] items, float maxWeight = -1) : this(items.ToList(), maxWeight) { }

    public IEnumerator<ItemHolder> GetEnumerator()
    {
        return items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public ItemHolder this[int index]
    {
        get => items[index];
        set => items[index] = value;
    }


    /// <returns>A list of the items that could not be added</returns>
    public List<ItemHolder> Add(List<ItemHolder> items)
    {
        List<ItemHolder> rejected = new();

        foreach (ItemHolder item in items)
        {
            ItemHolder toAdd = item.Clone();
            toAdd.amt = 0;

            //Add items to toAdd until we can't anymore
            while(item.amt > 0)
            {
                if(MaxWeight == -1 || Weight + toAdd.Item.Weight <= MaxWeight)
                {
                    toAdd.amt++;
                    item.amt--;
                }
                else
                {
                    rejected.Add(item);
                    break;
                }
            }

            if(toAdd.amt > 0) items.Add(toAdd);
            rejected.Add(item);
        }

        return rejected;
    }

    /// <returns>A list of the items that could not be added</returns>
    public List<ItemHolder> Add(ItemHolder[] items)
    {
        return Add(items.ToList());
    }


    /// <returns>Returns null if the entire stack was added, otherwise it returns what couldn't be added</returns>
    public ItemHolder? Add(ItemHolder item)
    {
        ItemHolder toAdd = item.Clone();
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
}