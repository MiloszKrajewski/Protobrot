using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Protobrot.Image.Model;
using SkiaSharp;

namespace Protobrot.Image
{
	[SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
	public class Paintbrot
	{
		private const int ESCAPE = 1 << 8;
		private static readonly double LOG2 = Math.Log(2);

		// ReSharper disable once InconsistentNaming
		private static SKColor RGB(int r, int g, int b) =>
			new SKColor((byte) r, (byte) g, (byte) b);

		private static readonly SKColor[] Palette = {
			RGB(66, 30, 15),
			RGB(25, 7, 26),
			RGB(9, 1, 47),
			RGB(4, 4, 73),
			RGB(0, 7, 100),
			RGB(12, 44, 138),
			RGB(24, 82, 177),
			RGB(57, 125, 209),
			RGB(134, 181, 229),
			RGB(211, 236, 248),
			RGB(241, 233, 191),
			RGB(248, 201, 95),
			RGB(255, 170, 0),
			RGB(204, 128, 0),
			RGB(153, 87, 0),
			RGB(106, 52, 3)
		};

		private static IEnumerable<Extent<int>> Partition(Extent<int> screen, int width, int height)
		{
			var ey = screen.E.Y;
			var ex = screen.E.X;

			var sy = screen.S.Y;
			while (sy < ey)
			{
				var sx = screen.S.X;
				while (sx < ex)
				{
					yield return Extent.Create(
						sx,
						sy,
						Math.Min(sx + width - 1, ex),
						Math.Min(sy + height - 1, ey));

					sx += width;
				}

				sy += height;
			}
		}

		public static IEnumerable<CellInfo> Plan(ScreenInfo screenInfo)
		{
			var w = screenInfo.Size.X;
			var h = screenInfo.Size.Y;
			var screen = Extent.Create(0, 0, w - 1, h - 1);
			var extent = screenInfo.Bounds.Aspect(screen.Aspect());

			double X(int x) => MathEx.Translate(x, 0, w - 1, extent.S.X, extent.E.X);
			double Y(int y) => MathEx.Translate(y, 0, h - 1, extent.S.Y, extent.E.Y);

			var i = (int) Math.Sqrt(screen.Area() / extent.Area()) / 4;

			return Partition(screen, 128, 128)
				.Select(
					e => new CellInfo {
						Iterations = i,
						Cell = e,
						Bounds = Extent.Create(X(e.S.X), Y(e.S.Y), X(e.E.X), Y(e.E.Y))
					});
		}

		public static byte[] Paint(int iterations, int width, int height, Extent<double> bounds)
		{
			using (var bitmap = new SKBitmap(width, height, SKColorType.Bgra8888, SKAlphaType.Opaque))
			{
				Paint(bitmap, Palette, iterations, width, height, bounds);

				using (var file = new MemoryStream())
				using (var image = SKImage.FromBitmap(bitmap))
				using (var data = image.Encode(SKEncodedImageFormat.Png, 80))
				{
					data.SaveTo(file);
					return file.ToArray();
				}
			}
		}

		private static void Paint(
			SKBitmap image, SKColor[] palette,
			int iterations, int width, int height, Extent<double> extent)
		{
			var iex = width - 1;
			var iey = height - 1;

			var fsx = extent.S.X;
			var fsy = extent.S.Y;
			var fex = extent.E.X;
			var fey = extent.E.Y;

			var bitmap = new double[width, height];
			var stats = Calculate(0, 0, iex, iey, fsx, fsy, fex, fey, iterations, bitmap);
			var histogram = Normalize(stats);
			Render(image, palette, bitmap, histogram, 0, 0, iex, iey);
		}

		private static int[] Calculate(
			int isx, int isy, int iex, int iey,
			double fsx, double fsy, double fex, double fey,
			int iterations, double[,] bitmap)
		{
			var stats = new int[iterations];

			for (var iy = isy; iy < iey; iy++)
			for (var ix = isx; ix < iex; ix++)
			{
				var z = Complex.Zero;
				var c = Complex.Create(
					MathEx.Translate(ix, isx, iex, fsx, fex),
					MathEx.Translate(iy, isy, iey, fsy, fey));

				var i = 0;

				while (z.Norm() < ESCAPE && i < iterations)
				{
					z = z * z + c;
					i++;
				}

				double f = i;

				if (i < iterations)
				{
					var logzn = Math.Log(z.Norm()) / 2;
					var nu = Math.Log(logzn / LOG2) / LOG2;
					f = i + 1 - nu;
				}

				i = (int) Math.Floor(f);
				if (i < iterations)
					stats[i]++;
				bitmap[ix - isx, iy - isy] = f;
			}

			return stats;
		}

		private static double[] Normalize(int[] stats)
		{
			var iterations = stats.Length;
			var total = stats.Sum();
			var histogram = new double[iterations];
			for (var i = 0; i < iterations; i++)
			{
				histogram[i] = (double) stats[i] / total;
				if (i > 0)
					histogram[i] += histogram[i - 1];
			}

			return histogram;
		}

		private static void Render(
			SKBitmap image, SKColor[] palette, 
			double[,] bitmap, double[] histogram,
			int isx, int isy, int iex, int iey)
		{
			var iterations = histogram.Length;

			for (var iy = isy; iy < iey; iy++)
			for (var ix = isx; ix < iex; ix++)
			{
				var f = bitmap[ix - isx, iy - isy];
				var i = (int) Math.Floor(f);
				if (i >= iterations)
				{
					image.SetPixel(ix, iy, SKColor.Empty);
					continue;
				}

				var hue = MathEx.Translate(f - i, 0, 1, histogram[i - 1], histogram[i]);
				var color = Interpolate(palette, MathEx.Translate(hue, 0, 1, 0, palette.Length - 1));
				image.SetPixel(ix, iy, color);
			}
		}

		private static SKColor Interpolate(SKColor[] palette, double f)
		{
			var trunc = Math.Floor(f);
			var i = (int) trunc;
			var colorA = palette[i];
			var colorB = palette[i + 1];
			var frac = f - trunc;
			return new SKColor(
				(byte) MathEx.Translate(frac, 0, 1, colorA.Red, colorB.Red),
				(byte) MathEx.Translate(frac, 0, 1, colorA.Green, colorB.Green),
				(byte) MathEx.Translate(frac, 0, 1, colorA.Blue, colorB.Blue));
		}
	}
}
