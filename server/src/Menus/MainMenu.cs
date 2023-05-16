using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menus
{
    public class MainMenu : Menu
    {

        public MainMenu(Session session)
        {
            this.session = session;
        }

        public override void OnStart()
        {
            Account account = session.Account;
            session.ClearLog();

            session.Log("Welcome to RMUD2!");
            session.Log($"Signed in as {account.username}");

            if (account.discordId == null)
                session.Log($"You have {Utils.Style("not", "maroon")} linked your Discord account. Please do so to ensure you can recover your account");
        }

        public override Input[] GetInputs(ServerResponse response)
        {
            List<Input> inputs = new List<Input>();
            Account account = session.Account;

            if (account.discordId == null)
                inputs.Add(new(InputMode.Option, "linkDiscord", "Link Discord Account"));

            return inputs.ToArray();
        }

        public override void HandleInput(ClientAction action, ServerResponse response)
        {

        }
    }
}
