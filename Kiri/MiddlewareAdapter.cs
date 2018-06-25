namespace Kiri
{
    using System;

    internal class MiddlewareAdapter : IMiddleware
    {
        private Action<IContext, Action> action;

        public MiddlewareAdapter(Action<IContext, Action> action)
        {
            this.action = action;
        }

        public void Execute(IContext context, Action next)
        {
            this.action(context, next);
        }
    }
}