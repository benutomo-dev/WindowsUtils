using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gdi32Fonts
{
    internal class FontResourceSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal FontResourceSafeHandle() : base(true)
        {
        }

        public static FontResourceSafeHandle AddMemoryFontResource(IntPtr fontData, uint fontDataLength)
        {
            return AddFontMemResourceEx(fontData, fontDataLength, IntPtr.Zero, out uint installedFontCount);
        }

        protected override bool ReleaseHandle()
        {
            return RemoveFontMemResourceEx(handle);
        }

        [DllImport("gdi32.dll")]
        private static extern FontResourceSafeHandle AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, out uint pcFonts);

        [DllImport("gdi32.dll")]
        private static extern bool RemoveFontMemResourceEx(IntPtr fh);
    }
}
