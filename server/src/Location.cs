using ItemTypes;
using WorldObjects;
using System.Collections.Concurrent;

public abstract class Location
{

    //We use ConcurrentDictionary instead of Dictionary because it is thread-safe
    //https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2?view=net-7.0
    private static ConcurrentDictionary<string, Location> locations = new(new Dictionary<string, Location>()
    {
        { "intro", new Locations.Intro() },
        { "afterlife", new Locations.Afterlife() },
        { "dungeonentrance", new Locations.DungeonEntrance() },
        { "townsquare", new Locations.TownSquare() },
        { "generalstore", new Locations.GeneralStore() },
        { "inn", new Locations.Inn() },
        { "blacksmith", new Locations.Blacksmith() },
        { "woods", new Locations.Woods() },
        { "bank", new Locations.Bank() },
        { "grotto", new Locations.Grotto() },
        { "wizardhouse", new Locations.WizardHouse() },
        { "deepwoods", new Locations.DeepWoods() }
    });

    public static Location? Get(string name)
    {
        if (name != null && !name.Equals(""))
        {
            locations.TryGetValue(name, out Location? location); //We use TryGetValue to avoid looking up the key twice
            return location;
        }

        return null;
    }

    public static void Remove(Location location)
    {
        locations.TryRemove(new(location.id, location));
    }

    public static void GenerateExits()
    {
        Utils.Log("Adding exits...");
        foreach (Location location in locations.Values)
        {
            location.AddExits();
        }
        Utils.Log("Exits added");
    }

    /// <summary>
    /// Adds the given location to list of locations
    /// </summary>
    public static void Add(Location location)
    {
        locations.TryAdd(location.id, location);
        Utils.AddLocationToOnTick(location);
    }

    //Actual class data below

    public string id = "unnamedLocation", name = "Unnamed Location", status = "The void";

    public virtual string FormattedName(Player player) => name;

    protected virtual string Description => "";

    public HashSet<Creature> creatures = new();
    public Player[] Players => creatures.Where(c => c is Player).Select(c => (Player)c).ToArray(); //.Select is used to transform each element

    public List<Exit> exits = new();

    public List<WorldObject> objects = new();

    public bool safe = false; //Shouldn't be changed outside of constructor

    public void Enter(Creature creature, Location? from)
    {
        //Prevent duplicates
        if (creatures.Contains(creature))
            return;

        creatures.Add(creature);

        creature.location = id;

        if (creature is Player)
        {
            Utils.Log($"Player {creature.name} enters {id} from {creature.location}");
            Player player = (Player)creature;

            if (player.session == null)
            {
                Utils.Log($"Finding session for player {player._id}...");
                player.session = Session.sessions.Where(s => s.Value.playerId.Equals(player._id)).First().Value;
            }

            if (player.session != null)
                player.session.Log(Utils.Style($"You enter {FormattedName(player)}", bold: true));
            else Utils.Log($"Player {player._id} has no session!");

            player?.session?.Log(GetOverviewMsg(player));
        }

        if (from != null)
        {
            Player[] players = Players;
            foreach (Player player in players)
                if (player != creature)
                    player.session?.Log($"{creature.FormattedName} enters {FormattedName(player)} from {from.name}");
        }

        OnEnter(creature);
    }

    //Maybe convert OnEnter and OnLeave to events?
    public virtual void OnEnter(Creature creature)
    {

    }

    //Leave, instead of Exit, to avoid confusion with the Exit class
    public void Leave(Creature creature, Location? to)
    {
        creatures.Remove(creature);

        Player[] players = Players;
        if (players.Any() && to != null)
        {
            foreach (Player player in players)
                player.session?.Log($"{creature.FormattedName} leaves {name}, going towards {to.name}");
        }
    }

    public virtual void OnLeave(Creature creature)
    {

    }

    public virtual void AddExits() //Add exits after the locations are initialized
    {

    }

