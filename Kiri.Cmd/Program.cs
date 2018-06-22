namespace Kiri
{
    using System;
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

            Console.ReadKey();
        }
    }
}