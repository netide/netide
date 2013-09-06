using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.TransformResources;
using NetIde.Xml.BuildConfiguration;

namespace NetIde.BuildTasks
{
    partial class NetIdeTask
    {
        private void ExecuteTransformResources(BuildConfiguration configuration)
        {
            foreach (var transformResource in configuration.TransformResources)
            {
                try
                {
                    var transformer = new ResourceTransformer(
                        TranslatePath(transformResource.Source),
                        TranslatePath(transformResource.Target)
                    );

                    transformer.Transform();
                }
                catch (Exception ex)
                {
                    Log.LogErrorFromException(ex);
                }
            }
        }
    }
}
