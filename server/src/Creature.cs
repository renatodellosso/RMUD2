using Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class Creature
{

    static List<string> ids = new();

    //We can't use id because of Player's _id, so we use baseId
    public string baseId = "unnamedCreature", name = "Unnamed Creature", location = "";

    public bool attackable = true;
    public int health, maxHealth;

    //Null for either means creature has no dialogue. The string is the dialogue state
    public Func<Session, DialogueMenu, Input[]>? talkInputs = null;
    public Action<Session, ClientAction, DialogueMenu>? talkHandler = null;
    public Action<Session>? talkStart = null;
    public bool HasDialogue => talkInputs != null && talkHandler != null && talkStart != null;

    //Name formatting
    public string FormattedName => Utils.Style(name, nameColor, nameBold, nameUnderline, nameItalic);
    public string nameColor = "";
    public bool nameBold, nameUnderline, nameItalic;

    public Creature(string id)
    {
        Utils.OnTick += Tick;

        //If multiple creatures have the same ID, add a number to the end of the ID
        int counter = 0;
        while ((counter > 0 && ids.Contains(id + counter)) || (counter == 0 && ids.Contains(id)))
            counter++;

        if(counter > 0) id += counter;
        ids.Add(id);
        baseId = id;
        //Utils.Log($"ID: {baseId}, Counter: {counter}");
    }

    public Location? GetLocation()
    {
        return Location.Get(location);
    }

    /// <summary>
    /// Move the creature to a new location
    /// </summary>
    /// <param name="location">The ID of the new location</param>
    public void Move(string location)
    {
        if (!location.Equals(""))
        {
            Location? loc = GetLocation();
            loc?.Leave(this);
        }

        Location.Get(location)?.Enter(this);

        if(this is Player)
        {
            Player player = (Player)this;

            player.session.SetMenu(new LocationMenu(player.session));
        }
    }

    protected virtual void Tick(int tickCount)
    {

    }

}