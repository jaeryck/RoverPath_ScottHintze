using RoverPath.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoverPath.Controllers
{
	public class AjaxErrorHandler : FilterAttribute, IExceptionFilter
	{
		public void OnException(ExceptionContext filterContext)
		{
			filterContext.ExceptionHandled = true;
			filterContext.Result = new JsonResult
			{
				Data = new { success = false, error = filterContext.Exception.ToString() },
				JsonRequestBehavior = JsonRequestBehavior.AllowGet
			};
		}
	}

	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		[AjaxErrorHandler]
		public JsonResult MoveRover(string gridSize, string rover1Start, string rover1Sequence, string rover2Start, string rover2Sequence)
		{
			try
			{
				Rover r1 = new Rover(1, new RoverPosition(rover1Start, 1));
				Rover r2 = new Rover(2, new RoverPosition(rover2Start, 2));
				r1.Position.Move(rover1Sequence, gridSize, r2);
				r2.Position.Move(rover2Sequence, gridSize, r1);
				return Json(new { success = true, status = "Success", rover1 = r1, rover2 = r2 }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				return Json(new { success = false, status = "Error", error = e.InnerException.Message }, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult ProgramLogic()
		{
			return View();
		}

		public ActionResult Requirements()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}