using Events;
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

    public List<string> tags = new();

    public Location? Location => GetLocation();

    public bool attackable = true;
    public int health;
    public virtual int MaxHealth => 5 + Constitution;
    public virtual int DodgeThreshold => 10 + (int)Math.Ceiling((double)Agility / 2); //Decimal is even more precise than double
    public int Defense => GetDefense();

    public Dictionary<DamageType, int> resistances = new(); //Negative values are weaknesses

    public virtual int MaxStamina => Config.Gameplay.BASE_STAMINA + Config.Gameplay.STAMINA_PER_END * Endurance;
    public int stamina {
        get
        {
            return (int)Math.Floor(rawStamina);
        }
        set
        {
            rawStamina = value;
        }
    }
    public float rawStamina = 0f;
    public virtual float BaseStaminaRegen => Config.Gameplay.BASE_STAMINA_REGEN + MathF.Floor(Agility / 2) * Config.Gameplay.STAMINA_REGEN_PER_EVERY_OTHER_AGI;
    public virtual float StaminaRegen => BaseStaminaRegen * (1 - Math.Max(0, inventory.Weight - MaxCarryWeight) * Config.Gameplay.ENCUMBRANCE_STAMINA_REGEN_REDUCTION_PER_LB);

    //Null for either means creature has no dialogue. The string is the dialogue state
    public Func<Session, DialogueMenu, Input[]>? talkInputs = null;
    public Action<Session, ClientAction, DialogueMenu>? talkHandler = null;
    public Action<Session>? talkStart = null;
    public bool HasDialogue => talkInputs != null && talkHandler != null && talkStart != null;

    //Name formatting
    public virtual string FormattedName => 
        Utils.Style($"{name}{(health != MaxHealth ? " " + Utils.FormatHealth(health, MaxHealth, true) : "")}", nameColor, nameBold, nameUnderline, nameItalic);
    public string nameColor = "";
    public bool nameBold, nameUnderline, nameItalic;

    public ItemHolder<Item>? mainHand, offHand;
    public ItemHolder<Armor>? armor;

    public virtual Weapon? Weapon => mainHand?.Item as Weapon;

    public Dictionary<AbilityScore, int> abilityScores = new()
    {
        { AbilityScore.Strength, 0 },
        { AbilityScore.Dexterity, 0 },
        { AbilityScore.Constitution, 0 },
        { AbilityScore.Agility, 0 },
        { AbilityScore.Endurance, 0 },
        { AbilityScore.Intelligence, 0 },
        { AbilityScore.Wisdom, 0 },
        { AbilityScore.Charisma, 0 }
    };

    //Ability score methods for convenience
    public int Strength => abilityScores[AbilityScore.Strength];
    public int Dexterity => abilityScores[AbilityScore.Dexterity];
    public int Constitution => abilityScores[AbilityScore.Constitution];
    public int Agility => abilityScores[AbilityScore.Agility];
    public int Endurance => abilityScores[AbilityScore.Endurance];
    public int Intelligence => abilityScores[AbilityScore.Intelligence];
    public int Wisdom => abilityScores[AbilityScore.Wisdom];
    public int Charisma => abilityScores[AbilityScore.Charisma];

    public virtual int MaxCarryWeight => Config.Gameplay.BASE_CARRY_WEIGHT + Strength * Config.Gameplay.CARRY_WEIGHT_PER_STR;

    public Inventory inventory = new();

    public int xpValue = 0;

    public Creature(string id, string name, bool actual = true) //Actual specifies whether this is a real creature or just being creatured to get data
    {
        if (actual)
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

        CalculateStats();
    }

    //Runs during deserialization
    public Creature()
    {
        CalculateStats();
    }

    Location? GetLocation()
    {
        return Location.Get(location);
    }

    /// <summary>
    /// Move the creature to a new location
    /// </summary>
    /// <param name="location">The ID of the new location</param>
    /// <param name="force">Whether to force the move, even if the exit can't be used, or if there is no exit</param>
    public void Move(string location, bool force = false)
    {
        Location? to = Location.Get(location);

        Exit? exit = force ? null : Location?.exits.Where(e => e.location == location).FirstOrDefault();

        if (force || (exit != null && exit.canExit(this, exit)))
        {
            if (!location.Equals(""))
            {
                Location? loc = GetLocation();
                loc?.Leave(this, to);
            }

            to?.Enter(this, Location);

            if (this is Player player)
            {
                player?.session?.SetMenu(new LocationMenu(player.session));
            }
        }
        else
        {
            if (this is Player player)
            {
                player?.session?.Log("You cannot use that exit.");
            }
        }
    }

    public virtual void Tick(int tickCount)
    {
        if (rawStamina < MaxStamina)
        {
            rawStamina += StaminaRegen;
            rawStamina = Math.Clamp(rawStamina, 0, MaxStamina);
        }
    }

    public void MoveThroughRandomExit(int maxEnemies = -1)
    {
        Location? location = Location;
        if (location != null)
        {
            if (location.exits.Count > 0)
            {
                Exit exit;
                int tries = 0, enemyCount = 0;

                do
                {
                    if (tries >= location.exits.Count)
                    {
                        Utils.Log($"Creature {name} tried to move through a random exit, but there were no valid exits");
                        return;
                    }

                    exit = location.exits[Utils.RandInt(location.exits.Count)]; //Min defaults to 0 (we made an overload)
                    enemyCount = Location.Get(exit.location)?.creatures.Where(c => c is not Player).Count() ?? 0; //IEnumerable.Count requires ()

                    tries++;
                } while (maxEnemies != -1 && enemyCount > maxEnemies);

                Move(exit.location);
            }
        }
    }

    /// <summary>
    /// Attacks using the default attack with the specified weapon
    /// </summary>
    public void Attack(Creature target, Weapon weapon)
    {
        weapon?.Attack?.execute(this, target);
    }

    public int CalculateDamage(int damage, DamageType? damageType = null)
    {
        damage -= GetDefense(damageType);
        damage = Math.Clamp(damage, 0, health);
        return damage;
    }

    public void TakeDamage(int damage, DamageType damageType, object source, bool calculateDamage = false)
    {
        if(calculateDamage) damage = CalculateDamage(damage, damageType);
        health -= damage;
        if (health <= 0)
            Die(new(source));
    }

    void Die(CreatureDeathEventData data)
    {
        Utils.Log($"{name} died");

        Player[] players = Location.Players;
        foreach (Player player in players)
        {
            player.session?.Log($"{FormattedName} died");
        }

        OnDie(data);
    }

    /// <returns>The amount healed</returns>
    public int Heal(int amt)
    {
        amt = Math.Min(amt, MaxHealth - health);
        health += amt;
        return amt;
    }

    protected virtual void OnDie(CreatureDeathEventData data)
    {
        Location?.creatures.Remove(this);
        location = "";
        Utils.OnTick -= Tick;

        if (data.killer is Player player)
        {
            string cause = "killing " + FormattedName;

            player.AddXp(xpValue, cause);
        }
    }

    //There's some stuff, like max inventory weight, where there's just not a good way auto-calculate the values, so we have to do it manually here
    public virtual void CalculateStats()
    {
        inventory.addedWeight = (armor?.Weight ?? 0) + (mainHand?.Weight ?? 0) + (offHand?.Weight ?? 0);
        inventory.maxWeight = MaxCarryWeight;

        resistances ??= new();
    }

    public virtual float ScaleTableWeight(Floor floor)
    {
        return 1;
    }

    public Attack[] GetAttacks()
    {
        List<Attack?> attacks = new();

        foreach(Attack attack in Weapon?.attacks.Values)
        {
            attacks.Add(attack);
        }

        if(offHand != null && offHand.Item is Weapon weapon)
        {
            foreach (Attack attack in weapon.attacks.Values)
            {
                attacks.Add(attack);
            }
        }

        attacks = attacks.Where(a => a?.CanUse(this) ?? false).ToList();

        return attacks.ToArray();
    }

    public int GetDefense(DamageType? damageType = null)
    {
        if (damageType == null)
            return armor?.Item?.Defense ?? 0;

        resistances.TryGetValue(damageType.Value, out int resistance);
        return (armor?.Item?.GetDefense(damageType) ?? 0) + resistance;
    }

    public float RestoreStamina(float amt, bool overrideMax = false)
    {
        float added = Math.Min(amt, overrideMax ? amt : MaxStamina - rawStamina);
        rawStamina += added;
        return added;
    }

}