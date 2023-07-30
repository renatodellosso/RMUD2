using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlashCommands
{
    public class ListCommand : DiscordSlashCommand
    {
        public override void Create(DiscordSocketClient client)
        {
            SlashCommandBuilder cmd = new SlashCommandBuilder();
            cmd.WithName("list");
            cmd.WithDescription("List all online users");
            cmd.WithDMPermission(true);
            client.CreateGlobalApplicationCommandAsync(cmd.Build()); //Build the command
        }

        public override async Task Execute(SocketSlashCommand cmd)
        {
            try
            {
                await cmd.DeferAsync(); //We need to await this

                EmbedBuilder embed = new();
                embed.Title = $"Online Players ({Player.Count})";

                int unsignedIn = 0;
                foreach (Session? session in Session.sessions.Values)
                {
                    if (session != null && session.SignedIn)
                    {
                        Account? account = session.Account;
                        embed.Description += account?.username ?? "Unknown";
                        if (account != null && account.discordId != 0) embed.Description += $" (<@{account.discordId}>";

                        if(session.Player != null)
                        {
                            embed.Description += $", level {session.Player.level}";
                        }

                        embed.Description += ")";

                        embed.Description += $" - {session.menu.Status}";
                        embed.Description += "\n";
                    } else unsignedIn++;
                }

                embed.Description += $"+{unsignedIn} sessions not signed in";

                await cmd.FollowupAsync(embed: embed.Build());
            } catch (Exception e)
            {
                Utils.Log($"Caught error executing slash command;");
                Utils.Log(e);
            }
        }
    }
}