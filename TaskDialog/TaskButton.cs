using System;

namespace WindowsControls
{
    public class TaskButton
    {
        public event EventHandler<TaskButtonClickEventArgs>? Click;

        public static readonly ulong GlobalIdStart = 0x1000;

        internal static ulong GlobalIdSeed = 0x1000;

        public ulong GlobalId { get; }

        public string Text { get; }


        public TaskButton(string text)
            : this(GlobalIdSeed++, text)
        {
        }

        private TaskButton(ulong id, string text)
        {
            GlobalId = id;
            Text = text;
        }

        public static TaskButton CreateCustumCommonButton(TaskDialogCommonButton button, string text)
        {
            return new TaskButton((ulong)button, text);
        }


        internal bool OnClick(ActiveTaskDialog activeTaskDialog)
        {
            TaskButtonClickEventArgs eventArgs = new TaskButtonClickEventArgs(activeTaskDialog, false);
            Click?.Invoke(this, eventArgs);
            return eventArgs.CancelDialogClose;
        }
    }
}
