using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menus
{
    public class CharacterMenu : Menu
    {

        public override bool ShowSidebar => true;

        public override void OnStart()
        {
            session?.Log(session.Player!.GetCharacterText());
        }

        public override Input[] GetInputs(ServerResponse response)
        {
            List<Input> inputs = new()
            {
                back
            };

            Player player = session!.Player!;

            if (state == "") {
                inputs.AddRange(new Input[] {
                    new(InputMode.Option, "mainHand", "Main Hand"),
                    new(InputMode.Option, "offHand", "Off Hand"),
                    new(InputMode.Option, "armor", "Armor"),
                    new(InputMode.Option, "bestiary", "Bestiary")
                });

                if (player.vault != null)
                    inputs.Add(new(InputMode.Option, "vault", "View Vault"));
            }
            else if(state == "bestiary")
            {
                foreach(KeyValuePair<string, string> entry in player!.bestiary)
                {
                    inputs.Add(new(InputMode.Option, entry.Key, entry.Value));
                }
            }

            return inputs.ToArray();
        }

        public override void HandleInput(ClientAction action, ServerResponse response)
        {
            Player player = session.Player!;

            if (state == "")
            {
                if (action.action == "back")
                    session?.SetMenu(new LocationMenu(session));
                else if (action.action == "mainHand")
                {
                    if (player == null || player.mainHand == null)
                        session?.Log("You have no weapon equipped.");
                    else session?.Log(player.mainHand.Overview(player));
                }
                else if (action.action == "offHand")
                {
                    if (player == null || player.offHand == null)
                        session?.Log("You have no weapon equipped.");
                    else session?.Log(player.offHand.Overview(player));
                }
                else if (action.action == "armor")
                {
                    if (player == null || player.armor == null)
                        session?.Log("You have no armor equipped.");
                    else session?.Log(player.armor.Overview(player));
                }
                else if (action.action == "vault")
                {
                    if (player.vault != null)
                    {
                        session.Log(player.vault.GetText());
                    }
                }
                else if (action.action == "bestiary")
                {
                    state = "bestiary";

                    string msg = Utils.Style($"Bestiary ({player.bestiary.Count}/{Creatures.MonsterList.MONSTERS.Count}, " +
                        $"{Utils.Percent((float)player.bestiary.Count/Creatures.MonsterList.MONSTERS.Count)}):", bold: true);
                    foreach(KeyValuePair<string, string> entry in player.bestiary)
                    {
                        msg += $"<br>-{entry.Value}";
                    }

                    session.Log(msg);
                }
            }
            else if (state == "bestiary")
            {
                if(action.action == "back")
                    state = "";
                else
                {
                    //IMPORANT: This could end up being pretty slow if we have a lot of monsters
                    KeyValuePair<float, Creatures.MonsterList.MonsterEntry>[] monsters = Creatures.MonsterList.MONSTERS.contents;

                    bool found = false;
                    foreach(KeyValuePair<float, Creatures.MonsterList.MonsterEntry> entry in monsters)
                    {
                        Creature creature = entry.Value(false);

                        if(creature.baseId == action.action)
                        {
                            session.Log(creature.GetBestiaryEntry());
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                        session.Log("Monster not found.");
                }
            }
        }
    }
}
