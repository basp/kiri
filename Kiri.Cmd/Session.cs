namespace Kiri.Cmd
{
    using System;
    using System.Collections.Generic;

    public class Session : IRegistrationProvider, IMarkovMemoryProvider
    {
        private IDictionary<Tuple<string, string>, IList<string>> memory;
        private readonly string nick;
        private readonly string url;
        private readonly IDictionary<string, ISet<string>> channels;

        public Session(string nick, string url)
        {
            this.nick = nick;
            this.url = url;
            this.channels = new Dictionary<string, ISet<string>>();
            this.memory = new Dictionary<Tuple<string, string>, IList<string>>();
        }

        public string Nick => this.nick;

        public string Info => this.url;

        public IDictionary<string, ISet<string>> Channels => this.channels;

        public IDictionary<Tuple<string, string>, IList<string>> Memory => this.memory;
    }
}