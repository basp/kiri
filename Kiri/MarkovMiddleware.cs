namespace Kiri
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class MarkovMiddleware<T> : IMiddleware<T>
        where T : class, IMarkovMemoryProvider, IIdentityProvider
    {
        private static readonly Random rng = new Random();

        public void Seed(T session, string path)
        {
            var lines = File.ReadAllLines(path);
            Seed(session, lines);
        }

        public void Seed(T session, string[] lines)
        {
            foreach (var line in lines)
            {
                Update(session, line);
            }
        }

        private bool ContainsOwnRef(IContext<T> context, string text)
        {
            foreach (var alias in context.Session.Aliases)
            {
                if (text.ToLowerInvariant().Contains(alias.ToLowerInvariant()))
                {
                    return true;
                }
            }

            return false;
        }

        public void Execute(IContext<T> context, Action next)
        {
            if (PrivateMessage.TryParse(context.Message, out var message))
            {
                if (ContainsOwnRef(context, message.Text))
                {
                    var resp = string.Join(" ", Words(context.Session).ToArray());
                    context.Client.Say(resp);
                }

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

            var words = input
                .Split(new[] { ' ' })
                .Select(x => x.Trim())
                .ToList();

            var bigrams = words
                .Zip(words.Skip(1), (x, y) => Tuple.Create(x, y))
                .ToList();

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