namespace Kiri.Facts
{
    using System;
    using Xunit;

    public class Protocol
    {
        public static string PasswordMessage(string password) =>
            $"PASS {password}";

        public static string NickMessage(string nickname) =>
            $"NICK {nickname}";
    }


    public class MessageParserFacts
    {
    }
}
