namespace Kiri
{
    public class FactCommand
    {
        public static bool TryParse(string s, out FactCommand command)
        {
            command = null;

            if (PrivateMessage.TryParse(s, out var message))
            {
                if (message.Text == "fact")
                {
                    command = new FactCommand();
                    return true;
                }
            }

            return false;
        }
    }
}