namespace Kiri.Cmd
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Kiri;
    using Newtonsoft.Json;
    using Serilog;

    internal class Config
    {
        public Config(string nick, string url, string host, int port)
        {
            this.Nick = nick;
            this.Url = url;
            this.Host = host;
            this.Port = port;
        }

        public string Nick { get; }
        public string Url { get; }
        public string Host { get; }
        public int Port { get; }
    }

    class Program
    {
        // const string Facts = @"D:\basp\kiri\Kiri.Cmd\facts.txt";
        const string Nick = "Methbot";
        const string Channel = "##vanityguild";
        static readonly string[] Aliases = new[] { Nick, "Meth" };

        private static async Task Run()
        {
            var cfg = new Config(
                nick: Nick,
                url: "https://github.com/basp/methbot",
                host: "chat.freenode.net",
                port: 6667);

            var session = new Session(cfg.Nick, cfg.Url, Aliases);
            var builder = ClientBuilder.Create(session);
            var client = builder
                .Use(new GreetingMiddleware<Session>())
                .UseIdentity()
                .UsePong()
                .UseLogging()
                .Build()
                .Connect(cfg.Host, cfg.Port);

            Thread.Sleep(20 * 1000);
            await client.JoinAsync(Channel);

            while (true)
            {
                var s = Console.ReadLine();
                await client.SendAsync(s);
            }
        }

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.LiterateConsole()
                .CreateLogger();

            Run().Wait();
        }
    }
}