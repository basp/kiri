namespace Kiri
{
    using Sprache;

    public class NumericReply
    {
        public const int RPL_WELCOME = 1;
        public const int RPL_YOURHOST = 2;
        public const int RPL_CREATED = 3;
        public const int RPL_MYINFO = 4;
        public const int RPL_BOUNCE = 5;
        public const int RPL_TOPIC = 332;
        public const int RPL_VERSION = 351;
        public const int RPL_WHOREPLY = 352;
        public const int RPL_ENDOFWHO = 315;
        public const int RPL_NAMREPLY = 353;
        public const int RPL_ENDOFNAMES = 366;
        public const int RPL_MOTDSTART = 375;
        public const int RPL_MOTD = 372;
        public const int RPL_ENDOFMOTD = 376;

        private readonly string prefix;
        private readonly int numeric;
        private readonly string target;
        private readonly string reply;

        public NumericReply(string prefix, int numeric, string target, string reply)
        {
            this.prefix = prefix;
            this.numeric = numeric;
            this.target = target;
            this.reply = reply;
        }

        public string Prefix => this.prefix;

        public int Numeric => this.numeric;

        public string Target => this.target;

        public string Reply => this.reply;

        private bool TryParse<T>(out T reply)
        {
            reply = default(T);
            object tmp = null;
            switch (this.Numeric)
            {
                case RPL_NAMREPLY:
                    NamesReply.TryParse(this.reply, out tmp);
                    break;
                case RPL_ENDOFNAMES:
                    EndOfNames.TryParse(this.reply, out tmp);
                    break;
                case RPL_MOTDSTART:
                    MotdStart.TryParse(this.reply, out tmp);
                    break;
                case RPL_MOTD:
                    Motd.TryParse(this.reply, out tmp);
                    break;
                case RPL_ENDOFMOTD:
                    EndOfMotd.TryParse(this.reply, out tmp);
                    break;
                default:
                    return false;
            }

            if (tmp is T)
            {
                reply = (T)tmp;
                return true;
            }

            return false;
        }

        public static bool TryParse<T>(string s, out T reply)
        {
            reply = default(T);

            var maybeNumericReply = ReplyGrammar.NumericReply.TryParse(s);
            if (maybeNumericReply.WasSuccessful)
            {
                if (maybeNumericReply.Value.TryParse(out reply))
                {
                    return true;
                }

                return false;
            }

            return false;
        }
    }

    public class NamesReply
    {
        private readonly string channel;
        private readonly string[] names;

        public NamesReply(string channel, string[] names)
        {
            this.channel = channel;
            this.names = names;
        }

        public string Channel => this.channel;

        public string[] Names => this.names;

        public static bool TryParse(string s, out object reply)
        {
            reply = null;

            var res = ReplyGrammar.NamesReply.TryParse(s);
            if (res.WasSuccessful)
            {
                reply = res.Value;
                return true;
            }

            return false;
        }
    }

    public class EndOfNames
    {
        private readonly string channel;

        public EndOfNames(string channel)
        {
            this.channel = channel;
        }

        public string Channel => this.channel;

        public static bool TryParse(string s, out object reply)
        {
            reply = null;

            var res = ReplyGrammar.EndOfNames.TryParse(s);
            if (res.WasSuccessful)
            {
                reply = res.Value;
                return true;
            }

            return false;
        }
    }

    public class MotdStart
    {
        private readonly string server;

        public MotdStart(string server)
        {
            this.server = server;
        }

        public string Server => this.server;

        public static bool TryParse(string s, out object reply)
        {
            reply = null;

            var res = ReplyGrammar.MotdStart.TryParse(s);
            if (res.WasSuccessful)
            {
                reply = res.Value;
                return true;
            }

            return false;
        }
    }

    public class Motd
    {
        private readonly string text;

        public Motd(string text)
        {
            this.text = text;
        }

        public string Text => this.text;

        public static bool TryParse(string s, out object reply)
        {
            reply = null;

            var res = ReplyGrammar.Motd.TryParse(s);
            if (res.WasSuccessful)
            {
                reply = res.Value;
                return true;
            }

            return false;
        }
    }

    public class EndOfMotd
    {
        public static bool TryParse(string s, out object reply)
        {
            reply = null;

            var res = ReplyGrammar.EndOfMotd.TryParse(s);
            if (res.WasSuccessful)
            {
                reply = res.Value;
                return true;
            }

            return false;
        }
    }
}