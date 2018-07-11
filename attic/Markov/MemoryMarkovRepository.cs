namespace Kiri.Cmd.Markov
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MemoryMarkovRepository : IMarkovRepository
    {
        private static readonly Random rng = new Random();

        private readonly IDictionary<Tuple<string, string>, List<string>> memory =
            new Dictionary<Tuple<string, string>, List<string>>();

        public Tuple<string, string> GetRandomKey() =>
            this.memory.Keys.ElementAt(rng.Next(this.memory.Keys.Count));

        public string[] GetWords(int limit) => GetWords(GetRandomKey(), limit);

        public string[] GetWords(Tuple<string, string> key, int limit)
        {
            var words = new List<string>(limit);
            words.Add(key.Item1);
            words.Add(key.Item2);

            for (var i = 0; i < limit; i++)
            {
                if (!this.memory.ContainsKey(key))
                {
                    break;
                }

                var row = this.memory[key];
                words.Add(row.ElementAt(rng.Next(row.Count - 1)));
                key = Tuple.Create(words[words.Count - 2], words[words.Count - 1]);
            }

            return words.ToArray();
        }

        public void PutWords(Tuple<string, string> key, params string[] words)
        {
            if (!this.memory.ContainsKey(key))
            {
                this.memory.Add(key, new List<string>());
            }

            this.memory[key].AddRange(words);
        }
    }
}