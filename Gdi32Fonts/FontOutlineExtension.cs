using System;
using System.Drawing;
using System.Text;

namespace Gdi32Fonts
{
    public static class FontOutlineExtension
    {
        public static string ToSvgPathData(this FontOutline outline, float size, PointF origin, bool enableHorizontalCenteringMode)
        {
            if (outline is null) throw new ArgumentNullException(nameof(outline));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

            // https://developer.mozilla.org/ja/docs/Web/SVG/Tutorial
            // https://svgwg.org/svg2-draft/paths.html#PathElement

            float horizontalOffset = enableHorizontalCenteringMode ? outline.GlyphMetrics.CalcHorizontalCenteringOffset(outline.EmSquare) : 0;

            var scaleFactor = (float)size / outline.EmSquare;

            var pathDataBuilder = new StringBuilder(4096);

            foreach (var polygon in outline.Polygons)
            {
                var normalizedStartPoint = NormalizePoint(polygon.StartPoint, horizontalOffset, scaleFactor, origin);
                pathDataBuilder.Append("M");
                pathDataBuilder.Append(normalizedStartPoint.X);
                pathDataBuilder.Append(",");
                pathDataBuilder.Append(normalizedStartPoint.Y);
                pathDataBuilder.Append(" ");

                foreach (var curve in polygon.Curves)
                {
                    if (curve.PrimitiveType == TtPrimitiveTypes.Line)
                    {
                        foreach (var point in curve.Points)
                        {
                            var normarizedPoint = NormalizePoint(point, horizontalOffset, scaleFactor, origin);
                            pathDataBuilder.Append("L");
                            pathDataBuilder.Append(normarizedPoint.X);
                            pathDataBuilder.Append(",");
                            pathDataBuilder.Append(normarizedPoint.Y);
                            pathDataBuilder.Append(" ");
                        }
                    }
                    else if (curve.PrimitiveType == TtPrimitiveTypes.CubicBezierSpline)
                    {
                        pathDataBuilder.Append($"C");

                        foreach (var point in curve.Points)
                        {
                            var normarizedPoint = NormalizePoint(point, horizontalOffset, scaleFactor, origin);
                            pathDataBuilder.Append(normarizedPoint.X);
                            pathDataBuilder.Append(",");
                            pathDataBuilder.Append(normarizedPoint.Y);
                            pathDataBuilder.Append(" ");
                        }
                    }
                    else if (curve.PrimitiveType == TtPrimitiveTypes.QuadraticBezierSpline)
                    {
                        if (curve.Points.Length == 2)
                        {
                            pathDataBuilder.Append($"Q");

                            var normarizedPoint1 = NormalizePoint(curve.Points[0], horizontalOffset, scaleFactor, origin);
                            pathDataBuilder.Append(normarizedPoint1.X);
                            pathDataBuilder.Append(",");
                            pathDataBuilder.Append(normarizedPoint1.Y);
                            pathDataBuilder.Append(" ");

                            var normarizedPoint2 = NormalizePoint(curve.Points[1], horizontalOffset, scaleFactor, origin);
                            pathDataBuilder.Append(normarizedPoint2.X);
                            pathDataBuilder.Append(",");
                            pathDataBuilder.Append(normarizedPoint2.Y);
                            pathDataBuilder.Append(" ");
                        }
                        else if (curve.Points.Length > 2)
                        {
                            PointF currentOffCurvePoint = NormalizePoint(curve.Points[0], horizontalOffset, scaleFactor, origin);
                            PointF onCurvePoint;

                            for (int i = 1; i < curve.Points.Length - 1; i++)
                            {
                                var nextOffCurvePoint = NormalizePoint(curve.Points[i], horizontalOffset, scaleFactor, origin);

                                onCurvePoint = Mul(Add(currentOffCurvePoint, nextOffCurvePoint), 0.5f);

                                pathDataBuilder.Append($"Q");

                                pathDataBuilder.Append(currentOffCurvePoint.X);
                                pathDataBuilder.Append(",");
                                pathDataBuilder.Append(currentOffCurvePoint.Y);
                                pathDataBuilder.Append(" ");

                                pathDataBuilder.Append(onCurvePoint.X);
                                pathDataBuilder.Append(",");
                                pathDataBuilder.Append(onCurvePoint.Y);
                                pathDataBuilder.Append(" ");

                                currentOffCurvePoint = nextOffCurvePoint;
                            }

                            onCurvePoint = NormalizePoint(curve.Points[curve.Points.Length - 1], horizontalOffset, scaleFactor, origin);

                            pathDataBuilder.Append($"Q");

                            pathDataBuilder.Append(currentOffCurvePoint.X);
                            pathDataBuilder.Append(",");
                            pathDataBuilder.Append(currentOffCurvePoint.Y);
                            pathDataBuilder.Append(" ");

                            pathDataBuilder.Append(onCurvePoint.X);
                            pathDataBuilder.Append(",");
                            pathDataBuilder.Append(onCurvePoint.Y);
                            pathDataBuilder.Append(" ");
                        }
                    }
                }

                pathDataBuilder.Append($"Z");
            }

            return pathDataBuilder.ToString();


            static PointF NormalizePoint(TtPolygonPoint point, float horizontalOffset, float scaleFactor, PointF origin)
            {
                return new PointF((point.X + horizontalOffset) * scaleFactor + origin.X, point.Y * scaleFactor + origin.Y);
            }

            static PointF Mul(PointF p, float a)
            {
                return new PointF(p.X * a, p.Y * a);
            }

            static PointF Add(PointF a, PointF b)
            {
                return new PointF(a.X + b.X, a.Y + b.Y);
            }

        }
    }
}
