namespace Kiri
{
    using System;
    using System.Collections.Generic;

    public interface IContext<T> : ISender where T: class
    {
        T Session { get; }

        Client<T> Client { get; }

        string From { get; }

        string Message { get; }
    }
}