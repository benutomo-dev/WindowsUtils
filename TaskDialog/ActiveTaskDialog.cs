using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsControls
{
    public class ActiveTaskDialog
    {
        internal static readonly OSVERSIONINFOEX OsVersionInfo;
        internal static readonly DLLVERSIONINFO DllVersionInfo;

        static ActiveTaskDialog()
        {
            OsVersionInfo.dwOSVersionInfoSize = (uint)Marshal.SizeOf(OsVersionInfo);
            if (!GetVersionEx(ref OsVersionInfo))
            {
                int hResult = Marshal.GetHRForLastWin32Error();
                Marshal.ThrowExceptionForHR(hResult);
            }

            DllVersionInfo.cbSize = Marshal.SizeOf(typeof(DLLVERSIONINFO));
            int hresult = DllGetVersion(ref DllVersionInfo);
            Marshal.ThrowExceptionForHR(hresult);
        }

        private TaskDialog TaskDialog { get; }

        public IntPtr Handle { get; private set; }

        public IntPtr OwnerHandle { get; private set; }

        private List<Exception>? internalExceptions;

        private TaskDialogProgressBarType CurrentProgressBarType;

        private ActiveTaskDialog(TaskDialog taskDialog)
        {
            TaskDialog = taskDialog;
        }

        internal static TaskDialogResult Show(TaskDialog taskDialog, out int button, out int radioButton, out bool verificationFlagChecked)
        {
            return new ActiveTaskDialog(taskDialog).Show(IntPtr.Zero, out button, out radioButton, out verificationFlagChecked);
        }

        internal static TaskDialogResult DoModal(TaskDialog taskDialog, IntPtr hWnd, out int button, out int radioButton, out bool verificationFlagChecked)
        {
            return new ActiveTaskDialog(taskDialog).Show(hWnd == IntPtr.Zero ? GetActiveWindow() : hWnd, out button, out radioButton, out verificationFlagChecked);
        }

        private TaskDialogResult Show(IntPtr hWnd, out int button, out int radioButton, out bool verificationFlagChecked)
        {
            if (OsVersionInfo.dwMajorVersion < 6 /* OSがVista以上であること */)
            {
                throw new InvalidOperationException(Resource.OSVersionError);
            }

            if (DllVersionInfo.dwMajorVersion < 6)
            {
                throw new InvalidOperationException(Resource.ComCtl32VersionError);
            }

            OwnerHandle = hWnd;

            button = 0;
            radioButton = 0;
            verificationFlagChecked = false;

            var callback = new TaskDialogCallbackProc(TaskDialogCallback);
            var callbackHandle = GCHandle.Alloc(callback);

            try
            {
                TaskDialog.CurrentPage.nativeTaskDialog.pfCallback = callback;

                TaskDialog.CurrentPage.PrepareShow(this);

                CurrentProgressBarType = TaskDialog.CurrentPage.ProgressBar;

                int hResult = TaskDialogIndirect(ref TaskDialog.CurrentPage.nativeTaskDialog, out button, out radioButton, out verificationFlagChecked);

                if (hResult < 0)
                {
                    Marshal.ThrowExceptionForHR(hResult);
                }

                if (internalExceptions != null)
                {
                    throw new AggregateException(internalExceptions).Flatten();
                }
            }
            finally
            {
                if (TaskDialog.PreviousPage != null)
                {
                    TaskDialog.PreviousPage.activeTaskDialog = null;
                }

                TaskDialog.CurrentPage.activeTaskDialog= null;
                TaskDialog.CurrentPage.nativeTaskDialog.pfCallback = null;
                if (callbackHandle.IsAllocated) callbackHandle.Free();
            }

            switch ((TASKDIALOG_COMMAND_ID)button)
            {
                case TASKDIALOG_COMMAND_ID.IDOK:
                    return TaskDialogResult.Ok;
                case TASKDIALOG_COMMAND_ID.IDYES:
                    return TaskDialogResult.Yes;
                case TASKDIALOG_COMMAND_ID.IDNO:
                    return TaskDialogResult.No;
                case TASKDIALOG_COMMAND_ID.IDCANCEL:
                    return TaskDialogResult.Cancel;
                case TASKDIALOG_COMMAND_ID.IDRETRY:
                    return TaskDialogResult.Retry;
                case TASKDIALOG_COMMAND_ID.IDCLOSE:
                    return TaskDialogResult.Close;
                default:
                    return TaskDialogResult.UserButtun;
            }
        }
        
        private int TaskDialogCallback(IntPtr hwnd, TaskDialogNotification uNotification, IntPtr wParam, IntPtr lParam, IntPtr refData)
        {
            try
            {
                switch (uNotification)
                {
                    case TaskDialogNotification.TDN_TIMER:
                        return TaskDialog.CurrentPage.OnTimer(this, (uint)wParam.ToInt32()) ? 1 : 0;
                    case TaskDialogNotification.TDN_BUTTON_CLICKED:
                        return TaskDialog.CurrentPage.OnButtonClickedInternal(this, wParam.ToInt32()) ? 1 : 0;
                    case TaskDialogNotification.TDN_RADIO_BUTTON_CLICKED:
                        TaskDialog.CurrentPage.OnRadioButtonClickedInternal(this, wParam.ToInt32());
                        return 0;
                    case TaskDialogNotification.TDN_CREATED:
                        if (this.Handle == IntPtr.Zero)
                        {
                            this.Handle = hwnd;
                            TaskDialog.CurrentPage.OnLoad(this);
                        }
                        else
                        {
                            this.Handle = hwnd;
                        }
                        TaskDialog.OnCreated(this);
                        return 0;
                    case TaskDialogNotification.TDN_DESTROYED:
                        TaskDialog.CurrentPage.OnUnload(this);
                        TaskDialog.OnDestroyed(this);
                        return 0;
                    case TaskDialogNotification.TDN_DIALOG_CONSTRUCTED:
                        TaskDialog.OnDialogConstructed(this);
                        return 0;
                    case TaskDialogNotification.TDN_EXPANDO_BUTTON_CLICKED:
                        TaskDialog.CurrentPage.OnExpandoButtonClicked(this, wParam != IntPtr.Zero);
                        return 0;
                    case TaskDialogNotification.TDN_HELP:
                        TaskDialog.CurrentPage.OnHelp(this);
                        return 0;
                    case TaskDialogNotification.TDN_HYPERLINK_CLICKED:
                        TaskDialog.CurrentPage.OnHyperlinkClicked(this, Marshal.PtrToStringUni(lParam));
                        return 0;
                    case TaskDialogNotification.TDN_NAVIGATED:
                        TaskDialog.OnNavigated(this);
                        TaskDialog.PreviousPage?.OnUnload(this);
                        TaskDialog.CurrentPage.OnLoad(this);
                        return 0;
                    case TaskDialogNotification.TDN_VERIFICATION_CLICKED:
                        TaskDialog.CurrentPage.OnVerificationClicked(this, wParam != IntPtr.Zero);
                        return 0;
                    default:
                        return 0;
                }
            }
            catch (Exception ex)
            {
                internalExceptions = internalExceptions ?? new List<Exception>();
                internalExceptions.Add(ex);
                ForceCancelClose();
            }

            return 0;
        }

        public void ForceCancelClose()
        {
            if (!TaskDialog.CurrentPage.AllowDialogCancellation)
            {
                var cancellationPage = new TaskDialogPage
                {
                    AllowDialogCancellation = true
                };
                Navigate(cancellationPage);
            }
            SendMessage(Handle, TASKDIALOG_MESSAGE.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }

        public void Navigate(TaskDialogPage taskDialogPage)
        {
            taskDialogPage.nativeTaskDialog.pfCallback = TaskDialog.CurrentPage.nativeTaskDialog.pfCallback;
            taskDialogPage.nativeTaskDialog.lpCallbackData = TaskDialog.CurrentPage.nativeTaskDialog.lpCallbackData;

            TaskDialog.PreviousPage = TaskDialog.CurrentPage;
            TaskDialog.CurrentPage  = taskDialogPage;

            TaskDialog.CurrentPage.PrepareShow(this);

            CurrentProgressBarType = taskDialogPage.ProgressBar;

            SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_NAVIGATE_PAGE, IntPtr.Zero, ref taskDialogPage.nativeTaskDialog);

            TaskDialog.PreviousPage.activeTaskDialog = null;
        }

        public void EnableButton(TaskButton button, bool enable)
        {
            SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_ENABLE_BUTTON, (IntPtr)TaskDialog.CurrentPage.ToId(button.GlobalId), (IntPtr)(enable ? 1 : 0));
        }

        public void EnableRadioButton(TaskRadioButton radioButton, [MarshalAs(UnmanagedType.U1)] bool enable)
        {
            SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_ENABLE_RADIO_BUTTON, (IntPtr)TaskDialog.CurrentPage.ToId(radioButton.GlobalId), (IntPtr)(enable ? 1 : 0));
        }

        public void SetButtonElevationRequiredState(TaskButton button, [MarshalAs(UnmanagedType.U1)] bool elevation)
        {
            SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_SET_BUTTON_ELEVATION_REQUIRED_STATE, (IntPtr)TaskDialog.CurrentPage.ToId(button.GlobalId), (IntPtr)(elevation ? 1 : 0));
        }

        public void ClickCommonButton(TaskDialogCommonButtonFlags button)
        {
            SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_CLICK_BUTTON, (IntPtr)button, IntPtr.Zero);
        }

        public void ClickButton(TaskButton button)
        {
            SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_CLICK_BUTTON, (IntPtr)TaskDialog.CurrentPage.ToId(button.GlobalId), IntPtr.Zero);
        }

        public void ClickRadioButton(TaskRadioButton radioButton)
        {
            SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_CLICK_RADIO_BUTTON, (IntPtr)TaskDialog.CurrentPage.ToId(radioButton.GlobalId), IntPtr.Zero);
        }

        public void ClickVerification(bool check, bool focus)
        {
            SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_CLICK_VERIFICATION, (IntPtr)(check ? 1 : 0), (IntPtr)(focus ? 1 : 0));
        }

        public void SetMarqueeProgressBar(bool marquee)
        {
            if (CurrentProgressBarType == TaskDialogProgressBarType.None)
            {
                throw new InvalidOperationException(Resource.InvalidProgressBarModification);
            }

            SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_SET_MARQUEE_PROGRESS_BAR, (IntPtr)(marquee ? 1 : 0), IntPtr.Zero);

            CurrentProgressBarType = marquee ? TaskDialogProgressBarType.Marquee : TaskDialogProgressBarType.Default;
        }

        public bool SetProgressBarState(TaskDialogProgressBarState newState)
        {
            if (CurrentProgressBarType == TaskDialogProgressBarType.None)
            {
                throw new InvalidOperationException(Resource.InvalidProgressBarModification);
            }

            return SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_SET_PROGRESS_BAR_STATE, (IntPtr)newState, IntPtr.Zero) != IntPtr.Zero;
        }

        public ProgressMinMax SetProgressBarRange(int minRange, int maxRange)
        {
            if (CurrentProgressBarType == TaskDialogProgressBarType.None)
            {
                throw new InvalidOperationException(Resource.InvalidProgressBarModification);
            }

            if (CurrentProgressBarType != TaskDialogProgressBarType.Default)
            {
                SetMarqueeProgressBar(false);
            }

            return new ProgressMinMax((uint)SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_SET_PROGRESS_BAR_RANGE, IntPtr.Zero, (IntPtr)(((maxRange & 0xFFFF) << 16) | (minRange & 0xFFFF))));
        }

        public int SetProgressBarPos(int newPos)
        {
            if (CurrentProgressBarType == TaskDialogProgressBarType.None)
            {
                throw new InvalidOperationException(Resource.InvalidProgressBarModification);
            }

            if (CurrentProgressBarType != TaskDialogProgressBarType.Default)
            {
                SetMarqueeProgressBar(false);
            }

            return (int)SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_SET_PROGRESS_BAR_POS, (IntPtr)newPos, IntPtr.Zero);
        }

        public bool SetProgressBarMarquee(bool marquee, uint speed)
        {
            if (CurrentProgressBarType == TaskDialogProgressBarType.None)
            {
                throw new InvalidOperationException(Resource.InvalidProgressBarModification);
            }

            if (CurrentProgressBarType != TaskDialogProgressBarType.Marquee)
            {
                SetMarqueeProgressBar(true);
            }

            return SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_SET_PROGRESS_BAR_MARQUEE, (IntPtr)(marquee?1:0), (IntPtr)speed) != IntPtr.Zero;
        }

        public void SetContentText(string text)
        {
            var temp = TaskDialog.CurrentPage.nativeTaskDialog.pszContent;
            TaskDialog.CurrentPage.nativeTaskDialog.pszContent = Marshal.StringToHGlobalUni(text);
            SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_SET_ELEMENT_TEXT, (IntPtr)TASKDIALOG_ELEMENTS.TDE_CONTENT, TaskDialog.CurrentPage.nativeTaskDialog.pszContent);
            Marshal.FreeHGlobal(temp);
        }

        public void SetExpandedInformationText(string text)
        {
            if (TaskDialog.CurrentPage.ExpandedInformationText == null)
            {
                throw new InvalidOperationException(Resource.InvalidExpandedInformationModification);
            }

            var temp = TaskDialog.CurrentPage.nativeTaskDialog.pszExpandedInformation;
            TaskDialog.CurrentPage.nativeTaskDialog.pszExpandedInformation = Marshal.StringToHGlobalUni(text);
            SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_SET_ELEMENT_TEXT, (IntPtr)TASKDIALOG_ELEMENTS.TDE_EXPANDED_INFORMATION, TaskDialog.CurrentPage.nativeTaskDialog.pszExpandedInformation);
            Marshal.FreeHGlobal(temp);
        }

        public void SetFooterText(string text)
        {
            if (TaskDialog.CurrentPage.FooterText == null)
            {
                throw new InvalidOperationException(Resource.InvalidFooterModification);
            }

            var temp = TaskDialog.CurrentPage.nativeTaskDialog.pszFooter;
            TaskDialog.CurrentPage.nativeTaskDialog.pszFooter = Marshal.StringToHGlobalUni(text);
            SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_SET_ELEMENT_TEXT, (IntPtr)TASKDIALOG_ELEMENTS.TDE_FOOTER, TaskDialog.CurrentPage.nativeTaskDialog.pszFooter);
            Marshal.FreeHGlobal(temp);
        }

        public void SetMainInstructionText(string text)
        {
            var temp = TaskDialog.CurrentPage.nativeTaskDialog.pszMainInstruction;
            TaskDialog.CurrentPage.nativeTaskDialog.pszMainInstruction = Marshal.StringToHGlobalUni(text);
            SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_SET_ELEMENT_TEXT, (IntPtr)TASKDIALOG_ELEMENTS.TDE_MAIN_INSTRUCTION, TaskDialog.CurrentPage.nativeTaskDialog.pszMainInstruction);
            Marshal.FreeHGlobal(temp);
        }

        public void UpdateContentText(string text)
        {
            var temp = TaskDialog.CurrentPage.nativeTaskDialog.pszContent;
            TaskDialog.CurrentPage.nativeTaskDialog.pszContent = Marshal.StringToHGlobalUni(text);
            SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_UPDATE_ELEMENT_TEXT, (IntPtr)TASKDIALOG_ELEMENTS.TDE_CONTENT, TaskDialog.CurrentPage.nativeTaskDialog.pszContent);
            Marshal.FreeHGlobal(temp);
        }

        public void UpdateExpandedInformationText(string text)
        {
            if (TaskDialog.CurrentPage.ExpandedInformationText == null)
            {
                throw new InvalidOperationException(Resource.InvalidExpandedInformationModification);
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var temp = TaskDialog.CurrentPage.nativeTaskDialog.pszExpandedInformation;
            TaskDialog.CurrentPage.nativeTaskDialog.pszExpandedInformation = Marshal.StringToHGlobalUni(text);
            SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_UPDATE_ELEMENT_TEXT, (IntPtr)TASKDIALOG_ELEMENTS.TDE_EXPANDED_INFORMATION, TaskDialog.CurrentPage.nativeTaskDialog.pszExpandedInformation);
            Marshal.FreeHGlobal(temp);
        }

        public void UpdateFooterText(string text)
        {
            if (TaskDialog.CurrentPage.FooterText == null)
            {
                throw new InvalidOperationException(Resource.InvalidFooterModification);
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var temp = TaskDialog.CurrentPage.nativeTaskDialog.pszFooter;
            TaskDialog.CurrentPage.nativeTaskDialog.pszFooter = Marshal.StringToHGlobalUni(text);
            SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_UPDATE_ELEMENT_TEXT, (IntPtr)TASKDIALOG_ELEMENTS.TDE_FOOTER, TaskDialog.CurrentPage.nativeTaskDialog.pszFooter);
            Marshal.FreeHGlobal(temp);
        }

        public void UpdateMainInstructionText(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var temp = TaskDialog.CurrentPage.nativeTaskDialog.pszMainInstruction;
            TaskDialog.CurrentPage.nativeTaskDialog.pszMainInstruction = Marshal.StringToHGlobalUni(text);
            SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_UPDATE_ELEMENT_TEXT, (IntPtr)TASKDIALOG_ELEMENTS.TDE_MAIN_INSTRUCTION, TaskDialog.CurrentPage.nativeTaskDialog.pszMainInstruction);
            Marshal.FreeHGlobal(temp);
        }

        // 表示開始後のアイコン変更には不具合が多いためトラブルを避けるために非公開にする。
        //public void UpdateFooterIcon(TaskDialogIcon icon)
        //{
        //    if (TaskDialog.CurrentPage.FooterText == null)
        //    {
        //        throw new InvalidOperationException(Resource.InvalidFooterModification);
        //    }
        //
        //    if (TaskDialog.PreviousPage != null)
        //    {
        //        throw new ArgumentException(Resource.FooterIconModificationAfterNavigate, nameof(icon));
        //    }
        //
        //    IntPtr iconValue;
        //
        //    if (icon == null)
        //    {
        //        iconValue = IntPtr.Zero;
        //    }
        //    else
        //    {
        //        if (icon.hIconFlagState != TaskDialog.CurrentPage.GetFlagState(TASKDIALOG_FLAGS.TDF_USE_HICON_FOOTER))
        //        {
        //            throw new ArgumentException(Resource.IncompatibleIconError, nameof(icon));
        //        }
        //
        //        iconValue = TaskDialog.CurrentPage.AdjustIconValue(icon);
        //    }
        //
        //
        //    SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_UPDATE_ICON, (IntPtr)TASKDIALOG_ICON_ELEMENTS.TDIE_ICON_FOOTER, iconValue);
        //}

        // 表示開始後のアイコン変更には不具合が多いためトラブルを避けるために非公開にする。
        //public void UpdateMainIcon(TaskDialogIcon icon)
        //{
        //    IntPtr iconValue;
        //
        //    if (icon == null)
        //    {
        //        iconValue = IntPtr.Zero;
        //    }
        //    else
        //    {
        //        if (TaskDialog.PreviousPage != null)
        //        {
        //            if (icon.hIconFlagState)
        //            {
        //                throw new ArgumentException(Resource.IncompatibleIconError, nameof(icon));
        //            }
        //        }
        //        else
        //        {
        //            if (icon.hIconFlagState != TaskDialog.CurrentPage.GetFlagState(TASKDIALOG_FLAGS.TDF_USE_HICON_MAIN))
        //            {
        //                throw new ArgumentException(Resource.IncompatibleIconError, nameof(icon));
        //            }
        //        }
        //
        //        iconValue = TaskDialog.CurrentPage.AdjustIconValue(icon);
        //    }
        //
        //    SendMessage(Handle, TASKDIALOG_MESSAGE.TDM_UPDATE_ICON, (IntPtr)TASKDIALOG_ICON_ELEMENTS.TDIE_ICON_MAIN, iconValue);
        //}


        [DllImport("comctl32.dll", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        static extern int TaskDialogIndirect(
           [In] ref TASKDIALOGCONFIG pTaskConfig,
           out int pnButton,
           out int pnRadioButton,
           out bool pfverificationFlagChecked
        );


        [DllImport("user32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        static extern IntPtr SendMessage(IntPtr hWnd, TASKDIALOG_MESSAGE Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        static extern IntPtr SendMessage(IntPtr hWnd, TASKDIALOG_MESSAGE Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        static extern IntPtr SendMessage(IntPtr hWnd, TASKDIALOG_MESSAGE Msg, IntPtr wParam, [In] ref TASKDIALOGCONFIG lParam);
        
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern bool GetVersionEx(ref OSVERSIONINFOEX osvi);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        internal struct OSVERSIONINFOEX
        {
            public uint dwOSVersionInfoSize;
            public uint dwMajorVersion;
            public uint dwMinorVersion;
            public uint dwBuildNumber;
            public uint dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
            public UInt16 wServicePackMajor;
            public UInt16 wServicePackMinor;
            public UInt16 wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct DLLVERSIONINFO
        {
            public int cbSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformID;
        }

        [DllImport("comctl32.dll", ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern int DllGetVersion(ref DLLVERSIONINFO dwVersion);

        [DllImport("user32.dll", ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        static extern IntPtr GetActiveWindow();

    }
}
