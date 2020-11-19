using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gdi32Fonts
{
    public enum FontSizeUnit
    {
        /// <summary>
        /// フォンサイズをピクセル単位で指定する
        /// </summary>
        Pixel,

        /// <summary>
        /// (未テスト)フォントサイズをDPIスケール設定に応じて変化するポイントで指定する
        /// </summary>
        DpiScaledPoint,

        /// <summary>
        /// (未テスト)フォントサイズを96DPI規準のポイントで指定する
        /// </summary>
        NonDpiScaledPoint,
    }
}
