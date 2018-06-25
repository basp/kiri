namespace Kiri
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    public class MarkovMiddleware<T> : IMiddleware<T> where T : class, IMarkovMemoryProvider
    {
        static readonly Random rng = new Random();

        public void Seed(string input)
        {

        }

        public void Execute(IContext<T> context, Action next)
        {
            if (PrivateMessage.TryParse(context.Message, out var message))
            {
                Update(context.Session, message.Text);
            }

            next();
        }

        private static IEnumerable<string> Words(T session)
        {
            var mem = session.Memory;
            var i = rng.Next(mem.Count);
            var key = mem.Keys.ElementAt(i);
            var words = new List<string> { key.Item1, key.Item2 };
            return Words(session, words, 20);
        }

        private static IEnumerable<string> Words(T session, IList<string> words, int count)
        {
            var mem = session.Memory;
            var t = Tuple.Create(words[words.Count - 2], words[words.Count - 1]);
            while (mem.ContainsKey(t) && count > 0)
            {
                var w = mem[t][rng.Next(mem[t].Count)];
                words.Add(w);
                t = Tuple.Create(words[words.Count - 2], words[words.Count - 1]);
                count--;
            }

            return words;
        }

        private void Update(T session, string input)
        {
            var mem = session.Memory;

            var words = input.Split(new[] { ' ' }).Select(x => x.Trim()).ToList();
            var bigrams = words.Zip(words.Skip(1), (x, y) => Tuple.Create(x, y)).ToList();

            for (var i = 0; i < bigrams.Count - 1; i++)
            {
                var tc = bigrams[i];
                var tn = bigrams[i + 1];

                if (!mem.ContainsKey(tc))
                {
                    mem.Add(tc, new List<string>());
                }

                mem[tc].Add(tn.Item2);
            }
        }
    }
}