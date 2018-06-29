namespace Kiri
{
    using System;
    using System.Threading.Tasks;

    internal class MiddlewareAdapter<T> : IMiddleware<T> where T: class
    {
        private Action<IContext<T>, Action> action;

        public MiddlewareAdapter(Action<IContext<T>, Action> action)
        {
            this.action = action;
        }

        public void Execute(IContext<T> context, Action next)
        {
            this.action(context, next);
        }

        public Task Execute(IContext<T> context, Task next)
        {
            throw new NotImplementedException();
        }
    }
}