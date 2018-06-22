namespace Kiri
{
    public class Command
    {
        private readonly string name;

        private readonly string[] args;

        public Command(string name, string[] args)
        {
            this.name = name;
            this.args = args;
        }

        public string Name => this.name;

        public string[] Args => this.args;
    }
}