using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class Account
{

    public ObjectId _id;

    public string? username;

    public string? password, salt;

    public ulong discordId;

    public Account(string username, string password)
    {
        _id = ObjectId.GenerateNewId();
        this.username = username;
        
        salt = Utils.RandomSalt();
        Utils.Log("Starting hash...");
        this.password = Utils.HashPassword(password, salt);
        Utils.Log("Completed hash");
    }

    /// <summary>
    /// Checks if the username and password are correct
    /// </summary>
    /// <returns>The _id of the account that matches the provided credentials. Returns null if the credentials are invalid</returns>
    public static ObjectId? VerifyCredentials(string username, string password)
    {
        Utils.Log($"Attempting sign in... Username: {username}");

        bool success;

        Account account = DB.Accounts.FindByUsername(username);
        if (account != null)
            success = account.password.Equals(Utils.HashPassword(password, account.salt));
        else success = false;

        if (success) Utils.Log($"{username} signed in");
        else Utils.Log($"Someone failed to sign in to {username}");

        return success ? account._id : null;
    }

    /// <summary>
    /// Checks if the given username is taken. Not case-sensitive
    /// </summary>
    /// <returns>Whether an account exists with this same username</returns>
    public static bool UsernameTaken(string username)
    {
        username = username.ToLower();

        //The next 4 lines are the old search. I tried to make it case-insensitive, but it gave an error
        //We have to create a filter before performing the search
        //FilterDefinition<Account> filter = Builders<Account>.Filter.Eq(a => a.username != null ? a.username.ToLower() : "", username);
        //IAsyncCursor<Account> found = DB.accounts.FindSync(filter);
        //return found.Any();

        //Case-insensitive search
        return DB.accounts.AsQueryable().Where(a => a.username.ToLower().Equals(username)).Any();
    }

    /// <summary>
    /// Attempts to create an account with the given credentials
    /// </summary>
    /// <returns>The ObjectId of the account that was created</returns>
    public static ObjectId CreateAccount(string username, string password)
    {
        Account account = new Account(username, password);
        DB.accounts.InsertOne(account);
        return account._id;
    }

}
