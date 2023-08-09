using Discord;
using Discord.WebSocket;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlashCommands
{
    public class RestartCommand : DiscordSlashCommand
    {

        static HashSet<ulong> votes = new();
        static bool restarting = false;

        public override void Create(Discord.WebSocket.DiscordSocketClient client)
        {
            SlashCommandBuilder cmd = new SlashCommandBuilder();
            cmd.WithName("restart");
            cmd.WithDescription("Vote to restart the server");
            cmd.WithDMPermission(true);
            client.CreateGlobalApplicationCommandAsync(cmd.Build()); //Build the command
        }

        public override async Task Execute(Discord.WebSocket.SocketSlashCommand cmd)
        {
            cmd.DeferAsync();

            try
            {
                votes.Add(cmd.User.Id);

                cmd.FollowupAsync($"Voted to restart the server. {votes.Count}/{Player.Count} votes");
                Utils.Announce($"{cmd.User.Username} voted to restart the server. {votes.Count}/{Player.Count} votes");

                if(votes.Count > Player.Count / 2 && !restarting)
                {
                    Utils.Log("Restarting server in 30 seconds...");
                    Utils.Announce("Restarting server in 30 seconds...");
                    Restart();
                }
            } catch (Exception e)
            {
                Utils.Log(e);
            }
        }

        async Task Restart()
        {
            restarting = true;
            await Task.Delay(30 * 1000);

            Utils.Log("Restarting server...");
            Environment.Exit(0);
        }
    }
}
