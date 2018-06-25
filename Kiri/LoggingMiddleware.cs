namespace Kiri
{
    using System;

    public class LoggingMiddleware<T> : IMiddleware<T> where T: class
    {
        private readonly Action<IContext<T>> log;

        public LoggingMiddleware(Action<IContext<T>> log)
        {
            this.log = log;
        }

        public void Execute(IContext<T> context, Action next)
        {
            this.log(context);
            next();
        }
    }
}