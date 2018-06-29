namespace Kiri
{
    using System;
    using System.Threading.Tasks;

    public class IdentityMiddleware<T> : IMiddleware<T> where T : class, IIdentityProvider
    {
        private bool registered;

        public async Task Execute(IContext<T> context, Func<Task> next)
        {
            var nick = context.Session.Nick;
            var info = context.Session.Info;

            if (!this.registered)
            {
                await context.Client.SendAsync($"NICK {nick}");
                await context.Client.SendAsync($"USER {nick} 8 * :{info}");

                this.registered = true;
            }

            await next();
        }
    }
}