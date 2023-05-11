using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ClientAction
{

    public string token = "", action = "", args = "";

    public ObjectId SessionId => new(token);
    public Session Session => GetSession();

    private Session? GetSession()
    {
        if (token.Equals("")) return null;
        else return Session.sessions[SessionId];
    }

}
