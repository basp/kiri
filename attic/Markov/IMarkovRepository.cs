namespace Kiri.Cmd.Markov
{
    using System;
    using System.Collections.Generic;

    public interface IMarkovRepository
    {
        Tuple<string, string> GetRandomKey();
        string[] GetWords(int limit);
        string[] GetWords(Tuple<string, string> key, int limit);
        void PutWords(Tuple<string, string> key, params string[] words);
    }
}