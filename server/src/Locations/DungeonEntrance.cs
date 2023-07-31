using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations
{
    public class DungeonEntrance : Location
    {

        protected override string Description => 
            Utils.Style("Welcome to RMUD2!", bold: true) +
            "<br>Thanks for playing my game. The core loop you'll need to do is explore the dungeon, kill monsters and loot their corpses, then use your loot to craft better items." +
            "<br>You are heavily encouraged to poke around in the various menus and locations. See what you can find!" +
            "<br><br>You are standing in a small, ruined courtyard. To the north lies two statues, one a knight and the other an unknown figure in hooded robes. " +
                    "Between them is a tunnel into the mountain.";

        public DungeonEntrance()
        {
            id = "dungeonentrance";
            name = "Outside the dungeon";
            status = "Preparing for an expedition";

            safe = true;
        }

        public override void AddExits()
        {
            //Add the exit to and from the dungeon
            Exit.AddExit(this, Get(Dungeon.startLocation), "E", true);
        }

    }
}