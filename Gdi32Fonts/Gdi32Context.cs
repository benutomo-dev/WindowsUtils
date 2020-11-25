using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;

namespace Gdi32Fonts
{
    public class Gdi32Context : IDisposable
    {
        static ConcurrentDictionary<IntPtr, Gdi32Context> ActiveInstances = new ConcurrentDictionary<IntPtr, Gdi32Context>();

        Graphics? DeviceContext { get; set; }
        IntPtr HDC { get; set; }

        private Gdi32Context(Graphics? graphics, IntPtr hDC)
        {
            if (!ActiveInstances.TryAdd(hDC, this))
            {
                throw new InvalidOperationException("指定されたデバイスコンテキストは既に使用中です。");
            }

            DeviceContext = graphics;
            HDC = hDC;
        }


        public static Gdi32Context FromGraphics(Graphics graphics)
        {
            return FromGraphics(graphics, true, true);
        }

        public static Gdi32Context FromGraphics(Graphics graphics, bool preserveClipping, bool preserveTranslateTransform)
        {
            HandleRef handleRef = default(HandleRef);

            try
            {
                if (preserveClipping || preserveTranslateTransform)
                {
                    Region? region = null;
                    Matrix? matrix = null;
                    WindowsRegion? windowsRegion = null;

                    if (graphics.GetContextInfo() is object[] contextInfo && contextInfo.Length == 2)
                    {
                        region = (contextInfo[0] as Region);
                        matrix = (contextInfo[1] as Matrix);

                        if (region is { })
                        {
                            windowsRegion = WindowsRegion.FromRegion(region, graphics);
                        }
                    }

                    handleRef = new HandleRef(graphics, graphics.GetHdc());
                    SaveDC(handleRef);

                    try
                    {

                        if (preserveClipping && windowsRegion is { })
                        {
                            using (var existsRegion = new WindowsRegion(0, 0, 0, 0))
                            {
                                int clipRgn = GetClipRgn(handleRef, existsRegion.HRegion);
                                if (clipRgn == 1)
                                {
                                    windowsRegion.CombineRegion(existsRegion, windowsRegion, RegionCombineMode.AND);
                                }

                                SelectClipRgn(handleRef, windowsRegion.HRegion);
                            }
                        }

                        if (preserveTranslateTransform && matrix is { })
                        {
                            var matrixElements = matrix.Elements;

                            OffsetViewportOrgEx(handleRef, (int)matrixElements[4], (int)matrixElements[5], out POINT point);
                        }
                    }
                    finally
                    {
                        region?.Dispose();
                        matrix?.Dispose();
                        windowsRegion?.Dispose();
                    }
                }
                else
                {
                    handleRef = new HandleRef(graphics, graphics.GetHdc());
                    SaveDC(handleRef);
                }

                return new Gdi32Context(graphics, handleRef.Handle);
            }
            catch (Exception)
            {
                if (handleRef.Handle != IntPtr.Zero)
                {
                    RestoreDC(handleRef, -1);
                    graphics.ReleaseHdc();
                }

                throw;
            }
        }

        public static Gdi32Context FromHdc(IntPtr hdc)
        {
            return new Gdi32Context(null, hdc);
        }

        public void DrawText(string text, Gdi32Font font, Point point, Color foreColor, Color backColor, Gdi32TextFormatFlags flags)
        {
            DrawText(text, font, new Rectangle(point, new Size(int.MaxValue, int.MaxValue)), foreColor, backColor, flags);
        }

        public void DrawText(string text, Gdi32Font font, Rectangle bounds, Color foreColor, Color backColor, Gdi32TextFormatFlags flags)
        {
            if (string.IsNullOrEmpty(text) || font == null || foreColor == Color.Transparent)
            {
                return;
            }

            var handleRef = new HandleRef(this, HDC);
            SaveDC(handleRef);

            try
            {

                SetTextAlign(handleRef, (int)DeviceContextTextAlignment.Top);


                if (!foreColor.IsEmpty)
                {
                    SetTextColor(handleRef, ColorTranslator.ToWin32(foreColor));
                }

                SelectObject(handleRef, font.Handle);



                DeviceContextBackgroundMode deviceContextBackgroundMode = (backColor.IsEmpty || backColor == Color.Transparent) ? DeviceContextBackgroundMode.Transparent : DeviceContextBackgroundMode.Opaque;
                SetBkMode(handleRef, (int)deviceContextBackgroundMode);

                if (deviceContextBackgroundMode != DeviceContextBackgroundMode.Transparent)
                {
                    SetBkColor(handleRef, ColorTranslator.ToWin32(backColor));
                }

                var textMargins = new DRAWTEXTPARAMS(0, 0);

                bounds = AdjustForVerticalAlignment(handleRef, text, bounds, flags, textMargins);
                if (bounds.Width == int.MaxValue)
                {
                    bounds.Width -= bounds.X;
                }
                if (bounds.Height == int.MaxValue)
                {
                    bounds.Height -= bounds.Y;
                }
                RECT rECT = new RECT(bounds);

                DrawTextExW(handleRef, text, text.Length, ref rECT, (int)flags, textMargins);
            }
            finally
            {
                RestoreDC(handleRef, -1);
            }
        }

