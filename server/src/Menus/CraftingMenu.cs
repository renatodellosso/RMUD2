using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menus
{
    public class CraftingMenu : Menu
    {
        //Set state to "exit" to exit crafting

        string title;

        Recipe[] recipes;

        public override bool ShowSidebar => true;

        public CraftingMenu(string title, Recipe[] recipes)
        {
            this.title = title;
            this.recipes = recipes;
        }

        public override void OnStart()
        {
            string msg = $"{Utils.Style(title, bold: true)}";

            foreach(Recipe recipe in recipes)
                msg += $"<br>-{recipe}{(recipe.MaxCraftable(session?.Player) == 0 ? " (Cannot afford)" : "")}";

            session?.Log(msg);
        }

        public override Input[] GetInputs(ServerResponse response)
        {
            List<Input> inputs = new();

            if (state == "")
            {
                inputs.Add(new(InputMode.Option, "exit", "Exit"));

                for (int i = 0; i < recipes.Length; i++)
                {
                    Recipe recipe = recipes[i];
                    inputs.Add(new(InputMode.Option, i.ToString(), recipe.summary));
                }
            }
            else
            {
                inputs.Add(back);

                Recipe recipe = recipes[int.Parse(state)];
                int max = recipe.MaxCraftable(session?.Player);

                if (max >= 1)
                {
                    inputs.Add(new(InputMode.Option, max.ToString(), $"Max - {max}"));
                    inputs.Add(new(InputMode.Option, "1", $"1"));
                    inputs.Add(new(InputMode.Text, "amt", $"Enter an amt, between 1 and {max}"));
                }
            }

            return inputs.ToArray();
        }

        public override void HandleInput(ClientAction action, ServerResponse response)
        {
            if (state.Equals("exit") || action.action == "exit") //Exit dialogue
                session?.SetMenu(new LocationMenu(session));
            else
            {
                if (state == "")
                {
                    int index = int.Parse(action.action);

                    if (index < 0 || index >= recipes.Length)
                    {
                        session?.Log("Invalid index.");
                        return;
                    }

                    Recipe recipe = recipes[index];
                    session?.Log(recipe.ToString()! + "<br>" + recipe.output.First().Overview());

                    state = index.ToString();
                }
                else
                {
                    if(action.action == "back")
                    {
                        state = "";
                        return;
                    }

                    int index = int.Parse(state);
                    Recipe recipe = recipes[index];
                    int max = recipe.MaxCraftable(session?.Player);
                    int amt;

                    try
                    {
                        amt = int.Parse(action.action);
                    }
                    catch
                    {
                        session?.Log($"Invalid amount. Enter a number between 1 and {max}");
                        return;
                    }

                    if (amt < 1 || amt > max)
                    {
                        session?.Log($"Invalid amount. Enter a number between 1 and {max}");
                        return;
                    }

                    recipe.Craft(session.Player, amt);
                }
            }
        }
    }
}