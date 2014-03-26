using System.Web;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Binders;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Infrastructure;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SportsStore.WebUI
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			// custom
			ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
			ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder());
		}

		protected void Application_EndRequest()
		{
			if (Context.Response.StatusCode == 404)
			{
				Response.Clear();

				var rd = new RouteData();
				rd.Values["controller"] = "Errors";
				rd.Values["action"] = "NotFound404";

				IController c = new ErrorsController();
				c.Execute(new RequestContext(new HttpContextWrapper(Context), rd)); 
			}
		}
	}
}