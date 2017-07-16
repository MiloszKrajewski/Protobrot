using Microsoft.AspNetCore.Mvc;
using Protobrot.Image;
using Protobrot.Image.Model;

namespace Protobrot.Api.Controllers
{
	[Route("api/brot")]
	public class BrotController: Controller
	{
		[HttpGet, Route("plan")]
		public IActionResult Plan(int w, int h) =>
			Json(
				Paintbrot.Plan(
					new ScreenInfo { Size = Point.Create(w, h), Bounds = Extent.Create(-2.5, -1, 1, 1) }));

		[HttpGet, Route("overview")]
		public IActionResult GetOverview() => GetImage(100, 1400, 800, -2.5, -1, 1, 1);

		[HttpGet, Route("image")]
		public IActionResult GetImage(int i, int w, int h, double sx, double sy, double ex, double ey) =>
			File(Paintbrot.Paint(i, w, h, Extent.Create(sx, sy, ex, ey)), "image/png");
	}
}
