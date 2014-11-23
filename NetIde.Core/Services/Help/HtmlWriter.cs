using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetIde.Core.Services.Help
{
    internal class HtmlWriter
    {
        private readonly StringBuilder _sb = new StringBuilder();
        private readonly XmlWriter _writer;

        public HtmlWriter()
        {
            _writer = XmlWriter.Create(_sb);
        }

        public HtmlWriter Tag(string tag)
        {
            return Tag(tag, null);
        }

        public HtmlWriter Tag(string tag, string value)
        {
            _writer.WriteElementString(tag, value);
            return this;
        }

        public HtmlWriter OpenTag(string tag)
        {
            _writer.WriteStartElement(tag);
            return this;
        }

        public HtmlWriter CloseTag()
        {
            _writer.WriteEndElement();
            return this;
        }

        public HtmlWriter Attribute(string name, string value)
        {
            _writer.WriteAttributeString(name, value);
            return this;
        }

        public HtmlWriter Text(string text)
        {
            _writer.WriteString(text);
            return this;
        }

        public override string ToString()
        {
            _writer.Flush();
            return _sb.ToString();
        }
    }
}
