using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NWDFoundation.Logger;
using NWDFoundation.Tools;
using NWDWebRuntime.Facades;
using NWDWebRuntime.Tools.Sessions;
using SkiaSharp;

namespace NWDWebRuntime.Models
{
    [Serializable]
    public class NWDCaptchaParameters
    {
         public int StrokeWidthMin { set; get; } = 1;
         public int StrokeWidthMax { set; get; } = 3;

         public int FontSizeMax { set; get; } = 48;
         public string FontName { set; get; } = string.Empty;

         public double MinDistortion { set; get; } = 9;
         public double MaxDistortion { set; get; } = 13;

         public double NoisePointsPercent { set; get; } = 0.002;
         public NWDCaptchaParameters()
        {
            
        }
    }

//  https://github.com/biggray/CaptchaGen.SkiaSharp/blob/master/src/CaptchaGen.SkiaSharp/CaptchaGenerator.cs
    public static class NWDCaptcha
    {
        private static NWDSessionString Captcha = new NWDSessionString("Captcha", "Captcha", "Captcha", NWDSessionDefinitionGroup.Invisible, "not defined");
        private static SKColor BackgroundColor { set; get; } = SKColors.Transparent;
        private static SKColor PaintColor { set; get; } = SKColors.DarkGray;
        private static SKColor NoisePointColor { set; get; } = SKColors.Black;

        private static SKColor[] LinesColor { set; get; } = new SKColor[]
        {
            SKColors.Red,
            SKColors.Green,
            SKColors.Yellow,
            SKColors.Blue,
            SKColors.Violet,
            //SKColors.Black,
            //SKColors.Black,
        };

        // private static int StrokeWidthMin { set; get; } = 1;
        // private static int StrokeWidthMax { set; get; } = 3;
        //
        // private static int FontSizeMax { set; get; } = 48;
        // //static public int FontSizeMin { set; get; } = 24;
        //
        // static string FontName { set; get; } = string.Empty;
        //
        // static double MinDistortion { set; get; } = 9;
        // static double MaxDistortion { set; get; } = 13;
        //
        // static double NoisePointsPercent { set; get; } = 0.002;

        public static FileStreamResult GenerateCaptcha(HttpContext sHttpContext, NWDCaptchaParameters? sParameters = null)
        {
            string? tValueString = GetStoredCaptcha(sHttpContext);
            NWDLogger.Information("GenerateCaptcha () tValueString = " + tValueString);
            if (tValueString != null)
            {
                byte[] tValue = GetCaptcha(tValueString, sParameters);
                return new FileStreamResult(new MemoryStream(tValue), "image/png");
            }
            else
            {
                NWDLogger.Critical("Error in GenerateCaptcha");
                byte[] tValue = GetCaptcha("Error", sParameters);
                return new FileStreamResult(new MemoryStream(tValue), "image/png");
            }
        }


