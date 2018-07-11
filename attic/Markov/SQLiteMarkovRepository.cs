using System;

namespace Kiri.Cmd.Markov
{
    public class SQLiteMarkovRepository : IMarkovRepository
    {
        public Tuple<string, string> GetRandomKey()
        {
            throw new NotImplementedException();
        }

        public string[] GetWords(Tuple<string, string> key, int limit)
        {
            throw new NotImplementedException();
        }

        public string[] GetWords(int limit)
        {
            throw new NotImplementedException();
        }

        public void PutWords(Tuple<string, string> key, params string[] words)
        {
            throw new NotImplementedException();
        }
    }
}