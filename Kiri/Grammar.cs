namespace Kiri
{
    using System.Linq;
    using Sprache;

    public static class Grammar
    {

        public static Parser<PingMessage> PingMessage =
            from _1 in Parse.String("PING").Token()
            from _2 in Parse.Char(':').Token()
            from ping in Parse.AnyChar.AtLeastOnce().Token().Text()
            select new PingMessage(ping);

        public static Parser<PrivateMessage> PrivateMessage =
            from _1 in Parse.Char(':').Token()
            from @from in Parse.AnyChar.Except(Parse.WhiteSpace).AtLeastOnce().Token().Text()
            from _2 in Parse.String("PRIVMSG").Token()
            from channel in Parse.AnyChar.Except(Parse.WhiteSpace).AtLeastOnce().Token().Text()
            from _3 in Parse.Char(':')
            from message in Parse.AnyChar.Many().Token().Text()
            select new PrivateMessage(@from, channel, message);

        private static Parser<string> Prefix =
            from _ in Parse.Char(':')
            from prefix in Parse.Until(Parse.AnyChar, Parse.WhiteSpace).Token().Text()
            select prefix;

        private static Parser<string> Nick =
            from _ in Parse.Chars('@', '+').Optional()
            from nick in Parse.AnyChar.Except(Parse.WhiteSpace).AtLeastOnce().Token().Text()
            select nick;

        public static Parser<NumericReply> NumericReply =
            from prefix in Prefix
            from numeric in Parse.Digit.Repeat(3).Token().Text()
            from target in Parse.AnyChar.Except(Parse.WhiteSpace).AtLeastOnce().Token().Text()
            from reply in Parse.AnyChar.Many().Token().Text()
            select new NumericReply(
                prefix,
                int.Parse(numeric),
                target,
                reply);

        public static Parser<MotdStart> MotdStart =
            from _1 in Parse.Char(':').Token()
            from _2 in Parse.Char('-').Token()
            from server in Parse.AnyChar.Except(Parse.WhiteSpace).AtLeastOnce().Token().Text()
            select new MotdStart(server);

        public static Parser<Motd> Motd =
            from _1 in Parse.Char(':').Token()
            from _2 in Parse.Char('-').Token()
            from text in Parse.AnyChar.Many().Token().Text()
            select new Motd(text);

        public static Parser<EndOfMotd> EndOfMotd =
            from _1 in Parse.Char(':').Token()
            from _2 in Parse.String("End of /MOTD command.").Token()
            select new EndOfMotd();

        public static Parser<NamesReply> NamesReply =
            from _1 in Parse.Chars('=', '*', '@').Token()
            from channel in Parse.AnyChar.Except(Parse.WhiteSpace).AtLeastOnce().Token().Text()
            from _2 in Parse.Char(':').Token()
            from nicks in Nick.AtLeastOnce().Token()
            select new NamesReply(channel, nicks.ToArray());

        public static Parser<EndOfNames> EndOfNames =
            from channel in Parse.AnyChar.Except(Parse.WhiteSpace).AtLeastOnce().Token().Text()
            from _1 in Parse.Char(':').Token()
            from _2 in Parse.String("End of /NAMES list.").Token()
            select new EndOfNames(channel);
    }
}
