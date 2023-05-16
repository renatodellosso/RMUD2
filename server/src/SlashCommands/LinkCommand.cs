﻿using Discord;
using Discord.WebSocket;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlashCommands
{
    public class LinkCommand : DiscordSlashCommand
    {

        public static Dictionary<string, ObjectId> codes = new();

        public override void Create(DiscordSocketClient client)
        {
            SlashCommandBuilder cmd = new SlashCommandBuilder();
            cmd.WithName("link");
            cmd.WithDescription("Link your Discord account with your RMUD2 account");
            cmd.WithDMPermission(true);
            cmd.AddOption(new SlashCommandOptionBuilder()
                .WithName("code")
                .WithDescription("The code you were given in-game")
                .WithRequired(true));
            client.CreateGlobalApplicationCommandAsync(cmd.Build()); //Build the command
        }

        public override void Execute(SocketSlashCommand cmd)
        {
            cmd.DeferAsync();

            string msg, code = cmd.Data.Options.FirstOrDefault().Value.ToString();
            if (codes.ContainsKey(code))
            {
                Account account = DB.Accounts.Find(codes[code]);
                if (account != null)
                {
                    account.discordId = cmd.User.Id;
                    msg = $"Successfully linked to account: *{account.username}*!";
                }
                else msg = "Invalid code";
            }
            else msg = "Invalid code";

            cmd.FollowupAsync(ephemeral: true, text: msg);
        }
    }
}