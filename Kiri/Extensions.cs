namespace Kiri
{
    using System;
    using System.Linq;

    public static class Extensions
    {
        public static IClientBuilder<T> WithIdentity<T>(this IClientBuilder<T> builder)
            where T : class, IIdentityProvider => builder.Use(new IdentityMiddleware<T>());

        public static IClientBuilder<T> WithPong<T>(this IClientBuilder<T> builder)
            where T : class => builder.Use(new PongMiddleware<T>());

        public static IClientBuilder<T> WithLogging<T>(this IClientBuilder<T> builder)
            where T : class => builder.Use(new LoggingMiddleware<T>());

        public static IClientBuilder<T> WithLogging<T>(this IClientBuilder<T> builder, Action<IContext<T>> log)
            where T : class => builder.Use(new LoggingMiddleware<T>(log));

        public static IClientBuilder<T> WithGreeting<T>(this IClientBuilder<T> builder)
            where T : class, IIdentityProvider => builder.Use(new GreetingMiddleware<T>());
    }
}