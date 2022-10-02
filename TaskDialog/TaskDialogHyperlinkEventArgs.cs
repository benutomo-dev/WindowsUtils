namespace WindowsControls
{
    public class TaskDialogHyperlinkEventArgs : TaskDialogEventArgs
    {
        public string Href { get; }

        public TaskDialogHyperlinkEventArgs(ActiveTaskDialog activeTaskDialog, string href)
            : base(activeTaskDialog)
        {
            Href = href;
        }
    }
}
