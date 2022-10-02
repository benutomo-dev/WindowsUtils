using System.ComponentModel;
using System.Drawing;

using static Gdi32Fonts.BooleanConsts;
using static Gdi32Fonts.FontWeightConsts;

namespace Gdi32Fonts
{
    public readonly ref struct Gdi32FontStyleInfo
    {
        public int Weight { get; }
        public byte Italic { get; }
        public byte Underline { get; }
        public byte StrikeOut { get; }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Gdi32FontStyleInfo(int weight, byte italic, byte underline, byte strikeOut)
        {
            Weight = weight;
            Italic = italic;
            Underline = underline;
            StrikeOut = strikeOut;
        }

        public Gdi32FontStyleInfo(int weight, bool italic, bool underline, bool strikeOut)
        {
            Weight = weight;
            Italic = (byte)(italic ? TRUE : FALSE);
            Underline = (byte)(underline ? TRUE : FALSE);
            StrikeOut = (byte)(strikeOut ? TRUE : FALSE);
        }

        public Gdi32FontStyleInfo(FontStyle style)
        {
            Weight = (((style & FontStyle.Bold) == FontStyle.Bold) ? FW_BOLD : FW_NORMAL);
            Italic = (byte)(((style & FontStyle.Italic) == FontStyle.Italic) ? TRUE : FALSE);
            Underline = (byte)(((style & FontStyle.Underline) == FontStyle.Underline) ? TRUE : FALSE);
            StrikeOut = (byte)(((style & FontStyle.Strikeout) == FontStyle.Strikeout) ? TRUE : FALSE);
        }
    }
}
