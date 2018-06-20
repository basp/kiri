using System;
using System.Collections.Generic;

namespace Kiri.Cmd
{
    class LineBuffer
    {
        private int head;
        private string[] lines;

        public LineBuffer(int maxSize)
        {
            if (maxSize <= 0)
            {
                throw new ArgumentException(nameof(maxSize));
            }

            this.lines = new string[maxSize];
            this.head = 0;
        }

        public string this[int index]
        {
            get => this.lines[index];
        }

        public int Head => this.head;

        public int Length => this.lines.Length;

        public void Add(string line)
        {
            this.lines[this.head] = line;
            this.head += 1;
            if (this.head >= this.lines.Length)
            {
                this.head = 0;
            }
        }

        public IEnumerable<string> Last(int count)
        {
            if (count > this.lines.Length)
            {
                count = this.lines.Length;
            }

            for (var offset = this.head - 1; offset >= this.head - count; offset--)
            {
                var i = offset;
                if (i < 0)
                {
                    i = this.lines.Length + i;
                }

                yield return this.lines[i];
            }
        }
    }
}