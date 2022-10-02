using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsControls
{
    public class TaskDialog
    {
        public event EventHandler<TaskDialogEventArgs>? DialogConstructed;

        public event EventHandler<TaskDialogEventArgs>? Navigated;

        public event EventHandler<TaskDialogEventArgs>? Destroyed;

        public event EventHandler<TaskDialogEventArgs>? Created;

        public TaskRadioButton? SelectedRadioButton { get; private set; }

        public TaskButton? ClickedButton { get; private set; }

        public bool VerificationFlagChecked { get; private set; }

        public TaskDialogPage? PreviousPage { get; internal set; }
        public TaskDialogPage CurrentPage { get; internal set; }

        public TaskDialog(TaskDialogPage taskDialogPage)
        {
            CurrentPage = taskDialogPage ?? throw new ArgumentNullException(nameof(taskDialogPage));
        }

        public static Task<TaskDialogResult> ShowAsync(TaskDialogPage taskDialogPage)
        {
            return new TaskDialog(taskDialogPage).ShowAsync();
        }

        public static TaskDialogResult DoModal(TaskDialogPage taskDialogPage)
        {
            return new TaskDialog(taskDialogPage).DoModal();
        }

        public static TaskDialogResult DoModal(IntPtr hWnd, TaskDialogPage taskDialogPage)
        {
            return new TaskDialog(taskDialogPage).DoModal(hWnd);
        }

        public Task<TaskDialogResult> ShowAsync()
        {
            var context = SynchronizationContext.Current;

            if (context == null)
            {
                return Task.FromResult(MethodImpl());
            }
            else
            {
                var taskCompletionSource = new TaskCompletionSource<TaskDialogResult>();

                context.Post(state =>
                {
                    try
                    {
                        taskCompletionSource.SetResult(MethodImpl());
                    }
                    catch (TaskCanceledException)
                    {
                        taskCompletionSource.SetCanceled();
                    }
                    catch (Exception ex)
                    {
                        taskCompletionSource.SetException(ex);
                    }

                }, null);

                return taskCompletionSource.Task;
            }
            
            TaskDialogResult MethodImpl()
            {
                var result = ActiveTaskDialog.Show(this, out int button, out int radioButton, out bool verificationFlagChecked);

                ClickedButton = null;
                SelectedRadioButton = null;

                if (CurrentPage.clrButtons.TryGetValue(button, out TaskButton clickedButton))
                {
                    ClickedButton = clickedButton;
                }

                if (CurrentPage.clrRadioButtons.TryGetValue(radioButton, out TaskRadioButton selectedRadioButton))
                {
                    SelectedRadioButton = selectedRadioButton;
                }

                VerificationFlagChecked = verificationFlagChecked;

                return result;
            }
        }

        public TaskDialogResult DoModal() => DoModal(IntPtr.Zero); 

        public TaskDialogResult DoModal(IntPtr hWnd)
        {
            var result = ActiveTaskDialog.DoModal(this, hWnd, out int button, out int radioButton, out bool verificationFlagChecked);

            ClickedButton = null;
            SelectedRadioButton = null;

            if (CurrentPage.clrButtons.TryGetValue(button, out TaskButton clickedButton))
            {
                ClickedButton = clickedButton;
            }

            if (CurrentPage.clrRadioButtons.TryGetValue(radioButton, out TaskRadioButton selectedRadioButton))
            {
                SelectedRadioButton = selectedRadioButton;
            }

            VerificationFlagChecked = verificationFlagChecked;

            return result;
        }

        protected internal virtual void OnCreated(ActiveTaskDialog activeTaskDialog)
        {
            Created?.Invoke(this, new TaskDialogEventArgs(activeTaskDialog));
        }


        protected internal virtual void OnDestroyed(ActiveTaskDialog activeTaskDialog)
        {
            Destroyed?.Invoke(this, new TaskDialogEventArgs(activeTaskDialog));
        }



        protected internal virtual void OnDialogConstructed(ActiveTaskDialog activeTaskDialog)
        {
            DialogConstructed?.Invoke(this, new TaskDialogEventArgs(activeTaskDialog));
        }

        protected internal virtual void OnNavigated(ActiveTaskDialog activeTaskDialog)
        {
            Navigated?.Invoke(this, new TaskDialogEventArgs(activeTaskDialog));
        }
    }
}
