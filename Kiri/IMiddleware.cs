namespace Kiri
{
    using System;
    using System.Threading.Tasks;

    public interface IMiddleware2<T> where T : class
    {
        void Execute(IContext<T> context, Action next);
    }

    public interface IMiddleware<T> where T : class
    {
        Task Execute(IContext<T> context, Task next);
    }
}