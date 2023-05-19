using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menus
{
    public class LocationMenu : Menu
    {

        public LocationMenu(Session session)
        {
            this.session = session;
        }

        public override void OnStart()
        {
            session.status = "Somewhere";
        }

        public override Input[] GetInputs(ServerResponse response)
        {
            List<Input> inputs = new List<Input>();
            Player player = session.Player;

            inputs.AddRange(player.Location.GetInputs(player));

            return inputs.ToArray();
        }

        public override void HandleInput(ClientAction action, ServerResponse response)
        {

        }

    }
}
