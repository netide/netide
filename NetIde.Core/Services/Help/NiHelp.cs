using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Actions;
using NetIde.Help;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util;

namespace NetIde.Core.Services.Help
{
    internal class NiHelp : ServiceBase, INiHelp
    {
        private const int SearchResultLeading = 100;
        private const int SearchResultMaxLength = 300;
        private static readonly Dictionary<string, byte[]> Resources = LoadResources();

        private static Dictionary<string, byte[]> LoadResources()
        {
            var resources = new Dictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase);

            LoadResource(resources, "help.js");
            LoadResource(resources, "help.css");

            return resources;
        }

        private static void LoadResource(Dictionary<string, byte[]> resources, string name)
        {
            string resource = typeof(NiHelp).Namespace + "." + name;

            using (var stream = typeof(NiHelp).Assembly.GetManifestResourceStream(resource))
            {
                var bytes = new byte[stream.Length];

                stream.Read(bytes, 0, bytes.Length);

                resources.Add("/" + name, bytes);
            }
        } 

        private HelpServer _server;
        private HelpForm _form;
        private readonly List<HelpRegistration> _registrations = new List<HelpRegistration>();
        private bool _disposed;
        private HelpSearchManager _searchManager;

        public NiHelp(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            // We register a priority command handler to catch the exit command.
            // The help window is a modeless dialog. If it has focus and Alt+F4
            // is pressed, the exit command would be send to the core package
            // and it would close the application. This priority command
            // target intercepts the exit command, checks whether the form is active
            // and, if so, closes the help form and suppresses further processing.

            int cookie;
            ((INiRegisterPriorityCommandTarget)GetService(typeof(INiRegisterPriorityCommandTarget))).RegisterPriorityCommandTarget(new ApplicationExitInterceptor(this), out cookie);
        }

        private HelpServer GetServer()
        {
            if (_server == null)
            {
                var env = (INiEnv)GetService(typeof(INiEnv));

                string workDirectory = Path.Combine(env.FileSystemRoot, "Cache", "Help");

                Directory.CreateDirectory(workDirectory);
                
                _server = new HelpServer();
                _server.Manager.Resolve += Manager_Resolve;
                _server.Manager.Registrations.AddRange(_registrations);

                _searchManager = new HelpSearchManager(_server.Manager, workDirectory);
            }

            return _server;
        }

        void Manager_Resolve(object sender, HelpResolveEventArgs e)
        {
            string path = e.Url;
            int index = path.IndexOf('?');
            if (index != -1)
                path = path.Substring(0, index);


            byte[] bytes;
            if (Resources.TryGetValue(path, out bytes))
                e.Stream = new MemoryStream(bytes);
            else if (path == "/find")
                e.Stream = PerformFind(e.Url);
            else if (path == "/")
                e.Stream = PerformRoot();
        }

        private Stream PerformRoot()
        {
            var writer = new HtmlWriter();

            FormatHome(writer);

            return new MemoryStream(Encoding.UTF8.GetBytes(writer.ToString()));
        }

        private void FormatHome(HtmlWriter writer)
        {
            writer.OpenTag("html");

            FormatHead(writer);
            FormatHomeBody(writer);

            writer.CloseTag();
        }

        private void FormatHomeBody(HtmlWriter writer)
        {
            writer.OpenTag("body").Attribute("class", "search-results");

            writer.OpenTag("h1").Text(Labels.NeedAssistance).CloseTag();

            writer.OpenTag("p").Text(Labels.UseSearchBox).CloseTag();
            writer.OpenTag("p").Text(Labels.UseHelpButton).CloseTag();

            writer.CloseTag();
        }

        private Stream PerformFind(string url)
        {
            int index = url.IndexOf('?');
            if (index == -1)
                return null;

            string queryString = url.Substring(index + 1);
            string query;
            if (!ParseQueryString(queryString).TryGetValue("q", out query))
                return null;

            var hits = _searchManager.Search(query);

            var writer = new HtmlWriter();

            FormatSearchResults(writer, query, hits);

            return new MemoryStream(Encoding.UTF8.GetBytes(writer.ToString()));
        }

