namespace Kiri
{
    public interface IIdentityProvider
    {
        string Nick { get; }

        string Info { get; }

        string[] Aliases { get; }
    }
}