namespace Kiri
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class GreetingMiddleware<T> : IMiddleware<T> where T : class, IIdentityProvider
    {
        private static readonly Random rng = new Random();

        private bool greeted = false;

        private static string[] greetings = new[]
        {
            "hallo",
            "hoi",
            "hello",
            "hi",
            "hei",
            "allo",
            "hola",
            "konnichiwa",
            "aloha",
        };

        private static string RandomGreeting() =>
            greetings[rng.Next(greetings.Length)];

        public void Execute(IContext<T> context, Action next)
        {
            if (!greeted)
            {
                if (NumericReply.TryParse<NamesReply>(context.Message, out var reply))
                {
                    var nick = context.Session.Nick;
                    var names = reply.Names.Except(context.Session.Aliases).ToList();
                    var grt = RandomGreeting();

                    if (names.Count > 1)
                    {
                        context.Client.Say($"{grt} guys!");
                    }
                    else if (names.Count > 0)
                    {
                        context.Client.Say($"{grt} {names[0]}!");
                    }
                }
            }

            next();
        }

        public Task Execute(IContext<T> context, Func<Task> next)
        {
            throw new NotImplementedException();
        }
    }
}