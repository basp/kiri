namespace Kiri
{
    using System;

    public class RegistrationMiddleware<T> : IMiddleware<T> where T: class, IRegistrationProvider
    {
        private bool registered;

        public void Execute(IContext<T> context, Action next)
        {
            var nick = context.Session.Nick;
            var info = context.Session.Info;

            if (!this.registered)
            {
                context.Send($"NICK {nick}");
                context.Send($"USER {nick} 8 * :{info}");

                this.registered = true;
            }

            next();
        }
    }
}