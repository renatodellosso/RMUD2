﻿using Discord;
using Discord.WebSocket;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SlashCommands
{
	public class PlaytimeCommand : DiscordSlashCommand
	{

		public static Dictionary<string, ObjectId> codes = new();

		public override void Create(DiscordSocketClient client)
		{
			SlashCommandBuilder cmd = new SlashCommandBuilder();
			cmd.WithName("playtime");
			cmd.WithDescription("View the RMUD2 Playtime leaderboard");
			cmd.WithDMPermission(true);
			client.CreateGlobalApplicationCommandAsync(cmd.Build()); //Build the command
		}

		public override async Task Execute(SocketSlashCommand cmd)
		{
			await cmd.DeferAsync(); //Remember to await!

			List<Account> accounts = DB.accounts.Find(new BsonDocument()).ToList();
			List<KeyValuePair<Account, Player>> found = new();

			foreach (Account account in accounts)
			{
				Player player = account.Player;
				if (player != null && player.playtime.Minutes > 0)
				{
					found.Add(new(account, player));
				}
			}

			found.Sort((a, b) => b.Value.playtime.CompareTo(a.Value.playtime));

			EmbedBuilder embed = new();
			embed.WithTitle("RMUD2 Playtime Leaderboard");

			string desc = "";
			for (int i = 0; i < found.Count; i++)
			{
				Account account = found[i].Key;
				Player player = found[i].Value;

				if (player != null)
				{
					desc += $"{i + 1}. {account.username?.Substring(0, Math.Min(account.username.Length, 30))}";

					if (account.discordId != 0)
						desc += $" (<@{account.discordId}>)";

					desc += $" - Playtime: {player.playtime}\n";
				}
			}

			embed.WithDescription(desc);

			cmd.FollowupAsync(embed: embed.Build());
		}
	}
}