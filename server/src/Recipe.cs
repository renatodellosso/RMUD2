using ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Recipe : IFormattable
{
    public string summary, verb = "Crafted";
    public ItemHolder<Item>[] ingredients, output;

    public Recipe(string summary, string verb, ItemHolder<Item>[] ingredients, ItemHolder<Item>[] output)
    {
        this.summary = summary;
        this.verb = verb;
        this.ingredients = ingredients;
        this.output = output;
    }

    public Recipe(string verb, ItemHolder<Item> ingredient, ItemHolder<Item> output)
        : this($"{output.FormattedName}", verb, new ItemHolder<Item>[] { ingredient }, new ItemHolder<Item>[] { output })
    { }

    public new string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        string msg = summary + ": ";

        foreach (ItemHolder<Item> item in ingredients)
            msg += $"{item.FormattedName} x{item.amt}, ";

        if (msg.EndsWith(", ")) msg = msg.Remove(msg.Length - 2);

        msg += " -> ";

        foreach (ItemHolder<Item> item in output)
            msg += $"{item.FormattedName} x{item.amt}, ";

        if (msg.EndsWith(", ")) msg = msg.Remove(msg.Length - 2);

        return msg;
    }

    public int MaxCraftable(Player player)
    {
        int min = -1;

        foreach (ItemHolder<Item> item in ingredients)
        {
            int amt = player.inventory.Where(i => i.id == item.id).Sum(i => i.amt);
            if (amt < item.amt)
                return 0;

            int craftable = (int)Math.Floor((decimal)amt / item.amt);
            if (min == -1 || craftable < min)
                min = craftable;
        }

        return min;
    }

    public void Craft(Player player, int amt)
    {
        int max = MaxCraftable(player);
        amt = Math.Min(amt, max);


        foreach (ItemHolder<Item> item in ingredients)
            player.inventory.Remove(new ItemHolder<Item>(item.id, item.amt * amt));

        string msg = verb + " ";
        foreach (ItemHolder<Item> item in output)
        {
            ItemHolder<Item>? added = player.inventory.Add(new ItemHolder<Item>(item.id, item.amt * amt));
            if (added != null)
                msg += $"{added.FormattedName} x{added.amt}, ";
        }

        if (msg.EndsWith(", "))
            msg = msg.Remove(msg.Length - 2);

        player?.session?.Log(msg);
        player?.Update();
    }
}
