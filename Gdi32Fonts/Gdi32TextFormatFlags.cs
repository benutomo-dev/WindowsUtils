using System;

namespace Gdi32Fonts
{
    [Flags]
    public enum Gdi32TextFormatFlags
    {
        Default = 0,
        HorizontalCenter = 1,
        Bottom = 8,
        CalculateRectangle = 1024,
        EndEllipsis = 32768,
        ExpandTabs = 64,
        ExternalLeading = 512,
        HidePrefix = 1048576,
        Internal = 4096,
        Left = 0,
        ModifyString = 65536,
        NoClipping = 256,
        NoPrefix = 2048,
        NoFullWidthCharacterBreak = 524288,
        PathEllipsis = 16384,
        PrefixOnly = 2097152,
        Right = 2,
        RightToLeft = 131072,
        SingleLine = 32,
        TabStop = 128,
        TextBoxControl = 8192,
        Top = 0,
        VerticalCenter = 4,
        WordBreak = 16,
        WordEllipsis = 262144
    }

}
