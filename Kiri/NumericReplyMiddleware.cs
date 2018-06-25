namespace Kiri
{
    using System;

    public class NumericReplyMiddleware : IMiddleware
    {
        public void Execute(IContext context, Action next)
        {
            if (NumericReply.TryParse(context.Message, out var reply))
            {
                switch (reply)
                {
                    case MotdStart x:
                        Console.WriteLine($"RPL_MOTDSTART");
                        break;
                    case Motd x:
                        Console.WriteLine($"RPL_MOTD {x.Text}");
                        break;
                    case EndOfMotd x:
                        Console.WriteLine($"RPL_ENDOFMOTD");
                        break;
                    case NamesReply x:
                        Console.WriteLine($"RPL_NAMREPLY: {string.Join(", ", x.Names)}");
                        break;
                    case EndOfNames x:
                        Console.WriteLine($"RPL_ENDOFNAMES");
                        break;
                    default:
                        break;
                }
            }
        }
    }
}