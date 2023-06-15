using Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creatures
{
    public class SimpleNPC : Creature
    {

        public Action<Events.OnCreatureTickEventData>? onTick;

        int maxHealth;
        public override int MaxHealth => maxHealth;

        public SimpleNPC(string id, string name, string nameColor = "", Func<Session, DialogueMenu, Input[]>? talkInputs = null, Action<Session, ClientAction, DialogueMenu>? talkHandler = null, 
            Action<Session>? talkStart = null, int maxHealth = 0, Action<Events.OnCreatureTickEventData>? onTick = null) : base(id, name)
        {
            this.nameColor = nameColor;

            attackable = false;

            this.talkInputs = talkInputs;
            this.talkHandler = talkHandler;
            this.talkStart = talkStart;

            this.maxHealth = maxHealth;
            health = maxHealth;

            this.onTick = onTick;

            //Utils.Log($"Created {baseId}");
        }

        protected override void Tick(int tickCount)
        {
            //Run onTick, as long as it's not null
            onTick?.Invoke(new(this));
        }

        public void FlavorMessage()
        {
            Location? location = Location;
            if (location != null)
            {
                Player[] players = location.Players;
                if (players.Any() && Utils.RandFloat() < Config.Gameplay.FLAVOR_MSG_CHANCE) //Don't send flavor messages if there are no players to receive them
                {
                    AI.FlavorMessage(this);
                }
            }
        }

    }
}
