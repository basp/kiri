namespace Kiri.Cmd
{
    using System;

    public class CatMiddleware<T> : IMiddleware<T> where T : class
    {
        public void Execute(IContext<T> context, Action next)
        {
            if (CatCommand.TryParse(context.Message, out CatCommand command))
            {
                var cat = Cats.Next();
                context.Client.Say(cat);
            }

            next();
        }
    }
}