using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Vault : Inventory
{

    //Key is cost, value is size
    public static readonly KeyValuePair<int, int>[] SIZES = new KeyValuePair<int, int>[]
    {
        new(200, 30),
        new(600, 60),
        new(1200, 90),
        new(3000, 120)
    };

    public int level;

    public Vault() : base()
    {
        //Default constructor for deserialization
    }

    public Vault(int level) : base(SIZES[level].Value)
    {
        this.level = level;
    }

    public void CalculateStats()
    {
        maxWeight = SIZES[level].Value;
    }

    public string GetText()
    {
        return GetText($"Tier {level + 1} Vault");
    }

}