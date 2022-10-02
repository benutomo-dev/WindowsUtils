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
