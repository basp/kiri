namespace Kiri
{
    using Sprache;

    public class CommandInfo
    {
        static Parser<CommandInfo> cmd =
            from colon in Parse.Char(':').Token()
            from name in Parse.Identifier(Parse.Letter, Parse.LetterOrDigit.Or(Parse.Char('_'))).Token()
            from argStr in Parse.Until(Parse.AnyChar, Parse.LineTerminator).Text().Token()
            select new CommandInfo(name, argStr);

        public CommandInfo(string name, string argStr)
        {
            this.Name = name;
            this.ArgStr = argStr;
        }

        public string Name { get; }

        public string ArgStr { get; }

        public static bool TryParse(string s, out CommandInfo command)
        {
            command = null;

            var res = cmd.TryParse(s);
            if (res.WasSuccessful)
            {
                command = res.Value;
                return true;
            }

            return false;
        }
    }
}