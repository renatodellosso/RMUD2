using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldObjects
{
    public abstract class WorldObject
    {

        public string id, name, color = "green";

        public virtual string FormattedName => Utils.Style(name, color);

        public string location;
        public Location Location => Location.Get(location);

        public WorldObject(string id, string name, string location)
        {
            this.id = id;
            this.name = name;
            this.location = location;
        }

        public virtual string GetOverview(Player player)
        {
            return FormattedName;
        }

        public abstract List<Input> GetInputs(Player player, string state);

        public abstract void HandleInput(Session session, ClientAction action, ref string state, ref bool addStateToPrev);

        public virtual void OnStart(Session session)
        {

        }

        public virtual void Delete(ref string state, ref bool addStateToPrev)
        {
            //Remove the object
            Location.objects.Remove(this);
            state = "interact";
            addStateToPrev = false;
        }

    }
}