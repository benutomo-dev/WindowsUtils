using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsControls
{
    public class TaskDialogEventArgs : EventArgs
    {
        public ActiveTaskDialog ActiveTaskDialog { get; }

        public TaskDialogEventArgs(ActiveTaskDialog activeTaskDialog)
        {
            ActiveTaskDialog = activeTaskDialog;
        }
    }
}
