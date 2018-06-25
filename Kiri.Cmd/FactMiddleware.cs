namespace Kiri.Cmd
{
    using System;
    using System.IO;

    public static class FactMiddleware
    {
        public static FactMiddleware<T> Create<T>(string path) where T : class => FactMiddleware<T>.Create(path);
    }

    public class FactMiddleware<T> : IMiddleware<T> where T : class
    {
        private static readonly Random rng = new Random();

        private readonly string[] facts;

        private FactMiddleware(string[] facts)
        {
            this.facts = facts;
        }

        public static FactMiddleware<T> Create(string path)
        {
            var lines = File.ReadAllLines(path);
            return new FactMiddleware<T>(lines);
        }

        public void Execute(IContext<T> context, Action next)
        {
            if (FactCommand.TryParse(context.Message, out var command))
            {
                var i = rng.Next(this.facts.Length);
                var fact = this.facts[i];
                context.Client.Say(fact);
            }
            else
            {
                next();
            }
        }
    }
}