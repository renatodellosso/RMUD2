using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class WorldObject
{

    public string id, name;

    public virtual string FormattedName => name;

    public WorldObject(string id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public abstract List<Input> GetInputs(Player player, string state);

    public abstract void HandleInput(Session session, ClientAction action, ref string state, ref bool addStateToPrev);

}