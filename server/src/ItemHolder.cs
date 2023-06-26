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

    public ItemHolder<T> Clone()
    {
        ItemHolder<T> clone = new(id, amt);

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

    public static implicit operator ItemHolder<ItemTypes.Item>(ItemHolder<T> holder)
    {
        return holder;
    }

    public string Overview()
    {
        return Item.Overview(data);
    }

}