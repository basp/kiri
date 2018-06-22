namespace Kiri
{
    public struct Point
    {
        public static readonly Point Empty;

        public int X;

        public int Y;

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Point(Size size)
        {
            this.X = size.Width;
            this.Y = size.Height;
        }

        public static bool operator ==(Point left, Point right) =>
            left.X == right.X && left.Y == right.Y;

        public static bool operator !=(Point left, Point right) =>
            left.X != right.X || left.Y != right.Y;

        public override bool Equals(object obj)
        {
            if (!(obj is Point))
            {
                return false;
            }

            return this == (Point)obj;
        }

        public override int GetHashCode() => this.X ^ this.Y;
    }
}