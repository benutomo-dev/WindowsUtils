namespace WindowsControls
{
    public class TaskDialogExpandButtonEventArgs : TaskDialogEventArgs
    {
        public bool Expanded { get; }

        public TaskDialogExpandButtonEventArgs(ActiveTaskDialog activeTaskDialog, bool expanded)
            : base(activeTaskDialog)
        {
            Expanded = expanded;
        }
    }
}
