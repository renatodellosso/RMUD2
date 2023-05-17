using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class DiscordSlashCommand
{

    public abstract void Create(DiscordSocketClient client);
    public virtual async Task Execute(SocketSlashCommand cmd) { } //Make sure to override!

}
