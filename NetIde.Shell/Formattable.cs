using System;
using System.Collections.Generic;
using System.Text;

namespace NetIde.Shell
{
    internal class Formattable
    {
        private string _text = String.Empty;
        private string _formatted;

        public string Text
        {
            get { return _text; }
            set { _text = value ?? String.Empty; }
        }

        public string Formatted
        {
            get { return _formatted ?? _text; }
        }

        public void Format(params object[] args)
        {
            if (args == null)
                throw new ArgumentNullException("args");

            _formatted = String.Format(_text, args);
        }
    }
}