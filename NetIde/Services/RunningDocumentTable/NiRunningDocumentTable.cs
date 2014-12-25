using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NetIde.Project.Interop;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.RunningDocumentTable
{
    internal class NiRunningDocumentTable : ServiceBase, INiRunningDocumentTable
    {
        private readonly Dictionary<int, Registration> _registrations = new Dictionary<int, Registration>();
        private int _lastCookie;

        public bool HaveOpenDocuments
        {
            get { return _registrations.Count > 0; }
        }

        public NiRunningDocumentTable(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public HResult GetDocumentIterator(out INiIterator<int> iterator)
        {
            iterator = null;

            try
            {
                iterator = new Iterator(_registrations.Keys.GetEnumerator());

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult Register(string document, INiHierarchy hier, INiPersistDocData docData, out int cookie)
        {
            cookie = -1;

            try
            {
                if (document == null)
                    throw new ArgumentNullException("document");
                if (docData == null)
                    throw new ArgumentNullException("docData");

                cookie = Interlocked.Increment(ref _lastCookie);

                _registrations.Add(cookie, new Registration(document, hier, docData));

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult Unregister(int cookie)
        {
            try
            {
                if (!_registrations.Remove(cookie))
                    throw new ArgumentOutOfRangeException("cookie", NeutralResources.DocumentNotRegistered);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetDocumentInfo(int cookie, out string document, out INiHierarchy hier, out INiPersistDocData docData)
        {
            document = null;
            hier = null;
            docData = null;

            try
            {
                Registration registration;
                if (!_registrations.TryGetValue(cookie, out registration))
                    throw new ArgumentOutOfRangeException("cookie", NeutralResources.DocumentNotRegistered);

                document = registration.Document;
                hier = registration.Item;
                docData = registration.DocData;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private class Registration
        {
            public string Document { get; private set; }
            public INiHierarchy Item { get; private set; }
            public INiPersistDocData DocData { get; private set; }

            public Registration(string document, INiHierarchy item, INiPersistDocData docData)
            {
                Document = document;
                Item = item;
                DocData = docData;
            }
        }

        private class Iterator : NiIterator<int>
        {
            public Iterator(IEnumerator<int> cookies)
                : base(cookies)
            {
            }
        }
    }
}
