namespace Kiri
{
    using System;
    using System.Linq;

    public static class Extensions
    {
        const string DefaultInfo = "https://github.com/basp/kiri";

        public static Client<T> WithRegistration<T>(this Client<T> client)
            where T : class, IRegistrationProvider => client.Use(new RegistrationMiddleware<T>());

        public static Client<T> WithPong<T>(this Client<T> client)
            where T : class => client.Use(new PongMiddleware<T>());

        public static Client<T> WithLogging<T>(this Client<T> client, Action<IContext<T>> log)
            where T : class => client.Use(new LoggingMiddleware<T>(log));

        public static Client<T> WithGreeting<T>(this Client<T> client)
            where T : class => client.Use(new GreetingMiddleware<T>());
    }
}