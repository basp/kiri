namespace Kiri.Cmd
{
    public class CatCommand
    {
        public static bool TryParse(string s, out CatCommand command)
        {
            command = null;

            if (PrivateMessage.TryParse(s, out var message))
            {
                if (message.Text.Contains("cat"))
                {
                    command = new CatCommand();
                    return true;
                }
            }

            return false;
        }
    }
}