namespace Kiri.Cmd
{
    // https://github.com/migueldeicaza/gui.cs

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading;

    class Terminal
    {
        int rows;

        int cols;

        int crow;

        int ccol;

        int[,,] contents;

        bool[] dirtyLine;

        public int Rows => this.rows;

        public int Cols => this.cols;

        public Terminal()
        {
            this.rows = Console.WindowHeight - 1;
            this.cols = Console.WindowWidth;
            UpdateOffscreen();
        }

        public void Init()
        {
            Console.Clear();
        }

        public void Move(int col, int row)
        {
            this.ccol = col;
            this.crow = row;

            Console.CursorTop = row;
            Console.CursorLeft = col;
        }

        public void Add(char c)
        {
            this.contents[this.crow, this.ccol, 0] = c;
            this.contents[this.crow, this.ccol, 1] = 0;
            this.contents[this.crow, this.ccol, 2] = 1;

            this.dirtyLine[this.crow] = true;

            this.ccol++;
            if (this.ccol == this.Cols)
            {
                this.ccol = 0;
                if (this.crow + 1 < this.Rows)
                {
                    this.crow++;
                }
            }
        }

        public void Add(string s)
        {
            foreach (var c in s)
            {
                Add(c);
            }
        }

        public void Refresh()
        {
            var rows = this.Rows;
            var cols = this.Cols;

            var savedRow = Console.CursorTop;
            var savedCol = Console.CursorLeft;

            Console.CursorVisible = false;

            for (var r = 0; r < rows; r++)
            {
                if (!this.dirtyLine[r])
                {
                    continue;
                }

                this.dirtyLine[r] = false;

                for (var c = 0; c < cols; c++)
                {
                    if (this.contents[r, c, 2] != 1)
                    {
                        continue;
                    }

                    Console.CursorTop = r;
                    Console.CursorLeft = c;

                    for (; c < cols && contents[r, c, 2] == 1; c++)
                    {
                        Console.Write((char)this.contents[r, c, 0]);
                        this.contents[r, c, 2] = 0;
                    }
                }
            }

            Console.CursorTop = savedRow;
            Console.CursorLeft = savedCol;

            Console.CursorVisible = true;
        }

        public void End()
        {
            Console.ResetColor();
            Console.Clear();
        }

        private void UpdateOffscreen()
        {
            var rows = this.Rows;
            var cols = this.Cols;

            this.contents = new int[rows, cols, 3];
            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < cols; c++)
                {
                    this.contents[r, c, 0] = ' ';
                    this.contents[r, c, 1] = 0;
                    this.contents[r, c, 2] = 0;
                }
            }

            this.dirtyLine = new bool[rows];
            for (var r = 0; r < rows; r++)
            {
                this.dirtyLine[r] = true;
            }
        }

        private void UpdateScreen()
        {
            var rows = this.Rows;
            var cols = this.Cols;

            Console.CursorTop = 0;
            Console.CursorLeft = 0;

            for (var r = 0; r < rows; r++)
            {
                this.dirtyLine[r] = false;
                for (var c = 0; c < cols; c++)
                {
                    this.contents[r, c, 2] = 0;
                    Console.Write((char)this.contents[r, c, 0]);
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var term = new Terminal();
            term.Init();
            term.Add("Foo");
            term.Move(0, term.Rows - 1);
            term.Add("# ");
            term.Move(2, term.Rows - 1);
            
            while(true)
            {
                term.Refresh();
                var key = Console.ReadKey(true);
                term.Move(0, 0);
                term.Add(key.Key.ToString().PadRight(term.Cols));
                term.Move(2, term.Rows - 1);
            }
        }
    }

    /*
    class Program
    {
        const string nick = "Methbot";
        
        const string url = "https://github.com/basp//methbot";

        private static readonly LineBuffer buffer = new LineBuffer(100);

        private static readonly object bufferLock = new object();

        private static readonly object writerLock = new object();

        static void Update()
        {
            var lines = buffer.Last(10).ToArray();
            var offset = Console.WindowHeight - 2;

            Console.Clear();
            for (var i = 0; i < 10; i++)
            {
                if (i >= lines.Length) break;
                Console.SetCursorPosition(0, offset - i);
                Console.WriteLine(lines[i]);
            }

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("# ");
        }

        static void Add(string line)
        {
            lock (bufferLock)
            {
                buffer.Add(line);
                Update();
            }
        }

        static void Pong(StreamWriter writer, string ident)
        {
            Send(writer, $"PONG :{ident}");
        }

        static string ParsePing(string ping)
        {
            var chunks = ping.Split(':').Select(x => x.Trim());
            return chunks.Skip(1).First();
        }

        static void StartReading(StreamWriter writer, StreamReader reader)
        {
            while (true)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                Add($"< {line}");

                if (line.StartsWith("PING"))
                {
                    var ident = ParsePing(line);
                    Pong(writer, ident);
                }
            }
        }

        static void Send(StreamWriter writer, string data)
        {
            Add($"> {data}");
            lock (writerLock)
            {
                writer.WriteLine(data);
                writer.Flush();
            }
        }

        static void Main(string[] args)
        {
            const string hostname = "irc.freenode.net";
            const int port = 6667;

            using (var client = new TcpClient(hostname, port))
            using (var stream = client.GetStream())
            using (var writer = new StreamWriter(stream))
            using (var reader = new StreamReader(stream))
            {
                var t = new Thread(() => StartReading(writer, reader));
                t.Start();

                Send(writer, $"NICK {nick}");
                Send(writer, $"USER {nick} 8 * :{url}");

                while (true)
                {
                    var cmd = Console.ReadLine();
                    Send(writer, cmd);
                }
            }
        }
    }
    */
}
