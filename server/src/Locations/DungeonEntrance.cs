using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations
{
    public class DungeonEntrance : Location
    {

        protected override string Description => "You are standing in a small, ruined courtyard. To the north lies two statues, one a knight and the other an unknown figure in hooded robes. " +
                    "Between them is a tunnel into the mountain.";

        public DungeonEntrance()
        {
            id = "dungeonentrance";
            name = "Outside the dungeon";
            status = "Preparing for an expedition";

            //Add event listener, so we can add an exit to the dungeon
            Dungeon.OnDungeonGenerated += OnDungeonGenerated;
        }

        void OnDungeonGenerated()
        {
            //Add the exit to and from the dungeon
            Exit.AddExit(this, Get(Dungeon.startLocation), "E", true);
        }

    }
}