using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using static Gdi32Fonts.FontWeightConsts;

namespace Gdi32Fonts
{
    public enum OutlineMode
    {
        Native,
        Bezier,
    }

    public class Gdi32Font : IDisposable
    {
        private const uint GDI_ERROR = 0xFFFFFFFF;

        private const uint GGO_METRICS = 0;
        private const uint GGO_BITMAP  = 1;
        private const uint GGO_NATIVE  = 2;
        private const uint GGO_BEZIER  = 3;

        private const uint GGO_GRAY2_BITMAP = 4;
        private const uint GGO_GRAY4_BITMAP = 5;
        private const uint GGO_GRAY8_BITMAP = 6;

        private const uint GGO_GLYPH_INDEX = 0x0080;
        private const uint GGO_UNHINTED    = 0x0100;

        private const uint TT_POLYGON_TYPE = 24;

        private const short TT_PRIM_LINE    = 1;
        private const short TT_PRIM_QSPLINE = 2;
        private const short TT_PRIM_CSPLINE = 3;

        private static Dictionary<FontCacheKey, SharedObject> FontCache = new Dictionary<FontCacheKey, SharedObject>();

        FontCacheKey Key { get; }

        SharedObject? _SharedFont;
        SharedObject SharedFont
        {
            get
            {
                var sharedFont = _SharedFont;
                if (sharedFont is null) throw new ObjectDisposedException(nameof(Gdi32Font));
                return sharedFont;
            }
        }

        internal FontSafeHandle Handle => SharedFont.Handle;

        internal readonly FontSafeHandle.LOGFONT Logfont;

        public float SizeInPixels => Logfont.lfHeight < 0 ? -Logfont.lfHeight : Logfont.lfHeight * 96f / 72f;

        public float SizeInPoints => Logfont.lfHeight < 0 ? -Logfont.lfHeight * 72f / 96f : Logfont.lfHeight;

        public byte Bold => Logfont.lfStrikeOut;
        public byte Italic => Logfont.lfItalic;
        public byte Strikeout => Logfont.lfStrikeOut;
        public byte Underline => Logfont.lfUnderline;

        public Gdi32Font(string faceName, FontSizeUnit fontSizeUnit, float size, Gdi32FontStyleInfo fontStyle, byte charSet, Gdi32FontQuality fontQuality)
            : this(ToFontCacheKey(faceName, fontSizeUnit, size, fontStyle, charSet, fontQuality))
        {
        }

        public Gdi32Font(string faceName, FontSizeUnit fontSizeUnit, float size, int weight, bool italic, bool underline, bool strikeOut, byte charSet, Gdi32FontQuality fontQuality)
            : this(ToFontCacheKey(faceName, fontSizeUnit, size, new Gdi32FontStyleInfo(weight, italic, underline, strikeOut), charSet, fontQuality))
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Gdi32Font(string faceName, FontSizeUnit fontSizeUnit, float size, int weight, byte italic, byte underline, byte strikeOut, byte charSet, Gdi32FontQuality fontQuality)
            : this(ToFontCacheKey(faceName, fontSizeUnit, size, new Gdi32FontStyleInfo(weight, italic, underline, strikeOut), charSet, fontQuality))
        {
        }

        internal Gdi32Font(FontCacheKey fontCacheKey)
        {
            Key = fontCacheKey;

            lock (FontCache)
            {
                if (!FontCache.TryGetValue(Key, out SharedObject sharedObject))
                {
                    sharedObject = new SharedObject(Key.FaceName, Key.Height, Key.Weight, Key.Italic, Key.Underline, Key.StrikeOut, Key.CharSet, Key.FontQuality);
                    FontCache[Key] = sharedObject;
                }

                _SharedFont = sharedObject;
                Logfont = _SharedFont.Logfont;

                _SharedFont.ReferenceCount++;
            }
        }

