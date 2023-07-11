using Discord;
using ItemTypes;
using Konscious.Security.Cryptography;
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
    /// <returns>The PBKDF2 hash</returns>
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

    public static string Dialogue(Creature creature, string text)
    {
        return creature.FormattedName + ": " + '"' + text + '"';
    }

    public static string Coins(float amt, bool color = true)
    {
        amt = Round(amt);
        string text = $"{amt}C";
        return color ? Style(text, "yellow") : text;
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
        direction = direction.Replace('N', 's');
        direction = direction.Replace('S', 'n');
        direction = direction.Replace('E', 'w');
        direction = direction.Replace('E', 'e');
        direction = direction.Replace("UP", "down");
        direction = direction.Replace("DOWN", "up");

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

    public static void AddItemOptionsFromInventory(List<Input> inputs, Inventory inventory, string[]? excludedIds = null)
    {
        for (int i = 0; i < inventory.Count; i++)
            if(excludedIds == null || !excludedIds.Contains(inventory[i].id))
                inputs.Add(new(InputMode.Option, i.ToString(), inventory[i].FormattedName + " x" + inventory[i].amt));
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
    /// <returns>How long to wait before the next tick</returns>
    public static int Tick() //We can't invoke OnTick outside of this class, so we need a method to do it
    {
        //Log($"Ticking... (Tick #{tickCount})");
        long ticks = DateTime.Now.Ticks;
        OnTick?.Invoke(tickCount);
        long elapsed = DateTime.Now.Ticks - ticks;
        double elapsedMs = (double)elapsed / TimeSpan.TicksPerMillisecond;

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

        Log($"Tick #{tickCount} complete. Took {elapsedMs}ms. RAM Usage: {Round(ramUsedGB, 3)} GB");

        tickCount++;
        return (int)(Config.TICK_INTERVAL - elapsedMs);
    }

    //End events

}