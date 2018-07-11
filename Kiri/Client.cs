namespace Kiri
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    public delegate Task RequestDelegate<T>(IContext<T> context) where T : class;

    public class Client<T> : IObservable<string> where T : class
    {
        private readonly IList<IObserver<string>> observers =
            new List<IObserver<string>>();

        private readonly object syncRoot = new object();

        private readonly ISet<string> channels = new HashSet<string>();

        private readonly RequestDelegate<T> requestDelegate;

        private readonly T session;

        private TcpClient tcpClient;

        private NetworkStream stream;

        private StreamWriter writer;

        private string currentChannel;

        public Client(T session, RequestDelegate<T> requestDelegate)
        {
            this.requestDelegate = requestDelegate;
            this.session = session;
        }

        public Client<T> Connect(string hostname, int port)
        {
            this.tcpClient = new TcpClient();
            this.tcpClient.Connect(hostname, port);
            this.stream = this.tcpClient.GetStream();
            this.writer = new StreamWriter(this.stream)
            {
                AutoFlush = true,
            };

            var thread = new Thread(this.StartReading);
            thread.Start();
            return this;
        }

        public void Send(string data)
        {
            if (this.stream == null)
            {
                throw new InvalidOperationException();
            }

            this.writer.WriteLine(data);
        }

        public Task SendAsync(string data)
        {
            if (this.stream == null)
            {
                throw new InvalidOperationException();
            }

            return this.writer.WriteLineAsync(data);
        }

        public void Join(string channel)
        {
            this.currentChannel = channel;

            if (this.channels.Contains(channel))
            {
                return;
            }

            this.Send($"JOIN {channel}");
            this.channels.Add(channel);
        }

        public async Task JoinAsync(string channel)
        {
            this.currentChannel = channel;
            if (this.channels.Contains(channel))
            {
                return;
            }

            await this
                .SendAsync($"JOIN {channel}")
                .ContinueWith(x =>
                {
                    if (x.Status == TaskStatus.RanToCompletion)
                    {
                        this.channels.Add(channel);
                    }
                });
        }

        public void Part(string channel)
        {
            this.currentChannel = null;
            if (!this.channels.Contains(channel))
            {
                return;
            }

            this.Send($"PART {channel}");
        }


        public async Task PartAsync(string channel)
        {
            this.currentChannel = null;
            if (!this.channels.Contains(channel))
            {
                return;
            }

            await this
                .SendAsync($"PART {channel}")
                .ContinueWith(x =>
                {
                    if (x.Status == TaskStatus.RanToCompletion)
                    {
                        this.channels.Remove(channel);
                    }
                });
        }

        public void Say(string message) =>
            this.Say(this.currentChannel, message);

        public async Task SayAsync(string message) =>
            await this.SayAsync(this.currentChannel, message);


        public void Say(string to, string message) =>
            this.Send($"PRIVMSG {to} :{message}");

        public async Task SayAsync(string to, string message) =>
            await this.SendAsync($"PRIVMSG {to} :{message}");

        public void Emote(string action) =>
            this.Emote(this.currentChannel, action);

        public async Task EmoteAsync(string action) =>
            await this.EmoteAsync(this.currentChannel, action);

        public void Emote(string to, string action) =>
            this.Send($"PRIVMSG {to} :\u0001ACTION {action}\u0001");

        public async Task EmoteAsync(string to, string action) =>
            await this.SendAsync($"PRIVMSG {to} :\u0001ACTION {action}\u0001");

        public IDisposable Subscribe(IObserver<string> observer)
        {
            lock (syncRoot)
            {
                if (!this.observers.Contains(observer))
                {
                    this.observers.Add(observer);
                }
            }

            return new Unsubscriber(this, observer);
        }

        private void OnNext(string value)
        {
            lock (syncRoot)
            {
                Parallel.ForEach(this.observers, x => x.OnNext(value));
            }
        }

        private void OnError(Exception error)
        {
            lock (syncRoot)
            {
                Parallel.ForEach(this.observers, x => x.OnError(error));
            }
        }

        private void OnCompleted()
        {
            lock (syncRoot)
            {
                Parallel.ForEach(this.observers, x => x.OnCompleted());
            }
        }

        private void StartReading()
        {
            using (var reader = new StreamReader(this.stream))
            {
                while (true)
                {
                    try
                    {
                        var line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            this.OnCompleted();
                            return;
                        }

                        var context = new ContextAdapter(this.currentChannel, line, this);
                        this.requestDelegate(context);
                        this.OnNext(line);
                    }
                    catch (Exception ex)
                    {
                        this.OnError(ex);
                        return;
                    }
                }
            }
        }

        private class Unsubscriber : IDisposable
        {
            private readonly Client<T> client;

            private IObserver<string> observer;

            public Unsubscriber(Client<T> client, IObserver<string> observer)
            {
                this.client = client;
                this.observer = observer;
            }

            public void Dispose()
            {
                if (this.observer == null)
                {
                    return;
                }

                lock (this.client.syncRoot)
                {
                    this.client.observers.Remove(this.observer);
                }
            }
        }

        private class ContextAdapter : IContext<T>
        {
            private readonly string from;

            private readonly string message;

            private readonly Client<T> client;

            public ContextAdapter(string from, string message, Client<T> client)
            {
                this.from = from;
                this.message = message;
                this.client = client;
            }

            public T Session => client.session;

            public string From => this.from;

            public string Message => this.message;

            public Client<T> Client => this.client;
        }
    }
}