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
        public override void Execute(SocketSlashCommand cmd)
        {
            cmd.DeferAsync();

            EmbedBuilder embed = new EmbedBuilder();
            embed.Title = $"Online Players ({Session.sessions.Count})";

            cmd.FollowupAsync(embed: embed.Build());
        }
    }
}