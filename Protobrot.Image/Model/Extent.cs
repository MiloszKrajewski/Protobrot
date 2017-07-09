using System;
using Newtonsoft.Json;

namespace Protobrot.Image.Model
{
	public static class Extent
	{
		public static Extent<T> Zero<T>() => Extent<T>.Zero;
		public static Extent<T> Create<T>(Point<T> s, Point<T> e) => new Extent<T>(s, e);

		public static Extent<T> Create<T>(T sx, T sy, T ex, T ey) =>
			Create(Point.Create(sx, sy), Point.Create(ex, ey));

		public static double Width(this Extent<double> extent) => extent.E.X - extent.S.X;
		public static double Height(this Extent<double> extent) => extent.E.Y - extent.S.Y;
		public static double Aspect(this Extent<double> extent) => extent.Width() / extent.Height();

		public static double Area(this Extent<double> extent) => Math.Abs(
			extent.Width() * extent.Height());

		public static int Width(this Extent<int> extent) => extent.E.X - extent.S.X + 1;
		public static int Height(this Extent<int> extent) => extent.E.Y - extent.S.Y + 1;
		public static double Aspect(this Extent<int> extent) => extent.Width() * 1.0 / extent.Height();
		public static double Area(this Extent<int> extent) => Math.Abs(extent.Width() * extent.Height());

		public static Extent<double> Aspect(this Extent<double> extent, double targetAspect)
		{
			var currentAspect = extent.Aspect();
			if (Math.Abs(currentAspect) < Math.Abs(targetAspect))
			{
				var width = extent.Width();
				var diff = width * (targetAspect / currentAspect - 1) / 2;
				return Create(extent.S.X - diff, extent.S.Y, extent.E.X + diff, extent.E.Y);
			}
			else
			{
				var height = extent.Height();
				var diff = height * (currentAspect / targetAspect - 1) / 2;
				return Create(extent.S.X, extent.S.Y - diff, extent.E.X, extent.E.Y + diff);
			}
		}
	}

	public struct Extent<T>
	{
		public static readonly Extent<T> Zero = new Extent<T>();

		[JsonProperty("s")]
		private readonly Point<T> _s;

		[JsonProperty("e")]
		private readonly Point<T> _e;

		[JsonIgnore]
		public Point<T> S => _s;

		[JsonIgnore]
		public Point<T> E => _e;

		public Extent(Point<T> s, Point<T> e)
		{
			_s = s;
			_e = e;
		}

		public override string ToString() => $"{GetType().Name}(S:{S}, E:{E})";
	}
}
