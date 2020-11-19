using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsControls;

using TaskDialog = WindowsControls.TaskDialog;
using TaskDialogIcon = WindowsControls.TaskDialogIcon;
using TaskDialogPage = WindowsControls.TaskDialogPage;
using TaskDialogProgressBarState = WindowsControls.TaskDialogProgressBarState;

namespace TaskDialogSample
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            TaskDialog.DoModal(MakeSamplePages());
        }

        static TaskDialogPage MakeSamplePages()
        {

            var basicTaskDialogPage = new TaskDialogPage
            {
                WindowTitle = "Samples",
                MainInstructionText = "Select sample",
                TaskButtonStyle = TaskDialogTaskButtonStyle.CommandLink,
                MainIcon = TaskDialogIcon.FromHIcon(StockIcons.UsersLarge.Handle),
                ProgressBar = TaskDialogProgressBarType.Marquee,
            };

            var radioTaskDialogPage = new TaskDialogPage
            {
                WindowTitle = "Samples Of Radio",
                TaskButtonStyle = TaskDialogTaskButtonStyle.CommandLink,
            };

            var progressTaskDialogPage = new TaskDialogPage
            {
                WindowTitle = "Samples Of Progress",
                TaskButtonStyle = TaskDialogTaskButtonStyle.CommandLink,
                ProgressBar = TaskDialogProgressBarType.Default,
            };

            var autoCloseTaskDialogPage = new TaskDialogPage
            {
                WindowTitle = "Auto Close Dialog",
                ProgressBar = TaskDialogProgressBarType.Default,
            };

            var ultimateTaskDialogPage = new TaskDialogPage
            {
                WindowTitle = "Full Sample",
                AllowDialogCancellation = true,
                MainIcon = TaskDialogIcon.Error,
                FooterIcon = TaskDialogIcon.Error,
                CanBeMinimized = true,
                ExpandedByDefault = true,
                CollapsedControlText = "Collapsed",
                ExpandedControlText = "Expanded",
                ContentText = "Here is contet text.\nYou can use <A HREF=\"http://www.googole.co.jp/\">hyperlink</A>.",
                EnableHyperLinks = true,
                CommonButtons = TaskDialogCommonButtonFlags.Ok
                                    | TaskDialogCommonButtonFlags.Yes
                                    | TaskDialogCommonButtonFlags.Retry
                                    | TaskDialogCommonButtonFlags.No
                                    | TaskDialogCommonButtonFlags.Close
                                    | TaskDialogCommonButtonFlags.Cancel,
                ExpandedInformationText = "Here is expanded Information.\nYou can use <A HREF=\"http://www.googole.co.jp/\">hyperlink</A>.",
                ExpandFooterArea = true,
                FooterText = "Here is fotter text.",
                MainInstructionText = "Here is main instruction text.",
                ProgressBar = TaskDialogProgressBarType.Default,
                VerificationText = "Here is verificaion text.",
                TaskButtonStyle = TaskDialogTaskButtonStyle.CommandLink,
                VerificationFlagCheckedByDefault = true,
                NoDefaultRadioButton = true,
            };

            var buttonStyle = new TaskButton("Use Button Style Command Links.");
            var commandStyle = new TaskButton("Use CommanLink Style Command Links.");
            var noiconStyle = new TaskButton("Use NoIconCommandLink Style Command Links.");
            var radioSample = new TaskButton("Sample Of Radio.");
            var progressSample = new TaskButton("Sample Of Progress.");
            var ultimateSample = new TaskButton("Ultimate Sample.");
            var autoCloseSample = new TaskButton("Auto close Sample.");
            var modalSample = new TaskButton("Modal TaskDialog Sample.");
            var modelessSample = new TaskButton("Modeless TaskDialog Sample.");
            var close = new TaskButton("Close");

            var radio1 = new TaskRadioButton("Radio1");
            var radio2 = new TaskRadioButton("Radio2");
            var radio3 = new TaskRadioButton("Radio3");
            
            
            var progressNormal = new TaskRadioButton("None");
            var progressError = new TaskRadioButton("Error");
            var progressPause = new TaskRadioButton("Pause");

            var disableButton = new TaskButton("dummy");

            basicTaskDialogPage.SetButtons(
                buttonStyle,
                commandStyle,
                noiconStyle,
                radioSample,
                progressSample,
                autoCloseSample,
                ultimateSample,
                modalSample,
                modelessSample,
                close);

            radioTaskDialogPage.SetRadioButtons(radio1, radio2, radio3);
            radioTaskDialogPage.SetButtons(commandStyle);

            progressTaskDialogPage.SetRadioButtons(progressNormal, progressError, progressPause);
            progressTaskDialogPage.SetButtons(commandStyle);

            autoCloseTaskDialogPage.SetButtons(disableButton);

            radioTaskDialogPage.SetRadioButtons(radio1, radio2, radio3);
            ultimateTaskDialogPage.SetButtons(commandStyle);


            buttonStyle.Click += (_, ev) =>
            {
                ev.CancelDialogClose = true;

                var navigatePage = MakeSamplePages();
                navigatePage.TaskButtonStyle = TaskDialogTaskButtonStyle.Default;
                ev.ActiveTaskDialog.Navigate(navigatePage);
            };
            commandStyle.Click += (_, ev) =>
            {
                ev.CancelDialogClose = true;

                var navigatePage = MakeSamplePages();
                navigatePage.TaskButtonStyle = TaskDialogTaskButtonStyle.CommandLink;
                ev.ActiveTaskDialog.Navigate(navigatePage);
            };
            noiconStyle.Click += (_, ev) =>
            {
                ev.CancelDialogClose = true;

                var navigatePage = MakeSamplePages();
                navigatePage.TaskButtonStyle = TaskDialogTaskButtonStyle.NoIconCommandLink;
                ev.ActiveTaskDialog.Navigate(navigatePage);
            };
            radioSample.Click += (_, ev) =>
            {
                ev.CancelDialogClose = true;
                ev.ActiveTaskDialog.Navigate(radioTaskDialogPage);
            };
            progressSample.Click += (_, ev) =>
            {
                ev.CancelDialogClose = true;
                ev.ActiveTaskDialog.Navigate(progressTaskDialogPage);
            };
            autoCloseSample.Click += (_, ev) =>
            {
                ev.CancelDialogClose = true;
                ev.ActiveTaskDialog.Navigate(autoCloseTaskDialogPage);
            };
            ultimateSample.Click += (_, ev) =>
            {
                ev.CancelDialogClose = true;
                ev.ActiveTaskDialog.Navigate(ultimateTaskDialogPage);
            };
            modalSample.Click += (_, ev) =>
            {
                ev.CancelDialogClose = true;
                var modalTaskDialog = new TaskDialog(MakeSamplePages());
                modalTaskDialog.DoModal(ev.ActiveTaskDialog.Handle);
            };
            modelessSample.Click += async (_, ev) =>
            {
                ev.CancelDialogClose = true;
                var modelessTaskDialog = new TaskDialog(MakeSamplePages());
                await modelessTaskDialog.ShowAsync();
            };

            var large = StockIcons.ErrorLarge;
            var small = StockIcons.ErrorSmall;


            TaskDialogProgressBarState barState = TaskDialogProgressBarState.Normal;

            progressNormal.Click += (_, ev) =>
            {
                barState = TaskDialogProgressBarState.Normal;
                ev.ActiveTaskDialog.SetProgressBarState(barState);
            };
            progressError.Click += (_, ev) =>
            {
                barState = TaskDialogProgressBarState.Error;
                ev.ActiveTaskDialog.SetProgressBarState(barState);
            };
            progressPause.Click += (_, ev) =>
            {
                barState = TaskDialogProgressBarState.Pause;
                ev.ActiveTaskDialog.SetProgressBarState(barState);
            };
            progressTaskDialogPage.Load += (_, ev) =>
            {
                ev.ActiveTaskDialog.SetProgressBarRange(0, 10000);
            };
            progressTaskDialogPage.Timer += (_, ev) =>
            {
                if (barState == TaskDialogProgressBarState.Normal)
                {
                    ev.ActiveTaskDialog.SetProgressBarPos((int)(ev.TickCount % 10000));
                }
            };

            autoCloseTaskDialogPage.Load += (_, ev) =>
            {
                ev.ActiveTaskDialog.SetProgressBarRange(0, 4000);
                ev.ActiveTaskDialog.EnableButton(disableButton, false);
            };
            autoCloseTaskDialogPage.Timer += (_, ev) =>
            {
                ev.ActiveTaskDialog.SetProgressBarPos((int)Math.Min(4000, ev.TickCount));

                if (ev.TickCount > 4000)
                {
                    ev.ActiveTaskDialog.ForceCancelClose();
                }
            };

            ultimateTaskDialogPage.HyperlinkClicked += (_, ev) =>
            {
                MessageBox.Show(ev.Href);
            };
            ultimateTaskDialogPage.Load += (_, ev) =>
            {
                ev.ActiveTaskDialog.SetProgressBarRange(0, 10000);
            };
            ultimateTaskDialogPage.Timer += (_, ev) =>
            {
                ev.ActiveTaskDialog.SetProgressBarPos((int)(ev.TickCount % 10000));
            };
            
            return basicTaskDialogPage;
        }
    }
}
