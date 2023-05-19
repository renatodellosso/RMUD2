using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Location
{

    private static Dictionary<string, Location> locations = new();
    public static Location? Get(string name)
    {
        if (locations.ContainsKey(name)) return locations[name];
        else return null;
    }

    public string name, status;

    public List<Creature> creatures = new();
    public Player[] Players => creatures.Where(c => c is Player).Select(c => (Player)c).ToArray(); //.Select is used to transform each element

    public void Enter(Creature creature)
    {
        creatures.Add(creature);

        if(creature is Player)
        {

        }
    }

    public Input[] GetInputs(Player player)
    {
        List<Input> inputs = new List<Input>();

        return inputs.ToArray();
    }

}
