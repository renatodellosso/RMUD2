using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Menu
{

    public Session session;

    public virtual string Status => "In some menu";
    public virtual bool ShowLocationSidebar => false;

    public virtual void OnStart() { } //Should be overridden in child classes

    public abstract Input[] GetInputs(ServerResponse response);

    public abstract void HandleInput(ClientAction action, ServerResponse response);

    //Can be reused in most menus
    protected Input back = new(InputMode.Option, "back", "< Back");
    protected void handleBack(ClientAction action, ServerResponse response)
    {
        if(action.action.Equals(back.id) && session.menuHistory.Count > 0)
        {
            session.menu = session.menuHistory.Last();
            session.menu.session = session;
            session.menuHistory.RemoveAt(session.menuHistory.Count - 1);
        }
    }

}
