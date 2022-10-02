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
