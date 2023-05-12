using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class ServerAction<T>
{

    public string action;
    public T args;

    public ServerAction(string action, T args)
    {
        this.action = action;
        this.args = args;
    }

}

public class ServerResponse 
{

    public List<ServerAction<object>> actions = new();

    public ServerResponse(params ServerAction<object>[] actions)
    {
        this.actions.AddRange(actions);
    }

    public void Add(params ServerAction<object>[] newActions)
    {
        actions.AddRange(newActions);
    }

}