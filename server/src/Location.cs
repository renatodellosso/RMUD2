using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Location
{

    private static Dictionary<string, Location> locations = new()
    {
        { "intro", new IntroLocation() }
    };
    public static Location? Get(string name)
    {
        if (locations.ContainsKey(name)) return locations[name];
        else return null;
    }

    public string name = "Unnamed Location", status = "The void";

    public List<Creature> creatures = new();
    public Player[] Players => creatures.Where(c => c is Player).Select(c => (Player)c).ToArray(); //.Select is used to transform each element

    public void Enter(Creature creature)
    {
        creatures.Add(creature);

        if(creature is Player)
        {
            Player player = (Player)creature;

            if(player.session != null)
                player.session.Log($"You enter {name}");
            else Utils.Log($"Player {player._id} has no session!");

            string creatureList = "Around you are:";
            foreach(Creature c in creatures)
            {
                if(c != creature) //Don't list the player
                    creatureList += $"<br>-{c.name}";
            }
            player.session.Log(creatureList);
        }

        OnEnter(creature);
    }

    public virtual void OnEnter(Creature creature)
    {

    }

    public virtual Input[] GetInputs(Player player, string state)
    {
        List<Input> inputs = new List<Input>();

        if(state.Equals(""))
        {
            //Dialogue
            List<Creature> dialogueCreatures = new();
            foreach (Creature creature in creatures)
                if (creature.HasDialogue) dialogueCreatures.Add(creature);
            if (dialogueCreatures.Any())
                inputs.Add(new(InputMode.Option, "talk", "Talk"));
        }

        return inputs.ToArray();
    }

    //We pass a ref to state so we can modify it
    public virtual void HandleInputs(Player player, ClientAction action, ref string state)
    {

    }

}