namespace Kiri.Cmd
{
    using System;
    using System.IO;
    using System.Net.Sockets;

    class Program
    {
        // connecting
        // identifying
        // welcome (registering)
        // ready
        
        static void Main(string[] args)
        {
            const string hostname = "irc.freenode.net";
            const int port = 6667;
            const string nick = "Methbot";
            const string url = "https://github.com/basp//methbot";

            // using (var client = new TcpClient(hostname, port))
            // using (var stream = client.GetStream())
            // using (var reader = new StreamReader(stream))
            // {
            //     while(true)
            //     {
            //         var line = reader.ReadLine();
            //         Console.WriteLine(line);
            //     }
            // }

            var bot = Bot.Connect(hostname, port, nick, url);
            
            // We'll run the bot and output of chat on another thread
            // And have the main thread poll for user input
       }
    }
}
