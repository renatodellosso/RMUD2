using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Session
{

    public static Dictionary<ObjectId, Session> sessions = new Dictionary<ObjectId, Session>();

    public ObjectId account, id = ObjectId.GenerateNewId();

    public bool signedIn;

    public static ObjectId CreateSession()
    {
        Session session = new();
        
        sessions.Add(session.id, session);
        return session.id;
    }

}
