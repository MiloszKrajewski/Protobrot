namespace Protobrot.Image
{
	public class MathEx
	{
		public static double Translate(int iv, int isv, int iev, double osv, double oev) => 
			osv + (iv - isv) * (oev - osv) / (iev - isv);

		public static double Translate(double iv, double isv, double iev, double osv, double oev) => 
			osv + (iv - isv) * (oev - osv) / (iev - isv);
	}
}
