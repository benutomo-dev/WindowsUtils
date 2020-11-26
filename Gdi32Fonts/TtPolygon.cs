using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gdi32Fonts
{
    public struct GlyphMetrics : IEquatable<GlyphMetrics>
    {
        public int BlackBoxX { get; }
        public int BlackBoxY { get; }
        public int GlyphOriginX { get; }
        public int GlyphOriginY { get; }
        public short CellIncX { get; }
        public short CellIncY { get; }

        public GlyphMetrics(int blackBoxX, int blackBoxY, int glyphOriginX, int glyphOriginY, short cellIncX, short cellIncY)
        {
            BlackBoxX = blackBoxX;
            BlackBoxY = blackBoxY;
            GlyphOriginX = glyphOriginX;
            GlyphOriginY = glyphOriginY;
            CellIncX = cellIncX;
            CellIncY = cellIncY;
        }

        /// <summary>
        /// 囲み文字及び半角文字を適切な横位置にセンタリングするためのオフセット値を計算する。
        /// </summary>
        /// <param name="emSize">emsize</param>
        /// <returns>オフセット値</returns>
        public int CalcHorizontalCenteringOffset(int emSize)
        {
            if (GlyphOriginX< 0 && CellIncX == 0)
            {
                // 囲み文字とみなす字形。文字一つ分右にずらす。
                return emSize;
            }
            else
            {
                return (emSize - CellIncX) / 2;
            }
        }

        /// <summary>
        /// 囲み文字及び半角文字を適切な横位置にセンタリングするためのオフセット値を計算する。
        /// </summary>
        /// <param name="emSize">emsize</param>
        /// <returns>オフセット値</returns>
        public float CalcHorizontalCenteringOffset(float emSize)
        {
            if (GlyphOriginX < 0 && CellIncX == 0)
            {
                // 囲み文字とみなす字形。文字一つ分右にずらす。
                return emSize;
            }
            else
            {
                return (emSize - CellIncX) / 2;
            }
        }

        public override string ToString() => $"{{{nameof(BlackBoxX)}={BlackBoxX},{nameof(BlackBoxY)}={BlackBoxY},{nameof(GlyphOriginX)}={GlyphOriginX},{nameof(GlyphOriginY)}={GlyphOriginY},{nameof(CellIncX)}={CellIncX},{nameof(CellIncY)}={CellIncY}}}";

        public override bool Equals(object? obj)
        {
            return obj is GlyphMetrics metrics && Equals(metrics);
        }

        public bool Equals(GlyphMetrics other)
        {
            return BlackBoxX == other.BlackBoxX &&
                   BlackBoxY == other.BlackBoxY &&
                   GlyphOriginX == other.GlyphOriginX &&
                   GlyphOriginY == other.GlyphOriginY &&
                   CellIncX == other.CellIncX &&
                   CellIncY == other.CellIncY;
        }

        public override int GetHashCode()
        {
            int hashCode = 1140044481;
            hashCode = hashCode * -1521134295 + BlackBoxX.GetHashCode();
            hashCode = hashCode * -1521134295 + BlackBoxY.GetHashCode();
            hashCode = hashCode * -1521134295 + GlyphOriginX.GetHashCode();
            hashCode = hashCode * -1521134295 + GlyphOriginY.GetHashCode();
            hashCode = hashCode * -1521134295 + CellIncX.GetHashCode();
            hashCode = hashCode * -1521134295 + CellIncY.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(GlyphMetrics left, GlyphMetrics right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GlyphMetrics left, GlyphMetrics right)
        {
            return !(left == right);
        }
    }

    public class FontOutline : IEquatable<FontOutline>
    {
        public uint EmSquare { get; }

        public int Ascent { get; }

        public GlyphMetrics GlyphMetrics { get; }

        public ImmutableArray<TtPolygon> Polygons { get; }

        private Lazy<int> _hash;

        public FontOutline(uint emSquare, int ascent, in GlyphMetrics glyphMetrics, ImmutableArray<TtPolygon> polygons)
        {
            EmSquare = emSquare;
            Ascent = ascent;
            GlyphMetrics = glyphMetrics;
            Polygons = polygons;

            _hash = new Lazy<int>(() =>
             {
                 var hash = 0;
                 hash ^= (int)EmSquare;
                 hash ^= Ascent;
                 hash ^= GlyphMetrics.GetHashCode();
                 foreach (var polygon in Polygons)
                 {
                     hash ^= polygon.GetHashCode();
                 }
                 return hash;
             }, LazyThreadSafetyMode.None);
        }

        public override string ToString() => $"{{{nameof(EmSquare)}={EmSquare},{nameof(Ascent)}={Ascent},{nameof(GlyphMetrics)}={GlyphMetrics},{nameof(Polygons)}={nameof(Polygons)}[{Polygons.Length}]}}";

        public override int GetHashCode() => _hash.Value;

        public override bool Equals(object obj) => (obj is FontOutline other) && Equals(other);

        public bool Equals(FontOutline other) => !(other is null) && EmSquare == other.EmSquare && Ascent == other.Ascent && GlyphMetrics == other.GlyphMetrics && Polygons.SequenceEqual(other.Polygons);

        public static bool operator ==(FontOutline a, FontOutline b) => (a is null && b is null) || (a?.Equals(b) ?? false);
        public static bool operator !=(FontOutline a, FontOutline b) => !(a == b);
    }

    public enum TtPrimitiveTypes
    {
        Line,
        QuadraticBezierSpline,
        CubicBezierSpline,
    }

    public class TtPolygon : IEquatable<TtPolygon>
    {
        public TtPolygonPoint StartPoint { get; }

        public ImmutableArray<TtPolygonCurve> Curves { get; }

        private Lazy<int> _hash;

        public TtPolygon(TtPolygonPoint startPoint, ImmutableArray<TtPolygonCurve> curves)
        {
            StartPoint = startPoint;
            Curves = curves;

            _hash = new Lazy<int>(() =>
            {
                var hash = 0;
                hash ^= StartPoint.GetHashCode();
                foreach (var curve in Curves)
                {
                    hash ^= curve.GetHashCode();
                }
                return hash;
            }, LazyThreadSafetyMode.None);
        }

        public override string ToString() => $"{{{nameof(StartPoint)}={StartPoint},{nameof(Curves)}={nameof(Curves)}[{Curves.Length}]}}";

        public override int GetHashCode() => _hash.Value;

        public override bool Equals(object obj) => (obj is TtPolygon other) && Equals(other);

        public bool Equals(TtPolygon other) => !(other is null) && StartPoint == other.StartPoint && Curves.SequenceEqual(other.Curves);


        public static bool operator ==(TtPolygon a, TtPolygon b) => (a is null && b is null) || (a?.Equals(b) ?? false);
        public static bool operator !=(TtPolygon a, TtPolygon b) => !(a == b);
    }

    public class TtPolygonCurve : IEquatable<TtPolygonCurve>
    {
        public TtPrimitiveTypes PrimitiveType { get; }
        public ImmutableArray<TtPolygonPoint> Points { get; }

        private Lazy<int> _hash;

        public TtPolygonCurve(TtPrimitiveTypes type, ImmutableArray<TtPolygonPoint> points)
        {
            PrimitiveType = type;
            Points = points;

            _hash = new Lazy<int>(() =>
            {
                var hash = 0;
                hash ^= PrimitiveType.GetHashCode();
                foreach (var point in Points)
                {
                    hash ^= point.GetHashCode();
                }
                return hash;
            }, LazyThreadSafetyMode.None);
        }

        internal PointF[] ToGraphicsPathPoint(ref TtPolygonPoint startPoint, float horizontalOffset, float scale, PointF translation)
        {
            var points = new PointF[Points.Length + 1];

            points[0] = new PointF((startPoint.X + horizontalOffset) * scale + translation.X, startPoint.Y * scale + translation.Y);

            for (int i = 0; i < Points.Length; i++)
            {
                points[i + 1] = new PointF((Points[i].X + horizontalOffset) * scale + translation.X, Points[i].Y * scale + translation.Y);
            }

            if (Points.Length > 0)
            {
                startPoint = Points[Points.Length - 1];
            }

            return points;
        }

        public override string ToString() => $"{{{nameof(PrimitiveType)}={PrimitiveType},{nameof(Points)}={nameof(Points)}[{Points.Length}]}}";

        public override int GetHashCode() => _hash.Value;

        public override bool Equals(object obj) => (obj is TtPolygonCurve other) && Equals(other);

        public bool Equals(TtPolygonCurve other) => !(other is null) && PrimitiveType == other.PrimitiveType && Points.SequenceEqual(other.Points);

        public static bool operator ==(TtPolygonCurve a, TtPolygonCurve b) => (a is null && b is null) || (a?.Equals(b) ?? false);
        public static bool operator !=(TtPolygonCurve a, TtPolygonCurve b) => !(a == b);
    }

    public struct TtPolygonPoint : IEquatable<TtPolygonPoint>
    {
        public short X { get; }
        public short Y { get; }

        public TtPolygonPoint(short x, short y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj) => (obj is TtPolygonPoint other) && Equals(other);

        public bool Equals(TtPolygonPoint other) => X == other.X && Y == other.Y;

        public override int GetHashCode() => X ^ Y;

        public static bool operator ==(TtPolygonPoint a, TtPolygonPoint b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(TtPolygonPoint a, TtPolygonPoint b) => !(a == b);
    }
}
