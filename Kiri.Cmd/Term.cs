namespace Kiri
{
    using System;

    public class Term
    {
        private int rows;

        private int cols;

        private int currentRow;

        private int currentCol;

        private int[,,] mem;

        private bool[] dirtyLines;

        public Term()
        {
            this.rows = Console.WindowHeight - 1;
            this.cols = Console.WindowWidth;
        }

        public int Rows => this.rows;

        public int Cols => this.cols;

        public void Write(char c)
        {
            this.mem[this.currentRow, this.currentCol, 0] = c;
            this.mem[this.currentRow, this.currentCol, 1] = 0;
            this.mem[this.currentRow, this.currentCol, 2] = 1;

            this.dirtyLines[this.currentRow] = true;

            this.currentCol++;
            if (this.currentCol == this.Cols)
            {
                this.currentCol = 0;
                if (this.currentRow + 1 < this.Rows)
                {
                    this.currentRow++;
                }
            }
        }

        public void Write(string s)
        {
            foreach (var c in s)
            {
                this.Write(c);
            }
        }

        public void Move(int row, int col)
        {
            this.currentRow = row;
            this.currentCol = col;

            Console.CursorTop = this.currentRow;
            Console.CursorLeft = this.currentCol;
        }

        private void UpdateOffscreen()
        {
            var rows = this.Rows;
            var cols = this.Cols;

            this.mem = new int[rows, cols, 3];
            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < cols; c++)
                {
                    this.mem[r, c, 0] = ' ';
                    this.mem[r, c, 1] = 0;
                    this.mem[r, c, 2] = 0;
                }
            }

            this.dirtyLines = new bool[rows];
            for (var r = 0; r < rows; r++)
            {
                this.dirtyLines[r] = true;
            }
        }
    }
}