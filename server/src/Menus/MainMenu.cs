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
            session.status = "Main Menu";

            session.ClearLog();

            session.Log("Welcome to RMUD2!");
            session.Log($"Signed in as {account.username}");

            if (account.discordId == 0)
                session.Log($"You have {Utils.Style("not", "maroon")} linked your Discord account. Please do so to ensure you can recover your account");
        }

        public override Input[] GetInputs(ServerResponse response)
        {
            List<Input> inputs = new List<Input>();
            Account account = session.Account;

            if (account.discordId == 0)
                inputs.Add(new(InputMode.Option, "linkDiscord", "Link Discord Account"));

            inputs.Add(new(InputMode.Option, "play", "Play"));

            return inputs.ToArray();
        }

        public override void HandleInput(ClientAction action, ServerResponse response)
        {
            if (action.action.Equals("linkDiscord"))
                LinkDiscord();
            else if(action.action.Equals("play"))
                Play();
        }

        void LinkDiscord()
        {
            string code = Utils.RandomCode();
            SlashCommands.LinkCommand.codes.Add(code, session.Account._id);
            session.Log($"Your code is: {Utils.Style(code, "green")}");
            session.Log($"Make sure you are in {Utils.Link("this Discord server", Env.instance.discordInvite)}");
            session.Log($"Please use the /link command in Discord and enter this code");
        }

        void Play()
        {
            session.ClearLog();
            session.SetMenu(new LocationMenu(session));
        }

    }
}
