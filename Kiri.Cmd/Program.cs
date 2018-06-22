namespace Kiri
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Sockets;

    class Program
    {
        public static void Main(string[] args)
        {
            var client = new Client();
            using(var subscription = client.Subscribe(new OutputObserver()))
            {
                client.Connect("chat.freenode.net", 6667);
            }
        }
    }

    class OutputObserver : IObserver<string>
    {
        public void OnCompleted()
        {
            Console.WriteLine("EOF");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error.ToString());
        }

        public void OnNext(string value)
        {
            Console.WriteLine($"< {value}");
        }
    }

    public class Client : IObservable<string>
    {
        private IList<IObserver<string>> observers = new List<IObserver<string>>();

        private TcpClient tcpClient;

        private NetworkStream stream;

        public void Connect(string hostname, int port)
        {
            this.tcpClient = new TcpClient();
            this.tcpClient.Connect(hostname, port);
            this.stream = this.tcpClient.GetStream();
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            if (!this.observers.Contains(observer))
            {
                this.observers.Add(observer);
            }

            return new Unsubscriber(this.observers, observer);
        }

        private void OnNext(string value)
        {
            foreach (var observer in this.observers)
            {
                observer.OnNext(value);
            }
        }

        private void OnError(Exception error)
        {
            foreach (var observer in this.observers)
            {
                observer.OnError(error);
            }
        }

        private void OnCompleted()
        {
            foreach (var observer in this.observers)
            {
                observer.OnCompleted();
            }
        }

        public void Send(string data)
        {
            if (this.stream == null)
            {
                return;
            }

            using (var writer = new StreamWriter(this.stream))
            {
                writer.WriteLine(data);
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
            private IList<IObserver<string>> observers;
            private IObserver<string> observer;

            public Unsubscriber(IList<IObserver<string>> observers, IObserver<string> observer)
            {
                this.observers = observers;
                this.observer = observer;
            }

            public void Dispose()
            {
                if (this.observer == null)
                {
                    return;
                }

                this.observers.Remove(this.observer);
            }
        }
    }
}