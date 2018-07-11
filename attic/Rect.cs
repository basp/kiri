using System;

namespace Kiri.Cmd
{
    public struct Rect
    {
        public static readonly Rect Empty;

        public int X;

        public int Y;

        public int Width;

        public int Height;

        public Rect(int left, int top, int right, int bottom)
        {
            this.X = left;
            this.Y = top;
            this.Width = right - left;
            this.Height = bottom - top;
        }

        public Rect(Point location, Size size)
        {
            this.X = location.X;
            this.Y = location.Y;
            this.Width = size.Width;
            this.Height = size.Height;
        }

        public int Left => this.X;

        public int Right => this.X + this.Width;

        public int Top => this.Y;

        public int Bottom => this.Y + this.Height;

        public Point Location => new Point(this.X, this.Y);

        public static bool operator ==(Rect left, Rect right) =>
            left.X == right.X &&
            left.Y == right.Y &&
            left.Width == right.Width &&
            left.Height == right.Height;

        public static bool operator !=(Rect left, Rect right) =>
            left.X != right.X ||
            left.Y != right.Y ||
            left.Width != right.Width ||
            left.Height != right.Height;

        public static Rect Intersect(Rect a, Rect b)
        {
            if (!a.IntersectsWithInclusive(b))
            {
                return Empty;
            }

            return new Rect(
                Math.Max(a.Left, b.Left),
                Math.Max(a.Top, b.Top),
                Math.Min(a.Right, b.Right),
                Math.Min(a.Bottom, b.Bottom));
        }

        public bool Contains(int x, int y) =>
            (x >= this.Left) && (x < this.Right) &&
            (y >= this.Top) && (y < this.Bottom);

        public bool Contains(Point p) => Contains(p.X, p.Y);

        public bool IntersectsWith(Rect rect) =>
            !(this.Left >= rect.Right ||
            this.Right <= rect.Left ||
            this.Top >= rect.Bottom ||
            this.Bottom <= rect.Top);

        public override bool Equals(object obj)
        {
            if (!(obj is Rect))
            {
                return false;
            }

            return this == (Rect)obj;
        }

        public override int GetHashCode() =>
            (this.X ^ this.Width) ^ (this.Y ^ this.Height);

        private bool IntersectsWithInclusive(Rect r) =>
            !(this.Left > r.Right ||
            this.Right < r.Left ||
            this.Top > r.Bottom ||
            this.Bottom < r.Top);
    }
}