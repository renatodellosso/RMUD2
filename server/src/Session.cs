using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Session
{

    public static Dictionary<ObjectId, Session> sessions = new Dictionary<ObjectId, Session>();

    public ObjectId? accountId, id = ObjectId.GenerateNewId();

    public bool SignedIn => accountId != null;

    public Menu menu;
    public List<Menu> menuHistory = new();

    public List<string> log = new();

    public static Session CreateSession()
    {
        Session session = new();
        
        sessions.Add(session.id, session);
        return session;
    }

    public void SetMenu(Menu menu)
    {
        menuHistory.Add(this.menu);
        this.menu = menu;
        this.menu.session = this;
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

}