        internal static FontCacheKey ToFontCacheKey(string faceName, FontSizeUnit fontSizeUnit, float size, Gdi32FontStyleInfo fontStyle, byte charSet, Gdi32FontQuality fontQuality)
        {
            int height;
            switch (fontSizeUnit)
            {
                case FontSizeUnit.Pixel:
                    height = (int)Math.Ceiling(size);
                    break;
                case FontSizeUnit.DpiScaledPoint:
                    throw new NotSupportedException();
                //height = (int)Math.Ceiling((double)((float)WindowsGraphicsCacheManager.MeasurementGraphics.DeviceContext.DpiY * size / 72f));
                //break;
                case FontSizeUnit.NonDpiScaledPoint:
                    height = (int)Math.Ceiling(96.0 * size / 72.0);
                    break;
                default:
                    throw new ArgumentException(nameof(FontSizeUnit));
            }

            return new FontCacheKey(faceName, height, fontStyle, charSet, fontQuality);
        }

        public byte[] GetFontData()
        {
            using (var hdc = DeviceContextSafeHandle.CreateMesurementDeviceContext())
            {
                var selectResult = NativeApi.SelectObject(hdc, Handle);

                if (selectResult == IntPtr.Zero)
                {
                    throw new Win32Exception();
                }

                var dataLength = GetFontData(hdc, 0, 0, null, 0);

                if (dataLength == GDI_ERROR)
                {
                    throw new Win32Exception();
                }

                var dataBuffer = new byte[dataLength];

                var writeLength = GetFontData(hdc, 0, 0, dataBuffer, dataLength);

                if (writeLength == GDI_ERROR)
                {
                    throw new Win32Exception();
                }

                return dataBuffer;
            }
        }

        public static bool ExistsOutline(string fontFaceName, string text)
        {
            return Gdi32FontPool.GetPoolingFont(
                faceName: fontFaceName,
                fontSizeUnit: FontSizeUnit.Pixel,
                size: 12,
                weight: FW_NORMAL,
                italic: 0,
                underline: 0,
                strikeOut: 0,
                charSet: 1,
                fontQuality: Gdi32FontQuality.Default
                ).ExistsOutline(text);
        }

        public bool ExistsOutline(string text)
        {
            MAT2 matrix = new MAT2();
            matrix.eM11.value = 1;
            matrix.eM12.value = 0;
            matrix.eM21.value = 0;
            matrix.eM22.value = 1;

            using (var hdc = DeviceContextSafeHandle.CreateMesurementDeviceContext())
            {
                var selectResult = NativeApi.SelectObject(hdc, Handle);

                if (selectResult == IntPtr.Zero)
                {
                    throw new Win32Exception();
                }

                NativeApi.GetCharacterPlacement(hdc, text, GCPFlags.GCP_GLYPHSHAPE, out var characterPlacement);

                if (characterPlacement.Glyphs is null || characterPlacement.Glyphs.Length == 0)
                {
                    return false;
                }

                uint glyphIndex = (uint)characterPlacement.Glyphs[0];

                int bufferSize = (int)NativeApi.GetGlyphOutline(hdc, glyphIndex, GGO_GLYPH_INDEX | GGO_NATIVE, out GLYPHMETRICS metrics, 0, IntPtr.Zero, ref matrix);

                return bufferSize > 0;
            }
        }

        public static bool TryGetGlyphMetrics(string fontFaceName, string text, FontSizeUnit fontSizeUnit, float size, out GlyphMetrics? glyphMetrics)
        {
            return Gdi32FontPool.GetPoolingFont(
                faceName: fontFaceName,
                fontSizeUnit: fontSizeUnit,
                size: size,
                weight: FW_NORMAL,
                italic: 0,
                underline: 0,
                strikeOut: 0,
                charSet: 1,
                fontQuality: Gdi32FontQuality.Default
                ).TryGetGlyphMetrics(text, out glyphMetrics);
        }

