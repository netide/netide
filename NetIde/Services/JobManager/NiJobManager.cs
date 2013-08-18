using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NetIde.Services.Shell;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util.Forms;

namespace NetIde.Services.JobManager
{
    internal class NiJobManager : ServiceBase, INiJobManager
    {
        private readonly object _syncRoot = new object();
        private readonly AutoResetEvent _event = new AutoResetEvent(false);
        private readonly Queue<NiJob> _queue = new Queue<NiJob>();
        private NiJob _currentJob;
        private readonly NiShell _shell;

        public event JobEventHandler ProgressChanged;

        private void OnProgressChanged(JobEventArgs e)
        {
            var ev = ProgressChanged;
            if (ev != null)
                ev(null, e);
        }

        public event JobEventHandler JobAdded;

        private void OnJobAdded(JobEventArgs e)
        {
            var ev = JobAdded;
            if (ev != null)
                ev(null, e);
        }

        public event JobEventHandler JobRemoved;

        private void OnJobRemoved(JobEventArgs e)
        {
            var ev = JobRemoved;
            if (ev != null)
                ev(null, e);
        }

        public NiJobManager(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _shell = (NiShell)GetService(typeof(INiShell));
        }

        public IList<NiJob> GetAllJobs()
        {
            NiJob[] result;

            lock (_queue)
            {
                result = new NiJob[_queue.Count];

                _queue.CopyTo(result, 0);
            }

            return result;
        }

        public HResult CreateJob(INiJobHandler handler, out INiJob job)
        {
            job = null;

            try
            {
                job = new NiJob(this, handler);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult Enqueue(params INiJob[] jobs)
        {
            try
            {
                if (jobs == null)
                    throw new ArgumentNullException("jobs");

                if (jobs.Length == 0)
                    return HResult.False;

                lock (_syncRoot)
                {
                    bool wasEmpty = _queue.Count == 0;

                    foreach (var job in jobs)
                    {
                        _queue.Enqueue((NiJob)job);

                        OnJobAdded(new JobEventArgs((NiJob)job));
                    }

                    if (wasEmpty)
                    {
                        OnProgressChanged(new JobEventArgs((NiJob)jobs[0]));

                        ThreadPool.QueueUserWorkItem(ProcessJobs);
                    }
                }

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private void ProcessJobs(object state)
        {
            lock (_syncRoot)
            {
                if (_queue.Count == 0)
                    return;

                _currentJob = _queue.Peek();
            }

            while (true)
            {
                _currentJob.RaisePerform();

                _event.Set();

                lock (_syncRoot)
                {
                    var removedJob = _queue.Dequeue();

                    OnJobRemoved(new JobEventArgs(removedJob));

                    if (_queue.Count == 0)
                    {
                        OnProgressChanged(new JobEventArgs(null));

                        break;
                    }

                    _currentJob = _queue.Peek();
                }
            }

            lock (_syncRoot)
            {
                _currentJob = null;
            }
        }

        public HResult WaitForJob(INiJob job)
        {
            try
            {
                if (job == null)
                    throw new ArgumentNullException("job");

                Cursor.Current = Cursors.WaitCursor;

                while (true)
                {
                    lock (job)
                    {
                        if (_currentJob != null)
                            OnProgressChanged(new JobEventArgs(_currentJob));

                        if (job.Completed)
                            return HResult.OK;
                    }

                    _event.WaitOne();
                }
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult WaitForAll()
        {
            return WaitForAll(false);
        }

        public HResult WaitForAll(bool showDialog)
        {
            try
            {
                if (showDialog)
                {
                    using (var form = new JobForm())
                    {
                        form.Site = new SiteProxy(this);

                        form.ShowDialog(_shell.GetActiveWindow());
                    }

                    return HResult.OK;
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;

                    while (true)
                    {
                        lock (_syncRoot)
                        {
                            if (_currentJob != null)
                                OnProgressChanged(new JobEventArgs(_currentJob));

                            if (_queue.Count == 0)
                                return HResult.OK;
                        }

                        _event.WaitOne();
                    }
                }
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public void ReportProgressChanged(NiJob job)
        {
            bool isCurrent;

            lock (_syncRoot)
            {
                isCurrent = ReferenceEquals(job, _currentJob);
            }

            if (isCurrent)
                OnProgressChanged(new JobEventArgs(job));
        }
    }
}
