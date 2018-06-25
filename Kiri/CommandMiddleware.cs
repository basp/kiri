namespace Kiri
{
    using System;
    using System.Collections.Generic;

    public class CommandMiddleware<T> : IMiddleware<T> where T: class
    {
        private readonly IDictionary<string, ICommand> commands =
            new Dictionary<string, ICommand>();

        public void Execute(IContext<T> context, Action next)
        {
            next();
        }
    }
}