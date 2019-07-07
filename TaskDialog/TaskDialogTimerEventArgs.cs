using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsControls
{
    public class TaskDialogTimerEventArgs : TaskDialogEventArgs
    {
        public uint TickCount { get; }

        public bool ResetTickCount { get; set; }

        public TaskDialogTimerEventArgs(ActiveTaskDialog activeTaskDialog, uint tickCount, bool resetTickCount)
            : base(activeTaskDialog)
        {
            TickCount = tickCount;
            ResetTickCount = resetTickCount;
        }
    }
}
