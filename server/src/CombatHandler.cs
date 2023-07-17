using ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CombatHandler
{

    public Session? session;
    public Player? Player => session?.Player; //Saving the player as a regular variable meant it didn't update when the player changed

    Attack? attack;

    public Input[] GetAttacks()
    {
        try
        {
            Player? player = Player;
            if (player == null)
                return Array.Empty<Input>();

            Attack[] attacks = player.GetAttacks();

            List<Input> inputs = new();

            foreach (Attack attack in attacks)
            {
                inputs.Add(new Input(InputMode.Option, "attack." + attack.id, attack.name, attack == this.attack));
            }

            attack ??= attacks[0];

            return MarkInputsAsCombat(inputs.ToArray());
        }
        catch(Exception e)
        {
            Utils.Log(e);
            return Array.Empty<Input>();
        }
    }

    public Input[] GetTargets()
    {
        try
        {
            Player? player = Player;
            if (player == null)
                return Array.Empty<Input>();

            if (attack == null)
            {
                Weapon? weapon = player.Weapon;

                if (weapon == null)
                    return Array.Empty<Input>();

                attack = weapon.Attack;
            }

            Creature[] targets = attack?.getTargets(player).ToArray() ?? Array.Empty<Creature>();
            List<Input> inputs = new();
            foreach (Creature target in targets)
            {
                inputs.Add(new Input(InputMode.Option, "target." + target.baseId, target.FormattedName));
            }

            return MarkInputsAsCombat(inputs.ToArray());
        }
        catch(Exception e)
        {
            Utils.Log(e);
            return Array.Empty<Input>();
        }
    }

    //Adds combat. to beginning of each input's id
    Input[] MarkInputsAsCombat(Input[] input)
    {
        foreach(Input i in input)
        {
            i.id = "combat." + i.id;
        }

        return input;
    }

    public void HandleInput(string input)
    {
        Player? player = Player;
        if (player == null)
            return;

        if (input.StartsWith("target."))
        {
            input = input["target.".Length..]; //Start after target.

            Creature? target = attack?.getTargets(player).FirstOrDefault(t => t.baseId == input);

            if (target == null)
                return;

            if(player.stamina >= attack.staminaCost)
                attack?.execute(player, target);
        }
        else
        {
            input = input["attack.".Length..];

            Utils.Log($"Setting attack to {input}");
            attack = player.GetAttacks().FirstOrDefault(a => a.id == input);
            Utils.Log($"Attack: {attack.id}");
        }
    }

}
