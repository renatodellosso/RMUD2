using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations
{
    public class Afterlife : Location
    {

        protected override string Description => "The universe floats around you as the lingering pain from your death fades.";

        public Afterlife()
        {
            id = "afterlife";
            name = "The Afterlife";

            //Add event listener, so we can add an exit to the dungeon
            Dungeon.OnDungeonGenerated += OnDungeonGenerated;
        }

        void OnDungeonGenerated()
        {
            //Add the exit to and from the dungeon
            Exit.AddExit(this, Get(Dungeon.startLocation), "N", false);
        }

    }
}