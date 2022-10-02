using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace Gdi32Fonts
{
    internal class FontSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {

        private const int OUT_DEFAULT_PRECIS   = 0;
        private const int OUT_STRING_PRECIS    = 1;
        private const int OUT_CHARACTER_PRECIS = 2;
        private const int OUT_STROKE_PRECIS    = 3;
        private const int OUT_TT_PRECIS        = 4;
        private const int OUT_DEVICE_PRECIS    = 5;
        private const int OUT_RASTER_PRECIS    = 6;
        private const int OUT_TT_ONLY_PRECIS   = 7;
        private const int OUT_OUTLINE_PRECIS   = 8;



        internal FontSafeHandle() : base(true)
        {
        }

        static FontSafeHandle CreateFont(in LOGFONT logfont)
        {
            return CreateFontIndirect(logfont);
        }

        public static FontSafeHandle CreateFont(string faceName, int height, int weight, bool italic, bool underline, bool strikeOut, byte charSet, Gdi32FontQuality fontQuality)
        {
            return CreateFont(faceName, height, new Gdi32FontStyleInfo(weight, italic, underline, strikeOut), charSet, fontQuality);
        }

        public static FontSafeHandle CreateFont(string faceName, int height, Gdi32FontStyleInfo fontStyle, byte charSet, Gdi32FontQuality fontQuality)
        {
            // https://support.microsoft.com/ja-jp/help/74299/info-calculating-the-logical-height-and-point-size-of-a-font

            var logfont = new LOGFONT
            {
                lfHeight       = -height,
                lfFaceName     = faceName ?? throw new ArgumentNullException(nameof(faceName)),
                lfCharSet      = charSet,
                lfOutPrecision = OUT_TT_PRECIS,
                lfQuality      = (byte)fontQuality,
                lfWeight       = fontStyle.Weight,
                lfItalic       = fontStyle.Italic,
                lfUnderline    = fontStyle.Underline,
                lfStrikeOut    = fontStyle.StrikeOut,
            };

            return CreateFontIndirect(logfont);
        }

        protected override bool ReleaseHandle()
        {
            return DeleteObject(handle);
        }


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct LOGFONT
        {
            public int lfHeight;

            public int lfWidth;

            public int lfEscapement;

            public int lfOrientation;

            public int lfWeight;

            public byte lfItalic;

            public byte lfUnderline;

            public byte lfStrikeOut;

            public byte lfCharSet;

            public byte lfOutPrecision;

            public byte lfClipPrecision;

            public byte lfQuality;

            public byte lfPitchAndFamily;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string lfFaceName;

            public LOGFONT(LOGFONT lf)
            {
                this.lfHeight = lf.lfHeight;
                this.lfWidth = lf.lfWidth;
                this.lfEscapement = lf.lfEscapement;
                this.lfOrientation = lf.lfOrientation;
                this.lfWeight = lf.lfWeight;
                this.lfItalic = lf.lfItalic;
                this.lfUnderline = lf.lfUnderline;
                this.lfStrikeOut = lf.lfStrikeOut;
                this.lfCharSet = lf.lfCharSet;
                this.lfOutPrecision = lf.lfOutPrecision;
                this.lfClipPrecision = lf.lfClipPrecision;
                this.lfQuality = lf.lfQuality;
                this.lfPitchAndFamily = lf.lfPitchAndFamily;
                this.lfFaceName = lf.lfFaceName;
            }
        }


        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern FontSafeHandle CreateFontIndirect(in LOGFONT lf);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);
    }
}
