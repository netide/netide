using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CustomChrome;
using NetIde.Services;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde
{
    partial class MainForm
    {
        private class NiMainWindowChrome : ServiceBase, INiMainWindowChrome
        {
            private readonly MainForm _mainForm;
            private bool _enabled;
            private Color _color = Color.Lime;
            private VisualStudioFormChrome _formChrome;

            public NiMainWindowChrome(MainForm mainForm, IServiceProvider serviceProvider)
                : base(serviceProvider)
            {
                _mainForm = mainForm;
            }

            public HResult SetEnabled(bool enabled)
            {
                try
                {
                    if (enabled != _enabled)
                    {
                        _enabled = enabled;

                        if (_enabled)
                        {
                            _formChrome = new VisualStudioFormChrome();
                            _formChrome.BorderColor = _color;
                            _formChrome.PrimaryColor = _color;
                            _formChrome.ContainerControl = _mainForm;
                        }
                        else
                        {
                            _formChrome.Dispose();
                            _formChrome = null;
                        }
                    }

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetColor(int red, int green, int blue)
            {
                try
                {
                    var color = Color.FromArgb(red, green, blue);

                    if (color != _color)
                    {
                        _color = color;
                        if (_formChrome != null)
                        {
                            _formChrome.PrimaryColor = _color;
                            _formChrome.BorderColor = _color;
                        }
                    }

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }
        }
    }
}
