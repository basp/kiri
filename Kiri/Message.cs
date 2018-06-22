namespace Kiri
{
    public class Message
    {
        // private string from;
        // private string command;        
        // private string args;

        public static bool TryParse(string s, out Message message)
        {
            message = null;

            if(!s.StartsWith(":"))
            {
                return false;
            }

            

            return false;
        }
    }
}