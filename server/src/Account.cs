using MongoDB.Bson;
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

    public Account(string username, string password)
    {
        _id = ObjectId.GenerateNewId();
        this.username = username;
        
        salt = Utils.RandomSalt();
        Console.WriteLine("Starting hash...");
        this.password = Utils.PBKDF2Hash(password, salt);
        Console.WriteLine("Completed hash");
    }

}
