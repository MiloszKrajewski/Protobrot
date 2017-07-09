using System;
using Newtonsoft.Json;

namespace Protobrot.Image.Model
{
	public static class Point
	{
		public static Point<T> Zero<T>() => new Point<T>();
		public static Point<T> Create<T>(T x, T y) => new Point<T>(x, y);

		public static Point<U> Map<T, U>(this Point<T> point, Func<T, U> map) =>
			new Point<U>(map(point.X), map(point.Y));
	}

	public struct Point<T>
	{
		public static readonly Point<T> Zero = new Point<T>();

		[JsonProperty("x")]
		private readonly T _x;

		[JsonProperty("y")]
		private readonly T _y;

		[JsonIgnore]
		public T X => _x;

		[JsonIgnore]
		public T Y => _y;

		public Point(T x, T y)
		{
			_x = x;
			_y = y;
		}

		public override string ToString() => $"{GetType().Name}(X:{X}, Y:{Y})";
	}
}
