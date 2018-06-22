namespace Kiri
{
    using System;

    public class LoggingMiddleware : IMiddleware
    {
        private readonly Action<IContext> log;

        public LoggingMiddleware(Action<IContext> log)
        {
            this.log = log;
        }

        public void Execute(IContext context, Action next)
        {
            this.log(context);
            next();
        }
    }
}