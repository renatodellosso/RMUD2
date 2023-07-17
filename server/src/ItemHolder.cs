using ItemTypes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

//So I remember, this is called a generic class
public class ItemHolder<T> where T : ItemTypes.Item
{

    public string id;

    public int amt { //Amount
        get => (int)data.GetValueOrDefault("amt", 1);
        set => data["amt"] = value;
    }

    public string FormattedName => Item.FormattedName;

    public float Weight => amt * Item.Weight;

    public int SellValue => amt * Item.SellValue;

    public T? Item => Items.ItemList.Get<T>(id);

    public Dictionary<string, object> data = new();

    public ItemHolder(string id, int amt = 1)
    {
        this.id = id;
        this.amt = amt;
    }

    public ItemHolder<J> Clone<J>() where J : Item
    {
        ItemHolder<J> clone = new(id, amt);

        //Copy data
        foreach(KeyValuePair<string, object> pair in data)
        {
            try
            {
                clone.data.Add(pair.Key, pair.Value); //Throws an exception if the key already exists
            }
            catch
            {
                clone.data[pair.Key] = pair.Value;
            }
        }

        return clone;
    }

    public ItemHolder<T> Clone()
    {
        return Clone<T>();
    }

    //Important! This is remove the reference to the item
    public static implicit operator ItemHolder<Item>(ItemHolder<T> holder)
    {
        return holder.Clone<Item>();
    }

    public string Overview(Creature? creature = null)
    {
        return Item?.Overview(this, creature) ?? "";
    }

}