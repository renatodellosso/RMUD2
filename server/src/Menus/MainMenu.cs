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

        bool waiting = false, loadingIntoGame = false;

        public override string Status => "In the main menu";

        public MainMenu(Session session)
        {
            this.session = session;
        }

        public override void OnStart()
        {
            try
            {
                Account? account = session?.Account;

                if (account == null)
                    Utils.Log("Account is null!");
                else
                {
                    session?.ClearLog();

                    session?.Log("Welcome to RMUD2!");
                    session?.Log($"Signed in as {account?.username}");

                    if (account?.discordId == 0)
                        session?.Log($"You have {Utils.Style("not", "maroon")} linked your Discord account. Please do so to ensure you can recover your account");
                }
            }
            catch (Exception ex)
            {
                Utils.Log(ex);
            }
        }

        public override Input[] GetInputs(ServerResponse response)
        {
            List<Input> inputs = new();

            try {
                if (!waiting)
                {
                    Account? account = session?.Account;

                    if (account?.discordId == 0)
                        inputs.Add(new(InputMode.Option, "linkDiscord", "Link Discord Account"));

                    inputs.Add(new(InputMode.Option, "play", "Play"));
                }
            }
            catch (Exception ex)
            {
                Utils.Log(ex);
            }

            return inputs.ToArray();
        }

        public override void HandleInput(ClientAction action, ServerResponse response)
        {
            try {
                if (action.action.Equals("linkDiscord"))
                    LinkDiscord();
                else if (action.action.Equals("play") && !loadingIntoGame)
                {
                    loadingIntoGame = true;
                    session?.Log("Loading...");
                    Task.Run(Play);
                }
            }
            catch (Exception ex)
            {
                Utils.Log(ex);
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
            Utils.Log("Attempting to start game...");
            try
            {
                Account? account = session?.Account;

                if (account != null)
                {
                    Utils.Log("Account is not null");
                    //If something is invalid here, we make a new player
                    Player? player = null; //We have to set it to null to avoid issues later
                    bool shouldInit = false; //This lets us make sure only one player is created
                    if (account.playerId != null)
                    {
                        Utils.Log("Account.playerId is not null");
                        session.playerId = account.playerId;
                        if (session.playerId != null)
                        {
                            Utils.Log("Session.playerId is not null. Id: " + session.playerId);
                            player = session.Player;

                            if (player != null)
                            {
                                Utils.Log("Player is not null");
                                if (player.location != null)
                                {
                                    Utils.Log("Player.location is not null");
                                    player.location = player.resetLocation ?? Config.Gameplay.START_LOCATION;
                                    //If the player doesn't have a reset location, set it to the respawn location
                                    player.resetLocation ??= Config.Gameplay.RESPAWN_LOCATION;
                                    player.visitedRooms = new();

                                    session.combatHandler.session = session;

                                    player.Update();
                                    Utils.Log("Successfully loaded player data");
                                }
                                else
                                {
                                    shouldInit = true;
                                    Utils.Log("LocationID is null");
                                    //I'm leaving the logs in here for debugging purposes and so I can see what each else is
                                }
                            }
                            else
                            {
                                shouldInit = true;
                                Utils.Log("Player is null");
                            }
                        }
                        else
                        {
                            shouldInit = true;
                            Utils.Log("Session playerID is null");
                        }
                    }
                    else
                    {
                        shouldInit = true;
                        Utils.Log("Account playerID is null");
                    }

                    if (shouldInit) InitPlayer();

                    account = session?.Account;
                    player = account?.Player;

                    if (player != null)
                    {
                        player.session = session; //Make sure the player has a session!

                        Player.Add(player); //Register the player as active

                        player.CalculateStats();

                        session?.ClearLog();
                        session?.SetMenu(new LocationMenu(session));

                        try
                        {
                            if (player.Location == null)
                                player.location = "dungeonentrance";

                            player.Location?.Enter(player, null);
                        } catch (Exception e)
                        {
                            Utils.Log(e);
                        }
                    }
                    else session?.Log(Utils.Style("Encountered an error: Player is null", "red"));
                }
                else session?.Log(Utils.Style("Encountered an error: Account is null", "red"));
            } catch (Exception ex) 
            {
                Utils.Log(ex);
                session?.Log(Utils.Style("Encountered an uncaught error", "red"));
            }

            Utils.Log("Finished attempting to start game");
        }

        public void InitPlayer()
        {
            try
            {
                Account? account = session?.Account;

                //Account doesn't have a player, we need to make a new one
                Utils.Log($"Creating new player for {account?.username}...");
                session?.Log("Setting up new game...");

                Player player = new(account._id)
                {
                    accountId = account._id,
                    session = session,
                    location = Config.Gameplay.START_LOCATION,
                    name = account.username,
                    baseId = account.username,
                    mainHand = new ItemHolder<Item>("spear"),
                    armor = new ItemHolder<Armor>("peasantclothes"),
                };

                player.health = player.MaxHealth;

                player.CalculateStats();

                account.playerId = player._id; //Make sure to set the ID!
                account?.Update();

                session.playerId = player._id;

                DB.players.InsertOne(player);
                Utils.Log($"Finished creating new player for {account?.username}");
                session.Log("Finished setting up new game");
            } 
            catch(Exception e)
            {
                Utils.Log(e);
                session?.Log(Utils.Style("Encountered an error: Error with initializing player", "red"));
            }
        }

    }
}
