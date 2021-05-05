using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsControls
{
    public class TaskDialogPage : IDisposable
    {
        private GCHandle buttonsHandle;

        private GCHandle radioButtonsHandle;

        private Dictionary<ulong, int> idTable;

        private TaskButton defaultButton;

        private TaskRadioButton defaultRadioButton;

        internal ActiveTaskDialog activeTaskDialog;

        internal TASKDIALOGCONFIG nativeTaskDialog;

        internal Dictionary<int, TaskButton> clrButtons;

        internal Dictionary<int, TaskRadioButton> clrRadioButtons;

        public event EventHandler<TaskDialogTimerEventArgs> Timer;

        public event EventHandler<TaskDialogEventArgs> Help;

        public event EventHandler<TaskDialogVerificationEventArgs> VerificationClicked;

        public event EventHandler<TaskDialogExpandButtonEventArgs> ExpandoButtonClicked;

        public event EventHandler<TaskDialogHyperlinkEventArgs> HyperlinkClicked;

        public event EventHandler<TaskDialogEventArgs> Load;

        public event EventHandler<TaskDialogEventArgs> UnLoad;

        public TaskDialogFallbackIcon ShieldBlueFallbackIcon { get; set; }
        public TaskDialogFallbackIcon SecurityWarningFallbackIcon { get; set; }
        public TaskDialogFallbackIcon SecurityErrorFallbackIcon { get; set; }
        public TaskDialogFallbackIcon SecuritySucessFallbackIcon { get; set; }
        public TaskDialogFallbackIcon ShieldGrayFallbackIcon { get; set; }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IntPtr Handle => activeTaskDialog.Handle;

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IntPtr OwnerHandle => activeTaskDialog.OwnerHandle;

        public TaskDialogIcon FooterIcon { get; set; }

        public TaskDialogIcon MainIcon { get; set; }
        
        public string FooterText
        {
            get
            {
                return Marshal.PtrToStringUni(nativeTaskDialog.pszFooter);
            }
            set
            {
                if (activeTaskDialog != null)
                {
                    throw new InvalidOperationException(Resource.AlreadyShowing);
                }

                var temp = nativeTaskDialog.pszFooter;
                var newText = Marshal.StringToHGlobalUni(value);
                nativeTaskDialog.pszFooter = newText;
                Marshal.FreeHGlobal(temp);
            }
        }

        public string CollapsedControlText
        {
            get
            {
                return Marshal.PtrToStringUni(nativeTaskDialog.pszCollapsedControlText);
            }
            set
            {
                if (activeTaskDialog != null)
                {
                    throw new InvalidOperationException(Resource.AlreadyShowing);
                }

                var temp = nativeTaskDialog.pszCollapsedControlText;
                var newText = Marshal.StringToHGlobalUni(value);
                nativeTaskDialog.pszCollapsedControlText = newText;
                Marshal.FreeHGlobal(temp);
            }
        }

        public string ExpandedControlText
        {
            get
            {
                return Marshal.PtrToStringUni(nativeTaskDialog.pszExpandedControlText);
            }
            set
            {
                if (activeTaskDialog != null)
                {
                    throw new InvalidOperationException(Resource.AlreadyShowing);
                }

                var temp = nativeTaskDialog.pszExpandedControlText;
                var newText = Marshal.StringToHGlobalUni(value);
                nativeTaskDialog.pszExpandedControlText = newText;
                Marshal.FreeHGlobal(temp);
            }
        }

        public string ExpandedInformationText
        {
            get
            {
                return Marshal.PtrToStringUni(nativeTaskDialog.pszExpandedInformation);
            }
            set
            {
                if (activeTaskDialog != null)
                {
                    throw new InvalidOperationException(Resource.AlreadyShowing);
                }

                var temp = nativeTaskDialog.pszExpandedInformation;
                var newText = Marshal.StringToHGlobalUni(value);
                nativeTaskDialog.pszExpandedInformation = newText;
                Marshal.FreeHGlobal(temp);
            }
        }

        public string VerificationText
        {
            get
            {
                return Marshal.PtrToStringUni(nativeTaskDialog.pszVerificationText);
            }
            set
            {
                if (activeTaskDialog != null)
                {
                    throw new InvalidOperationException(Resource.AlreadyShowing);
                }

                var temp = nativeTaskDialog.pszVerificationText;
                var newText = Marshal.StringToHGlobalUni(value);
                nativeTaskDialog.pszVerificationText = newText;
                Marshal.FreeHGlobal(temp);
            }
        }

        public string ContentText
        {
            get
            {
                return Marshal.PtrToStringUni(nativeTaskDialog.pszContent);
            }
            set
            {
                if (activeTaskDialog != null)
                {
                    throw new InvalidOperationException(Resource.AlreadyShowing);
                }

                var temp = nativeTaskDialog.pszContent;
                var newText = Marshal.StringToHGlobalUni(value);
                nativeTaskDialog.pszContent = newText;
                Marshal.FreeHGlobal(temp);
            }
        }

        public string MainInstructionText
        {
            get
            {
                return Marshal.PtrToStringUni(nativeTaskDialog.pszMainInstruction);
            }
            set
            {
                if (activeTaskDialog != null)
                {
                    throw new InvalidOperationException(Resource.AlreadyShowing);
                }

                var temp = nativeTaskDialog.pszMainInstruction;
                var newText = Marshal.StringToHGlobalUni(value);
                nativeTaskDialog.pszMainInstruction = newText;
                Marshal.FreeHGlobal(temp);
            }
        }

        public string WindowTitle
        {
            get
            {
                return Marshal.PtrToStringUni(nativeTaskDialog.pszWindowTitle);
            }
            set
            {
                if (activeTaskDialog != null)
                {
                    throw new InvalidOperationException(Resource.AlreadyShowing);
                }

                var temp = nativeTaskDialog.pszWindowTitle;
                var newText = Marshal.StringToHGlobalUni(value);
                nativeTaskDialog.pszWindowTitle = newText;
                Marshal.FreeHGlobal(temp);
            }
        }


        public TaskDialogCommonButtonFlags CommonButtons
        {
            get => (TaskDialogCommonButtonFlags)nativeTaskDialog.dwCommonButtons;
            set
            {
                if (activeTaskDialog != null)
                {
                    throw new InvalidOperationException(Resource.AlreadyShowing);
                }

                nativeTaskDialog.dwCommonButtons = (TASKDIALOG_COMMON_BUTTON_FLAGS)value;
            }
        }


        public TaskDialogDefaultCommonButton DefaultCommonButton
        {
            get => (TaskDialogDefaultCommonButton)nativeTaskDialog.nDefaultButton;
            set
            {
                if (activeTaskDialog != null)
                {
                    throw new InvalidOperationException(Resource.AlreadyShowing);
                }

                nativeTaskDialog.nDefaultButton = (int)value;
                defaultButton = null;
            }
        }


        internal bool GetFlagState(TASKDIALOG_FLAGS flag) => (nativeTaskDialog.dwFlags & flag) == flag;
        private void SetFlagState(TASKDIALOG_FLAGS flag, bool value)
        {
            if (activeTaskDialog != null)
            {
                throw new InvalidOperationException(Resource.AlreadyShowing);
            }

            nativeTaskDialog.dwFlags = value ? nativeTaskDialog.dwFlags | flag : nativeTaskDialog.dwFlags & ~flag;
        }

        public bool SizeToContent
        {
            get => GetFlagState(TASKDIALOG_FLAGS.TDF_SIZE_TO_CONTENT);
            set => SetFlagState(TASKDIALOG_FLAGS.TDF_SIZE_TO_CONTENT, value);
        }

        public bool NoSetForgroud
        {
            get => GetFlagState(TASKDIALOG_FLAGS.TDF_NO_SET_FOREGROUND);
            set => SetFlagState(TASKDIALOG_FLAGS.TDF_NO_SET_FOREGROUND, value);
        }

        public bool CanBeMinimized
        {
            get => GetFlagState(TASKDIALOG_FLAGS.TDF_CAN_BE_MINIMIZED);
            set => SetFlagState(TASKDIALOG_FLAGS.TDF_CAN_BE_MINIMIZED, value);
        }

        public bool NoDefaultRadioButton
        {
            get => GetFlagState(TASKDIALOG_FLAGS.TDF_NO_DEFAULT_RADIO_BUTTON);
            set => SetFlagState(TASKDIALOG_FLAGS.TDF_NO_DEFAULT_RADIO_BUTTON, value);
        }

        public bool RtlLayout
        {
            get => GetFlagState(TASKDIALOG_FLAGS.TDF_RTL_LAYOUT);
            set => SetFlagState(TASKDIALOG_FLAGS.TDF_RTL_LAYOUT, value);
        }

        public bool PositionRelativeToWindow
        {
            get => GetFlagState(TASKDIALOG_FLAGS.TDF_POSITION_RELATIVE_TO_WINDOW);
            set => SetFlagState(TASKDIALOG_FLAGS.TDF_POSITION_RELATIVE_TO_WINDOW, value);
        }

        public TaskDialogProgressBarType ProgressBar
        {
            get
            {
                if (GetFlagState(TASKDIALOG_FLAGS.TDF_SHOW_PROGRESS_BAR))
                {
                    if (GetFlagState(TASKDIALOG_FLAGS.TDF_SHOW_MARQUEE_PROGRESS_BAR))
                    {
                        return TaskDialogProgressBarType.Marquee;
                    }
                    else
                    {
                        return TaskDialogProgressBarType.Default;
                    }
                }

                return TaskDialogProgressBarType.None;
            }
            set
            {
                switch (value)
                {
                    case TaskDialogProgressBarType.Default:
                        SetFlagState(TASKDIALOG_FLAGS.TDF_SHOW_PROGRESS_BAR, true);
                        SetFlagState(TASKDIALOG_FLAGS.TDF_SHOW_MARQUEE_PROGRESS_BAR, false);
                        break;
                    case TaskDialogProgressBarType.Marquee:
                        SetFlagState(TASKDIALOG_FLAGS.TDF_SHOW_PROGRESS_BAR, true);
                        SetFlagState(TASKDIALOG_FLAGS.TDF_SHOW_MARQUEE_PROGRESS_BAR, true);
                        break;
                    default:
                        SetFlagState(TASKDIALOG_FLAGS.TDF_SHOW_PROGRESS_BAR, false);
                        SetFlagState(TASKDIALOG_FLAGS.TDF_SHOW_MARQUEE_PROGRESS_BAR, false);
                        break;
                }
            }
        }


        public TaskDialogTaskButtonStyle TaskButtonStyle
        {
            get
            {
                if (GetFlagState(TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS_NO_ICON))
                {
                    return TaskDialogTaskButtonStyle.NoIconCommandLink;
                }
                else if (GetFlagState(TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS))
                {
                    return TaskDialogTaskButtonStyle.CommandLink;
                }
                else
                {
                    return TaskDialogTaskButtonStyle.Default;
                }
            }
            set
            {
                switch (value)
                {
                    case TaskDialogTaskButtonStyle.CommandLink:
                        SetFlagState(TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS, true);
                        SetFlagState(TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS_NO_ICON, false);
                        break;
                    case TaskDialogTaskButtonStyle.NoIconCommandLink:
                        SetFlagState(TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS, false);
                        SetFlagState(TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS_NO_ICON, true);
                        break;
                    default:
                        SetFlagState(TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS, false);
                        SetFlagState(TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS_NO_ICON, false);
                        break;
                }
            }
        }

        public bool VerificationFlagCheckedByDefault
        {
            get => GetFlagState(TASKDIALOG_FLAGS.TDF_VERIFICATION_FLAG_CHECKED);
            set => SetFlagState(TASKDIALOG_FLAGS.TDF_VERIFICATION_FLAG_CHECKED, value);
        }

        public bool ExpandedByDefault
        {
            get => GetFlagState(TASKDIALOG_FLAGS.TDF_EXPANDED_BY_DEFAULT);
            set => SetFlagState(TASKDIALOG_FLAGS.TDF_EXPANDED_BY_DEFAULT, value);
        }

        public bool ExpandFooterArea
        {
            get => GetFlagState(TASKDIALOG_FLAGS.TDF_EXPAND_FOOTER_AREA);
            set => SetFlagState(TASKDIALOG_FLAGS.TDF_EXPAND_FOOTER_AREA, value);
        }

        public bool AllowDialogCancellation
        {
            get => GetFlagState(TASKDIALOG_FLAGS.TDF_ALLOW_DIALOG_CANCELLATION);
            set => SetFlagState(TASKDIALOG_FLAGS.TDF_ALLOW_DIALOG_CANCELLATION, value);
        }

        public bool EnableHyperLinks
        {
            get => GetFlagState(TASKDIALOG_FLAGS.TDF_ENABLE_HYPERLINKS);
            set => SetFlagState(TASKDIALOG_FLAGS.TDF_ENABLE_HYPERLINKS, value);
        }



        public TaskDialogPage()
        {
            nativeTaskDialog = new TASKDIALOGCONFIG
            {
                cbSize = (uint)Marshal.SizeOf(nativeTaskDialog),
            };

            clrButtons = new Dictionary<int, TaskButton>();
            clrRadioButtons = new Dictionary<int, TaskRadioButton>();


            ShieldBlueFallbackIcon = TaskDialogFallbackIcon.Shield;
            SecurityWarningFallbackIcon = TaskDialogFallbackIcon.Warning;
            SecurityErrorFallbackIcon = TaskDialogFallbackIcon.Error;
            SecuritySucessFallbackIcon = TaskDialogFallbackIcon.None;
            ShieldGrayFallbackIcon = TaskDialogFallbackIcon.Shield;
        }



        internal void PrepareShow(ActiveTaskDialog activeTaskDialog)
        {
            if (activeTaskDialog == null)
            {
                throw new ArgumentNullException(nameof(activeTaskDialog));
            }

            if (this.activeTaskDialog != null)
            {
                throw new InvalidOperationException(Resource.AlreadyShowing);
            }

            if (TaskButtonStyle != TaskDialogTaskButtonStyle.Default && clrButtons.Count == 0)
            {
                throw new InvalidOperationException(Resource.InvalidTaskButtonStyle);
            }

            idTable = clrButtons.Select(v => new { v.Value.GlobalId, Id = v.Key })
                        .Concat(clrRadioButtons.Select(v => new { v.Value.GlobalId, Id = v.Key }))
                        .ToDictionary(v => v.GlobalId, v => v.Id);

            if (defaultButton != null && idTable.TryGetValue(defaultButton.GlobalId, out int defaultButtonId))
            {
                nativeTaskDialog.nDefaultButton = defaultButtonId;
            }

            if (defaultRadioButton != null && idTable.TryGetValue(defaultRadioButton.GlobalId, out int defaultRadioId))
            {
                nativeTaskDialog.nDefaultRadioButton = defaultRadioId;
            }


            IntPtr mainIconValue = AdjustIconValue(MainIcon);
            IntPtr footerIconValue = AdjustIconValue(FooterIcon);
            
            SetFlagState(TASKDIALOG_FLAGS.TDF_USE_HICON_MAIN, MainIcon.hIconFlagState);
            nativeTaskDialog.hMainIcon = mainIconValue;

            SetFlagState(TASKDIALOG_FLAGS.TDF_USE_HICON_FOOTER, FooterIcon.hIconFlagState);
            nativeTaskDialog.hFooterIcon = footerIconValue;


            nativeTaskDialog.hwndParent = activeTaskDialog.OwnerHandle;

            SetFlagState(TASKDIALOG_FLAGS.TDF_CALLBACK_TIMER, Timer != null);

            this.activeTaskDialog = activeTaskDialog;
        }

        public void SetButtons(params TaskButton[] buttons)
        {
            if (activeTaskDialog != null)
            {
                throw new InvalidOperationException(Resource.AlreadyShowing);
            }

            if (buttons == null || buttons.Length == 0)
            {
                if (buttonsHandle.IsAllocated) buttonsHandle.Free();
                nativeTaskDialog.pButtons = IntPtr.Zero;
                nativeTaskDialog.cButtons = 0;

                clrButtons = new Dictionary<int, TaskButton>();

                return;
            }

            clrButtons = buttons
                            .Select((v, i) => new { Button = v, Id = v.GlobalId >= TaskButton.GlobalIdStart ? i + 0x1000 : (int)v.GlobalId })
                            .ToDictionary(v => v.Id, v => v.Button);

            TASKDIALOG_BUTTON[] nativeButtons = clrButtons
                                                .Select(v => new TASKDIALOG_BUTTON
                                                {
                                                    nButtonID = v.Key,
                                                    pszButtonText = Marshal.StringToHGlobalUni(v.Value.Text)
                                                })
                                                .ToArray();

            if (buttonsHandle.IsAllocated) buttonsHandle.Free();
            buttonsHandle = GCHandle.Alloc(nativeButtons, GCHandleType.Pinned);
            nativeTaskDialog.pButtons = Marshal.UnsafeAddrOfPinnedArrayElement(nativeButtons, 0);
            nativeTaskDialog.cButtons = (uint)buttons.Length;
        }

        public void SetDefaultButton(TaskButton defaultButton)
        {
            if (activeTaskDialog != null)
            {
                throw new InvalidOperationException(Resource.AlreadyShowing);
            }

            nativeTaskDialog.nDefaultButton = (int)TaskDialogDefaultCommonButton.Default;
            this.defaultButton = defaultButton;
        }

        public void SetRadioButtons(params TaskRadioButton[] radioButtons)
        {
            if (activeTaskDialog != null)
            {
                throw new InvalidOperationException(Resource.AlreadyShowing);
            }

            if (radioButtons == null || radioButtons.Length == 0)
            {
                if (radioButtonsHandle.IsAllocated) radioButtonsHandle.Free();
                nativeTaskDialog.pRadioButtons = IntPtr.Zero;
                nativeTaskDialog.cRadioButtons = 0;

                clrRadioButtons = new Dictionary<int, TaskRadioButton>();

                return;
            }

            clrRadioButtons = radioButtons
                            .Select((v, i) => new { Button = v, Id = v.GlobalId >= TaskButton.GlobalIdStart ? i + 0x2000 : (int)v.GlobalId })
                            .ToDictionary(v => v.Id, v => v.Button);

            TASKDIALOG_BUTTON[] nativeButtons = clrRadioButtons
                                                .Select(v => new TASKDIALOG_BUTTON
                                                {
                                                    nButtonID = v.Key,
                                                    pszButtonText = Marshal.StringToHGlobalUni(v.Value.Text)
                                                })
                                                .ToArray();

            if (buttonsHandle.IsAllocated) buttonsHandle.Free();
            buttonsHandle = GCHandle.Alloc(nativeButtons, GCHandleType.Pinned);
            nativeTaskDialog.pRadioButtons = Marshal.UnsafeAddrOfPinnedArrayElement(nativeButtons, 0);
            nativeTaskDialog.cRadioButtons = (uint)radioButtons.Length;
        }

        public void SetDefaultRadioButton(TaskRadioButton defaultRadioButton)
        {
            if (activeTaskDialog != null)
            {
                throw new InvalidOperationException(Resource.AlreadyShowing);
            }

            this.defaultRadioButton = defaultRadioButton;
        }

        public void SetWidth(int width)
        {
            if (activeTaskDialog != null)
            {
                throw new InvalidOperationException(Resource.AlreadyShowing);
            }

            nativeTaskDialog.cxWidth = (uint)Math.Max(0, width);
        }

        internal int ToId(ulong globalId)
        {
            if (idTable.TryGetValue(globalId, out int id))
            {
                return id;
            }
            return 0;
        }

        internal bool OnButtonClickedInternal(ActiveTaskDialog activeTaskDialog, int nButton)
        {
            if (clrButtons.TryGetValue(nButton, out TaskButton button))
            {
                return button.OnClick(activeTaskDialog);
            }

            return false;
        }

        internal void OnRadioButtonClickedInternal(ActiveTaskDialog activeTaskDialog, int nRadioButton)
        {
            if (clrRadioButtons.TryGetValue(nRadioButton, out TaskRadioButton radioButton))
            {
                radioButton.OnClick(activeTaskDialog);
            }
        }

        private static bool IsWin8OrNewer()
        {
            if (ActiveTaskDialog.OsVersionInfo.dwMajorVersion == 6)
            {
                return ActiveTaskDialog.OsVersionInfo.dwMinorVersion >= 2;
            }
            else
            {
                return ActiveTaskDialog.OsVersionInfo.dwMajorVersion > 6;
            }
        }

        internal IntPtr AdjustIconValue(TaskDialogIcon icon)
        {
            IntPtr iconValue;

            if (!icon.hIconFlagState && icon.win8Icon && !IsWin8OrNewer())
            {
                TaskDialogFallbackIcon fallback;
                switch ((TASKDIALOG_ICON_WIN8)icon.value)
                {
                    case TASKDIALOG_ICON_WIN8.TD_SHIELDBLUE_ICON: fallback = ShieldBlueFallbackIcon; break;
                    case TASKDIALOG_ICON_WIN8.TD_SECURITYWARNING_ICON: fallback = ShieldBlueFallbackIcon; break;
                    case TASKDIALOG_ICON_WIN8.TD_SECURITYERROR_ICON: fallback = ShieldBlueFallbackIcon; break;
                    case TASKDIALOG_ICON_WIN8.TD_SECURITYSUCCESS_ICON: fallback = ShieldBlueFallbackIcon; break;
                    case TASKDIALOG_ICON_WIN8.TD_SHIELDGRAY_ICON: fallback = ShieldBlueFallbackIcon; break;
                    default: throw new ArgumentException(nameof(icon));
                }

                switch (fallback)
                {
                    case TaskDialogFallbackIcon.None: iconValue = IntPtr.Zero; break;
                    case TaskDialogFallbackIcon.Warning: iconValue = TaskDialogIcon.Warning.value; break;
                    case TaskDialogFallbackIcon.Error: iconValue = TaskDialogIcon.Error.value; break;
                    case TaskDialogFallbackIcon.Infomation: iconValue = TaskDialogIcon.Infomation.value; break;
                    case TaskDialogFallbackIcon.Shield: iconValue = TaskDialogIcon.Shield.value; break;
                    default: throw new PlatformNotSupportedException();
                }
            }
            else
            {
                iconValue = icon.value;
            }

            return iconValue;
        }


        protected internal virtual void OnLoad(ActiveTaskDialog activeTaskDialog)
        {
            Load?.Invoke(this, new TaskDialogEventArgs(activeTaskDialog));
        }


        protected internal virtual void OnUnload(ActiveTaskDialog activeTaskDialog)
        {
            UnLoad?.Invoke(this, new TaskDialogEventArgs(activeTaskDialog));
        }

        protected internal virtual void OnHyperlinkClicked(ActiveTaskDialog activeTaskDialog, string href)
        {
            HyperlinkClicked?.Invoke(this, new TaskDialogHyperlinkEventArgs(activeTaskDialog, href));
        }

        protected internal virtual void OnExpandoButtonClicked(ActiveTaskDialog activeTaskDialog, bool expanded)
        {
            ExpandoButtonClicked?.Invoke(this, new TaskDialogExpandButtonEventArgs(activeTaskDialog, expanded));
        }

        protected internal virtual void OnVerificationClicked(ActiveTaskDialog activeTaskDialog, bool @checked)
        {
            VerificationClicked?.Invoke(this, new TaskDialogVerificationEventArgs(activeTaskDialog, @checked));
        }

        protected internal virtual void OnHelp(ActiveTaskDialog activeTaskDialog)
        {
            Help?.Invoke(this, new TaskDialogEventArgs(activeTaskDialog));
        }

        protected internal virtual bool OnTimer(ActiveTaskDialog activeTaskDialog, uint tickCount)
        {
            TaskDialogTimerEventArgs eventArgs = new TaskDialogTimerEventArgs(activeTaskDialog, tickCount, false);
            Timer?.Invoke(this, eventArgs);
            return eventArgs.ResetTickCount;
        }



        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // None
                }

                nativeTaskDialog.pButtons = IntPtr.Zero;
                nativeTaskDialog.cButtons = 0;
                if (buttonsHandle.IsAllocated) buttonsHandle.Free();

                nativeTaskDialog.pRadioButtons = IntPtr.Zero;
                nativeTaskDialog.cRadioButtons = 0;
                if (radioButtonsHandle.IsAllocated) radioButtonsHandle.Free();

                Marshal.FreeHGlobal(nativeTaskDialog.pszWindowTitle);
                nativeTaskDialog.pszWindowTitle = IntPtr.Zero;

                Marshal.FreeHGlobal(nativeTaskDialog.pszMainInstruction);
                nativeTaskDialog.pszMainInstruction = IntPtr.Zero;

                Marshal.FreeHGlobal(nativeTaskDialog.pszContent);
                nativeTaskDialog.pszContent = IntPtr.Zero;

                Marshal.FreeHGlobal(nativeTaskDialog.pszVerificationText);
                nativeTaskDialog.pszVerificationText = IntPtr.Zero;

                Marshal.FreeHGlobal(nativeTaskDialog.pszExpandedInformation);
                nativeTaskDialog.pszExpandedInformation = IntPtr.Zero;

                Marshal.FreeHGlobal(nativeTaskDialog.pszExpandedControlText);
                nativeTaskDialog.pszExpandedControlText = IntPtr.Zero;

                Marshal.FreeHGlobal(nativeTaskDialog.pszCollapsedControlText);
                nativeTaskDialog.pszCollapsedControlText = IntPtr.Zero;

                Marshal.FreeHGlobal(nativeTaskDialog.pszFooter);
                nativeTaskDialog.pszFooter = IntPtr.Zero;

                nativeTaskDialog.hwndParent = IntPtr.Zero;
                nativeTaskDialog.hInstance = IntPtr.Zero;

                disposedValue = true;
            }
        }

        ~TaskDialogPage()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
