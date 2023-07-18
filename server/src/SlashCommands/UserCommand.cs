using Discord;
using Discord.WebSocket;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlashCommands
{
    public class UserCommand : DiscordSlashCommand
    {
        public override void Create(Discord.WebSocket.DiscordSocketClient client)
        {
            SlashCommandBuilder cmd = new SlashCommandBuilder();
            cmd.WithName("user");
            cmd.WithDescription("Get info about a user");
            cmd.WithDMPermission(true);
            cmd.AddOption("user", ApplicationCommandOptionType.User, "Which user to get info about", true); //true for isRequired
            client.CreateGlobalApplicationCommandAsync(cmd.Build()); //Build the command
        }

        public override async Task Execute(Discord.WebSocket.SocketSlashCommand cmd)
        {
            try
            {
                await cmd.DeferAsync();

                SocketUser user = (SocketUser)cmd.Data.Options.First().Value;

                Account? account = DB.accounts.FindAsync(Builders<Account>.Filter.Eq("discordId", user.Id)).Result.FirstOrDefault();

                if (account == null)
                {
                    await cmd.FollowupAsync($"User <@{user.Id}> has not linked their RMUD2 account to their Discord account, so data cannot be fetched.");
                    return;
                }

                EmbedBuilder embed = new();
                embed.Title = $"User: {account.username}";

                embed.Description += $"Discord: <@{user.Id}>";

                if (account.playerId != null)
                {
                    Player? player = Player.Get(account.playerId.Value);

                    embed.Description += $"\n**Level: {player.level}** ({player.xp}/{player.XpToNextLevel})";

                    embed.Description += $"\n\n**{player.session?.menu.Status ?? "Offline"}**";

                    embed.Description += $"\n\nMax HP: {player.MaxHealth}";
                    embed.Description += $"\nDT: {player.DodgeThreshold}";

                    embed.Description += $"\n\nMain Hand: {player.mainHand?.Item?.name ?? "Empty"}";
                    embed.Description += $"\nOff Hand: {player.offHand?.Item?.name ?? "Empty"}";
                    embed.Description += $"\nArmor: {player.armor?.Item?.name ?? "None"}";

                    embed.Description += $"\n\nCoins: {player.coins}";

                    embed.Description += $"\n\n**Ability Scores:**";
                    foreach (KeyValuePair<AbilityScore, int> score in player.abilityScores)
                        embed.Description += $"\n{score.Key}: {Utils.Modifier(score.Value)}";
                }

                await cmd.FollowupAsync(embed: embed.Build());
            } catch (Exception e)
            {
                Utils.Log(e);
            }
        }
    }
}
