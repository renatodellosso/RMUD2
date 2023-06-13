using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//So I remember, this is called a generic class
public class ItemHolder<T> where T : ItemTypes.Item
{

    public string id;

    public T? Item => Items.ItemList.Get<T>(id);

    public Dictionary<string, object> data = new();

    public ItemHolder(string id)
    {
        this.id = id;
    }

}

//We can't specify a default type, so we use this
public class ItemHolder : ItemHolder<ItemTypes.Item>
{
    public ItemHolder(string id) : base(id)
    {
    }
}