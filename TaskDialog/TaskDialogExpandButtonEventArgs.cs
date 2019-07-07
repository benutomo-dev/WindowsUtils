﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
