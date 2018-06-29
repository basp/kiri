namespace Kiri.Cmd
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using Kiri;
    using Newtonsoft.Json;
    using Serilog;

    class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.LiterateConsole()
                .CreateLogger();

            const string Facts = @"D:\basp\kiri\Kiri.Cmd\facts.txt";
            const string Nick = "Methbot";
            const string Url = "https://github.com/basp/methbot";
            const string Host = "chat.freenode.net";
            const int Port = 6667;

            var session = new Session(Nick, Url, Nick, "Meth");
            
            var markov = new MarkovMiddleware<Session>();
            markov.Seed(session, @"D:\tmp\chat-sanitized.log");

            var builder = ClientBuilder.Create(session);
            var client = builder
                // .Use(new IdentityMiddleware<Session>())
                // .Use(new GreetingMiddleware<Session>())
                // .Use(new PongMiddleware<Session>())
                // .Use(new LoggingMiddleware<Session>())
                // .Use(new CatMiddleware<Session>())
                // .Use(FactMiddleware.Create<Session>(Facts))
                // .Use(markov)
                // .Build()
                .Connect(Host, Port);

            Thread.Sleep(20 * 1000);
            client.Join("##vanityguild");

            while (true)
            {
                var command = Console.ReadLine();
                if (command.StartsWith(":exit"))
                {
                    break;
                }

                client.Send(command);
            }

            client.Part("##vanityguild");
            Thread.Sleep(1 * 1000);
        }
    }
}