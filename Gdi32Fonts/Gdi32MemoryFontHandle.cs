using System;
using System.Runtime.InteropServices;

namespace Gdi32Fonts
{
    public class Gdi32MemoryFontHandle : IDisposable
    {
        FontResourceSafeHandle Handle { get; set; }

        private Gdi32MemoryFontHandle(FontResourceSafeHandle handle)
        {
            Handle = handle;
        }

        public static Gdi32MemoryFontHandle Install(IntPtr fontData, int length)
        {
            return new Gdi32MemoryFontHandle(FontResourceSafeHandle.AddMemoryFontResource(fontData, (uint)length));
        }

        public static Gdi32MemoryFontHandle Install(byte[] fontData)
        {
            return Install(fontData, 0, fontData.Length);
        }

        public static Gdi32MemoryFontHandle Install(byte[] fontData, int index, int length)
        {
            if (fontData == null)
            {
                throw new ArgumentNullException(nameof(fontData));
            }

            if (fontData.Length <= index || index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (fontData.Length > length || length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if (fontData.Length > index + length)
            {
                throw new ArgumentException();
            }

            GCHandle handle = GCHandle.Alloc(fontData, GCHandleType.Pinned);
            try
            {
                IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(fontData, index);
                return Install(ptr, length);
            }
            finally
            {
                handle.Free();
            }
        }

        public bool IsActive => !Handle.IsClosed;

        public void Dispose()
        {
            ((IDisposable)Handle).Dispose();
        }
    }
}
