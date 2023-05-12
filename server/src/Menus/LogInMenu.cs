using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menus
{
    public class LogInMenu : Menu
    {

        enum Mode
        {
            Selection,
            CreateAccount,
            SignIn
        }

        enum State
        {
            Selection,
            Username,
            Password
        }

        Mode mode = Mode.Selection;
        State state = State.Selection;

        public override Input[] GetInputs(ServerResponse response)
        {
            List<Input> inputs = new();

            if(state == State.Selection)
            {
                inputs.Add(new(InputMode.Option, "createAccount", "Create Account"));
                inputs.Add(new(InputMode.Option, "signIn", "Sign In"));
            }
            else
            {
                inputs.Add(back);
                inputs.Add(new(InputMode.Text, "username", mode == Mode.CreateAccount ? "Enter a username" : "Enter your username:"));
            }

            return inputs.ToArray();
        }

        public override void HandleInput(ClientAction action, ServerResponse response)
        {
            if(mode == Mode.Selection && !action.action.Equals("") && !action.action.Equals("init"))
            {
                if (action.action.Equals("createAccount"))
                {
                    mode = Mode.CreateAccount;
                    session.Log("Enter a username:");
                }
                else if (action.action.Equals("signIn"))
                {
                    mode = Mode.SignIn;
                    session.Log("Enter your username:");
                }

                state = State.Username;
            } else
            {
                if (action.action.Equals("back"))
                {
                    if(state == State.Username)
                    {
                        state = State.Selection;
                        mode = Mode.Selection;
                    }
                }
            }
        }
    }
}
