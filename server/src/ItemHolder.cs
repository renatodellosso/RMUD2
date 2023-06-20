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
    public string uid = ObjectId.GenerateNewId().ToString();

    public int amt { //Amount
        get => (int)data.GetValueOrDefault("amt", 1);
        set => data["amt"] = value;
    }

    public string FormattedName => Item.FormattedName + (amt > 1 ? $" x{amt}" : "");

    public float Weight => amt * Item.Weight;

    public T? Item => Items.ItemList.Get<T>(id);

    public Dictionary<string, object> data = new();

    public ItemHolder(string id, int amt = 1)
    {
        this.id = id;
        this.amt = amt;
    }

    public ItemHolder<T> Clone()
    {
        ItemHolder<T> clone = new ItemHolder<T>(id, amt);
        clone.data = data.ToDictionary(entry => entry.Key, entry => entry.Value); //Copies the data
        return clone;
    }

    public static implicit operator ItemHolder(ItemHolder<T> holder)
    {
        return (ItemHolder)holder;
    } 

}

//We can't specify a default type, so we use this
public class ItemHolder : ItemHolder<ItemTypes.Item>
{
    public ItemHolder(string id) : base(id)
    {
    }
}