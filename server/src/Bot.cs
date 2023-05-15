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

    public static async void Init()
    {
        Utils.Log("Initializing Discord bot...");

        client = new DiscordSocketClient();

        //Add event hooks
        client.Log += (msg) => new Task(()=>Utils.Log(msg.Message)); //We have to create a task with an action

        await client.LoginAsync(Discord.TokenType.Bot, Env.instance.botKey);
        await client.StartAsync();

        Utils.Log("Discord bot initialized");
    }

}
