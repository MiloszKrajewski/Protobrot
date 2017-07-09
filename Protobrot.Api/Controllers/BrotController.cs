using System.IO;
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

		[HttpGet]
		public IActionResult GetImage() =>
			File(Paintbrot.Paint(100, 1400, 800, Extent.Create(-2.5, -1, 1, 1)), "image/png");
	}
}
