using ItemTypes;
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

        public override string Status => "In the main menu";

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
                    //If something is invalid here, we make a new player
                    Player player = null; //We have to set it to null to avoid issues later
                    bool shouldInit = false; //This lets us make sure only one player is created
                    if (account.playerId != null)
                    {
                        session.playerId = account.playerId;
                        if (session.playerId != null)
                        {
                            player = Player.Get((ObjectId)session.playerId);

                            if (player != null)
                            {
                                if (player.location != null)
                                {
                                    player.location = player.resetLocation ?? Config.Gameplay.START_LOCATION;
                                    player.resetLocation = player.resetLocation ?? Config.Gameplay.RESPAWN_LOCATION;
                                    player.Update();
                                }
                                else
                                {
                                    shouldInit = true;
                                    //Utils.Log("LocationID is null");
                                    //I'm leaving the logs in here for debugging purposes and so I can see what each else is
                                }
                            }
                            else
                            {
                                shouldInit = true;
                                //Utils.Log("Player is null");
                            }
                        }
                        else
                        {
                            shouldInit = true;
                            //Utils.Log("Session playerID is null");
                        }
                    }
                    else
                    {
                        shouldInit = true;
                        //Utils.Log("Account playerID is null");
                    }

                    if (shouldInit) InitPlayer();

                    account = session.Account;
                    player = account.Player;

                    if (player != null)
                    {
                        player.session = session; //Make sure the player has a session!

                        Player.Add(player); //Register the player as active

                        player.CalculateStats();

                        session.ClearLog();
                        session.SetMenu(new LocationMenu(session));

                        player.Location.Enter(player, null);
                    }
                    else session.Log(Utils.Style("Encountered an error: Player is null", "red"));
                }
                else session.Log(Utils.Style("Encountered an error: Account is null", "red"));
            } catch (Exception ex) 
            {
                Utils.Log($"Error in MainMenu.Play(): {ex.Message} {ex.StackTrace}");
                session.Log(Utils.Style("Encountered an uncaught error", "red"));
            }
        }

        public void InitPlayer()
        {
            try
            {
                Account account = session.Account;

                //Account doesn't have a player, we need to make a new one
                Utils.Log($"Creating new player for {account.username}...");
                session.Log("Setting up new game...");

                Player player = new(account._id)
                {
                    accountId = account._id,
                    session = session,
                    location = Config.Gameplay.START_LOCATION,
                    name = account.username,
                    baseId = account.username,
                    mainHand = new ItemHolder<Item>("spear")
                };

                player.health = player.MaxHealth;

                player.CalculateStats();

                account.playerId = player._id; //Make sure to set the ID!
                account.Update();

                session.playerId = player._id;

                DB.players.InsertOne(player);
                Utils.Log($"Finished creating new player for {account.username}");
                session.Log("Finished setting up new game");
            } 
            catch(Exception e)
            {
                Utils.Log($"Error in MainMenu.InitPlayer(): {e.Message} {e.StackTrace}");
                session.Log(Utils.Style("Encountered an error: Error with initializing player", "red"));
            }
        }

    }
}
