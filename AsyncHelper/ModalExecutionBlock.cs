using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsControls.Aysnc
{
    public class ModalExecutionBlock
    {
        public static ModalExecutionBlock Default = new ModalExecutionBlock();

        public event Action? Entered;
        public event Action? Exited;

        [DllImport("kernel32.dll", ExactSpelling = true)]
        static extern uint GetCurrentThreadId();

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        delegate bool EnumWindowsProc([In] IntPtr hwnd, [In] IntPtr lParam);

        [DllImport("user32.dll", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumThreadWindows(uint dwThreadId, EnumWindowsProc lpfn, IntPtr lParam);

        [DllImport("user32.dll", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowEnabled(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnableWindow(IntPtr hWnd, [MarshalAs(UnmanagedType.Bool)] bool bEnable);

        static ThreadLocal<int> SharedNestCount = new ThreadLocal<int>();

        int SelfNestCount;

        public static bool IsEnteredAnyone => SharedNestCount.Value > 0;

        public bool IsEntered => SelfNestCount > 0;

        public struct ModalExecutionBlockContext : IDisposable
        {
            ModalExecutionBlock owner;
            List<IntPtr>? disabledWndList;

            internal ModalExecutionBlockContext(ModalExecutionBlock owner, List<IntPtr>? disabledWndList)
            {
                this.owner = owner;
                this.disabledWndList = disabledWndList;
            }

            public void Dispose()
            {
                Debug.Assert(SharedNestCount.Value > 0);
                Debug.Assert(owner.SelfNestCount > 0);

                if (disabledWndList is { })
                {
                    foreach (var hwnd in disabledWndList)
                    {
                        if (IsWindow(hwnd))
                        {
                            EnableWindow(hwnd, true);
                        }
                    }
                    disabledWndList.Clear();
                }


                if (Interlocked.Decrement(ref owner.SelfNestCount) == 0)
                {
                    owner.Exited?.Invoke();
                }
                SharedNestCount.Value--;
            }
        }

        public ModalExecutionBlockContext Enter()
        {
            SharedNestCount.Value += 1;
            if (Interlocked.Increment(ref SelfNestCount) == 1)
            {
                Entered?.Invoke();
            }

            List<IntPtr>? disabledWndList = null;

            EnumThreadWindows(GetCurrentThreadId(), (IntPtr hwnd, IntPtr lParam) =>
            {
                if (IsWindowVisible(hwnd) && IsWindowEnabled(hwnd))
                {
                    disabledWndList ??= new List<IntPtr>(32);

                    EnableWindow(hwnd, false);
                    disabledWndList.Add(hwnd);
                }
                return true;
            }
            , IntPtr.Zero);

            return new ModalExecutionBlockContext(this, disabledWndList);
        }
    }
}
