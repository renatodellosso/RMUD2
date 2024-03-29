﻿using System;
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
            Player? player = session?.Player;
            session?.Log($"{Utils.Style("Level up!", "yellow")} level {player?.level ?? 0} -> " +
                $"level {Utils.Style((player?.level + 1 ?? 0).ToString(), "yellow")} ");

            state = "abilityscores";
            session?.Log("Choose an ability score to increase:<br>" +
                $"STR: +{Config.Gameplay.CARRY_WEIGHT_PER_STR} lbs. max carry weight and +1 damage<br>" +
                $"DEX: +1 attack bonus<br>" +
                $"CON: +{Config.Gameplay.HP_PER_CON} HP<br>" +
                $"AGI: Alternates +1 dodge threshold and +{Utils.Percent(Config.Gameplay.STAMINA_REGEN_PER_EVERY_OTHER_AGI)} stamina regen<br>" +
                $"END: +{Config.Gameplay.STAMINA_PER_END} max stamina<br>" +
                $"WIS: +{Utils.Percent(Config.Gameplay.XP_PER_WIS)} XP<br>" +
                $"CHA: +{Config.Gameplay.SELL_CUT_PER_CHA * 100}% sell cut");
        }

        public override Input[] GetInputs(ServerResponse response)
        {
            List<Input> inputs = new List<Input>();
            Player? player = session?.Player;

            if (player != null)
            {
                if(state == "abilityscores")
                {
                    foreach(AbilityScore score in player.abilityScores.Keys)
                    {
                        int value = player.abilityScores[score];
                        inputs.Add(new(InputMode.Option, score.ToString(), score + ": " + Utils.Modifier(value)));
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
            Player player = session?.Player;
            player.level++;
            player.Update();

            session?.SetMenu(new Menus.LocationMenu(session));
            afterwards();
        }

    }
}
