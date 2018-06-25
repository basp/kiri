namespace Kiri
{
    using System;
    using System.Linq;

    public class PongMiddleware<T> : IMiddleware<T> where T: class
    {
        public void Execute(IContext<T> context, Action next)
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