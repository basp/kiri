namespace Kiri
{
    using System;
    
    public interface IMiddleware<T> where T: class
    {
        void Execute(IContext<T> context, Action next);
    }
}