﻿using System;
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

            creatures.Add(new Creatures.SimpleNPC("shadowyFigure", "Shadowy Figure", nameColor: "blue", talkInputs: (session, menu) =>
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
                    session.Log($"{Utils.Dialogue("A wise choice. What you seek you will find below,")} the figure says, though you can't make out a mouth. They raise an arm and" +
                        $" point towards the tunnel.");
                    menu.state = "end";
                }

            }, talkStart: (session) =>
            {
                //On dialogue start
                session.Log("What is it you seek?");
            }));

            //Add event listener, so we can add an exit to the dungeon
            Dungeon.OnDungeonGenerated += OnDungeonGenerated;
        }

        void OnDungeonGenerated()
        {
            //Add the exit to and from the dungeon
            Exit.AddExit(this, Get(Dungeon.startLocation), "E", false);
        }

    }
}