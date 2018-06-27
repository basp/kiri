namespace Kiri
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IClientBuilder<T> where T : class
    {
        IClientBuilder<T> Use(IMiddleware<T> middleware);

        IClientBuilder<T> Use(Action<IContext<T>, Action> middleware);

        IClientBuilder<T> Run(Action<IContext<T>> terminal);

        Client<T> Build();
    }

    public static class ClientBuilder
    {
        public static IClientBuilder<T> Create<T>(T session) where T : class => new ClientBuilder<T>(session);
    }

    public class ClientBuilder<T> : IClientBuilder<T> where T : class
    {
        private static readonly Action<IContext<T>> DefaultTerminal = ctx => { };

        private readonly IList<IMiddleware<T>> middleware = new List<IMiddleware<T>>();

        private readonly Stack<Action<IContext<T>>> terminals = new Stack<Action<IContext<T>>>();

        private readonly T session;

        public ClientBuilder(T session)
        {
            this.session = session;
            this.terminals.Push(DefaultTerminal);
        }

        public IClientBuilder<T> Use(IMiddleware<T> middleware)
        {
            this.middleware.Add(middleware);
            return this;
        }

        public IClientBuilder<T> Use(Action<IContext<T>, Action> middleware)
        {
            this.middleware.Add(new MiddlewareAdapter<T>(middleware));
            return this;
        }

        public IClientBuilder<T> Run(Action<IContext<T>> terminal)
        {
            this.terminals.Push(terminal);
            return this;
        }

        public Client<T> Build() => new Client<T>(session, BuildInternal());

        private static Action<IContext<T>> Continue(IMiddleware<T> middleware, Action<IContext<T>> request) =>
            context => middleware.Execute(context, () => request(context));

        private Action<IContext<T>> BuildInternal()
        {
            var @delegate = this.terminals.Peek();
            foreach (var m in this.middleware.Reverse())
            {
                @delegate = Continue(m, @delegate);
            }

            return @delegate;
        }
    }
}