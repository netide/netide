using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Core.Support
{
    internal static class SelectionItem
    {
        public static SelectionItem<T> Create<T>(T value, string text)
        {
            return new SelectionItem<T>(value, text);
        }
    }

    internal class SelectionItem<T>
    {
        public string Text { get; private set; }
        public T Value { get; private set; }

        public SelectionItem(T value, string text)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
