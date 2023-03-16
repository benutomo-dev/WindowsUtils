using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsControls.Aysnc.Forms
{
    public static class AsyncExtensions
    {
        /// <summary>
        /// 現在のスレッドのメッセージループで<paramref name="task"/>を待機する。現在のスレッドにメッセージループが存在しない場合は、<see cref="Task.Wait(int)"/>で待機する。
        /// </summary>
        /// <param name="task">待機対象のタスク</param>
        /// <param name="millisecondsTimeout">タイムアウト値</param>
        /// <returns>待機がタイムアウトした場合はfalse</returns>
        public static bool WaitOnMassageLoop(this Task task, int millisecondsTimeout = -1)
        {
            if (task.IsCompleted)
            {
                return true;
            }

            var timer = Stopwatch.StartNew();
            if (Application.MessageLoop)
            {
                var currentExecutionContext = ExecutionContext.Capture();

                if (currentExecutionContext is null)
                {
                    do
                    {
                        Windows.Win32.PInvoke.WaitMessage();
                        Application.DoEvents();
                    }
                    while (!task.IsCompleted && (millisecondsTimeout == -1 || timer.ElapsedMilliseconds < millisecondsTimeout));
                }
                else
                {
                    // Application.DoEvents()を実行するとExecutionContextが消失するため、
                    // ExecutionContext.Run内で実行して、実行元のExecutionContextを保護する
                    ExecutionContext.Run(currentExecutionContext, _ =>
                    {
                        do
                        {
                            Windows.Win32.PInvoke.WaitMessage();
                            Application.DoEvents();
                        }
                        while (!task.IsCompleted && (millisecondsTimeout == -1 || timer.ElapsedMilliseconds < millisecondsTimeout));
                    }, null);
                }

                if (task.IsCanceled || task.IsFaulted)
                {
                    task.Wait();
                }

                return task.IsCompleted;
            }
            else
            {
                return task.Wait(millisecondsTimeout);
            }
        }

        /// <summary>
        /// 現在のスレッドのメッセージループで<paramref name="valueTask"/>を待機する。現在のスレッドにメッセージループが存在しない場合は、<see cref="Task.Wait(int)"/>で待機する。
        /// </summary>
        /// <param name="valueTask">待機対象のタスク</param>
        /// <param name="millisecondsTimeout">タイムアウト値</param>
        /// <returns>待機がタイムアウトした場合はfalse</returns>
        public static bool WaitOnMassageLoop(this ValueTask valueTask, int millisecondsTimeout = -1)
        {
            if (valueTask.IsCompleted)
            {
                return true;
            }

            return valueTask.AsTask().WaitOnMassageLoop(millisecondsTimeout);
        }

        /// <summary>
        /// 現在のスレッドのメッセージループで<paramref name="valueTask"/>を待機する。現在のスレッドにメッセージループが存在しない場合は、<see cref="Task.Wait(int)"/>で待機する。
        /// </summary>
        /// <param name="valueTask">待機対象のタスク</param>
        /// <param name="result"><see cref="ValueTask{T}.Result"/></param>
        /// <param name="millisecondsTimeout">タイムアウト値</param>
        /// <returns>待機がタイムアウトした場合はfalse</returns>
        public static bool WaitOnMassageLoop<T>(this ValueTask<T> valueTask, [MaybeNullWhen(false)] out T result, int millisecondsTimeout = -1)
        {
            if (valueTask.IsCompleted)
            {
                result = valueTask.Result;
                return true;
            }

            var task = valueTask.AsTask();

            var waitResult = task.WaitOnMassageLoop(millisecondsTimeout);

            result = waitResult ? task.Result : default;

            return waitResult;
        }

        /// <summary>
        /// 現在のスレッドのメッセージループで<paramref name="task"/>を待機して<paramref name="task"/>の戻り値を返す。現在のスレッドにメッセージループが存在しない場合は、<see cref="Task.Wait(int)"/>で待機する。
        /// </summary>
        /// <param name="task">待機対象のタスク</param>
        /// <returns><see cref="Task{T}.Result"/></returns>
        public static T GetResultOnMassageLoop<T>(this Task<T> task)
        {
            if (task.IsCompleted)
            {
                return task.Result;
            }

            task.WaitOnMassageLoop();

            return task.Result;
        }

        /// <summary>
        /// 現在のスレッドのメッセージループで<paramref name="valueTask"/>を待機して<paramref name="valueTask"/>の戻り値を返す。現在のスレッドにメッセージループが存在しない場合は、<see cref="Task.Wait(int)"/>で待機する。
        /// </summary>
        /// <param name="valueTask">待機対象のタスク</param>
        /// <returns><see cref="ValueTask{T}.Result"/></returns>
        public static T GetResultOnMassageLoop<T>(this ValueTask<T> valueTask)
        {
            if (valueTask.IsCompleted)
            {
                return valueTask.Result;
            }

            var task = valueTask.AsTask();

            task.WaitOnMassageLoop();

            return task.Result;
        }

        /// <summary>
        /// 現在のスレッドのメッセージループで<paramref name="executionBlock"/>に入って<paramref name="task"/>を待機する。現在のスレッドにメッセージループが存在しない場合は、<see cref="Task.Wait(int)"/>で待機する。
        /// </summary>
        /// <param name="task">待機対象のタスク</param>
        /// <param name="millisecondsTimeout">タイムアウト値</param>
        /// <returns>待機がタイムアウトした場合はfalse</returns>
        public static bool WaitOnMessageLoop(this ModalExecutionBlock executionBlock, Task task, int millisecondsTimeout = -1)
        {
            using (executionBlock.Enter())
            {
                return task.WaitOnMassageLoop(millisecondsTimeout);
            }
        }

        /// <summary>
        /// 現在のスレッドのメッセージループで<paramref name="executionBlock"/>に入って<paramref name="valueTask"/>を待機する。現在のスレッドにメッセージループが存在しない場合は、<see cref="Task.Wait(int)"/>で待機する。
        /// </summary>
        /// <param name="valueTask">待機対象のタスク</param>
        /// <param name="millisecondsTimeout">タイムアウト値</param>
        /// <returns>待機がタイムアウトした場合はfalse</returns>
        public static bool WaitOnMessageLoop(this ModalExecutionBlock executionBlock, ValueTask valueTask, int millisecondsTimeout = -1)
        {
            using (executionBlock.Enter())
            {
                return valueTask.WaitOnMassageLoop(millisecondsTimeout);
            }
        }

        /// <summary>
        /// 現在のスレッドのメッセージループで<paramref name="executionBlock"/>に入って<paramref name="valueTask"/>を待機する。現在のスレッドにメッセージループが存在しない場合は、<see cref="Task.Wait(int)"/>で待機する。
        /// </summary>
        /// <param name="valueTask">待機対象のタスク</param>
        /// <param name="result"><see cref="ValueTask{T}.Result"/></param>
        /// <param name="millisecondsTimeout">タイムアウト値</param>
        /// <returns>待機がタイムアウトした場合はfalse</returns>
        public static bool WaitOnMessageLoop<T>(this ModalExecutionBlock executionBlock, ValueTask<T> valueTask, [MaybeNullWhen(false)] out T result, int millisecondsTimeout = -1)
        {
            using (executionBlock.Enter())
            {
                return valueTask.WaitOnMassageLoop(out result, millisecondsTimeout);
            }
        }

        /// <summary>
        /// 現在のスレッドのメッセージループで<paramref name="executionBlock"/>に入って<paramref name="task"/>を待機して<paramref name="task"/>の戻り値を返す。現在のスレッドにメッセージループが存在しない場合は、<see cref="Task.Wait(int)"/>で待機する。
        /// </summary>
        /// <param name="task">待機対象のタスク</param>
        /// <returns><see cref="ValueTask{T}.Result"/></returns>
        public static T GetResultOnMessageLoop<T>(this ModalExecutionBlock executionBlock, Task<T> task)
        {
            using (executionBlock.Enter())
            {
                return task.GetResultOnMassageLoop();
            }
        }

        /// <summary>
        /// 現在のスレッドのメッセージループで<paramref name="executionBlock"/>に入って<paramref name="valueTask"/>を待機して<paramref name="valueTask"/>の戻り値を返す。現在のスレッドにメッセージループが存在しない場合は、<see cref="Task.Wait(int)"/>で待機する。
        /// </summary>
        /// <param name="valueTask">待機対象のタスク</param>
        /// <returns><see cref="ValueTask{T}.Result"/></returns>
        public static T GetResultOnMessageLoop<T>(this ModalExecutionBlock executionBlock, ValueTask<T> valueTask)
        {
            using (executionBlock.Enter())
            {
                return valueTask.GetResultOnMassageLoop();
            }
        }


        /// <summary>
        /// <paramref name="executionBlock"/>に入って<paramref name="asyncTaskCallback"/>を実行し、現在のスレッドのメッセージループで完了を待機する。現在のスレッドにメッセージループが存在しない場合は、<see cref="Task.Wait(int)"/>で待機する。
        /// </summary>
        /// <param name="asyncTaskCallback">待機対象のコールバック処理</param>
        public static void RunOnMessageLoop(this ModalExecutionBlock executionBlock, Func<ValueTask> asyncTaskCallback)
        {
            using (executionBlock.Enter())
            {
                asyncTaskCallback().WaitOnMassageLoop();
            }
        }

        /// <summary>
        /// <paramref name="executionBlock"/>に入って<paramref name="asyncTaskCallback"/>を実行し、現在のスレッドのメッセージループで完了を待機する。現在のスレッドにメッセージループが存在しない場合は、<see cref="Task.Wait(int)"/>で待機する。
        /// </summary>
        /// <param name="asyncTaskCallback">待機対象のコールバック処理</param>
        /// <returns><paramref name="asyncTaskCallback"/>の戻り値</returns>
        public static T RunOnMessageLoop<T>(this ModalExecutionBlock executionBlock, Func<ValueTask<T>> asyncTaskCallback)
        {
            using (executionBlock.Enter())
            {
                return asyncTaskCallback().GetResultOnMassageLoop();
            }
        }


        [Obsolete("ModalExecutionBlock.Default.WaitTaskOnMessageLoop(task, [millisecondsTimeout]) の形に置き換える")]
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
                    var currentExecutionContext = ExecutionContext.Capture();

                    if (currentExecutionContext is null)
                    {
                        do
                        {
                            Windows.Win32.PInvoke.WaitMessage();
                            Application.DoEvents();
                        }
                        while (!task.IsCompleted && (millisecondsTimeout == -1 || timer.ElapsedMilliseconds < millisecondsTimeout));
                    }
                    else
                    {
                        // Application.DoEvents()を実行するとExecutionContextが消失するため、
                        // ExecutionContext.Run内で実行して、実行元のExecutionContextを保護する
                        ExecutionContext.Run(currentExecutionContext, _ =>
                        {
                            do
                            {
                                Windows.Win32.PInvoke.WaitMessage();
                                Application.DoEvents();
                            }
                            while (!task.IsCompleted && (millisecondsTimeout == -1 || timer.ElapsedMilliseconds < millisecondsTimeout));
                        }, null);
                    }

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

        [Obsolete("ModalExecutionBlock.Default.WaitTaskOnMessageLoop(valueTask, [millisecondsTimeout]) の形に置き換える")]
        public static bool WaitWhileDoingMessageLoopEvents(this in ValueTask valueTask, int millisecondsTimeout = -1, ModalExecutionBlock? modalExecutionBlock = null)
        {
            if (valueTask.IsCompleted)
            {
                return true;
            }

            return valueTask.AsTask().WaitWhileDoingMessageLoopEvents(millisecondsTimeout, modalExecutionBlock);
        }

        [Obsolete("ModalExecutionBlock.Default.WaitTaskOnMessageLoop(valueTask, [millisecondsTimeout]) の形に置き換える")]
        public static bool WaitWhileDoingMessageLoopEvents<T>(this in ValueTask<T> valueTask, out T result, int millisecondsTimeout = -1, ModalExecutionBlock? modalExecutionBlock = null)
        {
            if (valueTask.IsCompleted)
            {
                result = valueTask.Result;
                return true;
            }

            var task = valueTask.AsTask();

            task.WaitWhileDoingMessageLoopEvents(millisecondsTimeout, modalExecutionBlock);

            if (task.IsCompleted)
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

        [Obsolete("ModalExecutionBlock.Default.GetResultOnMessageLoop(task, [millisecondsTimeout]) の形に置き換える")]
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
                        Windows.Win32.PInvoke.WaitMessage();
                        Application.DoEvents();
                    }
                    while (!task.IsCompleted);
                }
            }

            return task.Result;
        }

        [Obsolete("ModalExecutionBlock.Default.GetResultOnMessageLoop(valueTask, [millisecondsTimeout]) の形に置き換える")]
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
                        Windows.Win32.PInvoke.WaitMessage();
                        Application.DoEvents();
                    }
                    while (!valueTask.IsCompleted);
                }
            }

            return valueTask.Result;
        }
    }
}
