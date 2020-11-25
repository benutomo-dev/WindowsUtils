using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gdi32Fonts
{
    internal class NativeApi
    {
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr SelectObject(DeviceContextSafeHandle hdc, FontSafeHandle obj);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        public static extern uint GetGlyphOutline(DeviceContextSafeHandle hdc, uint uChar, uint uFormat, out GLYPHMETRICS lpgm, uint cbBuffer, IntPtr lpvBuffer, ref MAT2 lpmat2);

        [DllImport("gdi32.dll", EntryPoint = "GetOutlineTextMetricsW", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        static extern uint GetOutlineTextMetrics(DeviceContextSafeHandle hdc, uint cbData, IntPtr lpOTM);

        [DllImport("gdi32.dll", EntryPoint = "GetTextMetricsW", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        static extern bool GetTextMetrics(DeviceContextSafeHandle hdc, out TEXTMETRIC lptm);

        [DllImport("gdi32.dll", EntryPoint = "GetCharacterPlacementW", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        static extern uint GetCharacterPlacement(DeviceContextSafeHandle hdc, [MarshalAs(UnmanagedType.LPWStr)] string lpString, int nCount, int nMaxExtent, ref GCP_RESULTS lpResults, uint dwFlags);

        [DllImport("gdi32.dll", EntryPoint = "GetTextExtentPoint32W", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        static extern bool GetTextExtentPoint32(DeviceContextSafeHandle hdc, string lpString, int cbString, out SIZE lpSize);


        /// <summary>
        /// デバイスコンテキストの現在のフォントのOutlineTextMetricを取得する。不正なハンドルや対象のフォントが字形を含まない場合はnullを返却する。
        /// </summary>
        /// <param name="hdc">デバイスコンテキスト</param>
        /// <param name="win32ErrorCode">エラーコード</param>
        /// <returns>OutlineTextMetric</returns>
        public static bool TryGetOutlineTextMetrics(DeviceContextSafeHandle hdc, out OutlineTextMetric outlineTextMetric, out int win32ErrorCode)
        {
            uint bufferSize = GetOutlineTextMetrics(hdc, 0, IntPtr.Zero);

            if (bufferSize == 0)
            {
                outlineTextMetric = default;
                win32ErrorCode = Marshal.GetLastWin32Error();
                return false;
            }

            IntPtr buffer = Marshal.AllocHGlobal((int)bufferSize);

            try
            {
                var getOutlineTextMetricsResult = GetOutlineTextMetrics(hdc, bufferSize, buffer);

                if (getOutlineTextMetricsResult == 0)
                {
                    outlineTextMetric = default;
                    win32ErrorCode = Marshal.GetLastWin32Error();
                    return false;
                }

                OUTLINETEXTMETRIC nativeOutlineTextMetrics = (OUTLINETEXTMETRIC)Marshal.PtrToStructure(buffer, typeof(OUTLINETEXTMETRIC));

                outlineTextMetric = new OutlineTextMetric
                {
                    otmSize = nativeOutlineTextMetrics.otmSize,
                    otmTextMetrics = nativeOutlineTextMetrics.otmTextMetrics,
                    otmFiller = nativeOutlineTextMetrics.otmFiller,
                    otmPanoseNumber = nativeOutlineTextMetrics.otmPanoseNumber,
                    otmfsSelection = nativeOutlineTextMetrics.otmfsSelection,
                    otmfsType = nativeOutlineTextMetrics.otmfsType,
                    otmsCharSlopeRise = nativeOutlineTextMetrics.otmsCharSlopeRise,
                    otmsCharSlopeRun = nativeOutlineTextMetrics.otmsCharSlopeRun,
                    otmItalicAngle = nativeOutlineTextMetrics.otmItalicAngle,
                    otmEMSquare = nativeOutlineTextMetrics.otmEMSquare,
                    otmAscent = nativeOutlineTextMetrics.otmAscent,
                    otmDescent = nativeOutlineTextMetrics.otmDescent,
                    otmLineGap = nativeOutlineTextMetrics.otmLineGap,
                    otmsCapEmHeight = nativeOutlineTextMetrics.otmsCapEmHeight,
                    otmsXHeight = nativeOutlineTextMetrics.otmsXHeight,
                    otmrcFontBox = nativeOutlineTextMetrics.otmrcFontBox,
                    otmMacAscent = nativeOutlineTextMetrics.otmMacAscent,
                    otmMacDescent = nativeOutlineTextMetrics.otmMacDescent,
                    otmMacLineGap = nativeOutlineTextMetrics.otmMacLineGap,
                    otmusMinimumPPEM = nativeOutlineTextMetrics.otmusMinimumPPEM,
                    otmptSubscriptSize = nativeOutlineTextMetrics.otmptSubscriptSize.ToPoint(),
                    otmptSubscriptOffset = nativeOutlineTextMetrics.otmptSubscriptOffset.ToPoint(),
                    otmptSuperscriptSize = nativeOutlineTextMetrics.otmptSuperscriptSize.ToPoint(),
                    otmptSuperscriptOffset = nativeOutlineTextMetrics.otmptSuperscriptOffset.ToPoint(),
                    otmsStrikeoutSize = nativeOutlineTextMetrics.otmsStrikeoutSize,
                    otmsStrikeoutPosition = nativeOutlineTextMetrics.otmsStrikeoutPosition,
                    otmsUnderscoreSize = nativeOutlineTextMetrics.otmsUnderscoreSize,
                    otmsUnderscorePosition = nativeOutlineTextMetrics.otmsUnderscorePosition,
                    otmpFamilyName = Marshal.PtrToStringUni(new IntPtr(buffer.ToInt64() + nativeOutlineTextMetrics.otmpFamilyName.ToInt64())),
                    otmpFaceName = Marshal.PtrToStringUni(new IntPtr(buffer.ToInt64() + nativeOutlineTextMetrics.otmpFaceName.ToInt64())),
                    otmpStyleName = Marshal.PtrToStringUni(new IntPtr(buffer.ToInt64() + nativeOutlineTextMetrics.otmpStyleName.ToInt64())),
                    otmpFullName = Marshal.PtrToStringUni(new IntPtr(buffer.ToInt64() + nativeOutlineTextMetrics.otmpFullName.ToInt64())),
                };

                Debug.Assert(outlineTextMetric.otmSize == bufferSize);

                win32ErrorCode = 0;
                return true;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }

        }

        public static TEXTMETRIC GetTextMetrics(DeviceContextSafeHandle hdc)
        {
            if (GetTextMetrics(hdc, out TEXTMETRIC textmetric))
            {
                return textmetric;
            }

            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        public static void GetCharacterPlacement(DeviceContextSafeHandle hdc, string text, GCPFlags flags, out GcpResults gcpResults)
        {
            int len = text.Length;

            gcpResults = new GcpResults
            {
                Order = new int[len],
                Dx = new int[len],
                CaretPos = new int[len],
                Class = new byte[len],
                Glyphs = new short[len],
            };

            GCHandle ordHnd = GCHandle.Alloc(gcpResults.Order,    GCHandleType.Pinned);
            GCHandle dxHnd  = GCHandle.Alloc(gcpResults.Dx,       GCHandleType.Pinned);
            GCHandle carHnd = GCHandle.Alloc(gcpResults.CaretPos, GCHandleType.Pinned);
            GCHandle clsHnd = GCHandle.Alloc(gcpResults.Class,    GCHandleType.Pinned);
            GCHandle glyHnd = GCHandle.Alloc(gcpResults.Glyphs,   GCHandleType.Pinned);

            try
            {
                GCP_RESULTS rs = new GCP_RESULTS
                {
                    StructSize = Marshal.SizeOf(typeof(GCP_RESULTS)),

                    OutString = new String('\0', len + 2),
                    Order = ordHnd.AddrOfPinnedObject(),
                    Dx = dxHnd.AddrOfPinnedObject(),
                    CaretPos = carHnd.AddrOfPinnedObject(),
                    Class = clsHnd.AddrOfPinnedObject(),
                    Glyphs = glyHnd.AddrOfPinnedObject(),
                    GlyphCount = len,

                    MaxFit = 0
                };

                uint r = GetCharacterPlacement(hdc, text, len, 0, ref rs, (uint)flags);
                if (r == 0 && Marshal.GetLastWin32Error() != 0)
                {
                    throw new Win32Exception();
                }

                gcpResults.OutString = rs.OutString;
                gcpResults.MaxFit = rs.MaxFit;

                return;
            }
            finally
            {
                ordHnd.Free();
                dxHnd.Free();
                carHnd.Free();
                clsHnd.Free();
                glyHnd.Free();
            }
        }

        public static Size GetTextExtentPoint(DeviceContextSafeHandle hdc, string text)
        {
            if (GetTextExtentPoint32(hdc, text, text.Length, out var size))
            {
                return size.ToSize();
            }
            else
            {
                return Size.Empty;
            }
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    struct FIXED
    {
        public short fract;
        public short value;

        public float ToFloat() => value + fract / 65536.0f;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct POINTFX
    {

        [MarshalAs(UnmanagedType.Struct)] public FIXED x;
        [MarshalAs(UnmanagedType.Struct)] public FIXED y;

        public PointF ToPointF(in OutlineTextMetric outlineTextMetric, GLYPHMETRICS metrics) => new PointF(
            x.ToFloat(),
            outlineTextMetric.otmEMSquare - outlineTextMetric.otmDescent - y.ToFloat()
            );

        public TtPolygonPoint ToTtPolygonPoint(in OutlineTextMetric outlineTextMetric, GLYPHMETRICS metrics) => new TtPolygonPoint(
            x.value,
            (short)(outlineTextMetric.otmEMSquare + outlineTextMetric.otmDescent - y.value)
            );
    }

    [StructLayout(LayoutKind.Sequential)]
    struct TTPOLYGONHEADER

    {

        public int cb;
        public int dwType;
        [MarshalAs(UnmanagedType.Struct)] public POINTFX pfxStart;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct TTPOLYCURVEHEADER
    {

        public short wType;
        public short cpfx;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct POINT
    {

        public int x;
        public int y;

        public Point ToPoint() => new Point(x, y);
    }

    [StructLayout(LayoutKind.Sequential)]
    struct SIZE
    {

        public int cx;
        public int cy;

        public Size ToSize() => new Size(cx, cy);
    }

    [StructLayout(LayoutKind.Sequential)]
    struct GLYPHMETRICS
    {

        public int gmBlackBoxX;
        public int gmBlackBoxY;
        [MarshalAs(UnmanagedType.Struct)] public POINT gmptGlyphOrigin;
        public short gmCellIncX;
        public short gmCellIncY;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct MAT2
    {

        [MarshalAs(UnmanagedType.Struct)] public FIXED eM11;
        [MarshalAs(UnmanagedType.Struct)] public FIXED eM12;
        [MarshalAs(UnmanagedType.Struct)] public FIXED eM21;
        [MarshalAs(UnmanagedType.Struct)] public FIXED eM22;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    struct TEXTMETRIC
    {
        public int tmHeight;
        public int tmAscent;
        public int tmDescent;
        public int tmInternalLeading;
        public int tmExternalLeading;
        public int tmAveCharWidth;
        public int tmMaxCharWidth;
        public int tmWeight;
        public int tmOverhang;
        public int tmDigitizedAspectX;
        public int tmDigitizedAspectY;
        public char tmFirstChar;
        public char tmLastChar;
        public char tmDefaultChar;
        public char tmBreakChar;
        public byte tmItalic;
        public byte tmUnderlined;
        public byte tmStruckOut;
        public byte tmPitchAndFamily;
        public byte tmCharSet;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct RECT
    {
        public int Left, Top, Right, Bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

        public int X
        {
            get { return Left; }
            set { Right -= (Left - value); Left = value; }
        }

        public int Y
        {
            get { return Top; }
            set { Bottom -= (Top - value); Top = value; }
        }

        public int Height
        {
            get { return Bottom - Top; }
            set { Bottom = value + Top; }
        }

        public int Width
        {
            get { return Right - Left; }
            set { Right = value + Left; }
        }

        public System.Drawing.Point Location
        {
            get { return new System.Drawing.Point(Left, Top); }
            set { X = value.X; Y = value.Y; }
        }

        public System.Drawing.Size Size
        {
            get { return new System.Drawing.Size(Width, Height); }
            set { Width = value.Width; Height = value.Height; }
        }

        public static implicit operator System.Drawing.Rectangle(RECT r)
        {
            return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
        }

        public static implicit operator RECT(System.Drawing.Rectangle r)
        {
            return new RECT(r);
        }

        public static bool operator ==(RECT r1, RECT r2)
        {
            return r1.Equals(r2);
        }

        public static bool operator !=(RECT r1, RECT r2)
        {
            return !r1.Equals(r2);
        }

        public bool Equals(RECT r)
        {
            return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
        }

        public override bool Equals(object obj)
        {
            if (obj is RECT)
                return Equals((RECT)obj);
            else if (obj is System.Drawing.Rectangle)
                return Equals(new RECT((System.Drawing.Rectangle)obj));
            return false;
        }

        public override int GetHashCode()
        {
            return ((System.Drawing.Rectangle)this).GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    struct PANOSE
    {
        public byte bFamilyType;
        public byte bSerifStyle;
        public byte bWeight;
        public byte bProportion;
        public byte bContrast;
        public byte bStrokeVariation;
        public byte bArmStyle;
        public byte bLetterform;
        public byte bMidline;
        public byte bXHeight;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    struct OUTLINETEXTMETRIC
    {
        public uint otmSize;
        [MarshalAs(UnmanagedType.Struct)] public TEXTMETRIC otmTextMetrics;
        public byte otmFiller;
        [MarshalAs(UnmanagedType.Struct)] public PANOSE otmPanoseNumber;
        public uint otmfsSelection;
        public uint otmfsType;
        public int otmsCharSlopeRise;
        public int otmsCharSlopeRun;
        public int otmItalicAngle;
        public uint otmEMSquare;
        public int otmAscent;
        public int otmDescent;
        public uint otmLineGap;
        public uint otmsCapEmHeight;
        public uint otmsXHeight;
        [MarshalAs(UnmanagedType.Struct)] public RECT otmrcFontBox;
        public int otmMacAscent;
        public int otmMacDescent;
        public uint otmMacLineGap;
        public uint otmusMinimumPPEM;
        [MarshalAs(UnmanagedType.Struct)] public POINT otmptSubscriptSize;
        [MarshalAs(UnmanagedType.Struct)] public POINT otmptSubscriptOffset;
        [MarshalAs(UnmanagedType.Struct)] public POINT otmptSuperscriptSize;
        [MarshalAs(UnmanagedType.Struct)] public POINT otmptSuperscriptOffset;
        public uint otmsStrikeoutSize;
        public int otmsStrikeoutPosition;
        public int otmsUnderscoreSize;
        public int otmsUnderscorePosition;
        public IntPtr otmpFamilyName;
        public IntPtr otmpFaceName;
        public IntPtr otmpStyleName;
        public IntPtr otmpFullName;
    }

    struct OutlineTextMetric
    {
        public uint otmSize;
        public TEXTMETRIC otmTextMetrics;
        public byte otmFiller;
        public PANOSE otmPanoseNumber;
        public uint otmfsSelection;
        public uint otmfsType;
        public int otmsCharSlopeRise;
        public int otmsCharSlopeRun;
        public int otmItalicAngle;
        public uint otmEMSquare;
        public int otmAscent;
        public int otmDescent;
        public uint otmLineGap;
        public uint otmsCapEmHeight;
        public uint otmsXHeight;
        public Rectangle otmrcFontBox;
        public int otmMacAscent;
        public int otmMacDescent;
        public uint otmMacLineGap;
        public uint otmusMinimumPPEM;
        public Point otmptSubscriptSize;
        public Point otmptSubscriptOffset;
        public Point otmptSuperscriptSize;
        public Point otmptSuperscriptOffset;
        public uint otmsStrikeoutSize;
        public int otmsStrikeoutPosition;
        public int otmsUnderscoreSize;
        public int otmsUnderscorePosition;
        public string otmpFamilyName;
        public string otmpFaceName;
        public string otmpStyleName;
        public string otmpFullName;
    }



    [StructLayout(LayoutKind.Sequential)]
    struct GCP_RESULTS
    {
        public int StructSize;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string OutString;
        public IntPtr Order;
        public IntPtr Dx;
        public IntPtr CaretPos;
        public IntPtr Class;
        public IntPtr Glyphs;
        public int GlyphCount;
        public int MaxFit;
    }

    struct GcpResults
    {
        public string OutString;
        public int[] Order;
        public int[] Dx;
        public int[] CaretPos;
        public byte[] Class;
        public short[] Glyphs;
        public int MaxFit;
    }

    [Flags]
    enum GCPFlags : uint
    {
        GCP_DBCS = 0x0001,
        GCP_REORDER = 0x0002,
        GCP_USEKERNING = 0x0008,
        GCP_GLYPHSHAPE = 0x0010,
        GCP_LIGATE = 0x0020,
        GCP_DIACRITIC = 0x0100,
        GCP_KASHIDA = 0x0400,
        GCP_ERROR = 0x8000,
        GCP_JUSTIFY = 0x00010000,
        GCP_CLASSIN = 0x00080000,
        GCP_MAXEXTENT = 0x00100000,
        GCP_JUSTIFYIN = 0x00200000,
        GCP_DISPLAYZWG = 0x00400000,
        GCP_SYMSWAPOFF = 0x00800000,
        GCP_NUMERICOVERRIDE = 0x01000000,
        GCP_NEUTRALOVERRIDE = 0x02000000,
        GCP_NUMERICSLATIN = 0x04000000,
        GCP_NUMERICSLOCAL = 0x08000000,
    }
}
