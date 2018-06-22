namespace Kiri
{
    public interface IContext : ISender
    {
        string Message { get; }
    }
}