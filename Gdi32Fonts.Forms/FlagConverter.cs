using System.Windows.Forms;

namespace Gdi32Fonts.Forms
{
    static class FlagConverter
    {
        public static Gdi32TextFormatFlags GetGdi32TextFormatFlags(TextFormatFlags flags)
        {
            if (((ulong)flags & 0xFFFFFFFFFF000000uL) == 0uL)
            {
                return (Gdi32TextFormatFlags)flags;
            }
            return (Gdi32TextFormatFlags)(flags & (TextFormatFlags)0x00FFFFFF);
        }
    }
}
