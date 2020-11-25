using Gdi32Fonts;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Gdi32FontsSample
{
    class Program
    {
        const string sampleSVG = @"
<svg width=""5cm"" height=""4cm"" viewBox=""0 0 500 400""
     xmlns=""http://www.w3.org/2000/svg"" version=""1.1"">
  <title>Example cubic01- cubic Bézier commands in path data</title>
  <desc>Picture showing a simple example of path data
        using both a ""C"" and an ""S"" command,
        along with annotations showing the control points
        and end points</desc>
  <style type=""text/css""><![CDATA[
    .Border { fill:none; stroke:blue; stroke-width:1 }
    .Connect { fill:none; stroke:#888888; stroke-width:2 }
    .SamplePath { fill:none; stroke:red; stroke-width:5 }
    .EndPoint { fill:none; stroke:#888888; stroke-width:2 }
    .CtlPoint { fill:#888888; stroke:none }
    .AutoCtlPoint { fill:none; stroke:blue; stroke-width:4 }
    .Label { font-size:22; font-family:Verdana }
  ]]></style>

  <rect class=""Border"" x=""1"" y=""1"" width=""498"" height=""398"" />

  <polyline class=""Connect"" points=""100,200 100,100"" />
  <polyline class=""Connect"" points=""250,100 250,200"" />
  <polyline class=""Connect"" points=""250,200 250,300"" />
  <polyline class=""Connect"" points=""400,300 400,200"" />
  <path class=""SamplePath"" d=""M100,200 C100,100 250,100 250,200
                                       S400,300 400,200"" />
  <circle class=""EndPoint"" cx=""100"" cy=""200"" r=""10"" />
  <circle class=""EndPoint"" cx=""250"" cy=""200"" r=""10"" />
  <circle class=""EndPoint"" cx=""400"" cy=""200"" r=""10"" />
  <circle class=""CtlPoint"" cx=""100"" cy=""100"" r=""10"" />
  <circle class=""CtlPoint"" cx=""250"" cy=""100"" r=""10"" />
  <circle class=""CtlPoint"" cx=""400"" cy=""300"" r=""10"" />
  <circle class=""AutoCtlPoint"" cx=""250"" cy=""300"" r=""9"" />
  <text class=""Label"" x=""25"" y=""70"">M100,200 C100,100 250,100 250,200</text>
  <text class=""Label"" x=""325"" y=""350""
        style=""text-anchor:middle"">S400,300 400,200</text>
</svg>
";

        static void Main(string[] args)
        {
            var size = 512;
            var origin = PointF.Empty;

            using (var ipaFont = new Gdi32Font("IPAmj明朝", FontSizeUnit.Pixel, size, FontWeightConsts.FW_NORMAL, false, false, false, 1, Gdi32FontQuality.Default))
            using (var msMinchoFont = new Gdi32Font("ＭＳ 明朝", FontSizeUnit.Pixel, size, FontWeightConsts.FW_NORMAL, false, false, false, 1, Gdi32FontQuality.Default))
            using (var meiryoFont = new Gdi32Font("Meiryo UI", FontSizeUnit.Pixel, size, FontWeightConsts.FW_NORMAL, false, false, false, 1, Gdi32FontQuality.Default))
            using (var arialFont = new Gdi32Font("Arial", FontSizeUnit.Pixel, size, FontWeightConsts.FW_NORMAL, false, false, false, 1, Gdi32FontQuality.Default))
            using (var gothicFont = new Gdi32Font("MS UI Gothic", FontSizeUnit.Pixel, size, FontWeightConsts.FW_NORMAL, false, false, false, 1, Gdi32FontQuality.Default))
            {
                var htmlBuilder = new StringBuilder(4096);

                htmlBuilder.AppendLine("<html><body>");
                htmlBuilder.AppendLine("<h1>SVGTest</h1>");


                htmlBuilder.AppendLine("<h2>IPAmj明朝</h2>");
                RenderSvgSample("f", size, origin, ipaFont, htmlBuilder);
                RenderSvgSample("g", size, origin, ipaFont, htmlBuilder);
                RenderSvgSample("薔", size, origin, ipaFont, htmlBuilder);
                RenderSvgSample("■", size, origin, ipaFont, htmlBuilder);
                htmlBuilder.AppendLine("<h2>ＭＳ 明朝</h2>");
                RenderSvgSample("f", size, origin, msMinchoFont, htmlBuilder);
                RenderSvgSample("g", size, origin, msMinchoFont, htmlBuilder);
                RenderSvgSample("薔", size, origin, msMinchoFont, htmlBuilder);
                RenderSvgSample("■", size, origin, msMinchoFont, htmlBuilder);
                htmlBuilder.AppendLine("<h2>Meiryo UI</h2>");
                RenderSvgSample("f", size, origin, meiryoFont, htmlBuilder);
                RenderSvgSample("g", size, origin, meiryoFont, htmlBuilder);
                RenderSvgSample("薔", size, origin, meiryoFont, htmlBuilder);
                RenderSvgSample("■", size, origin, meiryoFont, htmlBuilder);
                htmlBuilder.AppendLine("<h2>MS UI Gothic</h2>");
                RenderSvgSample("f", size, origin, gothicFont, htmlBuilder);
                RenderSvgSample("g", size, origin, gothicFont, htmlBuilder);
                RenderSvgSample("薔", size, origin, gothicFont, htmlBuilder);
                RenderSvgSample("■", size, origin, gothicFont, htmlBuilder);
                htmlBuilder.AppendLine("<h2>Arial</h2>");
                RenderSvgSample("f", size, origin, arialFont, htmlBuilder);
                RenderSvgSample("g", size, origin, arialFont, htmlBuilder);

                htmlBuilder.AppendLine("<h2>比較</h2>");
                RenderSvgSample("薔", size, origin, msMinchoFont, htmlBuilder);
                RenderSvgSample("薔", size, origin, meiryoFont, htmlBuilder);

                htmlBuilder.AppendLine("</body></html>");

                File.WriteAllText("test.html", htmlBuilder.ToString(), Encoding.UTF8);

                var info = new ProcessStartInfo
                {
                    FileName = "test.html",
                    UseShellExecute = true,
                };

                Process.Start(info);
            }

            static void RenderSvgSample(string text, int size, PointF origin, Gdi32Font font, StringBuilder htmlBuilder)
            {
                htmlBuilder.AppendLine(@$"<svg style=""border: solid black"" width=""10em"" height=""10em"" viewBox=""0 0 {size} {size}"">");

                var outline = font.GetOutline(text, OutlineMode.Native);

                if (outline is { })
                {
                    var enableHorizontalCenteringMode = true;

                    htmlBuilder.AppendLine(@$"<path style=""fill:black; stroke:red; stroke-width:3"" d=""{outline.ToSvgPathData(size, origin, enableHorizontalCenteringMode)}"" />");

                    RenderControlPoints(text, size, origin, font, htmlBuilder);
                }

                htmlBuilder.AppendLine("</svg>");
            }

            static void RenderControlPoints(string text, int size, PointF origin, Gdi32Font font, StringBuilder htmlBuilder)
            {
                var outline = font.GetOutline(text, OutlineMode.Native);

                if (outline is null)
                {
                    return;
                }

                var enableHorizontalCenteringMode = true;

                float horizontalOffset = enableHorizontalCenteringMode ? outline.GlyphMetrics.CalcHorizontalCenteringOffset(outline.EmSquare) : 0;

                var scaleFactor = (float)size / outline.EmSquare;

                var pathDataBuilder = new StringBuilder();

                foreach (var polygon in outline.Polygons)
                {
                    var normalizedStartPoint = NormalizePoint(polygon.StartPoint, horizontalOffset, scaleFactor, origin);
                    htmlBuilder.AppendLine($@"<circle style=""fill:red"" cx=""{normalizedStartPoint.X}"" cy=""{normalizedStartPoint.Y}"" r=""3""/>");

                    foreach (var curve in polygon.Curves)
                    {
                        if (curve.PrimitiveType == TtPrimitiveTypes.Line)
                        {
                            foreach (var point in curve.Points)
                            {
                                var normarizedPoint = NormalizePoint(point, horizontalOffset, scaleFactor, origin);
                                htmlBuilder.AppendLine($@"<circle style=""fill:black"" cx=""{normarizedPoint.X}"" cy=""{normarizedPoint.Y}"" r=""3""/>");
                            }
                        }
                        else if (curve.PrimitiveType == TtPrimitiveTypes.CubicBezierSpline)
                        {
                            foreach (var point in curve.Points)
                            {
                                var normarizedPoint = NormalizePoint(point, horizontalOffset, scaleFactor, origin);
                                htmlBuilder.AppendLine($@"<circle style=""fill:blue"" cx=""{normarizedPoint.X}"" cy=""{normarizedPoint.Y}"" r=""3""/>");
                            }
                        }
                        else if (curve.PrimitiveType == TtPrimitiveTypes.QuadraticBezierSpline)
                        {
                            foreach (var point in curve.Points)
                            {
                                var normarizedPoint = NormalizePoint(point, horizontalOffset, scaleFactor, origin);
                                htmlBuilder.AppendLine($@"<circle style=""fill:green"" cx=""{normarizedPoint.X}"" cy=""{normarizedPoint.Y}"" r=""3""/>");
                            }
                        }
                        var p1 = NormalizePoint(curve.Points.First(), horizontalOffset, scaleFactor, origin);
                        htmlBuilder.AppendLine($@"<circle style=""fill:white"" cx=""{p1.X}"" cy=""{p1.Y}"" r=""2""/>");
                        var p2 = NormalizePoint(curve.Points.Last(), horizontalOffset, scaleFactor, origin);
                        htmlBuilder.AppendLine($@"<circle style=""fill:gray"" cx=""{p2.X}"" cy=""{p2.Y}"" r=""2""/>");
                    }
                }

                static PointF NormalizePoint(TtPolygonPoint point, float horizontalOffset, float scaleFactor, PointF origin)
                {
                    return new PointF((point.X + horizontalOffset) * scaleFactor + origin.X, point.Y * scaleFactor + origin.Y);
                }

            }
        }
    }
}
