using System;

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
