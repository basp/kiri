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

        public void Execute(IContext<T> context, Action next)
        {
            this.log(context);
            next();
        }

        public Task Execute(IContext<T> context, Task next)
        {
            throw new NotImplementedException();
        }
    }
}