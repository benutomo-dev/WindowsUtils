using Gdi32Fonts;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Windows.Forms
{
    public static class RichTextBoxExtensions
    {
        private const int EM_SETCHARFORMAT = 1092;

        private const int CFM_BOLD                         = 0x00000001;
        private const int CFM_ITALIC                       = 0x00000002;
        private const int CFM_UNDERLINE                    = 0x00000004;
        private const int CFM_STRIKEOUT                    = 0x00000008;
        private const int CFM_PROTECTED                    = 0x00000010;
        private const int CFM_LINK                         = 0x00000020;
        private const int CFM_SMALLCAPS                    = 0x00000040;
        private const int CFM_ALLCAPS                      = 0x00000080;
        private const int CFM_HIDDEN                       = 0x00000100;
        private const int CFM_OUTLINE                      = 0x00000200;
        private const int CFM_SHADOW                       = 0x00000400;
        private const int CFM_EMBOSS                       = 0x00000800;
        private const int CFM_IMPRINT                      = 0x00001000;
        private const int CFM_DISABLED                     = 0x00002000;
        private const int CFM_REVISED                      = 0x00004000;
        private const int CFM_SUBSCRIPT                    = 0x00030000;
        private const int CFM_SUPERSCRIPT                  = 0x00030000;
        private const int CFM_COLOR                        = 0x40000000;

        private const int SCF_DEFAULT   = 0x0000;
        private const int SCF_SELECTION = 0x0001;
        private const int SCF_ALL       = 0x0004;

        public static void SetCharFormatFont(this RichTextBox richTextBox, bool selectionOnly, Gdi32Font font)
        {
            IntPtr handle = richTextBox.Handle;

            if (handle == IntPtr.Zero)
            {
                throw new InvalidOperationException();
            }

            int dwMask = -1476394993;
            int num = 0;
            if (font.Bold != BooleanConsts.FALSE)
            {
                num |= CFM_BOLD;
            }
            if (font.Italic != BooleanConsts.FALSE)
            {
                num |= CFM_ITALIC;
            }
            if (font.Underline != BooleanConsts.FALSE)
            {
                num |= CFM_UNDERLINE;
            }
            if (font.Strikeout != BooleanConsts.FALSE)
            {
                num |= CFM_STRIKEOUT;
            }
            byte[] bytes;

            if (Marshal.SystemDefaultCharSize == 1)
            {
                throw new PlatformNotSupportedException();
            }

            bytes = Encoding.Unicode.GetBytes(font.Logfont.lfFaceName);
            CHARFORMATW cHARFORMATW = new CHARFORMATW();
            for (int j = 0; j < bytes.Length; j++)
            {
                cHARFORMATW.szFaceName[j] = bytes[j];
            }
            cHARFORMATW.dwMask = dwMask;
            cHARFORMATW.dwEffects = num;
            cHARFORMATW.yHeight = (int)(font.SizeInPoints * 20f);
            cHARFORMATW.bCharSet = font.Logfont.lfCharSet;
            cHARFORMATW.bPitchAndFamily = font.Logfont.lfPitchAndFamily;
            SendMessage(new HandleRef(richTextBox, handle), EM_SETCHARFORMAT, selectionOnly ? SCF_SELECTION : SCF_ALL, cHARFORMATW);
        }



        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class CHARFORMATW
        {
            public int cbSize = Marshal.SizeOf(typeof(CHARFORMATW));

            public int dwMask;

            public int dwEffects;

            public int yHeight;

            public int yOffset;

            public int crTextColor;

            public byte bCharSet;

            public byte bPitchAndFamily;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] szFaceName = new byte[64];
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] CHARFORMATW lParam);


    }
}
