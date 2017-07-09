using Newtonsoft.Json;

namespace Protobrot.Image.Model
{
	public class CellInfo
	{
		[JsonProperty("iterations")]
		public int Iterations { get; set; }

		[JsonProperty("cell")]
		public Extent<int> Cell { get; set; }

		[JsonProperty("bounds")]
		public Extent<double> Bounds { get; set; }
	}
}
