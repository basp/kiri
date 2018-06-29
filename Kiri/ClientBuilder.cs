namespace Kiri
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class ClientBuilder
    {
        public static ClientBuilder<T> Create<T>(T session) where T: class =>
            new ClientBuilder<T>(session);
    }

    public class ClientBuilder<T> : IClientBuilder<T> where T : class
    {
        private readonly IList<IMiddleware<T>> middleware = new List<IMiddleware<T>>();

        private RequestDelegate<T> terminal = context => Task.CompletedTask;

        private readonly T session;

        public ClientBuilder(T session)
        {
            this.session = session;
        }

        public IClientBuilder<T> Use(IMiddleware<T> middleware)
        {
            throw new NotImplementedException();
        }

        public IClientBuilder<T> Use(Func<IContext<T>, Func<Task>, Task> middleware)
        {
            this.middleware.Add(new MiddlewareAdapter<T>(middleware));
            return this;
        }

        public IClientBuilder<T> Run(RequestDelegate<T> handler)
        {
            this.terminal = handler;
            return this;
        }

        public Client<T> Build()
        {
            RequestDelegate<T> @delegate = this.terminal;
            foreach (var m in this.middleware.Reverse())
            {
                @delegate = Continue(m.Execute, @delegate);
            }

            return new Client<T>(this.session, @delegate);
        }

        private RequestDelegate<T> Continue(Func<IContext<T>, Func<Task>, Task> middleware, RequestDelegate<T> requestDelegate) =>
            context => middleware(context, () => requestDelegate(context));
    }
}