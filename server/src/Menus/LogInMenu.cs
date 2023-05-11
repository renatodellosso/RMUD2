using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menus
{
    public class LogInMenu : Menu
    {

        enum State
        {
            Selection,
            CreateAccount,
            SignIn
        }

        State state = State.Selection;

        public override Input[] GetInputs(ServerResponse response)
        {
            List<Input> inputs = new();

            if(state == State.Selection)
            {
                inputs.Add(new(InputMode.Option, "createAccount", "Create Account"));
                inputs.Add(new(InputMode.Option, "signIn", "Sign In"));
            }

            return inputs.ToArray();
        }

        public override void HandleInput(ClientAction action, ServerResponse response)
        {

        }
    }
}
