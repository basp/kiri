namespace Kiri.Cmd
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading;

  
    class Program
    {
        private static readonly LineBuffer buffer = new LineBuffer(5);

        static void Update()
        {
            var lines = buffer.Last(10).ToList();
            var offset = Console.WindowHeight - 2;

            Console.Clear();
            for(var i = 0; i < 10; i++)
            {
                if(i >= lines.Count) break;
                Console.SetCursorPosition(0, offset - i);
                Console.WriteLine(lines[i]);
            }

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("# ");
        }

        static void StartReading(TcpClient client)
        {
            var stream = client.GetStream();
            var reader = new StreamReader(stream);
            while (true)
            {
                var line = reader.ReadLine();
                if(string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                buffer.Add($"< {line}");
                Update();
            }
        }

        static void Main(string[] args)
        {
            const string hostname = "irc.freenode.net";
            const int port = 6667;
            // const string nick = "Methbot";
            // const string url = "https://github.com/basp//methbot";

            using (var client = new TcpClient(hostname, port))
            {
                var t = new Thread(() => StartReading(client));
                t.Start();

                while (true)
                {
                    var cmd = Console.ReadLine();
                    buffer.Add($"> {cmd}");
                    Update();
                }
            }
        }
    }
}
