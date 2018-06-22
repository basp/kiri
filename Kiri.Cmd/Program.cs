namespace Kiri
{
    using System;
    using System.IO;
    using System.Threading;
    using Kiri;

    class Program
    {
        public static void Main(string[] args)
        {
            const string Nick = "Methbot";
            const string Url = "https://github.com/basp/methbot";

            var facts = File.ReadAllLines(@"D:\basp\kiri\Kiri.Cmd\facts.txt");

            var client = new Client()
                .WithRegistration(Nick, Url)
                .WithPong()
                .WithLogging(ctx => Console.WriteLine(ctx.Message))
                .Use(new RandomFactsMiddleware(facts))
                .Connect("chat.freenode.net", 6667);

            Thread.Sleep(30 * 1000);
            client.Join("##vanityguild");

            Thread.Sleep(5 * 1000);
            client.Say("##vanityguild", "Hello!");

            Thread.Sleep(5 * 1000);
            client.Emote("##vanityguild", "waves");

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