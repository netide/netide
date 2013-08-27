using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.EditorFactoryRegistry
{
    internal class NiEditorFactoryRegistry : ServiceBase, INiEditorFactoryRegistry
    {
        private readonly Dictionary<Guid, INiEditorFactory> _factories = new Dictionary<Guid, INiEditorFactory>();

        public NiEditorFactoryRegistry(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public HResult RegisterEditorFactory(Guid guid, INiEditorFactory editorFactory)
        {
            try
            {
                if (editorFactory == null)
                    throw new ArgumentNullException("editorFactory");

                if (_factories.ContainsKey(guid))
                    throw new InvalidOperationException(Labels.EditorFactoryAlreadyRegistered);

                _factories.Add(guid, editorFactory);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public bool TryGetEditorFactory(Guid guid, out INiEditorFactory editorFactory)
        {
            return _factories.TryGetValue(guid, out editorFactory);
        }
    }
}
