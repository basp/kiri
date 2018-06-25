using System;

namespace Kiri
{
    public class GreetingMiddleware : IMiddleware
    {
        private bool greeted = false;

        public void Execute(IContext context, Action next)
        {
            if(!greeted)
            {
            }

            next();
        }
    }
}