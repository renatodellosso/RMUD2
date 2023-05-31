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

        public Action<SimpleNPC, int>? onTick;

        public SimpleNPC(string id, string name, string nameColor = "", Func<Session, DialogueMenu, Input[]>? talkInputs = null, Action<Session, ClientAction, DialogueMenu>? talkHandler = null, 
            Action<Session>? talkStart = null, int maxHealth = 0, Action<SimpleNPC, int>? onTick = null) : base(id)
        {
            this.name = name;
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
            onTick?.Invoke(this, tickCount);
        }

    }
}
