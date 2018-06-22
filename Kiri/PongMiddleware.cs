namespace Kiri
{
    using System;
    using System.Linq;

    public class PongMiddleware : IMiddleware
    {
        public void Execute(IContext context, Action next)
        {
            if (context.Message.StartsWith("PING"))
            {
                var chunks = context.Message
                    .Split(':')
                    .Select(x => x.Trim())
                    .ToArray();

                var ping = chunks[1];
                context.Send($"PONG :{ping}");
            }

            next();
        }
    }
}