namespace Kiri
{
    using System;
    using System.Threading.Tasks;

    internal class MiddlewareAdapter<T> : IMiddleware<T> where T : class
    {
        private Func<IContext<T>, Func<Task>, Task> action;

        public MiddlewareAdapter(Func<IContext<T>, Func<Task>, Task> action)
        {
            this.action = action;
        }

        public async Task Execute(IContext<T> context, Func<Task> next) =>
            await this.action(context, next);
    }
}