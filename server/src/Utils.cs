using Discord;
using ItemTypes;
using Konscious.Security.Cryptography;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public static class Utils
{

    public static string Log(string msg)
    {
        msg = $"[{DateTime.Now}]: {msg}";
        Console.WriteLine(msg);
        return msg;
    }

    public static string Log(Exception e)
    {
        string msg = Log($"Error in {e.TargetSite}: {e.Message}\n{e.StackTrace}");
        return msg;
    }

    /// <summary>
    /// Sends a message to every session
    /// </summary>
    public static void Announce(string msg)
    {
        Log("Sending server announcement: " + msg);

        string source = "SERVER ANNOUNCEMENT";
        string wrapper = new('-', $"[{source}]: {msg}".Length);

        msg = $"[{Style(source, "aquamarine", true, true)}]: {msg}";
        msg = wrapper + "<br>" + msg + "<br>" + wrapper;

        foreach (Session session in Session.sessions.Values.Where(s => s != null)) //Send to all sessions that aren't null
        {
            session.Log(msg);
        }
    }

    /// <summary>
    /// Creates a random salt that is cryptographically secure
    /// </summary>
    /// <returns>Returns the salt</returns>
    public static string RandomSalt()
    {
        byte[] bytes = new byte[32];
        RandomNumberGenerator.Create().GetBytes(bytes); //Fills out the byte array with random values

        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// We use this instead of Sha256. It's more secure
    /// </summary>
    /// <param name="text">The password to hash</param>
    /// <param name="salt">The salt</param>
    /// <returns>The Argon2id hash</returns>
    public static string HashPassword(string text, string salt)
    {
        text = Env.instance.pepper + text; //Add the pepper to the password

        //Old PBKDF2 hashing
        //Rfc2898DeriveBytes pbkdf2 = new(text, Encoding.UTF8.GetBytes(salt));
        //pbkdf2.IterationCount = Config.PBKDF2_ITERATIONS;
        //return Convert.ToBase64String(pbkdf2.GetBytes(32));

        Argon2id argon = new Argon2id(Encoding.UTF8.GetBytes(text));

        argon.Salt = Encoding.UTF8.GetBytes(salt);
        argon.DegreeOfParallelism = 2; //1 Core
        argon.Iterations = 3;
        argon.MemorySize = 1024 * 4; //4MB of RAM

        return Convert.ToBase64String(argon.GetBytes(32));
    }

    //Formatting methods

    //Colors: https://www.w3schools.com/colors/colors_names.asp
    public static string Style(string? text, string color = "", bool bold = false, bool underline = false, bool italic = false)
    {
        if(text == null) return "";

        if (color != "")
            text = "<span style=" + '"' + "color:" + color + ";" + '"' + ">" + text + "</span>";
        if (bold) text = "<b>" + text + "</b>";
        if (underline) text = "<u>" + text + "</u>";
        if (italic) text = "<i>" + text + "</i>";

        return text;
    }

    public static string Link(string text, string url)
    {
        return $"<a href='{url}'>{text}</a>";
    }

    public static string Dialogue(Creature? creature, string text)
    {
        return (creature?.FormattedName ?? "Unknown") + ": " + '"' + text + '"';
    }

    public static string Coins(float amt, bool color = true)
    {
        amt = Round(amt);
        string text = $"{Format(amt)}C";
        return color ? Style(text, "yellow") : text;
    }

    public static string XP(int amt)
    {
        return Style(Format(amt) + " xp", "yellow");
    }

    public static string Modifier(float mod)
    {
        string text = mod.ToString();
        if (mod >= 0) text = "+" + text;
        return text;
    }

    public static string Modifier(int mod)
    {
        return Modifier((float)mod);
    }

    public static string Percent(float percent)
    {
        return $"{Round(percent * 100)}%";
    }

    public static string Percent(double percent)
    {
        return Percent((float)percent);
    }

    public static string PercentModifer(float percent)
    {
        if(percent >= 0) return "+" + Percent(percent);
        return "-" + Percent(percent);
    }

    public static string Weight(float? weight, bool unit = true)
    {
        return $"{Round(weight ?? 0, 2)}" + (unit ? " lbs." : "");
    }

    public static string StyleLevel(int level)
    {
        string color = "white", bracketColor = "white";
        bool bold = false, underline = false, bracketBold = false, bracketUnderline = false;

        if (level >= 5) color = "yellow";
        if (level >= 10) color = "gold";
        if (level >= 20) color = "orange";
        if (level >= 25) bold = true;
        if (level >= 30) color = "orangered";
        if(level >= 40) color = "red";
        if(level >= 50) underline = true;
        if (level >= 60) color = "lightseagreen";
        if (level >= 70) bracketBold = true;
        if (level >= 75) underline = true;
        if (level >= 80) color = "turquoise";
        if (level >= 90) color = "cyan";
        if (level >= 100) bracketColor = "darkgoldenrod";

        string prefix = Style($"[{Style(level.ToString(), color, bold, underline)}]", bracketColor, bracketBold, bracketUnderline);

        return prefix;
    }

    public static string Format(int num)
    {
        return Format((float)num);
    }

    public static string Format(float num)
    {
        return num.ToString("N0"); //Format with commas
    }

    public static bool HasItem(Creature creature, string id)
    {
        return creature.mainHand?.id == id || creature.offHand?.id == id || creature.armor?.id == id || creature.inventory.Where(i => i.id == id).Any();
    }

    //End formatting methods

    /// <summary>
    /// Checks if the input contains malicious text. NOT IMPLEMENTED YET
    /// </summary>
    /// <param name="text">The text to scan</param>
    /// <returns>Whether the text is safe or not</returns>
    public static bool IsInputSafe(string text)
    {
        if (text.Contains(' ')) return false;
        else if (text.Contains("<script")) return false;

        return true;
    }

    /// <summary>
    /// Generate a numeric code of length Config.CODE_LENGTH
    /// </summary>
    /// <returns>The code, as a string</returns>
    public static string RandomCode()
    {
        string code = "";
        Random rand = new();
        for (int i = 0; i < Config.CODE_LENGTH; i++)
            code += rand.Next(0, 10);
        return code;
    }

    public static string ReverseDirection(string direction)
    {
        //We use lowercase now to avoid reswapping the chars. We'll put it back to uppercase later
        direction = direction.Replace("UP", "down");
        direction = direction.Replace("DOWN", "up");
        direction = direction.Replace('N', 's');
        direction = direction.Replace('S', 'n');
        direction = direction.Replace('E', 'w');
        direction = direction.Replace('W', 'e');

        direction = direction.ToUpper();

        return direction;
    }

    /// <summary>
    /// Converts a Vector2 to the string representation of it's direction (NE, SW, etc)
    /// </summary>
    public static string Vector2ToDir(Vector2 dir)
    {
        string direction = "";

        if (dir.y > 0) direction += "N";
        else if (dir.y < 0) direction += "S";

        if (dir.x > 0) direction += "E";
        else if (dir.x < 0) direction += "W";

        return direction;
    }

    public static void AddItemOptionsFromInventory(List<Input> inputs, Inventory inventory, string[]? excludedIds = null, bool takeAll = false)
    {
        if (takeAll)
            inputs.Add(new(InputMode.Option, "takeall", "Take All"));

        for (int i = 0; i < inventory.Count; i++)
            if(excludedIds == null || !excludedIds.Contains(inventory[i].id))
                inputs.Add(new(InputMode.Option, i.ToString(), inventory[i].FormattedName + " x" + inventory[i].amt));
    }

    public static void AddItemAmountOptions(List<Input> inputs, ItemHolder<Item> item, int max = -1, string text = "")
    {
        max = max == -1 ? item.amt : max;

        inputs.Add(new(InputMode.Option, max.ToString(), $"Max - {max}"));
        inputs.Add(new(InputMode.Option, 1.ToString(), "1"));
        inputs.Add(new(InputMode.Text, "amt", text == "" ? $"Enter an amount between 1 and {max}" : text));
    }

    public static float Round(decimal num, int places)
    {
        return (float)Math.Round(num, places);
    }

    public static float Round(float num, int places)
    {
        return Round((decimal)num, places);
    }

    public static float Round(double num, int places)
    {
        return Round((decimal)num, places);
    }

    public static int Round(decimal num)
    {
        return (int)Math.Round(num);
    }

    public static int Round(float num)
    {
        return Round((decimal)num);
    }

    public static int Round(double num)
    {
        return Round((decimal)num);
    }

    public static string FormatHealth(float health, float maxHealth, bool parantheses = false, string addedText = "")
    {
        string msg = $"{health}/{maxHealth}{(addedText != "" ? " " + addedText : "")}";

        if (parantheses) msg = "(" + msg + ")";

        string color = "green";
        float percentile = health / maxHealth;
        if (percentile < .25) color = "red";
        else if (percentile < .5) color = "orange";
        else if (percentile < .75) color = "yellow";

        msg = Style(msg, color);

        return msg;
    }

    //Random methods
    static Random random = new();

    /// <param name="min">Inclusive</param>
    /// <param name="max">Exclusive</param>
    /// <returns>Return a random int between min (inclusive) and max (exclusive)</returns>
    public static int RandInt(int min, int max)
    {
        return random.Next(min, max);
    }

    /// <param name="max">Exclusive</param>
    public static int RandInt(int max)
    {
        return RandInt(0, max);
    }

    /// <param name="min">Inclusive</param>
    /// <param name="max">Exclusive</param>
    /// <returns>Returns a random Vector2 with X and Y coords between min (inclusive) and max (exclusive)</returns>
    public static Vector2 RandVector2(int min, int max)
    {
        return new(RandInt(min, max), RandInt(min, max));
    }

    /// <param name="min">Inclusive, defaults to 0</param>
    /// <param name="max">Exclusive, defaults to 1</param>
    /// <returns>Return a random float between min (inclusive) and max (exclusive)</returns>
    public static float RandFloat(float min = 0, float max = 1)
    {
        //Gets what % of the way between min and max the number is, then adds the min
        return (float) random.NextDouble() * (max-min) + min;
    }

    //End random methods

    //Events

    //Tick
    public static int tickCount = 0;
    public static event Action<int>? OnTick;

    public static void AddPlayerToOnTick(Player player)
    {
        if (!OnTick?.GetInvocationList().Where(c => c.Target == player).Any() ?? false)
            OnTick += player.Tick;
    }

    public static void RemoveDungeonCreatures()
    {
        Log("Removing dungeon inhabitants from OnTick...");
        Delegate[] invocationList = OnTick?.GetInvocationList() ?? Array.Empty<Delegate>();

        foreach (Delegate d in invocationList)
        {
            if (d.Target is Creature creature)
            {
                if (creature.Location == null)
                {
                    creature.Die(new("Dungeon Removal"));
                    OnTick -= (Action<int>)d;
                }
            }
        }
    }

    /// <returns>How long to wait before the next tick</returns>
    public static int Tick() //We can't invoke OnTick outside of this class, so we need a method to do it
    {
        //Log($"Ticking... (Tick #{tickCount})");
        long ticks = DateTime.Now.Ticks;
        OnTick?.Invoke(tickCount);
        long elapsed = DateTime.Now.Ticks - ticks;
        double elapsedMs = (double)elapsed / TimeSpan.TicksPerMillisecond;

        if(tickCount % Config.BOT_STATUS_UPDATE_FREQUENCY == 0)
            Bot.UpdateStatus();

        long ramUsed = GC.GetTotalMemory(false);

        //Check if we need to force garbage collection
        if(ramUsed > Config.MAX_RAM)
        {
            Log("FORCING GARBAGE COLLECTION...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Log("GARBAGE COLLECTION COMPLETE");

            //Get RAM usage again
            ramUsed = GC.GetTotalMemory(false);
        }

        double ramUsedGB = (double)ramUsed / 1000000000;

        int ticksTillDungeonReset = Config.RESET_DUNGEON_AFTER_TICKS - (tickCount % Config.RESET_DUNGEON_AFTER_TICKS) - 1;
        if (Config.RESET_DUNGEON_NOTIFICATION_POINTS.Contains(ticksTillDungeonReset))
        {
            Log("Notifying players of dungeon reset...");
            TimeSpan timeRemaining = TimeSpan.FromMilliseconds(Config.TICK_INTERVAL * ticksTillDungeonReset);
            Announce($"Dungeon will reset in {timeRemaining.Minutes}m{timeRemaining.Seconds}s!");
        }

        if(ticksTillDungeonReset == 0)
            Dungeon.Reset();

        Log($"Tick #{tickCount} complete. Took {elapsedMs}ms. RAM Usage: {Round(ramUsedGB, 3)} GB. Regenerating Dungeon in {ticksTillDungeonReset} ticks");

        tickCount++;
        return (int)(Config.TICK_INTERVAL - elapsedMs);
    }

    //End events

    //This is a continually-running thread
    public static void RemoveInactiveSessions()
    {
        while (true)
        {
            Thread.Sleep(Config.SessionRemoval.CHECK_INTERVAL);

            Log("Checking for inactive sessions...");

            List<ObjectId> toRemove = new(), nullSessions = new();
            foreach (ObjectId id in Session.sessions.Keys)
            {
                Session? session = Session.sessions[id];
                if (session == null) nullSessions.Add(id);
                else if (session.lastActionTime < DateTime.Now.AddMilliseconds(-Config.SessionRemoval.MAX_AGE))
                {
                    Log($"Removing inactive session {id}");
                    toRemove.Add(id);
                }
            }

            foreach (ObjectId id in nullSessions)
            {
                Log($"Removing null session {id}");
                Session.sessions.Remove(id);
            }

            foreach (ObjectId id in toRemove)
            {
                Session session = Session.sessions[id];
                Log($"Removing session: {session.Player?.name ?? "Unknown Player"}");

                try
                {
                    session.ShutDown();
                    Session.sessions.Remove(id);
                }
                catch (Exception e)
                {
                    Log(e);
                }
            }
        }
    }

}