using MongoDB.Bson;
using MongoDB.Driver;
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
            Password,
            ConfirmPassword,
            FinalConfirmation
        }

        Mode mode = Mode.Selection;
        State state = State.Selection;
        bool waiting = false;

        string username = "", password = "";

        public override Input[] GetInputs(ServerResponse response)
        {
            List<Input> inputs = new();

            if (!waiting)
            {
                if (state == State.Selection)
                {
                    inputs.Add(new(InputMode.Option, "createAccount", "Create Account"));
                    inputs.Add(new(InputMode.Option, "signIn", "Sign In"));
                }
                else
                {
                    inputs.Add(back);
                    if (state == State.Username)
                        inputs.Add(new(InputMode.Text, "username", mode == Mode.CreateAccount ? "Enter a username" : "Enter your username"));
                    else if (state == State.Password)
                        inputs.Add(new(InputMode.Secret, "password", mode == Mode.CreateAccount ? "Enter a password" : "Enter your password"));
                    else if (state == State.ConfirmPassword)
                        inputs.Add(new(InputMode.Secret, "confirmPassword", "Reenter your password"));
                    else if (state == State.FinalConfirmation)
                        inputs.Add(new(InputMode.Option, "confirm", "Confirm & Create Account"));
                }
            }

            return inputs.ToArray();
        }

        public override void HandleInput(ClientAction action, ServerResponse response)
        {
            if (!waiting)
            {
                if (mode == Mode.Selection && !action.action.Equals("") && !action.action.Equals("init"))
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
                }
                else
                {
                    if (action.action.Equals("back"))
                    {
                        if (state == State.Username)
                        {
                            state = State.Selection;
                            mode = Mode.Selection;
                        }
                        else if (state == State.Password) state = State.Username;
                        else if (state == State.ConfirmPassword) state = State.Password;
                        else state = State.ConfirmPassword;
                    }
                    else
                    {
                        if (state == State.Username)
                        {
                            if (mode == Mode.SignIn || !Account.UsernameTaken(action.action))
                            {
                                if (mode == Mode.SignIn || Utils.IsInputSafe(action.action))
                                {
                                    username = action.action;
                                    state = State.Password;
                                    session.ReplaceLog("Username: " + username);
                                    session.Log(mode == Mode.CreateAccount ? "Enter a password:" : "Enter your password:");
                                }
                                else
                                {
                                    session.ReplaceLog(Utils.Style($"{action.action} is not a valid username.", "red") + " Please enter a new username:");
                                }
                            }
                            else
                            {
                                session.ReplaceLog($"The username {action.action} is taken. Enter a new username:");
                            }
                        }
                        else if (state == State.Password)
                        {
                            password = action.action;
                            if (mode == Mode.CreateAccount)
                            {
                                state = State.ConfirmPassword;
                                session.ReplaceLog("Password: *****");
                                session.Log("Reenter your password:");
                            }
                            else
                            {
                                waiting = true;
                                Task.Run(SignIn);
                                session.ReplaceLog("Password: *****");
                                session.Log("Signing in...");
                            }
                        }
                        else if (state == State.ConfirmPassword)
                        {
                            if (action.action.Equals(password))
                            {
                                state = State.FinalConfirmation;
                                session.ReplaceLog("Password confirmed");
                            }
                            else session.ReplaceLog(Utils.Style("Passwords do not match. Reenter your password:", "red"));
                        }
                        else if (state == State.FinalConfirmation && action.action.Equals("confirm"))
                        {
                            waiting = true;
                            Task.Run(CreateAccount);
                            session.Log("Creating account...");
                        }
                    }
                }
            }
        }

        void CreateAccount()
        {
            ObjectId? id = Account.CreateAccount(username, password);
            if (id != null)
            {
                session.ReplaceLog(Utils.Style("Account created!", "green"));
            }
            else session.ReplaceLog(Utils.Style("Failed to create account", "red"));
            waiting = false;
        }

        void SignIn()
        {
            ObjectId? result = Account.VerifyCredentials(username, password);
            if (result != null)
            {
                session.ReplaceLog(Utils.Style("Logged in!", "green"));
                session.accountId = result;
            }
            else
            {
                session.ReplaceLog(Utils.Style("Invalid username or password", "red"));
            }
            waiting = false;
        }
        
    }
}