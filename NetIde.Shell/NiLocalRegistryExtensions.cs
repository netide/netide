using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public static class NiLocalRegistryExtensions
    {
        public static HResult CreateInstance(this INiLocalRegistry self, Guid guid, IServiceProvider serviceProvider, out object instance)
        {
            instance = null;

            try
            {
                var hr = self.CreateInstance(guid, out instance);
                if (ErrorUtil.Failure(hr))
                    return hr;

                if (serviceProvider != null)
                {
                    var objectWithSite = instance as INiObjectWithSite;
                    if (objectWithSite != null)
                    {
                        hr = objectWithSite.SetSite(serviceProvider);
                        if (ErrorUtil.Failure(hr))
                            return hr;
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
