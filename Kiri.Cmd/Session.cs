namespace Kiri.Cmd
{
    using System;
    using System.Collections.Generic;

    public class Session : IIdentityProvider, IMarkovMemoryProvider
    {
        private IDictionary<Tuple<string, string>, IList<string>> memory;
        private readonly string nick;
        private readonly string url;
        private readonly string[] aliases;
        private readonly IDictionary<string, ISet<string>> channels;

        public Session(string nick, string url) : this(nick, url, nick)
        {
        }

        public Session(string nick, string url, params string[] aliases)
        {
            this.nick = nick;
            this.url = url;
            this.aliases = aliases;
            this.channels = new Dictionary<string, ISet<string>>();
            this.memory = new Dictionary<Tuple<string, string>, IList<string>>();
        }

        public string Nick => this.nick;

        public string Info => this.url;

        public string[] Aliases => this.aliases;

        public IDictionary<string, ISet<string>> Channels => this.channels;

        public IDictionary<Tuple<string, string>, IList<string>> Memory => this.memory;
    }
}