namespace Kiri
{
    using System;
    using System.Threading;
    using Kiri;

    class Program
    {
        public static void Main(string[] args)
        {
            const string Nick = "Methbot";
            const string Url = "https://github.com/basp/methbot";

            var client = new Client()
                .WithRegistration(Nick, Url)
                .WithPong()
                .WithLogging(ctx => Console.WriteLine(ctx.Message))
                .Connect("chat.freenode.net", 6667);

            Thread.Sleep(30 * 1000);
            Console.WriteLine("Joining...");
            client.Join("##vanityguild");

            Thread.Sleep(5 * 1000);
            Console.WriteLine("Greeting...");
            client.Say("##vanityguild", "Hello!");

            Thread.Sleep(5 * 1000);
            Console.WriteLine("Waving...");
            client.Emote("##vanityguild", "waves");

            Thread.Sleep(1 * 1000);
            Console.WriteLine("Parting...");
            client.Part("##vanityguild");

            while (true)
            {
                var command = Console.ReadLine();
                client.Send(command);
            }
        }
    }
}