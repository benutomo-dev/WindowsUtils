using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gdi32Fonts
{
    public static class Gdi32FontPool
    {
        struct FontCacheEntry : IEquatable<FontCacheEntry>
        {
            public Gdi32Font.FontCacheKey Key { get; }
            public Gdi32Font Font { get; }

            public FontCacheEntry(Gdi32Font.FontCacheKey key, Gdi32Font font)
            {
                Key = key ?? throw new ArgumentNullException(nameof(key));
                Font = font ?? throw new ArgumentNullException(nameof(font));
            }

            public override bool Equals(object obj)
            {
                return obj is FontCacheEntry entry && Equals(entry);
            }

            public bool Equals(FontCacheEntry other)
            {
                return EqualityComparer<Gdi32Font.FontCacheKey>.Default.Equals(Key, other.Key) &&
                       EqualityComparer<Gdi32Font>.Default.Equals(Font, other.Font);
            }

            public override int GetHashCode()
            {
                int hashCode = -494826084;
                hashCode = hashCode * -1521134295 + EqualityComparer<Gdi32Font.FontCacheKey>.Default.GetHashCode(Key);
                hashCode = hashCode * -1521134295 + EqualityComparer<Gdi32Font>.Default.GetHashCode(Font);
                return hashCode;
            }
        }

        private static LinkedList<FontCacheEntry> FontCache = new LinkedList<FontCacheEntry>();

        public static Gdi32Font GetPoolingFont(string faceName, FontSizeUnit fontSizeUnit, float size, int weight, byte italic, byte underline, byte strikeOut, byte charSet, Gdi32FontQuality fontQuality)
        {
            return GetPoolingFont(faceName, fontSizeUnit, size, new Gdi32FontStyleInfo(weight, italic, underline, strikeOut), charSet, fontQuality);
        }

        public static Gdi32Font GetPoolingFont(string faceName, FontSizeUnit fontSizeUnit, float size, Gdi32FontStyleInfo fontStyle, byte charSet, Gdi32FontQuality fontQuality)
        {
            lock (FontCache)
            {
                var key = Gdi32Font.ToFontCacheKey(faceName, fontSizeUnit, size, fontStyle, charSet, fontQuality);

                var font = new Gdi32Font(key);

                for (var node = FontCache.First; node != null; node = node.Next)
                {
                    if (node.Value.Key == key)
                    {
                        if (FontCache.First != node)
                        {
                            FontCache.Remove(node);
                            FontCache.AddFirst(node);
                        }

                        return font;
                    }
                }

                FontCache.AddFirst(new FontCacheEntry(key, font));

                while (FontCache.Count > 20)
                {
                    var lastNode = FontCache.Last;
                    lastNode.Value.Font.Dispose();
                    FontCache.Remove(lastNode);
                }

                return font;
            }
        }
    }
}
