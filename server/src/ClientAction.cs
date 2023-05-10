using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ClientAction
{

    public string sessionId = "", action = "", args = "";

    public ObjectId SessionId => new(sessionId);

}