        private void FormatSearchResults(HtmlWriter writer, string query, IList<HelpSearchResult> hits)
        {
            writer.OpenTag("html");

            FormatHead(writer);
            FormatSearchResultsBody(writer, query, hits);

            writer.CloseTag();
        }

        private static void FormatHead(HtmlWriter writer)
        {
            writer.OpenTag("head");

            writer.Tag("title", Labels.SearchResults);
            writer.OpenTag("link").Attribute("type", "text/css").Attribute("rel", "stylesheet").Attribute("href", "/help.css").CloseTag();
            writer.OpenTag("script").Attribute("src", "/help.js").Attribute("type", "text/javascript").Text("").CloseTag();

            writer.CloseTag();
        }

        private void FormatSearchResultsBody(HtmlWriter writer, string query, IList<HelpSearchResult> hits)
        {
            writer.OpenTag("body").Attribute("class", "search-results");

            writer
                .OpenTag("h1").Text(Labels.SearchResultsFor).Text(" ")
                .OpenTag("span").Attribute("class", "search-query").Text(query).CloseTag()
                .CloseTag();

            var queryWords = ParseQuery(query);

            foreach (var hit in hits)
            {
                FormatHit(writer, queryWords, hit);
            }

            writer.CloseTag();
        }

        private HashSet<string> ParseQuery(string query)
        {
            var words = new HashSet<string>();
            var sb = new StringBuilder();

            foreach (char c in query)
            {
                if (Char.IsLetterOrDigit(c))
                {
                    sb.Append(c);
                }
                else if (sb.Length > 0)
                {
                    words.Add(sb.ToString().ToLower());
                    sb.Clear();
                }
            }

            if (sb.Length > 0)
                words.Add(sb.ToString());

            return words;
        }

        private void FormatHit(HtmlWriter writer, HashSet<string> query, HelpSearchResult hit)
        {
            writer.OpenTag("p").Attribute("class", "search-result");

            writer
                .OpenTag("div").Attribute("class", "search-title")
                .OpenTag("a").Attribute("href", hit.Path);
            
            FormatEmphasis(writer, query, hit.Title);
            
            writer
                .CloseTag()
                .CloseTag();

            writer.OpenTag("div").Attribute("class", "search-content");

            FormatEmphasis(writer, query, hit.Content);

            writer.CloseTag();

            writer.CloseTag();
        }

        private void FormatEmphasis(HtmlWriter writer, HashSet<string> query, string text)
        {
            var words = ParseContent(text);
            int start = -1;
            int end = -1;

            for (int i = 0; i < words.Count; i++)
            {
                if (!query.Contains(words[i].ToLower()))
                    continue;

                int leading = SearchResultLeading;
                start = 0;

                for (int j = i - 1; j >= 0; j--)
                {
                    leading -= words[j].Length;
                    if (leading < 0)
                    {
                        start = j + 1;
                        break;
                    }
                }

                end = FindEnd(start, words);

                break;
            }

            if (start == -1)
            {
                start = 0;
                end = FindEnd(start, words);
            }

            bool wasContained = false;

            for (int j = start; j < end; j++)
            {
                bool isContained = query.Contains(words[j].ToLower());

                if (isContained != wasContained)
                {
                    if (isContained)
                        writer.OpenTag("b");
                    else
                        writer.CloseTag();

                    wasContained = isContained;
                }

                writer.Text(words[j]);
            }

            if (wasContained)
                writer.CloseTag();
        }

        private static int FindEnd(int start, List<string> words)
        {
            int length = SearchResultMaxLength;

            for (int j = start; j < words.Count; j++)
            {
                length -= words[j].Length;
                if (length < 0)
                {
                    return j;
                }
            }

            return words.Count;
        }

