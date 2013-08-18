using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using log4net;

namespace NetIde.Core.PackageManagement
{
    internal static class PackageImageCache
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PackageImageCache));

        private static readonly object _syncRoot = new object();
        private static readonly Dictionary<string, Image> _cache = new Dictionary<string,Image>();
        private static readonly List<Tuple<string, PictureBox>> _pendingUpdates = new List<Tuple<string,PictureBox>>();

        public static void LoadImage(string url, PictureBox pictureBox)
        {
            if (url == null)
                throw new ArgumentNullException("url");
            if (pictureBox == null)
                throw new ArgumentNullException("pictureBox");

            lock (_syncRoot)
            {
                Image image;

                bool inCache = _cache.TryGetValue(url, out image);

                if (inCache && image != null)
                {
                    pictureBox.Image = image;
                    return;
                }

                _pendingUpdates.Add(Tuple.Create(url, pictureBox));

                if (!inCache)
                {
                    _cache.Add(url, null);

                    ThreadPool.QueueUserWorkItem(p => DownloadImage(url));
                }
            }
        }

        private static void DownloadImage(string url)
        {
            Image image = null;

            try
            {
                byte[] data;

                using (var webClient = new WebClient())
                {
                    data = webClient.DownloadData(url);
                }

                image = Image.FromStream(new MemoryStream(data));
            }
            catch (Exception ex)
            {
                Log.Warn(String.Format("Could not download package icon from '{0}'", url), ex);
            }

            lock (_syncRoot)
            {
                _cache[url] = image ?? NeutralResources.NuGetPackage;

                var pendingUpdates = _pendingUpdates.Where(p => p.Item1 == url).ToArray();

                foreach (var pendingUpdate in pendingUpdates)
                {
                    _pendingUpdates.Remove(pendingUpdate);

                    try
                    {
                        pendingUpdate.Item2.BeginInvoke(new Action(
                            () =>
                            {
                                if (!pendingUpdate.Item2.IsDisposed)
                                    pendingUpdate.Item2.Image = image;
                            }
                        ));
                    }
                    catch (Exception ex)
                    {
                        Log.Warn("Could not load package image", ex);
                    }
                }
            }
        }
    }
}
