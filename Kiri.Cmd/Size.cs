namespace Kiri
{
    public struct Size
    {
        public static readonly Size Empty;

        public int Width;

        public int Height;

        public Size(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public static bool operator ==(Size left, Size right) =>
            left.Width == right.Width && left.Height == right.Height;

        public static bool operator !=(Size left, Size right) =>
            left.Width != right.Width || left.Height != right.Height;

        public override bool Equals(object obj)
        {
            if (!(obj is Size))
            {
                return false;
            }

            return this == (Size)obj;
        }

        public override int GetHashCode() => this.Width ^ this.Height;
    }
}