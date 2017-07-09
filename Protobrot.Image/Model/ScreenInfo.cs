using Newtonsoft.Json;

namespace Protobrot.Image.Model
{
	public class ScreenInfo
	{
		[JsonProperty("size")]
		public Point<int> Size { get; set; }

		[JsonProperty("bounds")]
		public Extent<double> Bounds { get; set; }
	}
}
