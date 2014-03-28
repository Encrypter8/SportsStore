using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
	public abstract class BaseController : Controller
	{
		public JsonResult PartialViewAsJson(string viewName, object model)
		{
			string viewAsString = RenderRazorViewAsString(viewName, model);

			return Json(new { html = viewAsString }, JsonRequestBehavior.AllowGet);
		}

		private string RenderRazorViewAsString(string viewName)
		{
			return RenderRazorViewAsString(viewName, null);
		}
		private string RenderRazorViewAsString(string viewName, object model)
		{
			ViewData.Model = model;

			using (var sw = new StringWriter())
			{
				var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
				var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
				viewResult.View.Render(viewContext, sw);
				viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);

				return sw.GetStringBuilder().ToString();
			}
		}
	}
}