        public bool TryGetGlyphMetrics(string text, out GlyphMetrics? glyphMetrics)
        {
            MAT2 matrix = new MAT2();
            matrix.eM11.value = 1;
            matrix.eM12.value = 0;
            matrix.eM21.value = 0;
            matrix.eM22.value = 1;

            using (var hdc = DeviceContextSafeHandle.CreateMesurementDeviceContext())
            {
                var selectResult = NativeApi.SelectObject(hdc, Handle);

                if (selectResult == IntPtr.Zero)
                {
                    throw new Win32Exception();
                }

                NativeApi.GetCharacterPlacement(hdc, text, GCPFlags.GCP_GLYPHSHAPE, out var characterPlacement);

                if (characterPlacement.Glyphs is null || characterPlacement.Glyphs.Length == 0)
                {
                    glyphMetrics = null;
                    return false;
                }

                uint glyphIndex = (uint)characterPlacement.Glyphs[0];

                int bufferSize = (int)NativeApi.GetGlyphOutline(hdc, glyphIndex, GGO_GLYPH_INDEX | GGO_NATIVE, out GLYPHMETRICS metrics, 0, IntPtr.Zero, ref matrix);

                if (bufferSize <= 0)
                {
                    glyphMetrics = null;
                    return false;
                }

                glyphMetrics = new GlyphMetrics(metrics.gmBlackBoxX, metrics.gmBlackBoxY, metrics.gmptGlyphOrigin.x, metrics.gmptGlyphOrigin.y, metrics.gmCellIncX, metrics.gmCellIncY);
                return true;
            }
        }
        
        public static FontOutline? GetOutline(string fontFaceName, string text, OutlineMode outlineMode)
        {
            return Gdi32FontPool.GetPoolingFont(
                faceName: fontFaceName,
                fontSizeUnit: FontSizeUnit.Pixel,
                size: 12,
                weight: FW_NORMAL,
                italic: 0,
                underline: 0,
                strikeOut: 0,
                charSet: 1,
                fontQuality: Gdi32FontQuality.Default
                ).GetOutline(text, outlineMode);
        }

