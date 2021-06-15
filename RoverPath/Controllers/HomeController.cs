using RoverPath.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoverPath.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public JsonResult MoveRover(string gridSize, string rover1Start, string rover1Sequence, string rover2Start, string rover2Sequence)
		{
			try
			{
				Rover r1 = new Rover(1, new RoverPosition(rover1Start));
				Rover r2 = new Rover(2, new RoverPosition(rover2Start));
				r1.Position.Move(rover1Sequence, gridSize, r2.Position);
				r2.Position.Move(rover2Sequence, gridSize, r1.Position);
				return Json(new { status = "SUCCESS", rover1 = r1, rover2 = r2 });
			}
			catch (Exception e)
			{
				return Json(new { status = "ERROR", exception = e });
			}
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		private bool ValidateSequence(string gridSize, string roverStart, string roverSequence)
		{
			return false;
		}
	}
}