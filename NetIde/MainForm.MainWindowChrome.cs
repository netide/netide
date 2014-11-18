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

            public VisualStudioFormChrome FormChrome { get; private set; }

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
                            FormChrome = new VisualStudioFormChrome();
                            FormChrome.BorderColor = _color;
                            FormChrome.PrimaryColor = _color;
                            FormChrome.ContainerControl = _mainForm;

                            // Buttons may have been added before the chrome was
                            // available. Trigger the title bar button manager
                            // to actually create the buttons.

                            ((NiTitleBarButtonManager)GetService(typeof(INiTitleBarButtonManager))).RebuildButtons();
                        }
                        else
                        {
                            FormChrome.Dispose();
                            FormChrome = null;
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
                        if (FormChrome != null)
                        {
                            FormChrome.PrimaryColor = _color;
                            FormChrome.BorderColor = _color;
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
