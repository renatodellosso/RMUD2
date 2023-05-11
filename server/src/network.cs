using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

public static class Network
{

    static HttpListener? httpListener;

    static Task receiverTask;
    static Task[] handlerTasks = new Task[Config.HANDLER_THREADS];

    static List<HttpListenerContext> requests = new();

    public static void Init()
    {
        Console.WriteLine("Initializing Network...");

        httpListener = new HttpListener();

        string prefix = "http://localhost:" + Config.PORT + "/";
        Console.WriteLine("Adding prefix: " + prefix);
        httpListener.Prefixes.Add(prefix);

        httpListener.Start();

        receiverTask = Task.Run(Start);

        for(int i = 0; i < handlerTasks.Length; i++)
        {
            int id = i;
            handlerTasks[i] = Task.Run(()=>HandlerThread(id));
        }

        Console.WriteLine("Network initialized");
    }

    static void Start()
    {
        Console.WriteLine("Started Network thread");

        if (httpListener != null)
        {
            while (true)
            {
                //GetContext() waits until we receive a connection
                requests.Add(httpListener.GetContext());
            }
        }

        Console.WriteLine("Stopping Network thread");
    }

    static void HandlerThread(int id)
    {
        while (true)
        {
            try
            {
                if (requests.Count > 0)
                {
                    HttpListenerContext ctx = requests[0];
                    Console.WriteLine("Request received by handler " + id);

                    if (requests.Count > 0)
                    {
                        requests.RemoveAt(0);
                        HandleRequest(ctx);
                    }
                }
            } catch (Exception e)
            {
                Console.WriteLine("Error in handler " + id + ": " + e.Message);
                Console.Error.WriteLine(e.StackTrace);
            }

            Thread.Sleep(Config.HANDLER_SLEEP_INTERVAL);
        }
    }

    static void HandleRequest(HttpListenerContext ctx)
    {
        HttpListenerRequest req = ctx.Request;
        HttpListenerResponse resp = ctx.Response;

        //Read body
        string body = new StreamReader(ctx.Request.InputStream).ReadToEnd();
        Console.WriteLine(body);

        Console.WriteLine("Received HTTP request. Method: " + req.HttpMethod + ", Body: " + body);

        resp.StatusCode = (int)HttpStatusCode.OK;
        resp.StatusDescription = "Status OK";

        //From https://stackoverflow.com/a/28223114, prevents CORS errors on client
        if (req.HttpMethod.Equals("Options"))
        {
            resp.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With");
            resp.AddHeader("Access-Control-Allow-Methods", "GET, POST");
            resp.AddHeader("Access-Control-Max-Age", "1728000");
        }
        resp.AppendHeader("Access-Control-Allow-Origin", "*");

        resp.Headers.Set("Content-Type", "text/plain");

        //Parse body
        ClientAction action = JsonConvert.DeserializeObject<ClientAction>(body);

        ServerResponse response = new();

        if(defaultClientActions.ContainsKey(action.action))
        {
            defaultClientActions[action.action](action, response);
        }

        Console.WriteLine(action.token);
        if (action.Session != null)
        {
            Session session = Session.sessions[new ObjectId(action.token)];
            session.menu?.HandleInput(action, response); //? means if not null
            response.Add(new ActionList.SetInput(session.menu?.GetInputs(response)));
        }
        else Console.WriteLine("Session is null!");

        string respJson = JsonConvert.SerializeObject(response);

        //Write response
        Console.WriteLine("Writing response: " + respJson);
        byte[] responseBytes = Encoding.UTF8.GetBytes(respJson);
        resp.ContentLength64 = responseBytes.Length;
        resp.OutputStream.Write(responseBytes, 0, responseBytes.Length);

        resp.Close();
    }

    static readonly Dictionary<string, Action<ClientAction, ServerResponse>> defaultClientActions = new()
    {
        { "init", (action, response) =>
            {
                Console.WriteLine("Initting session...");
                Session session;

                if(action.token == null || !Session.sessions.ContainsKey(new ObjectId(action.token))) {
                    session = Session.CreateSession();
                    action.token = session.id.ToString();
                    response.Add(new ActionList.SetToken(session.id.ToString()));
                }
                else session = Session.sessions[new ObjectId(action.token)];

                if(!session.SignedIn && session.menu == null)
                    session.SetMenu(new Menus.LogInMenu());
            }
        }
    };

}