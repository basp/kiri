namespace Kiri
{
    using System;
    using System.Collections.Generic;

    public class CommandMiddleware : IMiddleware
    {
        private readonly IDictionary<string, ICommand> commands =
            new Dictionary<string, ICommand>();

        public void Execute(IContext context, Action next)
        {
            next();
        }
    }
}