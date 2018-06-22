namespace Kiri
{
    using System;
    using System.Threading.Tasks;

    public class RandomFactsMiddleware : IMiddleware
    {
        private static Random rng = new Random();

        private string[] facts;

        private double q;

        public RandomFactsMiddleware(string[] facts, double q = 0.5)
        {
            this.facts = facts;
            this.q = q;
        }

        public void Execute(IContext context, Action next)
        {
            if (context.Message.Contains("PRIVMSG"))
            {
                var roll = rng.Next(100);
                if (roll > 100 * this.q)
                {
                    roll = rng.Next(this.facts.Length - 1);
                    var fact = this.facts[roll];
                    Console.WriteLine(fact);
                    Task.Factory
                        .StartNew(() => context.Client.Say(context.From, "You know..."))
                        .ContinueWith(_1 => Task.Delay(3320)
                            .ContinueWith(_2 => context.Client.Say(context.From, fact)));
                }
            }

            next();
        }
    }
}