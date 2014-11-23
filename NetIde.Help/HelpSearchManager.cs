using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using HtmlAgilityPack;
using ICSharpCode.SharpZipLib.Zip;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;

namespace NetIde.Help
{
    public class HelpSearchManager : IDisposable
    {
        private const string VersionFileName = "help-version.txt";
        private const string PathField = "path";
        private const string TitleField = "title";
        private const string ContentField = "content";
        private const Version LuceneVersion = Version.LUCENE_30;
        private const int HitsLimit = 25;

        private readonly object _syncRoot = new object();
        private readonly HelpManager _manager;
        private readonly string _workDirectory;
        private bool _disposed;
        private Directory _directory;
        private StandardAnalyzer _analyzer;
        private IndexSearcher _index;

        public HelpSearchManager(HelpManager manager, string workDirectory)
        {
            if (manager == null)
                throw new ArgumentNullException("manager");
            if (workDirectory == null)
                throw new ArgumentNullException("workDirectory");

            _manager = manager;
            _workDirectory = workDirectory;
        }

        public IList<HelpSearchResult> Search(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            EnsureLoaded();

            var queryParser = new MultiFieldQueryParser(
                LuceneVersion,
                new [] { TitleField, ContentField },
                _analyzer
            );

            queryParser.DefaultOperator = QueryParser.Operator.OR;

            var query =  queryParser.Parse(text);
            var hits = _index.Search(query, HitsLimit);
            var result = new List<HelpSearchResult>();

            foreach (var hit in hits.ScoreDocs)
            {
                var document = _index.Doc(hit.Doc);

                string path = null;
                string title = null;
                string content = null;

                foreach (var field in document.GetFields())
                {
                    switch (field.Name)
                    {
                        case PathField: path = field.StringValue; break;
                        case TitleField: title = field.StringValue; break;
                        case ContentField: content = field.StringValue; break;
                    }
                }

                result.Add(new HelpSearchResult(path, title, content));
            }

            return result;
        }

        private void EnsureLoaded()
        {
            lock (_syncRoot)
            {
                if (_directory != null)
                    return;

                _directory = FSDirectory.Open(_workDirectory);
                _analyzer = new StandardAnalyzer(LuceneVersion);

                if (!IsUpToDate())
                    RebuildDatabase();

                _index = new IndexSearcher(_directory);
            }
        }

        private bool IsUpToDate()
        {
            // Create a hash based on the registrations. The hash consists of
            // the name of the root and the last modified time of the source.
            // Registrations are ordered by root name to ensure a stable hash.

            var registrations = new List<HelpRegistration>(_manager.Registrations);

            registrations.Sort((a, b) => String.Compare(a.Root, b.Root, StringComparison.Ordinal));

            var sb = new StringBuilder();

            foreach (var registration in registrations)
            {
                sb
                    .Append(registration.Root)
                    .Append(':')
                    .Append(new FileInfo(registration.Source).LastWriteTime.ToString("o"))
                    .Append('\n');
            }

            var hash = ComputeHash(sb.ToString());

            // We have a file in the Lucene directory that stores the hash.
            // Verify whether the file exists and, if so, the contents is
            // equal to our hash.

            string fileName = Path.Combine(_workDirectory, VersionFileName);

            bool outOfDate = true;

            if (File.Exists(fileName))
            {
                outOfDate = !String.Equals(
                    hash,
                    File.ReadAllText(fileName).Trim(),
                    StringComparison.OrdinalIgnoreCase
                );
            }

            // Update the version file if we're out of date.

            if (outOfDate)
                File.WriteAllText(fileName, hash);

            return !outOfDate;
        }

        private static string ComputeHash(string text)
        {
            byte[] hashBytes = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(text));

            var sb = new StringBuilder();

            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        private void RebuildDatabase()
        {
            using (var writer = new IndexWriter(_directory, _analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var registration in _manager.Registrations)
                {
                    RebuildDatabase(writer, registration);
                }

                writer.Optimize();
            }
        }

        private void RebuildDatabase(IndexWriter writer, HelpRegistration registration)
        {
            using (var source = HelpSource.FromSource(registration.Source))
            {
                foreach (var entry in source)
                {
                    if (!entry.Name.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
                        continue;

                    Parsed parsed;

                    using (var stream = entry.GetInputStream())
                    {
                        parsed = ParseDocument(stream);
                    }

                    string path = String.Format("/{0}/{1}", registration.Root, entry.Name);
                    var title = parsed.Title;
                    if (title == null)
                    {
                        int index = entry.Name.LastIndexOf('/');
                        if (index == -1)
                            title = entry.Name;
                        else
                            title = entry.Name.Substring(index + 1);
                    }

                    var document = new Document();

                    document.Add(new Field(PathField, path, Field.Store.YES, Field.Index.NO));
                    document.Add(new Field(TitleField, title, Field.Store.YES, Field.Index.ANALYZED));
                    document.Add(new Field(ContentField, parsed.GetContent(), Field.Store.YES, Field.Index.ANALYZED));

                    writer.AddDocument(document);
                }
            }
        }

        private Parsed ParseDocument(Stream stream)
        {
            try
            {
                var document = new HtmlDocument();

                document.Load(stream);

                var parsed = new Parsed();

                ParseHtml(document.DocumentNode, parsed);

                return parsed;
            }
            catch
            {
                return null;
            }
        }

        private void ParseHtml(HtmlNode node, Parsed parsed)
        {
            switch (node.NodeType)
            {
                case HtmlNodeType.Document:
                    ParseHtmlContent(node, parsed);
                    break;

                case HtmlNodeType.Text:
                    string html = ((HtmlTextNode)node).Text;

                    switch (node.ParentNode.Name)
                    {
                        case "script":
                        case "style":
                            // Ignore script and style blocks.
                            return;

                        case "title":
                            // The title is handled special.
                            parsed.Title = HtmlEntity.DeEntitize(html.Trim());
                            return;
                    }

                    // Is it in fact a special closing node output as text?

                    if (!HtmlNode.IsOverlappedClosingElement(html))
                        parsed.Append(HtmlEntity.DeEntitize(html)).Append(' ');
                    break;

                case HtmlNodeType.Element:
                    ParseHtmlContent(node, parsed);
                    break;
            }
        }

        private void ParseHtmlContent(HtmlNode node, Parsed parsed)
        {
            if (!node.HasChildNodes)
                return;

            foreach (var subnode in node.ChildNodes)
            {
                ParseHtml(subnode, parsed);
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_index != null)
                {
                    _index.Dispose();
                    _index = null;
                }

                if (_analyzer != null)
                {
                    _analyzer.Dispose();
                    _analyzer = null;
                }

                if (_directory != null)
                {
                    _directory.Dispose();
                    _directory = null;
                }

                _disposed = true;
            }
        }

        private class Parsed
        {
            private readonly StringBuilder _sb = new StringBuilder();
            private bool _hadSpace;

            public string Title { get; set; }

            public Parsed Append(string text)
            {
                foreach (char c in text)
                {
                    Append(c);
                }

                return this;
            }

            public Parsed Append(char c)
            {
                if (Char.IsWhiteSpace(c))
                {
                    _hadSpace = true;
                }
                else
                {
                    if (_sb.Length > 0 && _hadSpace)
                        _sb.Append(' ');

                    _hadSpace = false;

                    _sb.Append(c);
                }

                return this;
            }

            public string GetContent()
            {
                return _sb.ToString();
            }
        }
    }
}
