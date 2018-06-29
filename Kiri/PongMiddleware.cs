namespace Kiri
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Serilog;
    using Sprache;

    public class PongMiddleware<T> : IMiddleware<T> where T : class
    {
        public async Task Execute(IContext<T> context, Func<Task> next)
        {
            if (PingMessage.TryParse(context.Message, out var message))
            {
                await context.Client.SendAsync($"PONG :{message.Ping}");
            }

            await next();
        }
    }
}