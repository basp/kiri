namespace Kiri
{
    using System;
    using System.Linq;

    public static class Extensions
    {
        const string DefaultInfo = "https://github.com/basp/kiri";

        public static Client WithRegistration(this Client client, string nick) =>
            client.WithRegistration(nick, DefaultInfo);

        public static Client WithRegistration(this Client client, string nick, string info)
        {
            var registered = false;
            client.Use((ctx, next) =>
            {
            });

            return client;
        }

        public static Client WithPong(this Client client)
        {
            client.Use((ctx, next) =>
            {
            });

            return client;
        }

        public static Client WithLogging(this Client client, Action<IContext> log)
        {
            client.Use((ctx, next) =>
            {
            });

            return client;
        }
    }
}