        private static byte[] GetCaptcha(string sCaptchaText, NWDCaptchaParameters? sParameters = null)
        {
            if (sParameters == null)
            {
                sParameters = new NWDCaptchaParameters();
            }
            byte[] tImageBytes;
            int tImage2dX;
            int tImage2dY;
            SKRect tSize = new SKRect();
            int tCompensateDeepCharacters = 0;
            using (SKPaint tDrawStyle = new SKPaint())
            {
                tDrawStyle.Typeface = SKTypeface.FromFamilyName(sParameters.FontName, SKFontStyle.Italic);
                tDrawStyle.TextSize = sParameters.FontSizeMax;
                tDrawStyle.Color = PaintColor;
                tDrawStyle.IsAntialias = true;

                tCompensateDeepCharacters = (int)tDrawStyle.TextSize / 5;
                if (StringComparer.Ordinal.Equals(sCaptchaText, sCaptchaText.ToUpperInvariant()))
                {
                    tCompensateDeepCharacters = 0;
                }

                tDrawStyle.MeasureText(sCaptchaText, ref tSize);
                tImage2dX = (int)tSize.Width + sParameters.FontSizeMax / 2;
                tImage2dY = (int)tSize.Height + sParameters.FontSizeMax / 2 + tCompensateDeepCharacters;
            }

            using (SKBitmap tImage2d = new SKBitmap(tImage2dX, tImage2dY, SKColorType.Bgra8888, SKAlphaType.Premul))
            {
                using (SKCanvas tCanvas = new SKCanvas(tImage2d))
                {
                    tCanvas.DrawColor(BackgroundColor); // Clear 
                    using (SKPaint tDrawStyle = new SKPaint())
                    {
                        tDrawStyle.Typeface = SKTypeface.FromFamilyName(sParameters.FontName, SKFontStyle.Italic);
                        tDrawStyle.TextSize = sParameters.FontSizeMax;
                        tDrawStyle.Color = PaintColor;
                        tDrawStyle.IsAntialias = true;
                        tCanvas.DrawText(sCaptchaText, 0 + sParameters.FontSizeMax / 4, tImage2dY - sParameters.FontSizeMax / 4 - tCompensateDeepCharacters, tDrawStyle);
                    }

                    SKImageInfo tImageInfo = new SKImageInfo(tImage2dX, tImage2dY, SKColorType.Bgra8888, SKAlphaType.Premul);
                    using (var tPlainSkSurface = SKSurface.Create(tImageInfo))
                    {
                        var tPlainCanvas = tPlainSkSurface.Canvas;
                        tPlainCanvas.Clear(BackgroundColor);

                        using (var tPaintInfo = new SKPaint())
                        {
                            tPaintInfo.Typeface = SKTypeface.FromFamilyName(sParameters.FontName, SKFontStyle.Italic);
                            tPaintInfo.TextSize = sParameters.FontSizeMax;
                            tPaintInfo.Color = PaintColor;
                            tPaintInfo.IsAntialias = true;
                            tPlainCanvas.DrawText(sCaptchaText, 0 + sParameters.FontSizeMax / 4, tImage2dY - sParameters.FontSizeMax / 4 - tCompensateDeepCharacters, tPaintInfo);
                        }

                        tPlainCanvas.Flush();
                        SKImageInfo tImageInfoSurface = new SKImageInfo(tImage2dX, tImage2dY, SKColorType.Bgra8888, SKAlphaType.Premul);
                        using (SKSurface tCaptchaSkSurface = SKSurface.Create(tImageInfoSurface))
                        {
                            SKCanvas tCaptchaCanvas = tCaptchaSkSurface.Canvas;
                            Random tRandom = new Random();

                            // distorsion
                            double tDistortionLevel = sParameters.MinDistortion + (sParameters.MaxDistortion - sParameters.MinDistortion) * tRandom.NextDouble();
                            if (tRandom.NextDouble() > 0.5)
                            {
                                tDistortionLevel *= -1;
                            }

                            ;
                            SKPixmap tPlainPixmap = tPlainSkSurface.PeekPixels();
                            for (int tX = 0; tX < tImage2dX; tX++)
                            {
                                for (int tY = 0; tY < tImage2dY; tY++)
                                {
                                    var (tNewX, tNewY) = null == DistortionFunc ? (x: tX, y: tY) : DistortionFunc((tX, tY, tDistortionLevel, tImage2dX, tImage2dY));
                                    SKColor tOriginalPixel = tPlainPixmap.GetPixelColor(tNewX, tNewY);
                                    tCaptchaCanvas.DrawPoint(tX, tY, tOriginalPixel);
                                }
                            }

                            // add noise
                            var tNoisePointMap = NoisePointMapGenFunc((tImage2dX, tImage2dY, sParameters.NoisePointsPercent));
                            for (int tI = 0; tI < tNoisePointMap.Count(); tI++)
                            {
                                var tNoisePointPos = tNoisePointMap.ElementAt(tI);
                                tCaptchaCanvas.DrawPoint(tNoisePointPos.x, tNoisePointPos.y, NoisePointColor);
                            }

                            // draw lines
                            SKPaint tDrawLineNoise = new SKPaint();
                            for (int tI = 0; tI < LinesColor.Length; tI++)
                            {
                                tDrawLineNoise.Color = LinesColor[tI];
                                tDrawLineNoise.StrokeWidth = tRandom.Next(sParameters.StrokeWidthMin, sParameters.StrokeWidthMax);
                                tCaptchaCanvas.DrawLine(tRandom.Next(0, tImage2dX), tRandom.Next(0, tImage2dY), tRandom.Next(0, tImage2dX), tRandom.Next(0, tImage2dY), tDrawLineNoise);
                            }

                            tCaptchaCanvas.Flush();

                            using (SKData tP = tCaptchaSkSurface.Snapshot().Encode(SKEncodedImageFormat.Png, 100))
                            {
                                tImageBytes = tP.ToArray();
                            }
                        }
                    }
                }
            }

            return tImageBytes;
        }

        static Func<(int oldX, int oldY, double distortionLevel, int W, int H), (int newX, int newY)> DistortionFunc { set; get; } =
            sOldPos =>
            {
                var tNewX = (int)(sOldPos.oldX + (sOldPos.distortionLevel * Math.Sin(Math.PI * sOldPos.oldY / 64.0)));
                var tNewY = (int)(sOldPos.oldY + (sOldPos.distortionLevel * Math.Cos(Math.PI * sOldPos.oldX / 64.0)));
                if (tNewX < 0 || tNewX >= sOldPos.W) tNewX = 0;
                if (tNewY < 0 || tNewY >= sOldPos.H) tNewY = 0;

                return (newX: tNewX, newY: tNewY);
            };

        static Func<(int W, int H, double NoisePointsPercent), IEnumerable<(int x, int y)>> NoisePointMapGenFunc { set; get; } =
            sData =>
            {
                var tRandom = new Random();
                var tNoisePointCount = (int)(sData.W * sData.H * sData.NoisePointsPercent);
                var tNoisePointPosList = Enumerable.Range(0, tNoisePointCount)
                    .Select(
                        sX =>
                        (
                            tRandom.Next(sData.W),
                            tRandom.Next(sData.H)
                        )
                    ).ToArray();
                return tNoisePointPosList;
            };


        public static string RandomCaptchaToImage(HttpContext sHttpContext, NWDCaptchaParameters? sParameters = null)
        {
            string tCaptcha = NWDRandom.RandomCaptchaNoMistake(8);
            Captcha.SetValue(sHttpContext, tCaptcha);
            //NWDLogger.Warning("RandomCaptcha value is '" + tCaptcha + "';");
            byte[] tImage = GetCaptcha(tCaptcha, sParameters);
            return Convert.ToBase64String(tImage);
        }

        public static string GetStoredCaptcha(HttpContext sHttpContext)
        {
            string tCaptcha = Captcha.GetValue(sHttpContext);
            //NWDLogger.Warning("GetStoredCaptcha value is '" + tCaptcha + "';");
            return tCaptcha;
        }


        public static bool TestCaptcha(HttpContext sHttpContext, INWDCaptcha sObject)
        {
            bool rReturn = false;
            string? tCaptcha = GetStoredCaptcha(sHttpContext);
            if (tCaptcha != null)
            {
               //NWDLogger.Warning("tCaptcha session value is '" + tCaptcha + "' and  value receipted by sObject is '" + sObject.CaptchaValue + "'");
                if (sObject.CaptchaValue.ToLower() == tCaptcha.ToLower())
                {
                    rReturn = true;
                }
            }
            return rReturn;
        }
    }
}