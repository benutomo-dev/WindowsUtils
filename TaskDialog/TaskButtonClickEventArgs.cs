using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsControls
{
    public class TaskButtonClickEventArgs : TaskDialogEventArgs
    {
        public bool CancelDialogClose { get; set; }

        public TaskButtonClickEventArgs(ActiveTaskDialog activeTaskDialog, bool dialogClose)
            : base(activeTaskDialog)
        {
            CancelDialogClose = dialogClose;
        }
    }
}
