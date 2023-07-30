using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;
using static System.Collections.Specialized.BitVector32;

public static class Network
{

    static HttpListener? httpListener;

    static Task receiverTask;
    static Task[] handlerTasks = new Task[Config.HANDLER_THREADS];

    static volatile List<HttpListenerContext> requests = new();

    public static void Init()
    {
        Utils.Log("Initializing Network...");

        httpListener = new HttpListener();

        foreach (string prefix in Config.PREFIXES)
        {
            try
            {
                Utils.Log("Adding prefix: " + prefix);
                httpListener.Prefixes.Add(prefix);
            } catch (Exception e)
            {
                Utils.Log(e);
            }
        }

        httpListener.Start();

        receiverTask = Task.Run(Start);

        for(int i = 0; i < handlerTasks.Length; i++)
        {
            int id = i;
            handlerTasks[i] = Task.Run(()=>HandlerThread(id));
        }

        Utils.Log("Network initialized");
    }

    static void Start()
    {
        Utils.Log("Started Network thread");

        if (httpListener != null)
        {
            while (true)
            {
                //GetContext() waits until we receive a connection
                requests.Add(httpListener.GetContext());
            }
        }

        Utils.Log("Stopping Network thread");
    }

    static void HandlerThread(int id)
    {
        while (true)
        {
            try
            {
                if (requests.Count > 0)
                {
                    bool reqAvailable = true;
                    HttpListenerContext ctx = null;

                    try
                    {
                        ctx = requests[0];
                    } catch { reqAvailable = false; }
                    //Utils.Log("Request received by handler " + id);

                    if (requests.Count > 0 && reqAvailable)
                    {
                        try {
                            requests.RemoveAt(0);
                        } catch { reqAvailable = false; }

                        if(reqAvailable && ctx != null) HandleRequest(ctx);
                    }
                }
            } catch (Exception e)
            {
                Utils.Log($"Error in handler {id}");
                Utils.Log(e);
            }

            Thread.Sleep(Config.HANDLER_SLEEP_INTERVAL);
        }
    }

    static void HandleRequest(HttpListenerContext ctx)
    {
        Stopwatch? stopwatch = Stopwatch.StartNew();

        HttpListenerRequest req = ctx.Request;
        HttpListenerResponse resp = ctx.Response;

        try
        {
            //Read body
            string body = new StreamReader(ctx.Request.InputStream).ReadToEnd();
            //Utils.Log(body);

            //Utils.Log("Received HTTP request. Method: " + req.HttpMethod + ", Body: " + body);

            try
            {
                resp.StatusCode = (int)HttpStatusCode.OK;
                resp.StatusDescription = "Status OK";
            }
            catch (Exception e)
            {
                //Utils.Log(e);
            }

            try
            {
                //From https://stackoverflow.com/a/28223114, prevents CORS errors on client
                if (req.HttpMethod.Equals("Options"))
                {
                    resp.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With");
                    resp.AddHeader("Access-Control-Allow-Methods", "GET, POST");
                    resp.AddHeader("Access-Control-Max-Age", "1728000");
                }
                resp.AppendHeader("Access-Control-Allow-Origin", "*");

                resp.Headers.Set("Content-Type", "text/plain");
            } catch (Exception e)
            {
                //Utils.Log(e);
            }

            //Parse body
            ClientAction action = JsonConvert.DeserializeObject<ClientAction>(body);

            ServerResponse response = new();

            if (action != null)
            {
                Session? session = action.Session;

                response.Add(new ActionList.SentAtTime(action.time));

                if (!action.action.Equals("heartbeat"))
                {
                    double millisNow = DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                    Utils.Log($"Received HTTP request. Action: {action.action}, State: " +
                        $"{(session != null ? session.menu.state : "N/A")}, Prev States: {session?.menu.prevStates.Count}, Sent {Utils.Round(millisNow - action.time, 1)}ms ago");
                }
                else stopwatch = null;

                if (defaultClientActions.ContainsKey(action.action))
                {
                    defaultClientActions[action.action](action, response);
                }

                if (session != null)
                {
                    if (session.currentAction != "" && session.currentAction != "heartbeat" && session.lastActionTime > DateTime.Now.AddMilliseconds(-2 * 1000)) //Always 
                        Utils.Log($"Session is processing an action, ignoring request. Current Action: {session.currentAction}");
                    else
                    {
                        Player? player = session.Player;

                        if(player != null)
                            player.playtime = player.playtime.Add(new(0, 0, 0, 0, milliseconds: (int)(DateTime.Now - session.lastActionTime).TotalMilliseconds));

                        session.lastActionTime = DateTime.Now;
                        session.currentAction = action.action;

                        if(action.action != "heartbeat" || Utils.tickCount % 4 == 0)
                            session.logChanged = true;

                        if (!defaultClientActions.ContainsKey(action.action))
                        {
                            if(action.action.StartsWith("combat."))
                                session.combatHandler.HandleInput(action.action.Substring(7));
                            else session.menu?.HandleInput(action, response); //? means if not null
                        }

                        response.Add(new ActionList.SetInput(session.menu?.GetInputs(response)));
                        if (session.logChanged) response.Add(new ActionList.SetLog(session.log));

                        List<string> sidebar = session.GetSidebar();
                        if (session.SidebarChanged)
                            response.Add(new ActionList.SetSidebar(sidebar));

                        if (session.Player != null)
                        {
                            ServerAction<object>[] combatSidebar = session.GetCombatSidebar();
                            foreach (ServerAction<object> a in combatSidebar)
                            {
                                response.Add(a);
                            }
                        }

                        session.logChanged = false;
                        session.currentAction = "";
                    }
                }
                else Utils.Log("Session is null!");
            }

            string respJson = JsonConvert.SerializeObject(response);

            //Write response
            byte[] responseBytes = Encoding.UTF8.GetBytes(respJson);
            resp.ContentLength64 = responseBytes.Length;
            resp.OutputStream.Write(responseBytes, 0, responseBytes.Length);
        }
        catch (Exception e)
        {
            //Utils.Log(e);
        }

        Thread.Sleep(100); //Wait for response to finish writing. I have no clue why we have to do this, but it gives errors w/o it

        //If, for some reason, we still haven't finished writing the response, try again 100ms later
        try
        {
            resp.Close();
        } catch (Exception e)
        {
            //Utils.Log(e);
            try
            {
                Thread.Sleep(100);
                resp.Close();
            } catch { }
        }

        if(stopwatch != null)
        {
            stopwatch.Stop();
            Utils.Log($"Finished handling request in {stopwatch.ElapsedMilliseconds}ms");
        }
    }

    static readonly Dictionary<string, Action<ClientAction, ServerResponse>> defaultClientActions = new()
    {
        { "init", (action, response) =>
            {
                Utils.Log($"Initting session... Token: {action.token}, Null: {action.token == null}");
                Session session;

                if(action.token == null | action.token == "" || !Session.sessions.ContainsKey(new ObjectId(action.token))) {
                    session = Session.CreateSession();
                    action.token = session.id.ToString();
                    response.Add(new ActionList.SetToken(session.id.ToString()));
                }
                else session = Session.sessions[new ObjectId(action.token)];

                if(!session.SignedIn && session.menu == null)
                {
                    session.SetMenu(new Menus.LogInMenu());

                    response.Add(new ActionList.ClearLog());
                    session.Log("Welcome to RMUD2!");
                }

                session.logChanged = true;
                session.resendSidebar = true;
            }
        },
        { "heartbeat", (action, response) =>
            {
                if(!Session.sessions.ContainsKey(new(action.token)))
                    defaultClientActions["init"](action, response);
            }
        }
    };

}