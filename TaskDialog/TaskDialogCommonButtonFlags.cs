﻿using System;

namespace WindowsControls
{
    [Flags]
    public enum TaskDialogCommonButtonFlags
    {
        None = 0,
        Ok = TASKDIALOG_COMMON_BUTTON_FLAGS.TDCBF_OK_BUTTON,
        Yes = TASKDIALOG_COMMON_BUTTON_FLAGS.TDCBF_YES_BUTTON,
        No = TASKDIALOG_COMMON_BUTTON_FLAGS.TDCBF_NO_BUTTON,
        Cancel = TASKDIALOG_COMMON_BUTTON_FLAGS.TDCBF_CANCEL_BUTTON,
        Retry = TASKDIALOG_COMMON_BUTTON_FLAGS.TDCBF_RETRY_BUTTON,
        Close = TASKDIALOG_COMMON_BUTTON_FLAGS.TDCBF_CLOSE_BUTTON,
    }
}
