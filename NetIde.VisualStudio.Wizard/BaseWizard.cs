using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;

namespace NetIde.VisualStudio.Wizard
{
    public abstract class BaseWizard : IWizard
    {
        public virtual void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public virtual void ProjectFinishedGenerating(Project project)
        {
        }

        public virtual void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public virtual void RunFinished()
        {
        }

        public virtual void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
        }

        public virtual bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
