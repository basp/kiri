namespace Kiri
{
    using System;
    using System.Threading.Tasks;
    using Serilog;

    public class LoggingMiddleware<T> : IMiddleware<T> where T : class
    {
        private readonly Action<IContext<T>> log;

        public LoggingMiddleware() : this(c => Log.Logger.Information(c.Message))
        {
        }

        public LoggingMiddleware(Action<IContext<T>> log)
        {
            this.log = log;
        }

        public async Task Execute(IContext<T> context, Func<Task> next)
        {
            this.log(context);
            await next();
        }
    }
}