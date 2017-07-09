using System.Diagnostics.CodeAnalysis;

namespace Protobrot.Image
{
	[SuppressMessage("ReSharper", "ConvertToAutoPropertyWhenPossible")]
	public struct Complex
	{
		private readonly double _re;
		private readonly double _im;

		public static readonly Complex Zero = new Complex();
		public static Complex Create(double re, double im) => new Complex(re, im);

		public Complex(double re, double im)
		{
			_re = re;
			_im = im;
		}

		public double Re => _re;
		public double Im => _im;

		public static Complex operator +(Complex x, Complex y) =>
			new Complex(x._re + y._re, x._im + y._im);

		public static Complex operator *(Complex x, Complex y) =>
			new Complex(x._re * y._re - x._im * y._im, x._re * y._im + x._im * y._re);

		public double Norm() => _re * _re + _im * _im;
	}
}