    public virtual Input[] GetInputs(Session session, string state)
    {
        string[] args = state.Split('.');
        List<Input> inputs = new();

        try
        {
            if (state.Equals("") || args.Length == 0)
            {
                //The order we add these determines the order they appear in
                //We want to add the most common options first, but keep exit last
                inputs.Add(new(InputMode.Option, "look", "Look around"));
                //inputs.Add(new(InputMode.Option, "combat", "Combat"));

                //Dialogue
                List<Creature> dialogueCreatures = new();
                foreach (Creature creature in creatures)
                    if (creature.HasDialogue) dialogueCreatures.Add(creature);
                if (dialogueCreatures.Any())
                    inputs.Add(new(InputMode.Option, "talk", "Talk"));

                if (objects.Any()) inputs.Add(new(InputMode.Option, "interact", "Interact with objects"));

                inputs.Add(new(InputMode.Option, "inventory", "Inventory"));
                inputs.Add(new(InputMode.Option, "character", "Character"));
                inputs.Add(new(InputMode.Option, "chat", "Chat"));
                inputs.Add(new(InputMode.Option, "trade", "Trade"));
                inputs.Add(new(InputMode.Option, "move", "Move"));
                inputs.Add(new(InputMode.Option, "help", "Help"));
            }
            else
            {
                inputs.Add(new(InputMode.Option, "back", "< Back"));

                if (state.Equals("talk"))
                {
                    foreach (Creature creature in creatures)
                        if (creature.HasDialogue) inputs.Add(new(InputMode.Option, creature.baseId, creature.FormattedName));
                }
                else if (state.Equals("move"))
                {
                    foreach (Exit exit in exits)
                        if (exit != null && Get(exit.location) != null)
                            inputs.Add(new(InputMode.Option, exit.location, $"({exit.direction}) {Get(exit.location).FormattedName(session.Player)}"));
                }
                //else if (state.Equals("combat"))
                //{
                //    Attack[] attacks = session.Player?.GetAttacks() ?? Array.Empty<Attack>();
                //    foreach (Attack attack in attacks)
                //        inputs.Add(new(InputMode.Option, attack.id, attack.name));
                //}
                else if (args[0].Equals("atktarget"))
                {
                    //args[1] is the weapon, args[2] is the attack

                    List<ItemHolder<Item>> weapons = new() //The list of ItemHolders we'll check for the weapon
                    {
                        session.Player?.mainHand
                    };

                    IEnumerable<ItemHolder<Item>> found = weapons.Where(w => w.id.Equals(args[1]));
                    Weapon? weapon = found.First().Item as Weapon;
                    if (weapon != null)
                    {
                        Attack? attack = weapon.attacks[args[2]];
                        List<Creature> targets = attack?.getTargets(session.Player) ?? new();

                        foreach (Creature target in targets)
                            inputs.Add(new(InputMode.Option, $"atk.{weapon.id}.{attack?.id}.{target.baseId}", target.FormattedName));
                    }
                }
                else if (args[0].Equals("interact"))
                {
                    if (args.Length == 1)
                        foreach (WorldObject obj in objects)
                            inputs.Add(new(InputMode.Option, obj.id, obj.FormattedName));
                    else if (args.Length >= 2)
                    {
                        WorldObject? obj = objects.Where(o => o.id.Equals(args[1])).First();
                        if (obj != null)
                            inputs.AddRange(obj.GetInputs(session.Player, state));
                        else Utils.Log($"Object {args[1]} not found!");
                    }
                }
                else if (args[0].Equals("inventory"))
                {
                    if (args.Length == 1)
                    {
                        for (int i = 0; i < session.Player?.inventory.Count; i++)
                        {
                            ItemHolder<Item> item = session.Player?.inventory[i];
                            inputs.Add(new(InputMode.Option, i.ToString(), item.FormattedName));
                        }
                    }
                    else
                    {
                        ItemHolder<Item>? item = session.Player?.inventory[int.Parse(args[1])];
                        if (item != null)
                            inputs.AddRange(item.Item.GetInputs(session, item, state));
                        else Utils.Log($"Item {args[1]} not found!");
                    }
                }
                else if (args[0] == "chat")
                {
                    inputs.Add(new(InputMode.Text, "msg", "Message"));
                }
            }
        } catch (Exception e)
        {
            Utils.Log(e);
        }

        return inputs.ToArray();
    }

