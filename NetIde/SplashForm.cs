using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NetIde
{
    internal partial class SplashForm : Form
    {
        private SplashForm(string fileName)
        {
            InitializeComponent();

            var image = Image.FromFile(fileName);

            Size = image.Size;

            _pictureBox.Image = image;
        }

        public static IDisposable ShowSplashForm(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            // The splash image is shown in a different thread to keep the
            // splash form responsive.

            IDisposable finalizer = null;

            using (var @event = new ManualResetEvent(false))
            {
                var thread = new Thread(() =>
                {
                    var applicationContext = new ApplicationContext(fileName);

                    finalizer = applicationContext.GetFinalizer();

                    @event.Set();

                    Application.Run(applicationContext);
                });

                thread.SetApartmentState(ApartmentState.STA);
                thread.IsBackground = true;

                thread.Start();

                // Wait for the thread to make the finalizer available.

                @event.WaitOne();
            }

            return finalizer;
        }

        private class ApplicationContext : System.Windows.Forms.ApplicationContext
        {
            private readonly SplashForm _form;

            public ApplicationContext(string fileName)
            {
                _form = new SplashForm(fileName);

                _form.Show();

                _form.Disposed += (s, e) => ExitThread();
            }

            public IDisposable GetFinalizer()
            {
                return new Finalizer(_form);
            }

            private class Finalizer : IDisposable
            {
                private SplashForm _form;
                private bool _disposed;

                public Finalizer(SplashForm form)
                {
                    _form = form;
                }

                public void Dispose()
                {
                    if (!_disposed)
                    {
                        if (_form != null)
                        {
                            _form.Invoke(new Action(_form.Dispose));
                            _form = null;
                        }

                        _disposed = true;
                    }
                }
            }
        }
    }
}
