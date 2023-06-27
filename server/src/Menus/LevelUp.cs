using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Menus
{
    public class LevelUp : Menu
    {

        public override string Status => session.Player.Location.status;
        public override bool ShowSidebar => true;

        Action afterwards;

        public LevelUp(Session session, Action afterwards)
        {
            this.session = session;
            this.afterwards = afterwards;
        }

        public override void OnStart()
        {
            session?.Log($"{Utils.Style("Level up!", "yellow")} level {session.Player?.level - 1} -> level {Utils.Style(session.Player?.level.ToString(), "yellow")} ");

            state = "abilityscores";
            session?.Log("Choose an ability score to increase:");
        }

        public override Input[] GetInputs(ServerResponse response)
        {
            List<Input> inputs = new List<Input>();
            Player? player = session?.Player;

            if(state == "") state = "abilityscores";

            if (player != null)
            {
                if(state == "abilityscores")
                {
                    foreach(AbilityScore score in player.abilityScores.Keys)
                    {
                        int value = player.abilityScores[score];
                        inputs.Add(new(InputMode.Option, score.ToString(), score + ": " + value));
                    }
                }
            }

            return inputs.ToArray();
        }

        public override void HandleInput(ClientAction action, ServerResponse response)
        {
            Player? player = session?.Player;

            if (player != null)
            {
                if (state == "abilityscores")
                {
                    AbilityScore score = (AbilityScore)Enum.Parse(typeof(AbilityScore), action.action);

                    player.abilityScores[score] += 1;

                    Exit();
                }
            }
            else session?.SetMenu(new Menus.LocationMenu(session));
        }

        void Exit()
        {
            session?.Player?.Update();
            session?.SetMenu(new Menus.LocationMenu(session));
            afterwards();
        }

    }
}
