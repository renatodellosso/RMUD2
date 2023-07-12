using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations
{
    public class Intro : Location
    {

        protected override string Description => "You are standing in a small, ruined courtyard. To the north lies two statues, one a knight and the other an unknown figure in hooded robes. " +
                    "Between them is a tunnel into the mountain.";

        public Intro()
        {
            id = "intro";
            name = "Outside the dungeon";
            status = "Preparing for an expedition";

            safe = true;

            Creatures.SimpleNPC shadowyFigure = new Creatures.SimpleNPC("shadowyFigure", "Shadowy Figure", nameColor: "blue", talkInputs: (session, menu) =>
            {
                //Get inputs for dialogue
                List<Input> inputs = new();

                if (menu.state.Equals("end"))
                    inputs.Add(new(InputMode.Option, "exit", "Farewell."));
                else
                {
                    inputs.Add(new(InputMode.Option, "knowledge", "Knowledge"));
                    inputs.Add(new(InputMode.Option, "fortune", "Fortune"));
                    inputs.Add(new(InputMode.Option, "glory", "Glory"));
                }

                return inputs.ToArray();
            }, talkHandler: (session, action, menu) =>
            {
                //Handle inputs for dialogue
                if (action.action.Equals("exit")) menu.state = "exit";
                else
                {
                    session.Log($"{Utils.Dialogue(creatures.First(), "A wise choice. What you seek you will find below,")}. Strangely, you can't make out a mouth. They raise an arm and" +
                        $" point towards the tunnel.");
                    menu.state = "end";
                }

            }, talkStart: (session) =>
            {
                //On dialogue start
                session.Log(Utils.Dialogue(creatures.First(), "What is it you seek?"));
            });

            creatures.Add(shadowyFigure);
        }

        public override void AddExits()
        {
            //Add the exit to and from the dungeon
            Exit.AddExit(this, Get(Dungeon.startLocation), "E", false);
        }

    }
}