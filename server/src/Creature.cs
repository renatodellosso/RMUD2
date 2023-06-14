using ItemTypes;
using Menus;
using MongoDB.Bson.Serialization.Attributes;
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

    public Location? Location => GetLocation();

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

    public ItemHolder<Item>? mainHand, offHand;
    public Weapon? Weapon => mainHand?.Item as Weapon;

    public Dictionary<AbilityScore, int> abilityScores = new()
    {
        { AbilityScore.Strength, 0 },
        { AbilityScore.Dexterity, 0 },
        { AbilityScore.Constitution, 0 },
        { AbilityScore.Agility, 0 },
        { AbilityScore.Intelligence, 0 },
        { AbilityScore.Wisdom, 0 },
        { AbilityScore.Charisma, 0 }
    };

    //Ability score methods for convenience
    public int Strength => abilityScores[AbilityScore.Strength];
    public int Dexterity => abilityScores[AbilityScore.Dexterity];
    public int Constitution => abilityScores[AbilityScore.Constitution];
    public int Agility => abilityScores[AbilityScore.Agility];
    public int Intelligence => abilityScores[AbilityScore.Intelligence];
    public int Wisdom => abilityScores[AbilityScore.Wisdom];
    public int Charisma => abilityScores[AbilityScore.Charisma];

    public int DodgeThreshold => 10 + Agility;

    public Creature(string id, string name)
    {
        Utils.OnTick += Tick;

        //If multiple creatures have the same ID, add a number to the end of the ID
        int counter = 0;
        while ((counter > 0 && ids.Contains(id + counter)) || (counter == 0 && ids.Contains(id)))
            counter++;

        if (counter > 0)
        {
            id += counter;
            name += $" {counter}";
        }

        ids.Add(id);
        baseId = id;
        this.name = name;
        //Utils.Log($"ID: {baseId}, Counter: {counter}");
    }

    Location? GetLocation()
    {
        return Location.Get(location);
    }

    /// <summary>
    /// Move the creature to a new location
    /// </summary>
    /// <param name="location">The ID of the new location</param>
    public void Move(string location)
    {
        Location? to = Location.Get(location);

        if (!location.Equals(""))
        {
            Location? loc = GetLocation();
            loc?.Leave(this, to);
        }

        to?.Enter(this, Location);

        if(this is Player)
        {
            Player player = (Player)this;

            player?.session?.SetMenu(new LocationMenu(player.session));
        }
    }

    protected virtual void Tick(int tickCount)
    {

    }

    public void MoveThroughRandomExit()
    {
        Location? location = Location;
        if (location != null)
        {
            if (location.exits.Count > 0)
            {
                Exit exit = location.exits[Utils.RandInt(location.exits.Count)]; //Min defaults to 0 (we made an overload)
                Move(exit.location);
            }
        }
    }

    public void Attack(Creature target, Weapon weapon)
    {
        
    }

    public int TakeDamage(int damage)
    {
        damage = Math.Clamp(damage, 0, health);
        health -= damage;
        if (health < 0)
            health = 0;
        return damage;
    }

}