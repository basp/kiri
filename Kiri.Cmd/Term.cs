namespace Kiri
{
    using System;

    public class Term
    {
        private int rows;

        private int cols;

        private int[,,] mem;

        public Term()
        {
            this.rows = Console.WindowHeight;
            this.cols = Console.WindowWidth;
        }

        public int Rows => this.rows;

        public int Cols => this.cols;
    }
}