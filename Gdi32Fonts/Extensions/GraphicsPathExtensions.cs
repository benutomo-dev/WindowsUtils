using Gdi32Fonts;

namespace System.Drawing.Drawing2D
{
    public static class GraphicsPathExtensions
    {
        private static PointF Mul(PointF p, float a)
        {
            return new PointF(p.X * a, p.Y * a);
        }

        private static PointF Add(PointF a, PointF b)
        {
            return new PointF(a.X + b.X, a.Y + b.Y);
        }

        public static RectangleF AddWindowsFontOutline(this GraphicsPath graphicsPath, string text, string faceName, float size, FontStyle style, byte charSet, PointF origin, bool enableHorizontalCenteringMode)
        {
            var gdi32FontStyle = new Gdi32FontStyleInfo(style);

            // アウトライン取得用のフォントを作成する。
            // アウトラインは固定サイズで取得されるため、このときのフォントサイズは固定値にしてキャッシュが効きやすくする
            var font = Gdi32FontPool.GetPoolingFont(
                faceName,
                FontSizeUnit.Pixel,
                12,
                gdi32FontStyle.Weight,
                gdi32FontStyle.Italic,
                gdi32FontStyle.Underline,
                gdi32FontStyle.StrikeOut,
                charSet,
                Gdi32FontQuality.ClearType
                );

            var outline = font.GetOutline(text, OutlineMode.Bezier);

            if (outline is null)
            {
                return new RectangleF(origin, SizeF.Empty);
            }

            return AddWindowsFontOutline(graphicsPath, outline, size, origin, enableHorizontalCenteringMode);
        }


        public static RectangleF AddWindowsFontOutline(this GraphicsPath graphicsPath, FontOutline outline, float size, PointF origin, bool enableHorizontalCenteringMode)
        {
            if (outline is null || outline.Polygons.Length == 0)
            {
                return new RectangleF(origin, SizeF.Empty);
            }

            float horizontalOffset = enableHorizontalCenteringMode ? outline.GlyphMetrics.CalcHorizontalCenteringOffset(outline.EmSquare) : 0;

            var scaleFactor = size / outline.EmSquare;

            foreach (var polygon in outline.Polygons)
            {
                graphicsPath.StartFigure();

                var startPoint = polygon.StartPoint;

                foreach (var curve in polygon.Curves)
                {
                    var points = curve.ToGraphicsPathPoint(ref startPoint, horizontalOffset, scaleFactor, origin);

                    if (curve.PrimitiveType == TtPrimitiveTypes.Line)
                    {
                        graphicsPath.AddLines(points);
                    }
                    else if (curve.PrimitiveType == TtPrimitiveTypes.CubicBezierSpline)
                    {
                        graphicsPath.AddBeziers(points);
                    }
                    else if (curve.PrimitiveType == TtPrimitiveTypes.QuadraticBezierSpline)
                    {
                        var curveCount = (points.Length - 1) / 2;

                        var cubicBezierPoints = new PointF[points.Length + curveCount];

                        for (int i = 0; i < curveCount; i++)
                        {
                            var c0 = points[i * 3 + 0];
                            var c1 = points[i * 3 + 1];
                            var c2 = points[i * 3 + 2];

                            cubicBezierPoints[i * 4 + 0] = c0;
                            cubicBezierPoints[i * 4 + 1] = Add(Mul(c0, 1 / 3f), Mul(c1, 2 / 3f));
                            cubicBezierPoints[i * 4 + 2] = Add(Mul(c2, 1 / 3f), Mul(c1, 2 / 3f));
                            cubicBezierPoints[i * 4 + 3] = c2;
                        }

                        graphicsPath.AddBeziers(cubicBezierPoints);
                    }
                }

                graphicsPath.CloseFigure();
            }


            return new RectangleF(origin, new SizeF(outline.EmSquare, outline.EmSquare));
        }
    }
}
