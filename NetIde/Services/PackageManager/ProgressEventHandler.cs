using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Services.PackageManager
{
    internal class ProgressEventArgs : EventArgs
    {
        public string Progress { get; private set; }
        public int CurrentStep { get; private set; }
        public int TotalSteps { get; private set; }

        public ProgressEventArgs(string progress, int currentStep, int totalSteps)
        {
            if (progress == null)
                throw new ArgumentNullException("progress");

            Progress = progress;
            CurrentStep = currentStep;
            TotalSteps = totalSteps;
        }
    }

    internal delegate void ProgressEventHandler(object sender, ProgressEventArgs e);
}
