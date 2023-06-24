using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Session
{

    public static Dictionary<ObjectId, Session> sessions = new Dictionary<ObjectId, Session>();

    public ObjectId? accountId, playerId, id = ObjectId.GenerateNewId();
    public Account? Account => GetAccount();

    public Player? Player => GetPlayer();

    public bool SignedIn => accountId != null;

    public Menu menu;
    public List<Menu> menuHistory = new();

    public List<string> log = new();
    public bool logChanged = false;

    List<string> sidebar = new (), oldSidebar = new ();
    public bool SidebarChanged => DidSidebarChange();
    public bool resendSidebar = false;

    public static Session CreateSession()
    {
        Session session = new();
        
        sessions.Add((ObjectId)session.id, session);
        return session;
    }

    public void SetMenu(Menu menu)
    {
        menuHistory.Add(this.menu);
        this.menu = menu;
        this.menu.session = this;
        this.menu.OnStart();
    }

    public void Log(string msg)
    {
        log.Add(msg);
        logChanged = true;
    }

    /// <summary>
    /// Removes the last entry in the log
    /// </summary>
    public void PopLog()
    {
        log.RemoveAt(log.Count - 1);
        logChanged = true;
    }

    /// <summary>
    /// Replaces the most recent message
    /// </summary>
    public void ReplaceLog(string msg)
    {
        log[log.Count - 1] = msg;
        logChanged = true;
    }

    /// <summary>
    /// Clears the log portion of the screen for the user
    /// </summary>
    public void ClearLog()
    {
        log.Clear();
        logChanged = true;
    }

    bool DidSidebarChange()
    {
        if(resendSidebar)
        {
            resendSidebar = false;
            return true;
        }

        if (sidebar.Count != oldSidebar.Count) return true;
        for (int i = 0; i < sidebar.Count; i++)
        {
            if (!sidebar[i].Equals(oldSidebar[i])) return true;
        }
        return false;
    }

    public List<string> GetSidebar()
    {
        List<string> text = new();

        Player? player = Player;
        if(player != null)
        {
            text.Add($"User: {player.name}");

            if (menu.ShowSidebar)
            {
                string color = player.xp >= player.XpToNextLevel ? "yellow" : "white";
                text.Add(Utils.Style($"Level: {player.level} - {player.xp}/{player.XpToNextLevel} XP", color));

                color = "green";
                if (player.health < player.MaxHealth * .25) color = "red";
                else if(player.health < player.MaxHealth * .5) color = "orange";
                else if (player.health < player.MaxHealth * .75) color = "yellow";
                text.Add($"HP: {Utils.Style($"{player.health}/{player.MaxHealth}",color)}<br>" +
                    $"DT: {player.DodgeThreshold}");

                Location? location = player.Location;
                if (location != null)
                {
                    text.Add($"Location: {location?.name}");
                    text.Add(location?.GetCreatureListMessage(player));
                }

                text.Add($"Main Hand: {player.mainHand?.Item?.FormattedName ?? "Empty" }<br>" +
                         $"Off Hand: {player.offHand?.Item?.FormattedName ?? "Empty"}");

                text.Add($"Carrying: {player.inventory.Weight}/{player.inventory.MaxWeight} lbs.");
            }
        }

        oldSidebar = sidebar;
        sidebar = text;
        return text;
    }

    private Account? GetAccount()
    {
        if (accountId == null) return null;
        else return DB.Accounts.Find(accountId);
    }

    private Player? GetPlayer()
    {
        if (!SignedIn) return null;
        else if (playerId == null) playerId = Account.playerId; //We don't want to get the account unless we have to, so we save the player ID

        if(playerId != null)
            return Player.Get(playerId.Value); //We use .Value on vars with ? to get the value if it's not null
        return null;
    }

    static Session? Find(ObjectId id)
    {
        try
        {
            Session[] sessionList = sessions.Values.Where(s => s.accountId.Equals(id)).ToArray();
            if (sessionList.Length == 0) return null;
            return sessionList[0];
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Finds the session linked to the account
    /// </summary>
    /// <returns>The session. Returns null if no session was found</returns>
    public static Session? Find(Account account)
    {
        return Find(account._id);
    }

}
