using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoverPath;
using RoverPath.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace RoverPath.Tests.Controllers
{
	[TestClass]
	public class HomeControllerTest
	{
		[TestMethod]
		public void Index()
		{
			// Arrange
			HomeController controller = new HomeController();

			// Act
			ViewResult result = controller.Index() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Requirements()
		{
			// Arrange
			HomeController controller = new HomeController();

			// Act
			ViewResult result = controller.Requirements() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void ProgramLogic()
		{
			// Arrange
			HomeController controller = new HomeController();

			// Act
			ViewResult result = controller.ProgramLogic() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}
	}
}
