using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsControls;
using WindowsControls.Aysnc;
using WindowsControls.Aysnc.Forms;

using TaskDialog = WindowsControls.TaskDialog;
using TaskDialogIcon = WindowsControls.TaskDialogIcon;
using TaskDialogPage = WindowsControls.TaskDialogPage;
using TaskDialogProgressBarState = WindowsControls.TaskDialogProgressBarState;

namespace AsyncHelperSample
{
    public partial class Form1 : Form
    {
        static ExclusiveExecutionMachine Label1AffecredInvocation = new ExclusiveExecutionMachine(pushValueAtSubscribe: true);
        static ExclusiveExecutionMachine Label2AffecredInvocation = new ExclusiveExecutionMachine(pushValueAtSubscribe: true);

        public Form1()
        {
            InitializeComponent();

            normalDoDelayActionButton.AddBindingExclusiveExecutionMachines(Label1AffecredInvocation, Label2AffecredInvocation);
            modalDoDelayActionButton.AddBindingExclusiveExecutionMachines(Label1AffecredInvocation, Label2AffecredInvocation);
            exclusiveDelayActionButton.AddBindingExclusiveExecutionMachines(Label1AffecredInvocation, Label2AffecredInvocation);
            exclusiveDelayAction1Button.AddBindingExclusiveExecutionMachines(Label1AffecredInvocation);
            exclusiveDelayAction2Button.AddBindingExclusiveExecutionMachines(Label2AffecredInvocation);


            normalDoDelayActionButton.RemoveBindingExclusiveExecutionMachine(Label2AffecredInvocation);
            normalDoDelayActionButton.RemoveBindingExclusiveExecutionMachine(Label1AffecredInvocation);

            panel1.AddBindingExclusiveExecutionMachines(Label1AffecredInvocation, Label2AffecredInvocation);
        }


        const int WM_SETCURSOR = 0x0020;
        protected override void DefWndProc(ref Message m)
        {
            if (m.Msg == WM_SETCURSOR && m.WParam == Handle && ModalExecutionBlock.IsEnteredAnyone)
            {
                Cursor.Current = Cursors.WaitCursor;
            }
            else
            {
                base.DefWndProc(ref m);
            }
        }

        private async void normalDoDelayActionButton_Click(object sender, EventArgs e)
        {
            await Task.WhenAll(Label1DelayActionCoreAsync(), Label2DelayActionCoreAsync());
        }

        private async void modalDoDelayActionButton_Click(object sender, EventArgs e)
        {
            using (ModalExecutionBlock.Default.Enter())
            {
                await Task.WhenAll(Label1DelayActionCoreAsync(), Label2DelayActionCoreAsync());
            }
        }

        private async void exclusiveDelayActionButton_Click(object sender, EventArgs e)
        {
            using (await ExclusiveExecutionMachine.EnterAllAsync(Label1AffecredInvocation, Label2AffecredInvocation))
            {
                Application.UseWaitCursor = true;
                try
                {
                    await Task.WhenAll(Label1DelayActionCoreAsync(), Label2DelayActionCoreAsync());
                }
                finally
                {
                    Application.UseWaitCursor = false;
                }
            }
        }

        private async void exclusiveDelayAction1Button_Click(object sender, EventArgs e)
        {
            using (await Label1AffecredInvocation.EnterAsync())
            {
                await Label1DelayActionCoreAsync();
            }
        }

        private async void exclusiveDelayAction2Button_Click(object sender, EventArgs e)
        {
            using (await Label2AffecredInvocation.EnterAsync())
            {
                await Label2DelayActionCoreAsync();
            }
        }

        private async Task Label1DelayActionCoreAsync()
        {
            for (int i = 1; i < 6; i++)
            {
                statusLabel1.Text = i.ToString();
                await Task.Delay(1000);
            }
        }

        private async Task Label2DelayActionCoreAsync()
        {
            for (int i = 1; i < 6; i++)
            {
                statusLabel2.Text = i.ToString();
                await Task.Delay(1000);
            }
        }

        private void showDialogButton_Click(object sender, EventArgs e)
        {
            new Form1().ShowDialog();
        }

        private void showFormButton_Click(object sender, EventArgs e)
        {
            new Form1().Show();
        }

        private void showChildFormButton_Click(object sender, EventArgs e)
        {
            new Form1().Show(this);
        }

        private void showModalTaskDialogButton_Click(object sender, EventArgs e)
        {
            var page = new TaskDialogPage
            {
                WindowTitle = "ModalTaskDialog",
                TaskButtonStyle = TaskDialogTaskButtonStyle.CommandLink,
            };

            var showDialogButton = new TaskButton("Show Dialog.");
            showDialogButton.Click += (_, tbe) =>
            {
                new Form1().ShowDialog();
                tbe.CancelDialogClose = true;
            };

            var showFormButton = new TaskButton("Show Form.");
            showFormButton.Click += (_, tbe) =>
            {
                new Form1().Show();
                tbe.CancelDialogClose = true;
            };

            var closeButton = new TaskButton("Close.");

            page.SetButtons(showDialogButton, showFormButton, closeButton);

            TaskDialog.DoModal(this.Handle, page);
        }

        private void showModelessTaskDialogButton_Click(object sender, EventArgs e)
        {
            var page = new TaskDialogPage
            {
                WindowTitle = "ModalTaskDialog",
                TaskButtonStyle = TaskDialogTaskButtonStyle.CommandLink,
            };

            var showDialogButton = new TaskButton("Show Dialog.");
            showDialogButton.Click += (_, tbe) =>
            {
                new Form1().ShowDialog();
                tbe.CancelDialogClose = true;
            };

            var showFormButton = new TaskButton("Show Form.");
            showFormButton.Click += (_, tbe) =>
            {
                new Form1().Show();
                tbe.CancelDialogClose = true;
            };

            var closeButton = new TaskButton("Close.");

            page.SetButtons(showDialogButton, showFormButton, closeButton);

            TaskDialog.ShowAsync(page);
        }
    }
}
