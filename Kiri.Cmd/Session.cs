namespace Kiri.Cmd
{
    using System.Collections.Generic;

    public class Session : IRegistrationProvider
    {
        private readonly string nick;
        private readonly string url;
        private readonly IDictionary<string, ISet<string>> channels;

        public Session(string nick, string url)
        {
            this.nick = nick;
            this.url = url;
            this.channels = new Dictionary<string, ISet<string>>();
        }

        public string Nick => this.nick;

        public string Info => this.url;

        public IDictionary<string, ISet<string>> Channels => this.channels;
    }
}