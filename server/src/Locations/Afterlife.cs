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
        }

    }
}