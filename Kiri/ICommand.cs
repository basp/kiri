namespace Kiri
{
    public interface ICommand<T> where T : class
    {
        void Execute(IContext<T> context);
    }
}