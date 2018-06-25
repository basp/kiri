namespace Kiri
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using Kiri;

    class Program
    {
        class Session : IRegistrationProvider
        {
            private readonly string nick;
            private readonly string url;
            private readonly IDictionary<string, ISet<string>> channels;

            public Session(string nick, string url)
            {
                this.nick = nick;
                this.url = url;
                this.channels = new Dictionary<string, ISet<string>>();
            }

            public string Nick => this.nick;

            public string Info => this.url;

            public IDictionary<string, ISet<string>> Channels => this.channels;
        }

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
                .WithPong()
                .WithLogging(ctx => Console.WriteLine(ctx.Message))
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