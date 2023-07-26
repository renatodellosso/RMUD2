using Discord;
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

        public string username = "";
        string password = "";

        int tries = 0;
        Timer lockOut;
        TimeSpan lockOutDuration = Config.LOCK_OUT_DURATION;

        public string passwordResetCode = "";
        public bool passwordResetEnabled = false;

        public override string Status => mode == Mode.CreateAccount ? "Creating an account" : "Logging in";

        public override Input[] GetInputs(ServerResponse response)
        {
            List<Input> inputs = new();

            if (lockOut != null)
            {
                if (lockOut.IsComplete)
                {
                    tries--;
                    if (tries > 0)
                    {
                        lockOut = null;
                        session.ReplaceLog("Enter your password:");
                    }
                    else lockOut = new Timer(Config.LOCK_OUT_DURATION);
                }
                else if (tries > Config.MAX_SIGN_IN_TRIES)
                    session.ReplaceLog($"{Utils.Style("You have tried to sign in too many times.", "red")} Sign In Cooldown: {lockOut.FormattedTimeRemaining()}");
            }

            if (!waiting && tries <= Config.MAX_SIGN_IN_TRIES)
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
                    {
                        if (mode == Mode.SignIn && !passwordResetEnabled) inputs.Add(new(InputMode.Option, "reset", "Forgot Password"));
                        inputs.Add(new(InputMode.Secret, "password", mode == Mode.CreateAccount ? "Enter a password" : (passwordResetEnabled ? "Enter a new password" 
                            : "Enter your password")));
                    }
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
            if (!waiting && tries <= Config.MAX_SIGN_IN_TRIES)
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
                        passwordResetEnabled = false;
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
                        if (state == State.Username && action.action != "signIn")
                        {
                            if (mode == Mode.SignIn || !Account.UsernameTaken(action.action))
                            {
                                if (mode == Mode.SignIn || Utils.IsInputSafe(action.action))
                                {
                                    username = action.action;
                                    state = State.Password;
                                    session?.ReplaceLog("Username: " + username);
                                    session?.Log(mode == Mode.CreateAccount ? "Enter a password:" : "Enter your password:");
                                }
                                else
                                {
                                    session?.ReplaceLog(Utils.Style($"{action.action} is not a valid username.", "red") + " Please enter a new username:");
                                }
                            }
                            else
                            {
                                session?.ReplaceLog($"The username {action.action} is taken. Enter a new username:");
                            }
                        }
                        else if (state == State.Password)
                        {
                            if (action.action != "reset")
                            {
                                password = action.action;
                                if (!session.log.Last().Contains("Password") && session.log.Where(msg => msg.Contains("Password")).Count() > 0) session.PopLog();
                                session.ReplaceLog("Password: *****");
                                if (mode == Mode.CreateAccount)
                                {
                                    state = State.ConfirmPassword;
                                    session.Log("Reenter your password:");
                                }
                                else
                                {
                                    waiting = true;
                                    Task.Run(SignIn);
                                    session.Log(passwordResetEnabled ? "Updating password..." : "Signing in...");
                                }
                            }
                            else if (mode == Mode.SignIn)
                            {
                                //Password reset
                                StartPasswordReset();
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
                session.accountId = id;
                session.SetMenu(new MainMenu(session));
            }
            else session.ReplaceLog(Utils.Style("Failed to create account", "red"));
            waiting = false;
        }

        void SignIn()
        {
            if (!passwordResetEnabled)
            {
                ObjectId? result = Account.VerifyCredentials(username, password);
                if (result != null)
                {
                    session.ReplaceLog(Utils.Style("Logged in!", "green"));
                    session.accountId = result;
                    session.SetMenu(new MainMenu(session));
                }
                else
                {
                    tries++;
                    if (tries > Config.MAX_SIGN_IN_TRIES)
                    {
                        lockOut = new Timer(lockOutDuration);
                        lockOutDuration *= 2;
                        Utils.Log($"User exceeded sign in limit. Cooldown: {lockOut.FormattedTimeRemaining()}");
                    }

                    session.ReplaceLog(Utils.Style("Invalid username or password", "red"));
                    Utils.Log("User failed sign in. Try: " + tries + "/" + Config.MAX_SIGN_IN_TRIES);
                }
            }
            else
            {
                Account? account = DB.Accounts.FindByUsername(username);

                if(account == null)
                    session?.Log(Utils.Style($"No account found with the username: {username}", "red"));
                else
                {
                    account.password = Utils.HashPassword(password, account.salt);
                    account.Update();

                    passwordResetCode = "";
                    passwordResetEnabled = false;

                    session?.Log("Password reset!");
                }
            }

            waiting = false;
        }
        
        void StartPasswordReset()
        {
            Account? account = DB.Accounts.FindByUsername(username);

            if (account != null)
            {
                if(account.discordId == 0) //Ulongs default to 0
                {
                    session?.Log(Utils.Style("Account not linked to a Discord account. Password reset unavailable", "red"));
                }
                else
                {
                    passwordResetCode = Utils.RandomCode();
                    session?.Log($"Run the /reset command in the Discord server to reset your password and enter {passwordResetCode} as the code.");
                }
            }
            else session?.Log(Utils.Style($"No account found with the username: {username}", "red"));
        }

    }
}