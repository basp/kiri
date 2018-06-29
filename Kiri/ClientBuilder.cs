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

    public class ClientBuilder<T> where T : class
    {
        private readonly IList<Func<IContext<T>, Func<Task>, Task>> middleware =
            new List<Func<IContext<T>, Func<Task>, Task>>();

        private RequestDelegate<T> terminal = context => Task.CompletedTask;

        private readonly T session;

        public ClientBuilder(T session)
        {
            this.session = session;
        }

        public ClientBuilder<T> Use(Func<IContext<T>, Func<Task>, Task> middleware)
        {
            this.middleware.Add(middleware);
            return this;
        }

        public ClientBuilder<T> Run(RequestDelegate<T> handler)
        {
            this.terminal = handler;
            return this;
        }

        public Client<T> Build()
        {
            RequestDelegate<T> @delegate = this.terminal;
            foreach (var m in this.middleware.Reverse())
            {
                @delegate = Continue(m, @delegate);
            }

            return new Client<T>(this.session, @delegate);
        }

        private RequestDelegate<T> Continue(Func<IContext<T>, Func<Task>, Task> middleware, RequestDelegate<T> requestDelegate) =>
            context => middleware(context, () => requestDelegate(context));
    }
}