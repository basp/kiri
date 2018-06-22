namespace Kiri
{
    public interface ICommand
    {
        bool TryParse(string args, out ICommand command);

        void Execute();
    }
}