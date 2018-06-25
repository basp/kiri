namespace Kiri.Cmd
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using Kiri;
    using Newtonsoft.Json;
   
    class Program
    {
        public static void Main(string[] args)
        {
            const string Nick = "Methbot";
            const string Url = "https://github.com/basp/methbot";
            const string Host = "chat.freenode.net";
            const int Port = 6667;

            var facts = File.ReadAllLines(@"D:\basp\kiri\Kiri.Cmd\facts.txt");

            var session = new Session(Nick, Url);

            var client = Client
                .Create(session)
                .WithRegistration()
                .WithGreeting()
                .WithPong()
                .WithLogging(ctx => Console.WriteLine(ctx.Message))
                .Use(new MarkovMiddleware<Session>())
                .Connect(Host, Port);

            Thread.Sleep(30 * 1000);
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