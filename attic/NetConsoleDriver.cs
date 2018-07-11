namespace Kiri
{
    // https://github.com/migueldeicaza/gui.cs

    using System;

    class Terminal
    {
        int rows;

        int cols;

        int crow;

        int ccol;

        int[,,] contents;

        bool[] dirtyLine;

        public int Rows => this.rows;

        public int Cols => this.cols;

        public Terminal()
        {
            this.rows = Console.WindowHeight - 1;
            this.cols = Console.WindowWidth;
            UpdateOffscreen();
        }

        public void Init()
        {
            Console.Clear();
        }

        public void Move(int col, int row)
        {
            this.ccol = col;
            this.crow = row;

            Console.CursorTop = row;
            Console.CursorLeft = col;
        }

        public void Add(char c)
        {
            this.contents[this.crow, this.ccol, 0] = c;
            this.contents[this.crow, this.ccol, 1] = 0;
            this.contents[this.crow, this.ccol, 2] = 1;

            this.dirtyLine[this.crow] = true;

            this.ccol++;
            if (this.ccol == this.Cols)
            {
                this.ccol = 0;
                if (this.crow + 1 < this.Rows)
                {
                    this.crow++;
                }
            }
        }

        public void Add(string s)
        {
            foreach (var c in s)
            {
                Add(c);
            }
        }

        public void Refresh()
        {
            var rows = this.Rows;
            var cols = this.Cols;

            var savedRow = Console.CursorTop;
            var savedCol = Console.CursorLeft;

            Console.CursorVisible = false;

            for (var r = 0; r < rows; r++)
            {
                if (!this.dirtyLine[r])
                {
                    continue;
                }

                this.dirtyLine[r] = false;

                for (var c = 0; c < cols; c++)
                {
                    if (this.contents[r, c, 2] != 1)
                    {
                        continue;
                    }

                    Console.CursorTop = r;
                    Console.CursorLeft = c;

                    for (; c < cols && contents[r, c, 2] == 1; c++)
                    {
                        Console.Write((char)this.contents[r, c, 0]);
                        this.contents[r, c, 2] = 0;
                    }
                }
            }

            Console.CursorTop = savedRow;
            Console.CursorLeft = savedCol;

            Console.CursorVisible = true;
        }

        public void End()
        {
            Console.ResetColor();
            Console.Clear();
        }

        private void UpdateOffscreen()
        {
            var rows = this.Rows;
            var cols = this.Cols;

            this.contents = new int[rows, cols, 3];
            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < cols; c++)
                {
                    this.contents[r, c, 0] = ' ';
                    this.contents[r, c, 1] = 0;
                    this.contents[r, c, 2] = 0;
                }
            }

            this.dirtyLine = new bool[rows];
            for (var r = 0; r < rows; r++)
            {
                this.dirtyLine[r] = true;
            }
        }

        private void UpdateScreen()
        {
            var rows = this.Rows;
            var cols = this.Cols;

            Console.CursorTop = 0;
            Console.CursorLeft = 0;

            for (var r = 0; r < rows; r++)
            {
                this.dirtyLine[r] = false;
                for (var c = 0; c < cols; c++)
                {
                    this.contents[r, c, 2] = 0;
                    Console.Write((char)this.contents[r, c, 0]);
                }
            }
        }
    }
}