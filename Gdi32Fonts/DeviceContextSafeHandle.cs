using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gdi32Fonts
{
    internal class DeviceContextSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal DeviceContextSafeHandle() : base(true)
        {
        }

        public static DeviceContextSafeHandle CreateMesuremetnDeviceContext()
        {
            return CreateCompatibleDC(new HandleRef(null, IntPtr.Zero));
        }

        protected override bool ReleaseHandle()
        {
            return DeleteDC(handle);
        }

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        private static extern DeviceContextSafeHandle CreateCompatibleDC(HandleRef hDC);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        private static extern bool DeleteDC(IntPtr hDC);
    }
}