        private static Rectangle AdjustForVerticalAlignment(HandleRef hdc, string text, Rectangle bounds, Gdi32TextFormatFlags flags, DRAWTEXTPARAMS dtparams)
        {
            if (((flags & Gdi32TextFormatFlags.Bottom) == Gdi32TextFormatFlags.Default && (flags & Gdi32TextFormatFlags.VerticalCenter) == Gdi32TextFormatFlags.Default)
                || (flags & Gdi32TextFormatFlags.SingleLine) != Gdi32TextFormatFlags.Default
                || (flags & Gdi32TextFormatFlags.CalculateRectangle) != Gdi32TextFormatFlags.Default)
            {
                return bounds;
            }

            RECT rECT = new RECT(bounds);

            flags |= Gdi32TextFormatFlags.CalculateRectangle;
            int num = DrawTextExW(hdc, text, text.Length, ref rECT, (int)flags, dtparams);
            if (num > bounds.Height)
            {
                return bounds;
            }
            Rectangle result = bounds;
            if ((flags & Gdi32TextFormatFlags.VerticalCenter) != Gdi32TextFormatFlags.Default)
            {
                result.Y = result.Top + result.Height / 2 - num / 2;
            }
            else
            {
                result.Y = result.Bottom - num;
            }
            return result;
        }


        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                }

                var handleRef = new HandleRef(this, HDC);
                RestoreDC(handleRef, -1);

                if (!ActiveInstances.TryRemove(HDC, out Gdi32Context savedInstance))
                {
                    Debug.Fail("ActiveInstanceの登録が想定外のタイミングで解除されている");
                }

                Debug.Assert(ReferenceEquals(this, savedInstance), "ActiveInstanceの登録が想定外のタイミングで上書きされている");

                DeviceContext?.ReleaseHdc();
                DeviceContext = null;
                HDC = IntPtr.Zero;

                disposedValue = true;
            }
        }

        ~Gdi32Context() {
            Debug.Fail("using文を使わずにGDI32が使用されている。");
            Dispose(false);
        }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        #endregion





        private enum DeviceContextTextAlignment
        {
            BaseLine         = 24,
            Bottom           = 8,
            Top              = 0,
            Center           = 6,
            Default          = 0,
            Left             = 0,
            Right            = 2,
            RtlReading       = 256,
            NoUpdateCP       = 0,
            UpdateCP         = 1,
            VerticalBaseLine = 2,
            VerticalCenter   = 3
        }

        private enum DeviceContextBackgroundMode
        {
            Transparent = 1,
            Opaque      = 2,
        }


        [StructLayout(LayoutKind.Sequential)]
        private class DRAWTEXTPARAMS
        {
            private readonly int cbSize = Marshal.SizeOf(typeof(DRAWTEXTPARAMS));

            public int iTabLength;

            public int iLeftMargin;

            public int iRightMargin;

            public int uiLengthDrawn;

            public DRAWTEXTPARAMS()
            {
            }

            public DRAWTEXTPARAMS(DRAWTEXTPARAMS original)
            {
                this.iLeftMargin = original.iLeftMargin;
                this.iRightMargin = original.iRightMargin;
                this.iTabLength = original.iTabLength;
            }

            public DRAWTEXTPARAMS(int leftMargin, int rightMargin)
            {
                this.iLeftMargin = leftMargin;
                this.iRightMargin = rightMargin;
            }
        }


        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        private static extern int SaveDC(HandleRef hDC);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        private static extern bool RestoreDC(HandleRef hDC, int nSavedDC);


        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        private static extern int GetClipRgn(HandleRef hDC, WindowsRegion.WindowsRegionSafeHandle hRgn);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        private static extern RegionFlags SelectClipRgn(HandleRef hDC, WindowsRegion.WindowsRegionSafeHandle hRgn);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        private static extern bool OffsetViewportOrgEx(HandleRef hDC, int nXOffset, int nYOffset, out POINT point);


        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        private static extern int SetTextAlign(HandleRef hDC, int nMode);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        private static extern int SetTextColor(HandleRef hDC, int crColor);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr SelectObject(HandleRef hdc, FontSafeHandle obj);


        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        private static extern int SetBkMode(HandleRef hDC, int nBkMode);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        private static extern int SetBkColor(HandleRef hDC, int clr);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int DrawTextExW(HandleRef hDC, string lpszString, int nCount, ref RECT lpRect, int nFormat, [In] [Out] DRAWTEXTPARAMS lpDTParams);
    }
}
