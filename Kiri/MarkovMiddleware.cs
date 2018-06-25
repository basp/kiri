namespace Kiri
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class MarkovMiddleware<T> : IMiddleware<T> where T : class, IMarkovMemoryProvider
    {
        public void Execute(IContext<T> context, Action next)
        {
            if (Message.TryParse(context.Message, out var message))
            {
                var mem = context.Session.Memory;

                var words = message.Text.Split(new[] { ' ' }).Select(x => x.Trim()).ToList();
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

                // var json = JsonConvert.SerializeObject(mem);
                // Console.WriteLine(json);
            }

            next();
        }
    }
}