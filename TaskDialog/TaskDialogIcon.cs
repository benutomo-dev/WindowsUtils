using System;
using System.Runtime.CompilerServices;

namespace WindowsControls
{
    public enum TaskDialogFallbackIcon : uint
    {
        None       = 0,

        Warning    = TASKDIALOG_ICON.TD_WARNING_ICON,
        Error      = TASKDIALOG_ICON.TD_ERROR_ICON,
        Infomation = TASKDIALOG_ICON.TD_INFORMATION_ICON,
        Shield     = TASKDIALOG_ICON.TD_SHIELD_ICON,

        Exception = 1,
    }

    public struct TaskDialogIcon : IEquatable<TaskDialogIcon>
    {
        public readonly static TaskDialogIcon None       = new TaskDialogIcon(false, false, IntPtr.Zero);
        public readonly static TaskDialogIcon Warning    = new TaskDialogIcon(false, false, (IntPtr)TASKDIALOG_ICON.TD_WARNING_ICON);
        public readonly static TaskDialogIcon Error      = new TaskDialogIcon(false, false, (IntPtr)TASKDIALOG_ICON.TD_ERROR_ICON);
        public readonly static TaskDialogIcon Infomation = new TaskDialogIcon(false, false, (IntPtr)TASKDIALOG_ICON.TD_INFORMATION_ICON);
        public readonly static TaskDialogIcon Shield     = new TaskDialogIcon(false, false, (IntPtr)TASKDIALOG_ICON.TD_SHIELD_ICON);

        public readonly static TaskDialogIcon ShieldBlue      = new TaskDialogIcon(false, true, (IntPtr)TASKDIALOG_ICON_WIN8.TD_SHIELDBLUE_ICON);
        public readonly static TaskDialogIcon SecurityWarning = new TaskDialogIcon(false, true, (IntPtr)TASKDIALOG_ICON_WIN8.TD_SECURITYWARNING_ICON);
        public readonly static TaskDialogIcon SecurityError   = new TaskDialogIcon(false, true, (IntPtr)TASKDIALOG_ICON_WIN8.TD_SECURITYERROR_ICON);
        public readonly static TaskDialogIcon SecuritySucess  = new TaskDialogIcon(false, true, (IntPtr)TASKDIALOG_ICON_WIN8.TD_SECURITYSUCCESS_ICON);
        public readonly static TaskDialogIcon ShieldGray      = new TaskDialogIcon(false, true, (IntPtr)TASKDIALOG_ICON_WIN8.TD_SHIELDGRAY_ICON);
        
        internal readonly bool hIconFlagState;
        internal readonly bool win8Icon;
        internal readonly IntPtr value;
        
        internal TaskDialogIcon(bool hIconFlagState, bool win8Icon, IntPtr value)
        {
            this.hIconFlagState = hIconFlagState;
            this.win8Icon = win8Icon;
            this.value = value;
        }

        public static TaskDialogIcon FromHIcon(IntPtr hIcon)
        {
            return new TaskDialogIcon(true, false, hIcon);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return (int)value ^ (int)(hIconFlagState ? 0x80000000: 0x00000000);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (obj is TaskDialogIcon taskDialogIcon)
            {
                return Equals(taskDialogIcon);
            }
            else
            {
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(TaskDialogIcon other)
        {
            return other != null && hIconFlagState == other.hIconFlagState && value == other.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(TaskDialogIcon a, TaskDialogIcon b) => a.Equals(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(TaskDialogIcon a, TaskDialogIcon b) => !(a == b);
    }
}
