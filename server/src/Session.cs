using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Session
{

    public static Dictionary<ObjectId, Session> sessions = new Dictionary<ObjectId, Session>();

    public ObjectId? accountId, id = ObjectId.GenerateNewId();
    public Account Account => DB.accounts.Find<Account>(Builders<Account>.Filter.Eq("_id", accountId)).First();

    public bool SignedIn => accountId != null;

    public Menu menu;
    public List<Menu> menuHistory = new();

    public List<string> log = new();

    public static Session CreateSession()
    {
        Session session = new();
        
        sessions.Add((ObjectId)session.id, session);
        return session;
    }

    public void SetMenu(Menu menu)
    {
        menuHistory.Add(this.menu);
        this.menu = menu;
        this.menu.session = this;
        this.menu.OnStart();
    }

    public void Log(string msg)
    {
        log.Add(msg);
    }

    /// <summary>
    /// Removes the last entry in the log
    /// </summary>
    public void PopLog()
    {
        log.RemoveAt(log.Count - 1);
    }

    /// <summary>
    /// Replaces the most recent message
    /// </summary>
    public void ReplaceLog(string msg)
    {
        log[log.Count - 1] = msg;
    }

    /// <summary>
    /// Clears the log portion of the screen for the user
    /// </summary>
    public void ClearLog()
    {
        log.Clear();
    }

}
