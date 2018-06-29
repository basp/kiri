namespace Kiri
{
    using System;
    using System.Threading.Tasks;

    public interface IMiddleware<T> where T : class
    {
        Task Execute(IContext<T> context, Func<Task> next);
    }
}