using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetIde.VisualStudio.Wizard
{
    internal class WizardConfiguration
    {
        public Dictionary<string, string> ReplacementsDictionary { get; private set; }
        public byte[] MainIcon { get; set; }
        public string KeyFile { get; set; }
        public bool GenerateKeyFile { get; set; }

        public WizardConfiguration(Dictionary<string, string> replacementsDictionary)
        {
            if (replacementsDictionary == null)
                throw new ArgumentNullException("replacementsDictionary");

            ReplacementsDictionary = replacementsDictionary;
            GenerateKeyFile = true;

            string resourceName = GetType().Namespace + ".Resources.mainicon.ico";

            using (var source = GetType().Assembly.GetManifestResourceStream(resourceName))
            using (var target = new MemoryStream())
            {
                source.CopyTo(target);

                MainIcon = target.ToArray();
            }
        }
    }
}
