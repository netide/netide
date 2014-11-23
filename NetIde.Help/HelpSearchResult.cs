using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Help
{
    public class HelpSearchResult
    {
        public string Path { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }

        public HelpSearchResult(string path, string title, string content)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (title == null)
                throw new ArgumentNullException("title");
            if (content == null)
                throw new ArgumentNullException("content");

            Path = path;
            Title = title;
            Content = content;
        }
    }
}
