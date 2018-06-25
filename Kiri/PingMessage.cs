namespace Kiri
{
    using Sprache;

    public class PingMessage
    {
        private readonly string ping;

        public PingMessage(string ping)
        {
            this.ping = ping;
        }

        public string Ping => this.ping;

        public static bool TryParse(string s, out PingMessage message)
        {
            message = null;

            var res = Grammar.PingMessage.TryParse(s);
            if (res.WasSuccessful)
            {
                message = res.Value;
                return true;
            }

            return false;
        }
    }
}