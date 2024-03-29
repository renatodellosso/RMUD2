﻿using ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Items
{
    public class SimpleConsumable : Item, IConsumable
    {

        string verb = "Use";

        protected Action<Session>? onUse;

        int sellValue = 1;
        public override int SellValue(ItemHolder<Item>? item) => (int)(sellValue * (uses > 0 && item != null ? (float)(int)item.data["uses"] / uses : 1));

        int uses = 1; //Set to -1 for infinite uses

        public SimpleConsumable(string id, string name, float weight, Action<Session>? onUse, string verb = "Use", string description = "No description provided",
            int sellValue = 1, int uses = 1, string color = "white")
            : base(id, name, weight, description, color)
        {
            this.verb = verb;
            this.onUse = onUse;
            this.sellValue = sellValue;
            this.uses = uses;
        }

        public override List<Input> GetInputs(Session session, ItemHolder<Item> item, string state)
        {
            List<Input> inputs = base.GetInputs(session, item, state);
            inputs.Add(((IConsumable)this).GetInput());
            return inputs; //There has to be a better way to call interface methods
        }

        public override void HandleInput(Session session, ClientAction action, ItemHolder<Item> item, ref string state, ref bool addStateToPrev)
        {
            if(action.action == "use")
                Use(session, ref state, ref addStateToPrev, item.data);
            else base.HandleInput(session, action, item, ref state, ref addStateToPrev);
        }

        public string Verb()
        {
            return verb;
        }

        public void Use(Session session, ref string state, ref bool addStateToPrev, Dictionary<string, object> data)
        {
            onUse?.Invoke(session);

            data.TryGetValue("uses", out object? currentUses);
            data["uses"] = (int)(currentUses ?? uses) - 1;
            data.TryGetValue("uses", out currentUses);

            session.Log($"Used {name}.{(uses > 0 ? $" {(int)currentUses!}/{uses} uses remaining." : "")}");

            if ((int)(currentUses ?? uses) <= 0)
            {
                session.Player?.inventory.Remove(new ItemHolder<Item>(id, 1));
                state = "inventory";
                addStateToPrev = false;
            }

            session.Player?.Update();
        }

        public override string Overview(ItemHolder<Item> item, Creature? creature = null)
        {
            try
            {
                item.data.TryGetValue("uses", out object? currentUses);

                if (currentUses == null)
                {
                    item.data["uses"] = uses;
                    currentUses = uses;
                }

                return base.Overview(item, creature) + (uses > 0 ? $"<br>{currentUses}/{uses} uses left" : "<br>Unlimited uses");
            }
            catch (Exception e)
            {
                Utils.Log(e);
                return base.Overview(item, creature);
            }
        }
    }
}
