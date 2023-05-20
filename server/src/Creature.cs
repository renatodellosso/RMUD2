using Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class Creature
{

    public string name = "Unnamed Creature";

    public bool attackable = true;

    //Null for either means creature has no dialogue. The string is the dialogue state
    public Func<Player, string, Input[]>? talkInputs = null;
    public Action<Player, ClientAction, string>? talkHandler = null;
    public bool HasDialogue => talkInputs != null && talkHandler != null;

}