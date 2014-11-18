using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class ResourceManager : IDisposable
    {
        private ImageManager _images = new ImageManager();
        private IconManager _icons = new IconManager();
        private bool _disposed;

        public Bitmap GetImage(IResource resource)
        {
            return _images.GetResource(resource);
        }

        public Icon GetIcon(IResource resource)
        {
            return _icons.GetResource(resource);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_images != null)
                {
                    _images.Dispose();
                    _images = null;
                }

                if (_icons != null)
                {
                    _icons.Dispose();
                    _icons = null;
                }

                _disposed = true;
            }
        }

        private abstract class ConcreteResourceManager<T> : IDisposable
            where T : IDisposable
        {
            private bool _disposed;
            private Dictionary<string, T> _resources = new Dictionary<string, T>();

            public T GetResource(IResource resource)
            {
                if (resource == null)
                    throw new ArgumentNullException("resource");

                string key = resource.Key;
                T result;

                if (!_resources.TryGetValue(key, out result))
                {
                    result = ResolveResource(resource);
                    _resources.Add(key, result);
                }

                return result;
            }

            protected abstract T ResolveResource(IResource resource);

            public void Dispose()
            {
                if (!_disposed)
                {
                    if (_resources != null)
                    {
                        foreach (var image in _resources.Values)
                        {
                            image.Dispose();
                        }

                        _resources = null;
                    }

                    _disposed = true;
                }
            }
        }

        private class ImageManager : ConcreteResourceManager<Bitmap>
        {
            protected override Bitmap ResolveResource(IResource resource)
            {
                return (Bitmap)Image.FromStream(resource.ToStream());
            }
        }

        private class IconManager : ConcreteResourceManager<Icon>
        {
            protected override Icon ResolveResource(IResource resource)
            {
                return new Icon(resource.ToStream());
            }
        }
    }
}
