using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.JobManager
{
    internal class NiJob : ServiceObject, INiJob
    {
        private readonly NiJobManager _jobManager;
        private string _currentStatus;
        private double? _progress;
        private bool _cancelled;

        public Exception Exception { get; private set; }
        public bool Completed { get; private set; }
        public bool Running { get; private set; }
        public bool Success { get; private set; }
        public INiJobHandler Handler { get; private set; }

        public string CurrentStatus
        {
            get { return _currentStatus; }
            set
            {
                if (_currentStatus != value)
                {
                    _currentStatus = value;
                    _jobManager.ReportProgressChanged(this);
                }
            }
        }

        public double? Progress
        {
            get { return _progress; }
            set
            {
                if (value.HasValue)
                {
                    if (double.IsNaN(value.Value))
                        value = 0;
                    else
                        value = Math.Min(Math.Max(value.Value, 0), 1);
                }

                if (_progress != value)
                {
                    _progress = value;
                    _jobManager.ReportProgressChanged(this);
                }
            }
        }

        public bool Cancelled
        {
            get { return _cancelled; }
            set
            {
                if (_cancelled != value)
                {
                    _cancelled = value;
                    _jobManager.ReportProgressChanged(this);
                }
            }
        }

        public NiJob(NiJobManager jobManager, INiJobHandler handler)
        {
            if (jobManager == null)
                throw new ArgumentNullException("jobManager");
            if (handler == null)
                throw new ArgumentNullException("handler");

            _jobManager = jobManager;
            Handler = handler;
        }

        public void RaisePerform()
        {
            Running = true;

            try
            {
                ErrorUtil.ThrowOnFailure(Handler.Perform(this));

                Success = true;
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
            finally
            {
                Running = false;
                Completed = true;
            }
        }
    }
}
