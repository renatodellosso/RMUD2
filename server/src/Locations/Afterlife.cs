using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations
{
    public class Afterlife : Location
    {

        public Afterlife()
        {
            id = "afterlife";
            name = "The Afterlife";
        }

        public override void OnEnter(Creature creature)
        {
            if (creature is Player)
            {
                Player player = (Player)creature;

                player?.session?.Log("The universe floats around you as the lingering pain from your death fades.");
            }
        }

    }
}