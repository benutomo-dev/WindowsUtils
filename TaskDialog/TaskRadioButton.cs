using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsControls
{
    public class TaskRadioButton
    {
		public event EventHandler<TaskDialogEventArgs>? Click;

        public ulong GlobalId { get; }

        public string Text { get; }


        public TaskRadioButton(string text)
            : this(TaskButton.GlobalIdSeed++, text)
        {
        }


        private TaskRadioButton(ulong id, string text)
        {
            GlobalId = id;
            Text = text;
        }

        internal void OnClick(ActiveTaskDialog activeTaskDialog)
        {
            TaskDialogEventArgs eventArgs = new TaskDialogEventArgs(activeTaskDialog);
            Click?.Invoke(this, eventArgs);
        }
    }
}
