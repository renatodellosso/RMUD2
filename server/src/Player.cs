using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Player
{

    public string locationId;
    public Location Location => Location.locations[locationId];

    public string logInLocation;

}
