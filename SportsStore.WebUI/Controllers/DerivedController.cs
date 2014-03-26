using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.WebUI.Infrastructure;

namespace SportsStore.WebUI.Controllers
{
	public class DerivedController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Message = "Hello from the DerivedController Index method";
			return View("MyView");
		}

		public ActionResult ProduceOutput(bool redirect = false)
		{
			if (redirect)
			{
				return new CustomRedirectResult("/Basic/Index");
			}
			
			return Content("Controller: Derived, Action: ProduceOutput");
		}

		public ActionResult ProduceRedirect()
		{
			return Redirect("/Basic/Index");
		}
	}
}