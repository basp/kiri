namespace Kiri
{
    public class CommandParser
    {
        public static bool TryParse(string s, out Command command)
        {
            command = null;

            if(!s.StartsWith(":"))
            {
                return false;
            }
        }
    }
}