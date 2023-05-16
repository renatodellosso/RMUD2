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
    };

    public static async void Init()
    {
        Utils.Log("Initializing Discord bot...");

        client = new DiscordSocketClient();

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
            //List online users
            SlashCommandBuilder cmd = new SlashCommandBuilder();
            cmd.WithName("list");
            cmd.WithDescription("List all online users");
            cmd.WithDMPermission(true);
            client.CreateGlobalApplicationCommandAsync(cmd.Build()); //Build the command
        } catch (Exception e) 
        {
            Utils.Log("Caught error creating bot slash commands: " + e.Message);
            Utils.Log(e.StackTrace);
        }
    }

    static async Task OnSlashCommand(SocketSlashCommand cmd)
    {
        Utils.Log($"Received slash command: {cmd.Data.Name}");
        if (commands.ContainsKey(cmd.Data.Name))
            commands[cmd.Data.Name].Execute(cmd);
        else Utils.Log("Invalid slash command");
    }

}
