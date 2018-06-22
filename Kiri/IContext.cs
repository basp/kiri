namespace Kiri
{
    public interface IContext : ISender
    {
        Client Client { get; }

        string From { get; }

        string Message { get; }
    }
}