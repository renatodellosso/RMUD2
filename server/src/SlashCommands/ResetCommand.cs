using Discord;
using Discord.WebSocket;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlashCommands
{
    public class ResetCommand : DiscordSlashCommand
    {

        public static Dictionary<string, ObjectId> codes = new();

        public override void Create(DiscordSocketClient client)
        {
            SlashCommandBuilder cmd = new SlashCommandBuilder();
            cmd.WithName("reset");
            cmd.WithDescription("Reset your RMUD2 password");
            cmd.WithDMPermission(true);
            cmd.AddOption(new SlashCommandOptionBuilder()
                .WithName("code")
                .WithDescription("The code you were given in-game")
                .WithRequired(true)
                .WithType(ApplicationCommandOptionType.String));
            client.CreateGlobalApplicationCommandAsync(cmd.Build()); //Build the command
        }

        public override async Task Execute(SocketSlashCommand cmd)
        {
            await cmd.DeferAsync(ephemeral: true); //Remember to await!

            Account? account = (await DB.accounts.FindAsync(s => s.discordId == cmd.User.Id)).FirstOrDefault();

            if (account == null)
            {
                await cmd.FollowupAsync(ephemeral: true, text: "You do not have an account linked to your Discord account.");
                return;
            }

            IEnumerable<Session> sessions = Session.sessions.Values.Where(s => s.menu is Menus.LogInMenu);
            sessions = sessions.Where(s => ((Menus.LogInMenu)s.menu).username == account.username);
            sessions = sessions.Where(s => ((Menus.LogInMenu)s.menu).passwordResetCode == cmd.Data.Options.First().Value.ToString());

            Session? session = sessions.Any() ? sessions.FirstOrDefault() : null;
            if (session == null)
            {
                await cmd.FollowupAsync(ephemeral: true, text: "Invalid code.");
                return;
            }

            ((Menus.LogInMenu)session.menu).passwordResetEnabled = true;
            session.Log("Password reset enabled");
            cmd.FollowupAsync(ephemeral: true, text: $"Enabled password reset for account: {account.username}");
        }
    }
}