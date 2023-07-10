using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ClientAction
{

    public string? token = "";
    public string action = "";

    public ObjectId SessionId => new(token);
    public Session Session => GetSession();

    private Session? GetSession()
    {
        if (token == null || token.Equals("")) return null;
        else
        {
            Session.sessions.TryGetValue(SessionId, out Session? session);
            if (session == null) return null;
            return session;
        }
    }

}
