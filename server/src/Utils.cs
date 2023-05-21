using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
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
        argon.DegreeOfParallelism = 8; //4 Cores
        argon.Iterations = 5;
        argon.MemorySize = 1024 * 1024; //1GB of RAM

        return Convert.ToBase64String(argon.GetBytes(32));
    }

    //Formatting methods

    public static string Style(string text, string color = "", bool bold = false, bool underline = false)
    {
        if (!color.Equals(""))
            text = "<span style=" + '"' + "color:" + color + ";" + '"' + ">" + text + "</span>";
        if (bold) text = "<b>" + text + "</b>";
        if (underline) text = "<u>" + text + "</u>";

        return text;
    }

    public static string Link(string text, string url)
    {
        return $"<a href='{url}'>{text}</a>";
    }

    public static string Dialogue(string text)
    {
        return '"' + text + '"';
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

    //Random methods
    static Random random = new();

    /// <param name="min">Inclusive</param>
    /// <param name="max">Exclusive</param>
    /// <returns>Return a random int between min (inclusive) and max (exclusive)</returns>
    public static int RandInt(int min, int max)
    {
        return random.Next(min, max);
    }

    //End random methods

}