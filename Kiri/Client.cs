namespace Kiri
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    public class Client : IObservable<string>, ISender
    {
        private readonly IList<IObserver<string>> observers =
            new List<IObserver<string>>();

        private readonly IList<IMiddleware> pipeline = new List<IMiddleware>();

        private readonly object syncRoot = new object();

        private TcpClient tcpClient;

        private NetworkStream stream;

        private StreamWriter writer;

        public Client Use(IMiddleware middleware)
        {
            this.pipeline.Add(middleware);
            return this;
        }

        public Client Use(Action<IContext, Action> middleware)
        {
            this.pipeline.Add(new MiddlewareAdapter(middleware));
            return this;
        }

        public Client Connect(string hostname, int port)
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

                        this.ExecutePipeline(new ContextAdapter(line, this));
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

        private void ExecutePipeline(IContext context)
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
            private readonly Client client;

            private IObserver<string> observer;

            public Unsubscriber(Client client, IObserver<string> observer)
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

        private class ContextAdapter : IContext
        {
            private readonly string message;

            private readonly ISender sender;

            public ContextAdapter(string message, ISender sender)
            {
                this.message = message;
                this.sender = sender;
            }

            public string Message => this.message;

            public void Send(string data)
            {
                this.sender.Send(data);
            }
        }
    }
}