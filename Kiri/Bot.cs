namespace Kiri
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    

    public class Bot
    {
        private readonly string nick;
        private readonly string url;
        private readonly TcpClient client;
        private readonly StreamWriter writer;
        private readonly StreamReader reader;
        private Bot(string nick, string url, TcpClient client)
        {
            this.nick = nick;
            this.url = url;
            this.client = client;

            var stream = client.GetStream();
            this.writer = new StreamWriter(stream);
            this.reader = new StreamReader(stream);
        }

        public static Bot Connect(string hostname, int port, string nick, string url)
        {
            var client = new TcpClient(hostname, port);
            return new Bot(nick, url, client);
        }

        public IObservable<string> StartReading()
        {
            while (true)
            {
                var line = reader.ReadLine();
                Console.WriteLine($"> {line}");
            }
        }

        public Bot Identify()
        {
            Console.WriteLine("Identifying");
            this.Send($"NICK {this.nick}");
            this.Send($"USER {this.nick} 8 * :{this.url}");
            return this;
        }

        public Bot Join(string channel)
        {
            this.Send($"JOIN {channel}");
            return this;
        }

        public Bot Say(string to, string msg)
        {
            this.Send($"PRIVMSG {to} :{msg}");
            return this;
        }

        public Bot Send(string data)
        {
            Console.WriteLine($"Sending: {data}");
            this.writer.WriteLine(data);
            return this;
        }
    }
}
