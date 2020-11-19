using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsControls.Aysnc.Forms
{
    public static class AsyncExtensions
    {
        public static bool WaitWhileDoingMessageLoopEvents(this Task task, int millisecondsTimeout = -1, ModalExecutionBlock? modalExecutionBlock = null)
        {
            if (task.IsCompleted)
            {
                return true;
            }

            var timer = Stopwatch.StartNew();
            if (Application.MessageLoop)
            {
                modalExecutionBlock ??= ModalExecutionBlock.Default;

                using (modalExecutionBlock.Enter())
                {
                    do
                    {
                        Application.DoEvents();
                    }
                    while (!task.IsCompleted && (millisecondsTimeout == -1 || timer.ElapsedMilliseconds < millisecondsTimeout));

                    if (task.IsCanceled || task.IsFaulted)
                    {
                        task.Wait();
                    }

                    return task.IsCompleted;
                }
            }
            else
            {
                return task.Wait(millisecondsTimeout);
            }
        }

        public static bool WaitWhileDoingMessageLoopEvents(this in ValueTask valueTask, int millisecondsTimeout = -1, ModalExecutionBlock? modalExecutionBlock = null)
        {
            if (valueTask.IsCompleted)
            {
                return true;
            }

            var timer = Stopwatch.StartNew();
            if (Application.MessageLoop)
            {
                modalExecutionBlock ??= ModalExecutionBlock.Default;

                using (modalExecutionBlock.Enter())
                {
                    do
                    {
                        Application.DoEvents();
                    }
                    while (!valueTask.IsCompleted && (millisecondsTimeout == -1 || timer.ElapsedMilliseconds < millisecondsTimeout));

                    if (valueTask.IsCanceled || valueTask.IsFaulted)
                    {
                        valueTask.GetAwaiter().GetResult();
                    }

                    return valueTask.IsCompleted;
                }
            }
            else
            {
                var task = valueTask.AsTask();
                return task.Wait(millisecondsTimeout);
            }
        }

        public static bool WaitWhileDoingMessageLoopEvents<T>(this in ValueTask<T> valueTask, out T result, int millisecondsTimeout = -1, ModalExecutionBlock? modalExecutionBlock = null)
        {
            if (valueTask.IsCompleted)
            {
                result = valueTask.Result;
                return true;
            }

            var timer = Stopwatch.StartNew();
            if (Application.MessageLoop)
            {
                modalExecutionBlock ??= ModalExecutionBlock.Default;

                using (modalExecutionBlock.Enter())
                {
                    do
                    {
                        Application.DoEvents();
                    }
                    while (!valueTask.IsCompleted && (millisecondsTimeout == -1 || timer.ElapsedMilliseconds < millisecondsTimeout));

                    if (valueTask.IsCompleted)
                    {
                        result = valueTask.Result;
                        return true;
                    }
                    else
                    {
                        result = default!;
                        return false;
                    }
                }
            }
            else
            {
                var task = valueTask.AsTask();
                if (task.Wait(millisecondsTimeout))
                {
                    result = task.Result;
                    return true;
                }
                else
                {
                    result = default!;
                    return false;
                }
            }
        }

        public static T GetResultWhileDoingMessageLoopEvents<T>(this Task<T> task, ModalExecutionBlock? modalExecutionBlock = null)
        {
            if (task.IsCompleted)
            {
                return task.Result;
            }

            if (Application.MessageLoop)
            {
                modalExecutionBlock ??= ModalExecutionBlock.Default;

                using (modalExecutionBlock.Enter())
                {
                    do
                    {
                        Application.DoEvents();
                    }
                    while (!task.IsCompleted);
                }
            }

            return task.Result;
        }

        public static T GetResultWhileDoingMessageLoopEvents<T>(this in ValueTask<T> valueTask, ModalExecutionBlock? modalExecutionBlock = null)
        {
            if (valueTask.IsCompleted)
            {
                return valueTask.Result;
            }

            if (Application.MessageLoop)
            {
                modalExecutionBlock ??= ModalExecutionBlock.Default;

                using (modalExecutionBlock.Enter())
                {
                    do
                    {
                        Application.DoEvents();
                    }
                    while (!valueTask.IsCompleted);
                }
            }

            return valueTask.Result;
        }
    }
}
