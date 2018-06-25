namespace Kiri
{
    using System;
    using System.Linq;
    using Serilog;
    using Sprache;

    public class PongMiddleware<T> : IMiddleware<T> where T : class
    {
        public void Execute(IContext<T> context, Action next)
        {
            if (PingMessage.TryParse(context.Message, out var message))
            {
                context.Send($"PONG :{message.Ping}");
            }

            next();
        }
    }
}