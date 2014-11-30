using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NetIde.Util.Forms
{
    public class TaskDialogButtonCollection : Collection<TaskDialogButton>
    {
        public void Add(int id, string text)
        {
            Add(new TaskDialogButton(id, text));
        }

        internal TaskDialogButton[] ToArray()
        {
            var result = new TaskDialogButton[Count];

            for (int i = 0; i < Count; i++)
            {
                result[i] = this[i];
            }

            return result;
        }
    }
}
