using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public static class Network
{

    static HttpListener? httpListener;

    static Task task;

    public static void Init()
    {
        Console.WriteLine("Initializing Network...");

        httpListener = new HttpListener();

        string prefix = "http://localhost:" + Config.port + "/";
        Console.WriteLine("Adding prefix: " + prefix);
        httpListener.Prefixes.Add(prefix);

        httpListener.Start();

        task = Task.Run(Start);

        Console.WriteLine("Network initialized");
    }

    static void Start()
    {
        Console.WriteLine("Started Network thread");

        if (httpListener != null)
        {
            while (true)
            {
                HttpListenerContext ctx = httpListener.GetContext();

                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                Console.WriteLine("Received HTTP request. Method: " + req.HttpMethod + ", Body: ");

                resp.StatusCode = (int)HttpStatusCode.OK;
                resp.StatusDescription = "Status OK";

                //From https://stackoverflow.com/a/28223114
                if (req.HttpMethod.Equals("Options"))
                {
                    resp.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With");
                    resp.AddHeader("Access-Control-Allow-Methods", "GET, POST");
                    resp.AddHeader("Access-Control-Max-Age", "1728000");
                }
                resp.AppendHeader("Access-Control-Allow-Origin", "*");

                resp.Close();
            }
        }

        Console.WriteLine("Stopping Network thread");
    }

}