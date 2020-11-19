using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gdi32Fonts
{
    public enum RegionFlags
    {
        ERROR,
        NULLREGION,
        SIMPLEREGION,
        COMPLEXREGION
    }

    internal enum RegionCombineMode
    {
        AND = 1,
        OR,
        XOR,
        DIFF,
        COPY,
        MIN = 1,
        MAX = 5
    }

    internal sealed class WindowsRegion : MarshalByRefObject, ICloneable, IDisposable
    {
        public WindowsRegionSafeHandle HRegion { get; set; }

        public bool IsInfinite
        {
            get
            {
                return HRegion == null || HRegion.IsInvalid;
            }
        }

        private WindowsRegion(WindowsRegionSafeHandle hRegion)
        {
            HRegion = hRegion;
        }

        public WindowsRegion(Rectangle rect)
        {
            HRegion = CreateRectRgn(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
        }

        public WindowsRegion(int x, int y, int width, int height)
            : this(new Rectangle(x, y, width, height))
        {
        }

        public static WindowsRegion FromHregion(WindowsRegionSafeHandle hRegion)
        {
            return new WindowsRegion(hRegion);
        }

        public static WindowsRegion FromRegion(Region region, Graphics g)
        {
            if (region.IsInfinite(g))
            {
                return new WindowsRegion(new WindowsRegionSafeHandle(IntPtr.Zero, false));
            }
            else
            {
                return new WindowsRegion(new WindowsRegionSafeHandle(region.GetHrgn(g), true));
            }
        }

        public object Clone()
        {
            if (!IsInfinite)
            {
                return new WindowsRegion(this.ToRectangle());
            }
            return new WindowsRegion(new WindowsRegionSafeHandle(IntPtr.Zero, false));
        }

        public RegionFlags CombineRegion(WindowsRegion region1, WindowsRegion region2, RegionCombineMode mode)
        {
            return CombineRgn(HRegion, region1.HRegion, region2.HRegion, mode);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            if (HRegion != null)
            {
                HRegion.Dispose();
                HRegion = null;

                if (disposing)
                {
                    GC.SuppressFinalize(this);
                }
            }
        }

        ~WindowsRegion()
        {
            this.Dispose(false);
        }

        public Rectangle ToRectangle()
        {
            if (IsInfinite)
            {
                return new Rectangle(-int.MaxValue, -int.MaxValue, int.MaxValue, int.MaxValue);
            }

            RECT rECT = default(RECT);
            GetRgnBox(HRegion, ref rECT);

            return rECT;
        }


        public class WindowsRegionSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            internal WindowsRegionSafeHandle() : base(true)
            {
            }

            internal WindowsRegionSafeHandle(IntPtr handleValue, bool ownsHandle) : base(ownsHandle)
            {
                SetHandle(handleValue);
            }

            protected override bool ReleaseHandle()
            {
                return DeleteObject(handle);
            }
        }


        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        private static extern RegionFlags CombineRgn(WindowsRegionSafeHandle hRgnDest, WindowsRegionSafeHandle hRgnSrc1, WindowsRegionSafeHandle hRgnSrc2, RegionCombineMode combineMode);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        private static extern WindowsRegionSafeHandle CreateRectRgn(int x1, int y1, int x2, int y2);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        public static extern RegionFlags GetRgnBox(WindowsRegionSafeHandle hRgn, [In] [Out] ref RECT clipRect);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);

    }
}
