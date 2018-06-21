namespace Kiri.Cmd
{
    using System;

    public abstract class ConsoleDriver
    {
        private Rect clip;

        public abstract int Cols { get; }

        public abstract int Rows { get; }

        public abstract void Init(Action terminalResized);

        public abstract void Move(int col, int row);

        public abstract void Add(char c);

        public abstract void Add(string s);

        public abstract void Refresh();

        public abstract void UpdateCursor();

        public abstract void End();

        public abstract void UpdateScreen();

        public abstract void Suspend();


    }
}