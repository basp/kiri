namespace Kiri
{
    using System;

    public class RegistrationMiddleware : IMiddleware
    {
        private readonly string nick;

        private readonly string info;

        private bool registered;

        public RegistrationMiddleware(string nick, string info)
        {
            this.nick = nick;
            this.info = info;
        }

        public void Execute(IContext context, Action next)
        {
            if (!this.registered)
            {
                context.Send($"NICK {this.nick}");
                context.Send($"USER {this.nick} 8 * :{this.info}");

                this.registered = true;
            }

            next();
        }
    }
}