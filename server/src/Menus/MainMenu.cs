using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menus
{
    public class MainMenu : Menu
    {

        bool waiting = false;

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

            if (!waiting)
            {
                Account account = session.Account;

                if (account.discordId == 0)
                    inputs.Add(new(InputMode.Option, "linkDiscord", "Link Discord Account"));

                inputs.Add(new(InputMode.Option, "play", "Play"));
            }

            return inputs.ToArray();
        }

        public override void HandleInput(ClientAction action, ServerResponse response)
        {
            if (action.action.Equals("linkDiscord"))
                LinkDiscord();
            else if (action.action.Equals("play"))
            {
                session.Log("Loading...");
                Task.Run(Play);
            }
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
            try
            {
                Account account = session.Account;

                if (account != null)
                {
                    Player player = null; //We have to set it to null to avoid issues later
                    if (account.playerId != null)
                    {
                        session.playerId = session.Account.playerId;
                        if (session.playerId != null)
                        {
                            player = Player.Get((ObjectId)session.playerId); //Rly annoying having to do this
                        }
                        else session.Log(Utils.Style("Encountered an error loading game", "red"));
                    }
                    else
                    {
                        //Account doesn't have a player, we need to make a new one
                        Utils.Log($"Creating new player for {account.username}...");
                        session.Log("Setting up new game...");

                        player = new(ObjectId.GenerateNewId())
                        {
                            accountId = account._id,
                            session = session,
                            locationId = Config.START_LOCATION
                        };

                        DB.players.InsertOne(player);
                        Utils.Log($"Finished creating new player for {account.username}");
                    }

                    if (player != null)
                    {
                        Player.Add(player); //Register the player as active

                        session.ClearLog();
                        session.SetMenu(new LocationMenu(session));

                        player.Location.Enter(player);
                    }
                }
                else session.Log(Utils.Style("Encountered an error", "red"));
            } catch (Exception ex) 
            {
                Utils.Log($"Error: {ex.Message}");
                session.Log(Utils.Style("Encountered an error", "red"));
            }
        }

    }
}
