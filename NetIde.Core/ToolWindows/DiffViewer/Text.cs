using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGit.Diff;
using NGit.Util;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    internal partial class Text : Sequence
    {
        public string Content { get; private set; }

		private readonly IntList _lines;

		/// <summary>Create a new sequence from an existing content char array.</summary>
		/// <remarks>
		/// Create a new sequence from an existing content char array.
		/// <p/>
		/// The entire array (indexes 0 through length-1) is used as the content.
		/// </remarks>
		/// <param name="input">
		/// the content array. The array is never modified, so passing
		/// through cached arrays is safe.
		/// </param>
		public Text(string input)
		{
			Content = input;
			_lines = LineMap(0, Content.Length);
		}

        private IntList LineMap(int ptr, int end)
        {
            // Experimentally derived from multiple source repositories
            // the average number of bytes/line is 36. Its a rough guess
            // to initially size our map close to the target.
            //
            IntList map = new IntList((end - ptr) / 36);
            map.FillTo(1, int.MinValue);
            for (; ptr < end; ptr = NextLF(ptr))
            {
                map.Add(ptr);
            }
            map.Add(end);
            return map;
        }

        private int NextLF(int ptr)
        {
            return Next(ptr, '\n');
        }

        private int Next(int ptr, char chrA)
        {
            int sz = Content.Length;
            while (ptr < sz)
            {
                if (Content[ptr++] == chrA)
                {
                    return ptr;
                }
            }
            return ptr;
        }

		/// <returns>total number of items in the sequence.</returns>
		public override int Size()
		{
			// The line map is always 2 entries larger than the number of lines in
			// the file. Index 0 is padded out/unused. The last index is the total
			// length of the buffer, and acts as a sentinel.
			//
			return _lines.Size() - 2;
		}

		/// <summary>Write a specific line to the output stream, without its trailing LF.</summary>
		/// <remarks>
		/// Write a specific line to the output stream, without its trailing LF.
		/// <p/>
		/// The specified line is copied as-is, with no character encoding
		/// translation performed.
		/// <p/>
		/// If the specified line ends with an LF ('\n'), the LF is <b>not</b>
		/// copied. It is up to the caller to write the LF, if desired, between
		/// output lines.
		/// </remarks>
		/// <param name="out">stream to copy the line data onto.</param>
		/// <param name="i">
		/// index of the line to extract. Note this is 0-based, so line
		/// number 1 is actually index 0.
		/// </param>
		/// <exception cref="System.IO.IOException">the stream write operation failed.</exception>
		public void WriteLine(StringBuilder @out, int i)
		{
			int start = GetStart(i);
			int end = GetEnd(i);
			if (Content[end - 1] == '\n')
			{
				end--;
			}
			@out.Append(Content, start, end - start);
		}

		/// <summary>Determine if the file ends with a LF ('\n').</summary>
		/// <remarks>Determine if the file ends with a LF ('\n').</remarks>
		/// <returns>true if the last line has an LF; false otherwise.</returns>
		public bool IsMissingNewlineAtEnd()
		{
			int end = _lines.Get(_lines.Size() - 1);
			if (end == 0)
			{
				return true;
			}
			return Content[end - 1] != '\n';
		}

		private int GetStart(int i)
		{
			return _lines.Get(i + 1);
		}

		private int GetEnd(int i)
		{
			return _lines.Get(i + 2);
		}
    }
}
