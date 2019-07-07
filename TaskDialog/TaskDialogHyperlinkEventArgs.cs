using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
