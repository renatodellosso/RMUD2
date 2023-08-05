using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Bot
{

    public static DiscordSocketClient client;

    static Dictionary<string, DiscordSlashCommand> commands = new()
    {
        { "list", new SlashCommands.ListCommand() },
        { "link", new SlashCommands.LinkCommand() },
        { "user", new SlashCommands.UserCommand() },
        { "reset", new SlashCommands.ResetCommand() },
        { "leaderboard", new SlashCommands.LeaderboardCommand() }
    };

    public static async void Init()
    {
        Utils.Log("Initializing Discord bot...");

        DiscordSocketConfig config = new()
        {
            UseInteractionSnowflakeDate = false, //This avoids issues with not being able to interact after 3 seconds
            //I have no clue what this enum set up is. Might have something to do with the Flags attribute
            //We need GuildMembers and GuildPresences to DM users
            GatewayIntents = GatewayIntents.GuildMembers | GatewayIntents.GuildPresences | GatewayIntents.AllUnprivileged //Have to enable these here and on the bot page
        };

        client = new(config);

        //Add event hooks
        client.Log += Log;
        client.LoggedIn += OnLoggedIn;
        client.Ready += OnReady;
        client.SlashCommandExecuted += OnSlashCommand;

        //Log in
        Utils.Log("Bot logging in...");
        await client.LoginAsync(TokenType.Bot, Env.instance.botKey);

        Utils.Log("Discord bot initialized");
    }

    static async Task Log(LogMessage msg)
    {
        Utils.Log(msg.Message);
    }

    static async Task OnLoggedIn()
    {
        Utils.Log("Bot logged in. Starting...");
        await client.StartAsync();
    }

    static async Task OnReady()
    {
        Utils.Log("Building bot slash commands...");
        try
        {
            foreach (DiscordSlashCommand cmd in commands.Values)
                cmd.Create(client);
        } catch (Exception e) 
        {
            Utils.Log("Caught error creating bot slash commands");
            Utils.Log(e);
        }
    }

    static async Task OnSlashCommand(SocketSlashCommand cmd)
    {
        Utils.Log($"Received slash command: {cmd.Data.Name}");
        try
        {
            if (commands.TryGetValue(cmd.Data.Name, out DiscordSlashCommand value))
                await value.Execute(cmd);
            else Utils.Log("Invalid slash command");
        } catch (Exception e)
        {
            Utils.Log($"Caught error executing command: {cmd.Data.Name}");
            Utils.Log(e);
        }
    }

    public static async void UpdateStatus()
    {
        Utils.Log("Updating bot status...");
        await client.SetActivityAsync(new Game($"{Player.Count} Players Online"));
    }

    public static void DMUser(ulong id, string msg)
    {
        try
        {
            Utils.Log($"Sending DM to {id}: {msg}");
            SocketUser user = client.GetUser(id);
            Utils.Log($"User: {user}");
            if (user == null) return;

            user.SendMessageAsync(msg);
        }
        catch (Exception e)
        {
            Utils.Log(e);
        }
    }

}
