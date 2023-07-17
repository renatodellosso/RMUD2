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

    static DiscordSocketClient client;

    static Dictionary<string, DiscordSlashCommand> commands = new()
    {
        { "list", new SlashCommands.ListCommand() },
        { "link", new SlashCommands.LinkCommand() },
    };

    public static async void Init()
    {
        Utils.Log("Initializing Discord bot...");

        DiscordSocketConfig config = new()
        {
            UseInteractionSnowflakeDate = false, //This avoids issues with not being able to interact after 3 seconds
        };

        client = new(config);

        //Add event hooks
        client.Log += Log;
        client.LoggedIn += OnLoggedIn;
        client.Ready += OnReady;
        client.SlashCommandExecuted += OnSlashCommand;

        //Log in
        Utils.Log("Bot logging in...");
        await client.LoginAsync(Discord.TokenType.Bot, Env.instance.botKey);

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
            Utils.Log("Caught error creating bot slash commands: " + e.Message);
            Utils.Log(e.StackTrace);
        }
    }

    static async Task OnSlashCommand(SocketSlashCommand cmd)
    {
        Utils.Log($"Received slash command: {cmd.Data.Name}");
        try
        {
            if (commands.ContainsKey(cmd.Data.Name))
                await commands[cmd.Data.Name].Execute(cmd);
            else Utils.Log("Invalid slash command");
        } catch (Exception e)
        {
            Utils.Log($"Caught error executing command: {cmd.Data.Name}, Error: {e.Message}\n{e.StackTrace}");
        }
    }

    public static async void UpdateStatus()
    {
        Utils.Log("Updating bot status...");
        await client.SetActivityAsync(new Game($"{Player.Count} Players Online"));
    }

}
