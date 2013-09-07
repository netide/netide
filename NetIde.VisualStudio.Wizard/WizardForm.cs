using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.VisualStudio.Wizard
{
    internal partial class WizardForm : DialogForm
    {
        private readonly WizardConfiguration _configuration;
        private readonly Dictionary<WizardStep, Type> _stepControls;
        private readonly WizardStep[] _steps;
        private WizardStep _currentStep;
        private readonly string _title;

        private WizardStepControl Control
        {
            get { return _clientArea.Controls.Cast<WizardStepControl>().SingleOrDefault(); }
        }

        public WizardForm(WizardConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            _configuration = configuration;

            _stepControls = LoadStepControls();
            _steps = Enum.GetValues(typeof(WizardStep)).Cast<WizardStep>().ToArray();

            InitializeComponent();

            _title = Text;

            SetCurrentStep(WizardStep.Welcome);
        }

        private void SetCurrentStep(WizardStep wizardStep)
        {
            _clientArea.SuspendLayout();

            var toRemove = Control;

            var control = (WizardStepControl)Activator.CreateInstance(_stepControls[wizardStep]);

            control.Configuration = _configuration;
            control.Dock = DockStyle.Fill;

            _clientArea.Controls.Add(control);

            _clientArea.ResumeLayout();

            _clientArea.Controls.Remove(toRemove);

            _currentStep = wizardStep;

            Text = String.Format(
                "{0} ({1} of {2})",
                _title,
                Array.IndexOf(_steps, _currentStep) + 1,
                _steps.Length
            );

            UpdateButtons();
        }

        private void UpdateButtons()
        {
            int index = Array.IndexOf(_steps, _currentStep);

            bool first = index == 0;
            bool last = index == _steps.Length - 1;

            _previousButton.Enabled = !first;
            _nextButton.Enabled = !last;
            _finishButton.Enabled = last;

            AcceptButton = _nextButton.Enabled ? _nextButton : _finishButton;
        }

        private Dictionary<WizardStep, Type> LoadStepControls()
        {
            return (
                from type in GetType().Assembly.GetTypes()
                let attributes = type.GetCustomAttributes(typeof(WizardStepAttribute), false)
                where attributes.Length == 1
                let attribute = (WizardStepAttribute)attributes[0]
                select new { Type = type, attribute.Step }
            ).ToDictionary(p => p.Step, p => p.Type);
        }

        private void _finishButton_Click(object sender, EventArgs e)
        {
            if (CanNext())
                DialogResult = DialogResult.OK;
        }

        private void _previousButton_Click(object sender, EventArgs e)
        {
            SetCurrentStep(_steps[Array.IndexOf(_steps, _currentStep) - 1]);
        }

        private void _nextButton_Click(object sender, EventArgs e)
        {
            if (CanNext())
                SetCurrentStep(_steps[Array.IndexOf(_steps, _currentStep) + 1]);
        }

        private bool CanNext()
        {
            var control = Control;

            if (control != null && !control.CanNext())
            {
                MessageBox.Show(
                    this,
                    "Please enter all information",
                    Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return false;
            }

            return true;
        }
    }
}
