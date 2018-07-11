namespace Kiri
{
    using System;
    using System.Linq;

    public static class Extensions
    {
        public static IClientBuilder<T> UseIdentity<T>(this IClientBuilder<T> builder)
            where T : class, IIdentityProvider => builder.Use(new IdentityMiddleware<T>());

        public static IClientBuilder<T> UsePong<T>(this IClientBuilder<T> builder)
            where T : class => builder.Use(new PongMiddleware<T>());

        public static IClientBuilder<T> UseLogging<T>(this IClientBuilder<T> builder)
            where T : class => builder.Use(new LoggingMiddleware<T>());

        public static IClientBuilder<T> UseLogging<T>(this IClientBuilder<T> builder, Action<IContext<T>> log)
            where T : class => builder.Use(new LoggingMiddleware<T>(log));
    }
}