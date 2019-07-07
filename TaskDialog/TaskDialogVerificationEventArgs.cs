using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsControls
{
    public class TaskDialogVerificationEventArgs : TaskDialogEventArgs
    {
        public bool Checked { get; }

        public TaskDialogVerificationEventArgs(ActiveTaskDialog activeTaskDialog, bool checkedValue)
            : base(activeTaskDialog)
        {
            Checked = checkedValue;
        }
    }
}
