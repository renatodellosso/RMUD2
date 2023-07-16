using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Die : IFormattable
{

    public int dieSize, numOfDice;
    public Func<int> modifier;

    public List<Die> bonusDice = new();

    public Die(int dieSize, int numOfDice, Func<int> modifier, List<Die>? bonusDice = null)
    {
        this.dieSize = dieSize;
        this.numOfDice = numOfDice;
        this.modifier = modifier;
        if(bonusDice != null)
            this.bonusDice = bonusDice;
    }

    public Die(int dieSize, Func<int> modifier) : this(dieSize, 1, modifier) { } //Modifier as Func
    public Die(int dieSize, int numOfDice = 1, int modifier = 0) : this(dieSize, numOfDice, () => modifier) { } //Modifier as value
    public Die(int dieSize) : this(dieSize, () => 0) { } //No modifier

    public int Roll()
    {
        //Roll the die, then roll the bonus dice, then add the modifier
        int total = 0;

        for (int i = 0; i < numOfDice; i++)
            total += Utils.RandInt(1, dieSize + 1);

        foreach (Die die in bonusDice)
            total += die.Roll();

        return total + modifier();
    }

    public Die Clone()
    {
        return new(dieSize, numOfDice, modifier, bonusDice);
    }

    //IFormattable
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return $"{numOfDice}d{dieSize}{(modifier() >= 0 ? "+" : "")}{modifier()}";
    }

    //Implicit casts
    public static implicit operator int(Die die) => die.Roll();
    public static implicit operator double(Die die) => die.Roll();
    public static implicit operator float(Die die) => die.Roll();
    public static implicit operator string(Die die) => die.ToString(null, null);

    //Static methods for ease of use
    public static int Roll(int dieSize, int numOfDice, int modifier) => new Die(dieSize, numOfDice, () => modifier).Roll();
    public static int Roll(int dieSize, int modifier) => Roll(dieSize, 1, modifier);
    public static int Roll(int dieSize) => Roll(dieSize, 0);

}
