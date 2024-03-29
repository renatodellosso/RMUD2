﻿using Events;
using ItemTypes;
using Locations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

public class Player : Creature
{

    //Static stuff
    public static Dictionary<ObjectId, Player> players = new Dictionary<ObjectId, Player>();

    public static Player Get(ObjectId id)
    {
        if (players.ContainsKey(id))
            return players[id];
        else return DB.Players.Find(id);
    }

    public static void Add(Player player)
    {
        //Use TryAdd instead of Add to avoid looking up the key twice
        if (!players.TryAdd(player._id, player))
            players[player._id] = player;
        Utils.AddPlayerToOnTick(player);
    }

    public static int Count => players.Count;
    //End of static stuff

    [BsonIgnore]
    public Session? session;

    public ObjectId _id = ObjectId.GenerateNewId(), accountId;
    public Account? Account => DB.Accounts.Find(accountId);

    public override int MaxHealth => Config.Gameplay.BASE_PLAYER_HP + Constitution * Config.Gameplay.HP_PER_CON;

    public int xp = 0, level = 0;
    public int XpToNextLevel => CalculateNextXPRequirement(level);
    public bool hasSentLevelUpNotification = false;

    public string? resetLocation;

    public float SellCut => MathF.Min(Config.Gameplay.BASE_SELL_CUT + Config.Gameplay.SELL_CUT_PER_CHA * Charisma, 1f);

    float XpMult => 1 + Wisdom * Config.Gameplay.XP_PER_WIS;

    public override string FormattedName => $"{Utils.StyleLevel(level)} " + base.FormattedName;

    [BsonIgnore] //We don't want to save this to the database
    public HashSet<DungeonLocation>? visitedRooms = new(); //We use a HashSet because we don't want duplicates

    public Vault? vault;
    public int vaultLevel;

    //Value is the name, since we don't want to look up every monster in the table every time we want to display the bestiary
    public Dictionary<string, string> bestiary = new();

    public TimeSpan playtime = TimeSpan.Zero;

    public bool craftFromVault = false;
    public Inventory CraftingInventory => craftFromVault ? vault! : inventory;

    public List<TradeOffer> tradeOffers = new();

    public bool hasUsedSoulGem = false;

    public int coins
    {
        get
        {
            return inventory.Where(i => i.Item?.id == "coin").Sum(i => i.amt);
        }
        set
        {
            IEnumerable<ItemHolder<Item>> items = inventory.Where(i => i.Item?.id == "coin");
            if (items.Any())
            {
                items.First().amt = value;
            }
            else
            {
                inventory.Add(new ItemHolder<Item>("coin", value));
            }

            Update();
        }
    }

    public Player(ObjectId accountId) : base(accountId.ToString(), "Unnamed Player")
    {
        this.accountId = accountId;
        name = Account?.username ?? "Unnamed Player";
        nameColor = "orange";
    }

    public void Update()
    {
        CalculateStats();
        if (_id != null)
            DB.players.ReplaceOneAsync(Builders<Player>.Filter.Eq("_id", _id), this);
        else Utils.Log("_id is null");
    }

    protected override void OnDie(CreatureDeathEventData data)
    {
        health = MaxHealth;
        if (!location.Equals("afterlife")) Move("afterlife", true);
    }

    /// <summary>
    /// Adds XP and evels up the player, formatted as: You gained {amount} xp from {cause}.
    /// </summary>
    /// <param name="amount">How much XP to give</param>
    /// <param name="cause">The cause of the XP gain</param>
    public void AddXp(int amount, string cause)
    {
        int baseAmt = amount;
        amount = (int)Math.Ceiling(amount * XpMult); //We round up to the nearest integer so that bonuses feel noticeable

        xp += amount;

        session?.Log($"You gained {Utils.XP(amount)} from {cause}. ({baseAmt} * {Utils.Percent(XpMult)} = {amount} xp)");

        if (xp > XpToNextLevel && !hasSentLevelUpNotification)
        {
            session?.Log(Utils.Style("Level up available! Rest to level up", "yellow"));
            hasSentLevelUpNotification = true;
        }

        Update();
    }

    public void Rest()
    {
        session?.Log(Utils.Style("You drift off into the comfort of sleep...", "honeydew"));

        if (xp > XpToNextLevel)
            LevelUp(() => CompleteRest());
        else
            CompleteRest(); //We complete the rest after the user levels up, but we have to pause to get their input
    }

    void CompleteRest(Action? afterLevelUp = null)
    {
        if (xp > XpToNextLevel)
            LevelUp(afterLevelUp ?? (() => CompleteRest()));
        else
        {
            health = MaxHealth;

            session?.Log(Utils.Style("You wake up feeling refreshed.", "yellow") + "<br>" +
                Utils.Style("HP restored!", "green"));

            hasSentLevelUpNotification = false;
            Update();
            session?.Log(Location.GetOverviewMsg(this));
        }
    }

    void LevelUp(Action afterwards)
    {
        session?.SetMenu(new Menus.LevelUp(session, afterwards));

        hasSentLevelUpNotification = false;
        Update();
    }

    //Basically just the character sheet
    public string GetCharacterText()
    {
        string text = Utils.Style(FormattedName, bold: true, underline: true);
        text += $"<br>Time Played: {playtime.Hours}h{playtime.Minutes}m";
        text += Utils.Style($"<br>Level {level} - {Utils.Format(xp)}/{Utils.Format(XpToNextLevel)} XP", xp >= XpToNextLevel ? "yellow" : "white");
        text += $"<br>{Utils.FormatHealth(health, MaxHealth, addedText: "HP")}";

        text += $"<br>Dodge Threshold: {DodgeThreshold}";
        text += $"<br>Defense: {Defense}";

        text += $"<br>Max Stamina: {MaxStamina} ({Utils.Modifier(Utils.Round(BaseStaminaRegen, 2))}/s)";

        float excessWeight = inventory.Weight - MaxCarryWeight;
        if (excessWeight > 0)
            text += $" - Encumbered: Actual Stamina Regen: {Utils.Modifier(Utils.Round(StaminaRegen))}/s";

        text += $"<br><br>Sell Cut: {Utils.Percent(SellCut)}";
        text += $"<br>XP Gain: {Utils.Percent(XpMult)}";

        text += Utils.Style("<br><br>Ability Scores:", bold: true);
        foreach(AbilityScore score in abilityScores.Keys)
        {
            text += $"<br>{score}: {GetAbilityScore(score)} ({abilityScores[score]}{Utils.Modifier(GetAbilityScoreBonus(score))})";
        }

        return text;
    }

    public void SignOut()
    {
        Location?.Leave(this, null);
        players.Remove(_id);
    }

    public override void CalculateStats()
    {
        base.CalculateStats();

        tradeOffers ??= new();

        float totalTradeWeight = 0;
        foreach(TradeOffer offer in tradeOffers)
            totalTradeWeight += offer.item.Weight;
        inventory.addedWeight += totalTradeWeight;

        //We want to avoid the below stuff, but we have to update it for old accounts
        nameColor = "orange";

        if(vault != null) vault.level = vaultLevel;
        vault?.CalculateStats();

        bestiary ??= new();

        if (tradeOffers == null)
            tradeOffers = new();
    }

    int CalculateNextXPRequirement(int level)
    {
        return (int)Math.Round(30f + (level * 50f) + MathF.Pow(level, 2.75f) + MathF.Pow(level * 50f, 0.9f));
    }

    public int GetNumOfLevelUpsAvailable()
    {
        int level = this.level;

        while(CalculateNextXPRequirement(level) <= xp)
        {
            level++;
        }

        return level - this.level;
    }

}