        private List<string> ParseContent(string content)
        {
            var sb = new StringBuilder();
            bool wasLetterOrDigit = false;
            var words = new List<string>();

            foreach (char c in content)
            {
                bool isLetterOrDigit = Char.IsLetterOrDigit(c);

                if (isLetterOrDigit != wasLetterOrDigit && sb.Length > 0)
                {
                    words.Add(sb.ToString());
                    sb.Clear();
                }

                sb.Append(c);

                wasLetterOrDigit = isLetterOrDigit;
            }

            if (sb.Length > 0)
                words.Add(sb.ToString());

            return words;
        }

        private Dictionary<string, string> ParseQueryString(string queryString)
        {
            var result = new Dictionary<string, string>();

            if (queryString != null)
            {
                foreach (string part in queryString.Split('&'))
                {
                    int index = part.IndexOf('=');
                    if (index == -1)
                    {
                        result.Add(Uri.UnescapeDataString(part), "");
                    }
                    else
                    {
                        result.Add(
                            Uri.UnescapeDataString(part.Substring(0, index)),
                            Uri.UnescapeDataString(part.Substring(index + 1))
                        );
                    }
                }
            }

            return result;
        }

        private HelpForm GetForm()
        {
            if (_form == null)
            {
                _form = new HelpForm();
                _form.Find += _form_Find;
                _form.Disposed += (s, e) => _form = null;
                _form.Home = String.Format("http://localhost:{0}/", GetServer().EndPoint.Port);
            }

            return _form;
        }

        void _form_Find(object sender, HelpFindEventArgs e)
        {
            string text = e.Text.Trim();

            if (text.Length == 0)
                return;

            _form.NavigateTo(String.Format(
                "http://localhost:{0}/find?q={1}",
                GetServer().EndPoint.Port ,
                Uri.EscapeDataString(text)
            ));
        }

        public HResult Show()
        {
            try
            {
                if (_form == null)
                    GetForm().Show();
                else
                    _form.BringToFront();

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult Hide()
        {
            try
            {
                if (_form != null)
                    _form.Close();

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult Home()
        {
            try
            {
                ErrorUtil.ThrowOnFailure(Show());

                string home = _form.Home;

                if (home != null)
                    _form.NavigateTo(home);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult Navigate(string root, string path)
        {
            try
            {
                if (root == null)
                    throw new ArgumentNullException("root");
                if (path == null)
                    throw new ArgumentNullException("path");

                path = path.TrimStart('/');
                if (path.Length == 0)
                    path = "/";

                ErrorUtil.ThrowOnFailure(Show());

                _form.NavigateTo(String.Format(
                    "http://localhost:{0}/{1}/{2}",
                    GetServer().EndPoint.Port,
                    root,
                    path
                ));

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult Register(string root, string source)
        {
            try
            {
                if (root == null)
                    throw new ArgumentNullException("root");
                if (source == null)
                    throw new ArgumentNullException("source");

                var helpRegistration = new HelpRegistration(root, source);

                _registrations.Add(helpRegistration);

                if (_server != null)
                    _server.Manager.Registrations.Add(helpRegistration);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_form != null)
                {
                    _form.Dispose();
                    _form = null;
                }

                if (_server != null)
                {
                    _server.Dispose();
                    _server = null;
                }

                if (_searchManager != null)
                {
                    _searchManager.Dispose();
                    _searchManager = null;
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        private class ApplicationExitInterceptor : ServiceObject, INiCommandTarget
        {
            private readonly NiHelp _help;

            public ApplicationExitInterceptor(NiHelp help)
            {
                _help = help;
            }

            public HResult QueryStatus(Guid command, out NiCommandStatus status)
            {
                status = 0;
                return HResult.False;
            }

            public HResult Exec(Guid command, object argument, out object result)
            {
                result = null;

                try
                {
                    if (
                        command == Shell.NiResources.File_Exit &&
                        _help._form != null &&
                        _help._form == Form.ActiveForm
                    ) {
                        _help._form.Close();
                        return HResult.OK;
                    }

                    return HResult.False;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }
        }
    }
}
