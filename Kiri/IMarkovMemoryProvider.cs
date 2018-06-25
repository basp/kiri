namespace Kiri
{
    using System;
    using System.Collections.Generic;

    interface IMarkovMemoryProvider
    {
        IDictionary<Tuple<string, string>, IList<string>> Memory { get; }
    }
}