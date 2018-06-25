namespace Kiri
{
    using Sprache;

    public class Message
    {
        private readonly string from;

        private readonly string channel;

        private readonly string text;

        public Message(string from, string channel, string text)
        {
            this.from = from;
            this.channel = channel;
            this.text = text;
        }

        public string From => this.from;

        public string Channel => this.channel;

        public string Text => this.text;

        public static bool TryParse(string s, out Message message)
        {
            message = null;

            var res = ReplyGrammar.PrivateMessage.TryParse(s);
            if (res.WasSuccessful)
            {
                message = res.Value;
                return true;
            }

            return false;
        }
    }
}