namespace Kiri
{
    using System;
    
    public interface IClientBuilder<T> where T : class
    {
        IClientBuilder<T> Use(IMiddleware<T> middleware);

        IClientBuilder<T> Use(Action<IContext<T>, Action> middleware);

        IClientBuilder<T> Run(Action<IContext<T>> terminal);

        Client<T> Build();
    }
}