    //We pass a ref to state so we can modify it
    public virtual void HandleInputs(Session session, ClientAction action, ref string state, List<string> prevStates, ref bool addStateToPrev)
    {
        try
        {
            string[] args = action.action.Split('.'), stateArgs = state.Split('.');

            if (state.Equals("") || args.Length == 0)
            {
                if (action.action.Equals("talk"))
                    state = "talk";
                else if (action.action.Equals("move"))
                    state = "move";
                else if (action.action.Equals("look"))
                    session.Log(GetOverviewMsg(session.Player));
                //else if (action.action.Equals("combat"))
                //    state = "combat";
                else if (action.action.Equals("interact"))
                    state = "interact";
                else if (action.action == "chat")
                    state = "chat";
                else if (action.action.Equals("inventory"))
                {
                    state = "inventory";
                    Player? player = session.Player;

                    string msg = $"Currently Carrying ({Utils.Weight(player?.inventory.Weight, false)}/{Utils.Weight(player?.inventory.MaxWeight)}, " +
                        $"{Utils.Coins(player?.inventory.Sum(i => i.SellValue * player.SellCut) ?? 0)} total): ";

                    msg += $"<br>Armor: {player?.armor?.FormattedName} - {Utils.Weight(player?.armor?.Weight)}";
                    msg += $"<br>Main Hand: {player?.mainHand?.FormattedName} - {Utils.Weight(player?.mainHand?.Weight)}";
                    if (player?.offHand != null) msg += $"<br>Off Hand: {player?.offHand?.FormattedName} - {Utils.Weight(player?.offHand?.Weight)}";

                    foreach (ItemHolder<Item> item in player?.inventory)
                    {
                        msg += $"<br>-{item.FormattedName} x{item.amt} ({Utils.Weight(item.Weight)} total)";
                    }

                    session.Log(msg);
                }
                else if (args[0] == "character")
                    session.SetMenu(new Menus.CharacterMenu());
                else if (args[0] == "trade")
                    session.SetMenu(new Menus.TradeMenu());
                else if (args[0] == "help")
                    session.SetMenu(new Menus.HelpMenu(session));
                else
                    Utils.Log("Invalid action: " + action.action);
            }
            else
            {
                if (!action.action.Equals("back"))
                {
                    if (state.Equals("talk"))
                    {
                        Creature? target = creatures.Where(c => c.baseId.Equals(action.action)).FirstOrDefault();

                        if (target != null)
                        {
                            session.SetMenu(new Menus.DialogueMenu(target));
                        }
                    }
                    else if (state.Equals("move"))
                    {
                        if (exits.Where(e => e.location.Equals(action.action)).Any())
                        {
                            Player? player = session.Player;
                            if (player != null)
                            {
                                player.Move (action.action);

                                Location? location = player.Location;
                                if (location != null)
                                {
                                    //Avoids a glitch where 2 copies of a player would enter a room
                                    location.RemoveDuplicateCreatures();
                                }
                            }

                        }
                    }
                    else if (stateArgs[0].Equals("interact"))
                    {
                        if (stateArgs.Length == 1)
                            foreach (WorldObject obj in objects)
                            {
                                if (obj.id.Equals(action.action))
                                {
                                    session.Log(obj.GetOverview(session.Player));
                                    obj.OnStart(session);
                                    state = "interact." + obj.id;
                                    break;
                                }
                            }
                        else if (stateArgs.Length >= 2)
                        {
                            IEnumerable<WorldObject?> objs = objects.Where(o => o.id.Equals(stateArgs[1]));
                            WorldObject? obj = objs.FirstOrDefault();
                            if (obj != null)
                                obj?.HandleInput(session, action, ref state, ref addStateToPrev); //Only if we actually found the object
                            else Utils.Log($"Object {args[0]}");
                        }
                    }
                    else if (stateArgs[0].Equals("inventory"))
                    {
                        if (stateArgs.Length == 1)
                        {
                            try
                            {
                                state = "inventory." + args[0];
                                ItemHolder<Item>? item = session.Player?.inventory[int.Parse(args[0])];
                                session.Log(item?.Overview() ?? "Item not found. Please report this bug.");
                            }
                            catch { }
                        }
                        else
                        {
                            try
                            {
                                ItemHolder<Item>? item = session.Player?.inventory[int.Parse(stateArgs[1])];
                                if (item != null)
                                    item.Item?.HandleInput(session, action, item, ref state, ref addStateToPrev);
                                else Utils.Log($"Item {args[1]} not found!");
                            }
                            catch (Exception e)
                            {
                                Utils.Log(e);
                            }
                        }
                    }
                    else if (stateArgs[0] == "chat")
                    {
                        if (Utils.IsInputSafe(action.action, false))
                        {
                            Player[] players = session.Player?.Location?.Players ?? Array.Empty<Player>();
                            foreach (Player player in players)
                                player.session?.Log(Utils.Dialogue(session.Player, action.action));
                            state = "";
                        }
                        else
                        {
                            session.Log("Disallowed input");
                        }
                    }
                }

                //Set action.action to back to go back a state
                if (action.action.Equals("back"))
                {
                    if (prevStates.Any())
                    {
                        string prevState = state;

                        do
                        {
                            state = prevStates.Last();
                            prevStates.Remove(prevStates.Last());
                            addStateToPrev = false;
                        } while (state == prevState && prevStates.Any());
                    }
                    else
                        state = "";
                }
            }
        }
        catch (Exception e)
        {
            Utils.Log(e);
        }
    }

    public string GetOverviewMsg(Player player)
    {
        string overview = Description;

        overview += "<br>" + GetCreatureListMessage(player);
        overview += "<br>" + GetObjectListMessage();

        return overview;
    }

    public string GetCreatureListMessage(Player player)
    {
        if (creatures.Where(c => c != player).Any()) //If there are creatures other than the player
        {
            string creatureList = "Around you are:";
            foreach (Creature c in creatures)
            {
                if (c != player) //Don't list the player
                    creatureList += $"<br>-{c.FormattedName}";
            }

            return creatureList;
        }
        else return "You are alone.";
    }

    public string GetObjectListMessage()
    {
        if (objects.Any())
        {
            string msg = "In this location are:";
            foreach (WorldObject obj in objects)
            {
                msg += $"<br>-{obj.FormattedName}";
            }

            return msg;
        }
        else return "This room is empty.";
    }

    /// <summary>
    /// Might be unnecessary now
    /// </summary>
    void RemoveDuplicateCreatures()
    {
        creatures = creatures.Distinct().ToHashSet();
    }

    /// <summary>
    /// For each player in the location, log the provided message
    /// </summary>
    public void Log(string msg)
    {
        foreach (Player player in Players)
            player.session?.Log(msg);
    }

    /// <summary>
    /// Gets added to Utils.OnTick automatically when we register the location
    /// </summary>
    public virtual void OnTick(int tickCount)
    {
        IEnumerable<Player> missingPlayers = Players.Where(p => p.location != id);
        foreach (Player player in missingPlayers)
        {
            creatures.Remove(player);
            Utils.Log($"Removed player {player.name} from room they were not in");
        }
    }

}