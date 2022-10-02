using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsControls
{
    enum TASKDIALOG_COMMAND_ID
    {
        IDOK = 1,
        IDCANCEL = 2,
        IDABORT = 3,
        IDRETRY = 4,
        IDIGNORE = 5,
        IDYES = 6,
        IDNO = 7,
        IDCLOSE = 8,
        IDHELP = 9,
        IDTRYAGAIN = 10,
        IDCONTINUE = 11,
    }

    enum TASKDIALOG_COMMON_BUTTON_FLAGS
    {
        TDCBF_OK_BUTTON = 0x0001,
        TDCBF_YES_BUTTON = 0x0002,
        TDCBF_NO_BUTTON = 0x0004,
        TDCBF_CANCEL_BUTTON = 0x0008,
        TDCBF_RETRY_BUTTON = 0x0010,
        TDCBF_CLOSE_BUTTON = 0x0020,
    }

    enum TASKDIALOG_ELEMENTS : uint
    {
        TDE_CONTENT,
        TDE_EXPANDED_INFORMATION,
        TDE_FOOTER,
        TDE_MAIN_INSTRUCTION
    }

    enum TASKDIALOG_ICON_ELEMENTS : uint
    {
        TDIE_ICON_MAIN,
        TDIE_ICON_FOOTER
    }



    enum TASKDIALOG_ICON : uint
    {
        TD_WARNING_ICON = 0xFFFF,
        TD_ERROR_ICON = 0xFFFE,
        TD_INFORMATION_ICON = 0xFFFD,
        TD_SHIELD_ICON = 0xFFFC,
    }

    enum TASKDIALOG_ICON_WIN8 : uint
    {
        TD_SHIELDBLUE_ICON = 0xFFFB,
        TD_SECURITYWARNING_ICON = 0xFFFA,
        TD_SECURITYERROR_ICON = 0xFFF9,
        TD_SECURITYSUCCESS_ICON = 0xFFF8,
        TD_SHIELDGRAY_ICON = 0xFFF7,
    }

    enum TASKDIALOG_FLAGS : uint
    {
        TDF_ENABLE_HYPERLINKS = 0x0001,
        TDF_USE_HICON_MAIN = 0x0002,
        TDF_USE_HICON_FOOTER = 0x0004,
        TDF_ALLOW_DIALOG_CANCELLATION = 0x0008,
        TDF_USE_COMMAND_LINKS = 0x0010,
        TDF_USE_COMMAND_LINKS_NO_ICON = 0x0020,
        TDF_EXPAND_FOOTER_AREA = 0x0040,
        TDF_EXPANDED_BY_DEFAULT = 0x0080,
        TDF_VERIFICATION_FLAG_CHECKED = 0x0100,
        TDF_SHOW_PROGRESS_BAR = 0x0200,
        TDF_SHOW_MARQUEE_PROGRESS_BAR = 0x0400,
        TDF_CALLBACK_TIMER = 0x0800,
        TDF_POSITION_RELATIVE_TO_WINDOW = 0x1000,
        TDF_RTL_LAYOUT = 0x2000,
        TDF_NO_DEFAULT_RADIO_BUTTON = 0x4000,
        TDF_CAN_BE_MINIMIZED = 0x8000,
        TDF_NO_SET_FOREGROUND = 0x00010000,
        TDF_SIZE_TO_CONTENT = 0x01000000
    }

    enum TaskDialogNotification : uint
    {
        TDN_CREATED = 0,
        TDN_NAVIGATED = 1,
        TDN_BUTTON_CLICKED = 2,
        TDN_HYPERLINK_CLICKED = 3,
        TDN_TIMER = 4,
        TDN_DESTROYED = 5,
        TDN_RADIO_BUTTON_CLICKED = 6,
        TDN_DIALOG_CONSTRUCTED = 7,
        TDN_VERIFICATION_CLICKED = 8,
        TDN_HELP = 9,
        TDN_EXPANDO_BUTTON_CLICKED = 10
    }


    enum TASKDIALOG_MESSAGE : uint
    {
        WM_CLOSE = 0x0010,

        //WM_USER = 0x0400,

        /// <summary>
        /// wParam = 0
        /// lParam = address of TASKDIALOGCONFIG
        /// </summary>
        TDM_NAVIGATE_PAGE = 0x0400 + 101,

        /// <summary>
        /// wParam = ButtonID
        /// lParam = 0
        /// </summary>
        TDM_CLICK_BUTTON = 0x0400 + 102,

        /// <summary>
        /// wParam = A BOOL that indicates whether the progress bar should be shown in marquee mode. A value of TRUE turns on marquee mode, and a value of FALSE turns off marquee mode.
        /// lParam = 0
        /// </summary>
        TDM_SET_MARQUEE_PROGRESS_BAR = 0x0400 + 103,

        /// <summary>
        /// wParam = TaskDialogProgressBarState
        /// lParam = 0
        /// </summary>
        TDM_SET_PROGRESS_BAR_STATE = 0x0400 + 104,

        /// <summary>
        /// wParam = TaskDialogProgressBarState
        /// lParam = The LOWORD specifies the minimum value. By default, the minimum value is zero. The HIWORD specifies the maximum value. By default, the maximum value is 100.
        /// </summary>
        TDM_SET_PROGRESS_BAR_RANGE = 0x0400 + 105,

        /// <summary>
        /// wParam = An int that specifies the new position.
        /// lParam = 0
        /// </summary>
        TDM_SET_PROGRESS_BAR_POS = 0x0400 + 106,


        /// <summary>
        /// wParam = A BOOL that indicates whether to turn the marquee display on or off. Use TRUE to turn on the marquee display, or FALSE to turn it off.
        /// lParam = A UINT that specifies the time, in milliseconds, between marquee animation updates. If this parameter is zero, the marquee animation is updated every 30 milliseconds.
        /// </summary>
        TDM_SET_PROGRESS_BAR_MARQUEE = 0x0400 + 107,

        /// <summary>
        /// wParam = TASKDIALOG_ELEMENTS
        /// lParam = LPCWSTR
        /// </summary>
        TDM_SET_ELEMENT_TEXT = 0x0400 + 108,


        /// <summary>
        /// wParam = ButtonID
        /// lParam = 0
        /// </summary>
        TDM_CLICK_RADIO_BUTTON = 0x0400 + 110,

        /// <summary>
        /// wParam = ButtonID
        /// lParam = Specifies button state. Set to 0 to disable the button; set to nonzero to enable the button.
        /// </summary>
        TDM_ENABLE_BUTTON = 0x0400 + 111,

        /// <summary>
        /// wParam = ButtonID
        /// lParam = Specifies button state. Set to 0 to disable the button; set to nonzero to enable the button.
        /// </summary>
        TDM_ENABLE_RADIO_BUTTON = 0x0400 + 112,

        /// <summary>
        /// wParam = TRUE to set the state of the checkbox to be checked; FALSE to set it to be unchecked.
        /// lParam = TRUE to set the keyboard focus to the checkbox; FALSE otherwise.
        /// </summary>
        TDM_CLICK_VERIFICATION = 0x0400 + 113, // wParam = 0 (unchecked), 1 (checked), lParam = 1 (set key focus)

        /// <summary>
        /// wParam = TASKDIALOG_ELEMENTS
        /// lParam = LPCWSTR
        /// </summary>
        TDM_UPDATE_ELEMENT_TEXT = 0x0400 + 114,

        /// <summary>
        /// wParam = ButtonID
        /// lParam = Set to 0 to designate that the action invoked by the button does not require elevation. Set to nonzero to designate that the action requires elevation.
        /// </summary>
        TDM_SET_BUTTON_ELEVATION_REQUIRED_STATE = 0x0400 + 115,

        /// <summary>
        /// wParam = TASKDIALOG_ICON_ELEMENTS
        /// lParam = TaskDialogIcon
        /// </summary>
        TDM_UPDATE_ICON = 0x0400 + 116
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
    struct TASKDIALOG_BUTTON
    {
        public int nButtonID;
        public IntPtr pszButtonText;
    }

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    delegate int TaskDialogCallbackProc(IntPtr hwnd, TaskDialogNotification uNotification, IntPtr wParam, IntPtr lParam, IntPtr refData);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
    struct TASKDIALOGCONFIG
    {
        public uint cbSize;
        public IntPtr hwndParent;
        public IntPtr hInstance;
        public TASKDIALOG_FLAGS dwFlags;
        public TASKDIALOG_COMMON_BUTTON_FLAGS dwCommonButtons;
        public IntPtr pszWindowTitle;
        public IntPtr hMainIcon;
        public IntPtr pszMainInstruction;
        public IntPtr pszContent;
        public uint cButtons;
        public IntPtr pButtons;
        public int nDefaultButton;
        public uint cRadioButtons;
        public IntPtr pRadioButtons;
        public int nDefaultRadioButton;
        public IntPtr pszVerificationText;
        public IntPtr pszExpandedInformation;
        public IntPtr pszExpandedControlText;
        public IntPtr pszCollapsedControlText;
        public IntPtr hFooterIcon;
        public IntPtr pszFooter;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public TaskDialogCallbackProc? pfCallback;
        public IntPtr lpCallbackData;
        public uint cxWidth;
    };
}
