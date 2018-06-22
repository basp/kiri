namespace Kiri
{
    using System;
    
    public interface IMiddleware
    {
        void Execute(IContext context, Action next);
    }
}