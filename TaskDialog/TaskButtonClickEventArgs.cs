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
