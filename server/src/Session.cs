using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Session
{

    public static Dictionary<ObjectId, Session> sessions = new Dictionary<ObjectId, Session>();

    public ObjectId account = new(), id = ObjectId.GenerateNewId();

    public bool SignedIn => !account.Equals(new());

    public Menu menu;
    public List<Menu> menuHistory = new();

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

}
