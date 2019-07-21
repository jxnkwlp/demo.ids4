using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebImplicitClientTest2.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		// GET: Home
		public ActionResult Index()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult Check()
		{
			return Redirect("~/");
		}

		public ActionResult Logout()
		{
			Request.GetOwinContext().Authentication.SignOut();

			return Redirect("~/");
		}
	}
}