using Items;
using ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Recipe : IFormattable
{
    public string summary, verb = "Crafted"; //Verb should be past-tense
    public ItemHolder<Item>[] ingredients, output;
    public int xpValue = 0;

    public Recipe(string summary, string verb, ItemHolder<Item>[] ingredients, ItemHolder<Item>[] output, int xpValue = 0)
    {
        this.summary = summary;
        this.verb = verb;
        this.ingredients = ingredients;
        this.output = output;
        this.xpValue = xpValue;
    }
    public Recipe(string verb, ItemHolder<Item>[] ingredient, ItemHolder<Item>[] output, int xpValue = 0)
            : this($"{output.First().FormattedName}", verb, ingredient, output, xpValue)
        { }
    public Recipe(string verb, ItemHolder<Item>[] ingredient, ItemHolder<Item> output, int xpValue = 0)
        : this(verb, ingredient, new ItemHolder<Item>[] { output }, xpValue)
    { }

    public Recipe(string verb, ItemHolder<Item> ingredient, ItemHolder<Item> output, int xpValue = 0)
        : this(verb, new ItemHolder<Item>[] { ingredient }, new ItemHolder<Item>[] { output }, xpValue)
    { }

    public Recipe(string id)
        : this("Bought", new ItemHolder<Item>("coin", ItemList.Get(id).SellValue), new ItemHolder<Item>(id, 1))
    { }

    public Recipe(string input, int inAmt, string output, int outAmt, int xpValue = 0)
        : this("Crafted", new ItemHolder<Item>(input, inAmt), new ItemHolder<Item>(output, outAmt), xpValue)
    { }
    public Recipe(ItemHolder<Item>[] input, string output, int outAmt, int xpValue = 0)
        : this("Crafted", input, new ItemHolder<Item>(output, outAmt), xpValue)
    { }

    public Recipe(string id, float markUp) : this("Bought", new ItemHolder<Item>("coin", (int)Math.Round(ItemList.Get(id).SellValue * markUp)), new ItemHolder<Item>(id, 1))
    { }



    public new string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        string msg = summary + ": ";

        foreach (ItemHolder<Item>? item in ingredients)
            msg += $"{item?.FormattedName ?? Utils.Style("ERROR", "red")} x{item.amt}, ";

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
            ItemHolder<Item>? added = player.inventory.Add(new ItemHolder<Item>(item.id, item.amt * amt), true);
            if (added != null)
                msg += $"{added.FormattedName} x{added.amt}, ";
        }

        if (msg.EndsWith(", "))
            msg = msg.Remove(msg.Length - 2);

        if(xpValue > 0)
            player?.AddXp(amt * xpValue, $"crafting {amt}x {output.First().FormattedName}");

        player?.session?.Log(msg);
        player?.Update();
    }
}
