using Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class Creature
{
    //We can't use id because of Player's _id, so we use baseId
    public string baseId = "unnamedCreature", name = "Unnamed Creature";

    public bool attackable = true;

    //Null for either means creature has no dialogue. The string is the dialogue state
    public Func<Session, DialogueMenu, Input[]>? talkInputs = null;
    public Action<Session, ClientAction, DialogueMenu>? talkHandler = null;
    public Action<Session>? talkStart = null;
    public bool HasDialogue => talkInputs != null && talkHandler != null && talkStart != null;

}