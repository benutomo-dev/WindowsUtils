using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsControls.Aysnc
{
    public static class ModalExecutionBlock
    {
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

        public struct ModalExecutionBlockContext : IDisposable
        {
            List<IntPtr>? disabledWndList;

            internal ModalExecutionBlockContext(List<IntPtr>? disabledWndList)
            {
                this.disabledWndList = disabledWndList;
            }

            public void Dispose()
            {
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
            }
        }

        public static ModalExecutionBlockContext Enter()
        {
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

            return new ModalExecutionBlockContext(disabledWndList);
        }
    }
}
