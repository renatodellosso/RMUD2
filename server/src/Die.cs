﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Die : IFormattable
{

    public int dieSize, numOfDice, modifier;

    public List<Die> bonusDice = new();

    public Die(int dieSize, int numOfDice = 1, int modifier = 0, List<Die>? bonusDice = null)
    {
        this.dieSize = dieSize;
        this.numOfDice = numOfDice;
        this.modifier = modifier;
        if(bonusDice != null)
            this.bonusDice = bonusDice;
    }

    public Die(int dieSize) : this(dieSize, 1) { } //No modifier

    public int Roll()
    {
        //Roll the die, then roll the bonus dice, then add the modifier
        int total = 0;

        for (int i = 0; i < numOfDice; i++)
            total += Utils.RandInt(1, dieSize + 1);

        foreach (Die die in bonusDice)
            total += die.Roll();

        return total + modifier;
    }

    public Die Clone()
    {
        return new(dieSize, numOfDice, modifier, bonusDice);
    }

    //IFormattable
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Utils.Log(modifier.ToString());
        return $"{numOfDice}d{dieSize}{(modifier != 0 ? Utils.Modifier(modifier) : "")}";
    }

    //Implicit casts
    public static implicit operator int(Die die) => die.Roll();
    public static implicit operator double(Die die) => die.Roll();
    public static implicit operator float(Die die) => die.Roll();
    public static implicit operator string(Die die) => die.ToString(null, null);
    public static implicit operator Die(int size) => new(size);

    //Static methods for ease of use
    public static int Roll(int dieSize, int numOfDice, int modifier) => new Die(dieSize, numOfDice, modifier).Roll();
    public static int Roll(int dieSize, int modifier) => Roll(dieSize, 1, modifier);
    public static int Roll(int dieSize) => Roll(dieSize, 0);

}
