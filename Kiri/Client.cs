namespace Kiri
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    public static class Client
    {
        public static Client<T> Create<T>(T session) where T : class => new Client<T>(session);
    }

    public class Client<T> : IObservable<string>, ISender where T : class
    {
        private readonly IList<IObserver<string>> observers =
            new List<IObserver<string>>();

        private readonly IList<IMiddleware<T>> pipeline = new List<IMiddleware<T>>();

        private readonly object syncRoot = new object();

        private readonly ISet<string> channels = new HashSet<string>();

        private readonly T session;

        private TcpClient tcpClient;

        private NetworkStream stream;

        private StreamWriter writer;

        private string currentChannel;

        public Client(T session)
        {
            this.session = session;
        }

        public Client<T> Use(IMiddleware<T> middleware)
        {
            this.pipeline.Add(middleware);
            return this;
        }

        public Client<T> Use(Action<IContext<T>, Action> middleware)
        {
            this.pipeline.Add(new MiddlewareAdapter<T>(middleware));
            return this;
        }

        public Client<T> Connect(string hostname, int port)
        {
            this.tcpClient = new TcpClient();
            this.tcpClient.Connect(hostname, port);
            this.stream = this.tcpClient.GetStream();
            this.writer = new StreamWriter(this.stream);

            var thread = new Thread(this.StartReading);
            thread.Start();
            return this;
        }

        public void Send(string data)
        {
            if (this.stream == null)
            {
                return;
            }

            this.writer.WriteLine(data);
            this.writer.Flush();
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


        public void Part(string channel)
        {
            this.currentChannel = null;

            if (!this.channels.Contains(channel))
            {
                return;
            }

            this.Send($"PART {channel}");
            this.channels.Remove(channel);
        }

        public void Say(string message) =>
            this.Say(this.currentChannel, message);

        public void Say(string to, string message) =>
            this.Send($"PRIVMSG {to} :{message}");

        public void Emote(string action) =>
            this.Emote(this.currentChannel, action);

        public void Emote(string to, string action) =>
            this.Send($"PRIVMSG {to} :\u0001ACTION {action}\u0001");

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

                        this.ExecutePipeline(new ContextAdapter(this.currentChannel, line, this));
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

        private void ExecutePipeline(IContext<T> context)
        {
            bool cont;
            Action next = () => cont = true;
            for (var i = 0; i < this.pipeline.Count; i++)
            {
                cont = false;
                this.pipeline[i].Execute(context, next);
                if (!cont)
                {
                    break;
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

            private readonly ISender sender;

            public ContextAdapter(string from, string message, Client<T> client)
            {
                this.from = from;
                this.message = message;
                this.sender = client;
                this.client = client;
            }

            public T Session => client.session;

            public string From => this.from;

            public string Message => this.message;

            public Client<T> Client => this.client;

            [Obsolete]
            public void Send(string data)
            {
                this.sender.Send(data);
            }
        }
    }
}