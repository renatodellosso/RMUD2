using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Table<T>
{

    KeyValuePair<float, T>[] contents = new KeyValuePair<float, T>[0];
    
    float total = 0;

    public Table(params KeyValuePair<float, T>[] contents)
    {
        this.contents = contents;

        foreach(KeyValuePair<float, T> pair in contents)
            total += pair.Key;
    }
    
    public void Add(float key, T value)
    {
        KeyValuePair<float, T>[] newContents = new KeyValuePair<float, T>[contents.Length + 1];
        Array.Copy(contents, newContents, contents.Length);
        newContents[^1] = new KeyValuePair<float, T>(key, value);
        contents = newContents;

        total += key;
    }

    public void Add(params KeyValuePair<float, T>[] pairs)
    {
        KeyValuePair<float, T>[] newContents = new KeyValuePair<float, T>[contents.Length + pairs.Length];
        Array.Copy(contents, newContents, contents.Length);
        Array.Copy(pairs, 0, newContents, contents.Length, pairs.Length);
        contents = newContents;

        total += pairs.Sum(p => p.Key);
    }

    public void Add(Table<T> table)
    {
        Add(table.contents);
    }

    /// <summary>
    /// Removes ALL instances of item from the table
    /// </summary>
    public void Remove(T item)
    {
        total -= contents.Where(c => c.Value.Equals(item)).Sum(c => c.Key); //Reduce the total

        KeyValuePair<float, T>[] withoutRemoved = contents.Where(c => c.Value.Equals(item)).ToArray();
        contents = withoutRemoved;
    }

    public T Get()
    {
        //Loop through each item in contents, keeping track of how many we've searched so far. If searched + the current item's key pass id, return the value
        float id = Utils.RandFloat(0, total), searched = 0;

        foreach(KeyValuePair<float, T> pair in contents)
        {
            if(id <= searched + pair.Key)
                return pair.Value;
            searched += pair.Key;
        }

        return contents.First().Value;
    }

    /// <summary>
    /// Applies an action to each item in the table
    /// </summary>
    public void Apply(Action<float, T> action)
    {
        foreach(KeyValuePair<float, T> pair in contents)
            action(pair.Key, pair.Value);
    }

    /// <summary>
    /// Applies a function to each item in the table, and multiplies the old weight by the new one
    /// </summary>
    public void Scale(Func<float, T, float> func)
    {
        List<KeyValuePair<float, T>> newContents = new();
        float newTotal = 0;

        for (int i = 0; i < contents.Length; i++)
        {
            KeyValuePair<float, T> pair = contents[i];
            float mult = func(pair.Key, pair.Value), newWeight = pair.Key * mult;

            Utils.Log($"Weight: {pair.Key} -> {newWeight}");

            //If mult is 0 or less, remove the entry from the table
            if (mult > 0)
            {
                newTotal += newWeight;
                newContents.Add(new(pair.Key * mult, pair.Value));
            }
        }

        total = newTotal;
        contents = newContents.ToArray();
    }

    public Table<T> Clone()
    {
        Table<T> table = new();

        table.contents = (KeyValuePair<float, T>[])contents.Clone();

        return table;
    }

}
