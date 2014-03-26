using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class ErrorsController : Controller
    {
	    public ActionResult NotFound404()
	    {
		    object model = Request.Url.PathAndQuery;

		    if (Request.IsAjaxRequest())
		    {
			    return PartialView(model);
		    }

		    return View(model);
	    }
    }
}
