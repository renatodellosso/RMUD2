using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class IntroLocation : Location
{

    public IntroLocation()
    {
        name = "Outside the dungeon";

        creatures.Add(new Creatures.SimpleNPC("Shadowy Figure"));
    }

    public override void OnEnter(Creature creature)
    {
        if(creature is Player)
        {
            Player player = (Player)creature;

            player.session.Log("You are standing in a small, ruined courtyard. To the north lies two statues, one a knight and the other an unknown figure in hooded robes. " +
                "Between them is a tunnel into the mountain.");
        }
    }

}
