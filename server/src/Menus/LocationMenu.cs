using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menus
{
    public class LocationMenu : Menu
    {

        public override string Status => session.Player.Location.status;
        public override bool ShowSidebar => true;

        public LocationMenu(Session session)
        {
            this.session = session;
        }

        public override void OnStart()
        {

        }

        public override Input[] GetInputs(ServerResponse response)
        {
            List<Input> inputs = new List<Input>();
            Player? player = session.Player;

            if (player != null)
            {
                if (player.Location != null)
                {
                    inputs.AddRange(player.Location.GetInputs(session, state));
                }
            }

            return inputs.ToArray();
        }

        public override void HandleInput(ClientAction action, ServerResponse response)
        {
            Player? player = session.Player;

            if (player == null || player.Location == null) return;

            player.Location.HandleInputs(session, action, ref state);
        }

    }
}
