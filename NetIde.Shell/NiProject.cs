using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public abstract class NiProject : NiHierarchy, INiProject
    {
        public virtual HResult AddItem(INiHierarchy location, string file)
        {
            throw new NotImplementedException();
        }

        public virtual HResult OpenItem(INiHierarchy hier, out INiWindowFrame windowFrame)
        {
            windowFrame = null;

            try
            {
                if ((NiHierarchyType?)hier.GetPropertyEx(NiHierarchyProperty.ItemType) == NiHierarchyType.File)
                {
                    string fileName;
                    ErrorUtil.ThrowOnFailure(((INiProjectItem)hier).GetFileName(out fileName));

                    return ((INiOpenDocumentManager)GetService(typeof(INiOpenDocumentManager))).OpenStandardEditor(
                        null,
                        fileName,
                        hier,
                        this,
                        out windowFrame
                    );
                }

                return HResult.False;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }
        public virtual HResult RemoveItem(INiHierarchy hier)
        {
            throw new NotImplementedException();
        }
    }
}