        public FontOutline? GetOutline(string text, OutlineMode outlineMode)
        {
            if (SharedFont.OutlineTextMetric is null)
            {
                return null;
            }

            // 参考文献
            // http://marupeke296.com/TIPS_Main.html
            // http://marupeke296.com/WINT_GetGlyphOutline.html
            // http://marupeke296.com/DXG_No67_NewFont.html
            // https://msdn.microsoft.com/ja-jp/library/cc410385.aspx
            // https://msdn.microsoft.com/ja-jp/library/cc428640.aspx
            // https://www11.atwiki.jp/slice/pages/78.html
            // http://dendrocopos.jp/tips/win32.html
            // http://www.geocities.co.jp/Playtown-Dice/9391/program/win04.html
            // http://misohena.jp/article/ggo_trap/index.html
            // https://oshiete.goo.ne.jp/qa/4743793.html
            // http://eternalwindows.jp/graphics/bitmap/bitmap12.html
            // https://social.msdn.microsoft.com/Forums/ja-JP/3442d813-823a-449a-993e-7fc073aea949/opentype?forum=vcgeneralja
            // http://phys.cool.coocan.jp/physjpn/htextmetric.htm
            // https://docs.microsoft.com/ja-jp/windows/desktop/api/wingdi/ns-wingdi-_outlinetextmetricw
            // 
            // 参考資料
            // https://support.microsoft.com/en-us/help/87115/how-to-getglyphoutline-native-buffer-format
            // http://kone.vis.ne.jp/diary/diaryb08.html
            // https://msdn.microsoft.com/ja-jp/library/windows/desktop/dd144891(v=vs.85).aspx
            //

            try
            {

                GLYPHMETRICS metrics = new GLYPHMETRICS();
                MAT2 matrix = new MAT2();
                matrix.eM11.value = 1;
                matrix.eM12.value = 0;
                matrix.eM21.value = 0;
                matrix.eM22.value = 1;

                using (var hdc = DeviceContextSafeHandle.CreateMesurementDeviceContext())
                {
                    // アウトライン取得用のフォントを生成。この時フォントサイズは制御店のメッシュサイズと一致させる。
                    // 一致しない場合は取得されるアウトラインの精度が著しく低下する場合がある。
                    var outlineAccessFont = Gdi32FontPool.GetPoolingFont(Key.FaceName, FontSizeUnit.Pixel, SharedFont.OutlineTextMetric.Value.otmEMSquare, Key.Weight, Key.Italic, Key.Underline, Key.StrikeOut, Key.CharSet, Key.FontQuality);

                    if (outlineAccessFont.SharedFont.OutlineTextMetric is null)
                    {
                        return null;
                    }

                    var selectResult = NativeApi.SelectObject(hdc, outlineAccessFont.Handle);

                    if (selectResult == IntPtr.Zero)
                    {
                        throw new Win32Exception();
                    }

                    NativeApi.GetCharacterPlacement(hdc, text, GCPFlags.GCP_GLYPHSHAPE, out var characterPlacement);

                    if (characterPlacement.Glyphs is null || characterPlacement.Glyphs.Length == 0)
                    {
                        return null;
                    }

                    var size = NativeApi.GetTextExtentPoint(hdc, text);

                    uint glyphIndex = (uint)characterPlacement.Glyphs[0];

                    uint format = GGO_GLYPH_INDEX | (outlineMode == OutlineMode.Bezier ? GGO_BEZIER : GGO_NATIVE);

                    int bufferSize = (int)NativeApi.GetGlyphOutline(hdc, glyphIndex, format, out metrics, 0, IntPtr.Zero, ref matrix);

                    if (bufferSize <= 0)
                    {
                        return null;
                    }

                    IntPtr buffer = Marshal.AllocHGlobal(bufferSize);

                    List<TtPolygon> ttPolygons = new List<TtPolygon>();
                    try
                    {
                        uint ret = NativeApi.GetGlyphOutline(hdc, glyphIndex, format, out metrics, (uint)bufferSize, buffer, ref matrix);

                        if (ret == GDI_ERROR)
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }

                        int polygonHeaderSize = Marshal.SizeOf(typeof(TTPOLYGONHEADER));
                        int curveHeaderSize = Marshal.SizeOf(typeof(TTPOLYCURVEHEADER));
                        int pointFxSize = Marshal.SizeOf(typeof(POINTFX));

                        int index = 0;
                        while (index < bufferSize)
                        {
                            TTPOLYGONHEADER header = (TTPOLYGONHEADER)Marshal.PtrToStructure(new IntPtr(buffer.ToInt64() + index), typeof(TTPOLYGONHEADER));

                            POINTFX startPoint = header.pfxStart;

                            // サイズをfontEmSquareで指定して内部メッシュと一致させているので、正しくできている限り端数は生じない。
                            Debug.Assert(startPoint.x.fract == 0);
                            Debug.Assert(startPoint.y.fract == 0);

                            int endCurvesIndex = index + header.cb;
                            index += polygonHeaderSize;

                            List<TtPolygonCurve> ttPolygonCurves = new List<TtPolygonCurve>();

                            while (index < endCurvesIndex)
                            {
                                TTPOLYCURVEHEADER curveHeader = (TTPOLYCURVEHEADER)Marshal.PtrToStructure(new IntPtr(buffer.ToInt64() + index), typeof(TTPOLYCURVEHEADER));
                                index += curveHeaderSize;

                                TtPolygonPoint[] curvePoints = new TtPolygonPoint[curveHeader.cpfx];

                                for (int i = 0; i < curveHeader.cpfx; i++)
                                {
                                    var curvePoint = (POINTFX)Marshal.PtrToStructure(new IntPtr(buffer.ToInt64() + index), typeof(POINTFX));

                                    // サイズをfontEmSquareで指定して内部メッシュと一致させているので、正しくできている限り端数は生じない。
                                    // ただし、ベジェ曲線に変換している場合は制御店の創出により端数が生じる。
                                    Debug.Assert(outlineMode != OutlineMode.Native || curvePoint.x.fract == 0);
                                    Debug.Assert(outlineMode != OutlineMode.Native || curvePoint.y.fract == 0);

                                    curvePoints[i] = curvePoint.ToTtPolygonPoint(outlineAccessFont.SharedFont.OutlineTextMetric.Value, metrics);

                                    index += pointFxSize;
                                }

                                TtPrimitiveTypes type;
                                switch (curveHeader.wType)
                                {
                                    case TT_PRIM_LINE:
                                        type = TtPrimitiveTypes.Line;
                                        break;
                                    case TT_PRIM_QSPLINE:
                                        Debug.Assert(outlineMode == OutlineMode.Native);
                                        type = TtPrimitiveTypes.QuadraticBezierSpline;
                                        break;
                                    case TT_PRIM_CSPLINE:
                                        Debug.Assert(outlineMode == OutlineMode.Bezier);
                                        type = TtPrimitiveTypes.CubicBezierSpline;
                                        break;
                                    default: throw new FormatException();
                                }

                                ttPolygonCurves.Add(new TtPolygonCurve(type, curvePoints.ToImmutableArray()));
                            }

                            ttPolygons.Add(new TtPolygon(startPoint.ToTtPolygonPoint(outlineAccessFont.SharedFont.OutlineTextMetric.Value, metrics), ttPolygonCurves.ToImmutableArray()));
                        }
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(buffer);
                    }

                    Debug.Assert(metrics.gmCellIncX >= 0);
                    Debug.Assert(metrics.gmCellIncY >= 0);

                    var glyphMetrics = new GlyphMetrics(metrics.gmBlackBoxX, metrics.gmBlackBoxY, metrics.gmptGlyphOrigin.x, metrics.gmptGlyphOrigin.y, metrics.gmCellIncX, metrics.gmCellIncY);

                    return new FontOutline(
                        outlineAccessFont.SharedFont.OutlineTextMetric.Value.otmEMSquare,
                        outlineAccessFont.SharedFont.OutlineTextMetric.Value.otmMacAscent,
                        glyphMetrics,
                        ttPolygons.ToImmutableArray()
                        );
                }
            }
            catch (Win32Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }


        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_SharedFont is { })
                    {
                        lock (FontCache)
                        {
                            _SharedFont.ReferenceCount--;

                            Debug.Assert(SharedFont.ReferenceCount >= 0);

                            if (_SharedFont.ReferenceCount <= 0)
                            {
                                FontCache.Remove(Key);
                                _SharedFont.Dispose();
                            }
                        }

                        _SharedFont = null;
                    }
                }

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~Gdi32Font() {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion


        internal class FontCacheKey : IEquatable<FontCacheKey>
        {
            public string FaceName { get; }
            public int Height { get; }
            public int Weight { get; }
            public byte Italic { get; }
            public byte Underline { get; }
            public byte StrikeOut { get; }
            public byte CharSet { get; }
            public Gdi32FontQuality FontQuality { get; }

            int? _hashCode;

            public FontCacheKey(string faceName, int height, Gdi32FontStyleInfo fontStyle, byte charSet, Gdi32FontQuality fontQuality)
            {
                FaceName = faceName ?? throw new ArgumentNullException(nameof(faceName));
                Height = height;
                Weight = fontStyle.Weight;
                Italic = fontStyle.Italic;
                Underline = fontStyle.Underline;
                StrikeOut = fontStyle.StrikeOut;
                CharSet = charSet;
                FontQuality = fontQuality;
            }

            public override bool Equals(object? obj) => obj is FontCacheKey key && Equals(key);

            public bool Equals(FontCacheKey other)
            {
                if (other is null) return false;

                if (_hashCode.HasValue && other._hashCode.HasValue && _hashCode.Value != other._hashCode.Value) return false;

                return FaceName == other.FaceName &&
                       Height == other.Height &&
                       Weight == other.Weight &&
                       Italic == other.Italic &&
                       Underline == other.Underline &&
                       StrikeOut == other.StrikeOut &&
                       CharSet == other.CharSet &&
                       FontQuality == other.FontQuality;
            }

            public override int GetHashCode()
            {
                if (_hashCode.HasValue) return _hashCode.Value;

                var hashCode = 1549922059;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FaceName);
                hashCode = hashCode * -1521134295 + Height.GetHashCode();
                hashCode = hashCode * -1521134295 + Weight.GetHashCode();
                hashCode = hashCode * -1521134295 + Italic.GetHashCode();
                hashCode = hashCode * -1521134295 + Underline.GetHashCode();
                hashCode = hashCode * -1521134295 + StrikeOut.GetHashCode();
                hashCode = hashCode * -1521134295 + CharSet.GetHashCode();
                hashCode = hashCode * -1521134295 + FontQuality.GetHashCode();

                _hashCode = hashCode;
                return hashCode;
            }

            public static bool operator ==(FontCacheKey left, FontCacheKey right)
            {
                return EqualityComparer<FontCacheKey>.Default.Equals(left, right);
            }

            public static bool operator !=(FontCacheKey left, FontCacheKey right)
            {
                return !(left == right);
            }
        };

        private class SharedObject : IDisposable
        {
            public FontSafeHandle Handle { get; private set; }

            public int ReferenceCount { get; set; }

            public readonly FontSafeHandle.LOGFONT Logfont;

            public int Win32ErrorCode { get; }

            public readonly OutlineTextMetric? OutlineTextMetric;

            public SharedObject(string faceName, int height, int weight, byte italic, byte underline, byte strikeOut, byte charSet, Gdi32FontQuality fontQuality)
            {
                Handle = FontSafeHandle.CreateFont(faceName, height, new Gdi32FontStyleInfo(weight, italic, underline, strikeOut), charSet, fontQuality);

                Logfont = new FontSafeHandle.LOGFONT();

                var bufferSize = Marshal.SizeOf(Logfont);
                var writeSize = GetObject(Handle, bufferSize, ref Logfont);

                if (writeSize == 0)
                {
                    Win32ErrorCode = Marshal.GetLastWin32Error();
                    return;
                }

                if (bufferSize != writeSize)
                {
                    throw new InvalidOperationException();
                }

                using (var hdc = DeviceContextSafeHandle.CreateMesurementDeviceContext())
                {
                    var selectResult = NativeApi.SelectObject(hdc, Handle);

                    if (selectResult == IntPtr.Zero)
                    {
                        Win32ErrorCode = Marshal.GetLastWin32Error();
                        return;
                    }

                    if (NativeApi.TryGetOutlineTextMetrics(hdc, out var outlineTextMetric, out var getOutlineTextMetricsErrorCode))
                    {
                        Win32ErrorCode = 0;
                        OutlineTextMetric = outlineTextMetric;
                    }
                    else
                    {
                        Win32ErrorCode = getOutlineTextMetricsErrorCode;
                        OutlineTextMetric = null;
                    }
                }
            }

            public void Dispose()
            {
                ((IDisposable)Handle).Dispose();
            }
        }

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetObject(FontSafeHandle hFont, int nSize, ref FontSafeHandle.LOGFONT lf);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern uint GetFontData(DeviceContextSafeHandle hdc, uint dwTable, uint dwOffset, [Out] byte[]? lpvBuffer, uint cbData);
    }
}
