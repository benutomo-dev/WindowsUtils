using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsControls
{
    public enum TaskDialogDefaultCommonButton
    {
        Default = 0,
        Ok = TASKDIALOG_COMMAND_ID.IDOK,
        Yes = TASKDIALOG_COMMAND_ID.IDYES,
        No = TASKDIALOG_COMMAND_ID.IDNO,
        Cancel = TASKDIALOG_COMMAND_ID.IDCANCEL,
        Retry = TASKDIALOG_COMMAND_ID.IDRETRY,
        Close = TASKDIALOG_COMMAND_ID.IDCLOSE,
    }
}
