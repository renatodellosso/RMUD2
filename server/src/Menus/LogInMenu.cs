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

        string username = "", password = "";

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
                if (state == State.Username)
                    inputs.Add(new(InputMode.Text, "username", mode == Mode.CreateAccount ? "Enter a username" : "Enter your username:"));
                else if (state == State.Password)
                    inputs.Add(new(InputMode.Secret, "password", mode == Mode.CreateAccount ? "Enter a password" : "Enter your password:"));
                else if (state == State.ConfirmPassword)
                    inputs.Add(new(InputMode.Secret, "password", "Reenter your password:"));
                else if (state == State.FinalConfirmation)
                    inputs.Add(new(InputMode.Option, "confirm", "Confirm & Create Account"));
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
                        if (mode == Mode.SignIn || !UsernameTaken(action.action))
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
                            ObjectId? result = VerifyCredentials();
                            if(result != null)
                            {
                                session.Log(Utils.Style("Logged in!", "green"));
                                session.accountId = result;
                            }
                            else
                            {
                                session.Log(Utils.Style("Invalid username or password", "red"));
                            }
                        }
                    }
                    else if(state == State.ConfirmPassword)
                    {
                        if(action.action.Equals(password))
                        {
                            state = State.FinalConfirmation;
                            session.ReplaceLog("Password confirmed");
                        }
                        else session.ReplaceLog(Utils.Style("Passwords do not match. Reenter your password:", "red"));
                    }
                    else if(state == State.FinalConfirmation && action.action.Equals("confirm"))
                    {
                        if(CreateAccount())
                        {
                            session.Log(Utils.Style("Account created!", "green"));
                        }
                    }
                }
            }
        }

        bool CreateAccount()
        {
            Account account = new Account(username, password);
            DB.accounts.InsertOne(account);
            return true;
        }


        /// <summary>
        /// Checks if the given username is taken. Not case-sensitive
        /// </summary>
        /// <returns>Whether an account exists with this same username</returns>
        static bool UsernameTaken(string username)
        {
            username = username.ToLower();

            //The next 4 lines are the old search. I tried to make it case-insensitive, but it gave an error
            //We have to create a filter before performing the search
            //FilterDefinition<Account> filter = Builders<Account>.Filter.Eq(a => a.username != null ? a.username.ToLower() : "", username);
            //IAsyncCursor<Account> found = DB.accounts.FindSync(filter);
            //return found.Any();

            //Case-insensitive search
            return DB.accounts.AsQueryable().Where(a => a.username.ToLower().Contains(username)).Any();
        }

        /// <summary>
        /// Checks if the username and password are correct
        /// </summary>
        /// <returns>The _id of the account that matches the provided credentials</returns>
        ObjectId? VerifyCredentials()
        {
            Console.WriteLine($"Attempting sign in... Username: {username}");

            //We have to create a filter before performing the search
            FilterDefinition<Account> filter = Builders<Account>.Filter.Eq("username", username);
            IAsyncCursor<Account> found = DB.accounts.FindSync(filter);

            if (!found.Any()) return null;

            found = DB.accounts.FindSync(filter); //Using .Any() consumes the cursor, so we need a new one

            Account account = found.First();
            bool success = account.password.Equals(Utils.PBKDF2Hash(password, account.salt));

            if (success) Console.WriteLine($"{username} signed in");
            else Console.WriteLine($"Someone failed to sign in to {username}");

            found.Dispose();

            return success ? account._id : null;
        }
        
    }
}