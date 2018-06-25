namespace Kiri
{
    using System;
    using System.Linq;

    public class GreetingMiddleware<T> : IMiddleware<T> where T: class
    {
        private bool greeted = false;

        public void Execute(IContext<T> context, Action next)
        {
            if (!greeted)
            {
                if (NumericReply.TryParse<NamesReply>(context.Message, out var reply))
                {
                    var names = reply.Names.Except(new[] { "Methbot" }).ToList();

                    if (names.Count > 1)
                    {
                        context.Client.Say($"Hi guys!");
                    }
                    else if (names.Count > 0)
                    {
                        context.Client.Say($"Hi {names[0]}!");
                    }
                }
            }

            next();
        }
    }
}