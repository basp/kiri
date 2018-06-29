namespace Kiri
{
    using System;
    using System.Threading.Tasks;

    public interface IClientBuilder<T> where T : class
    {
        IClientBuilder<T> Use(IMiddleware<T> middleware);

        IClientBuilder<T> Use(Func<IContext<T>, Func<Task>, Task> middleware);

        IClientBuilder<T> Run(RequestDelegate<T> terminal);

        Client<T> Build();
    }
}