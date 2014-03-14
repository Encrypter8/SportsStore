using System.Web.Mvc;
using System.Web.Routing;

namespace SportsStore.WebUI
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			#region AdminController

			routes.MapRoute(
				name: null,
				url: "Admin/Edit/{productId}",
				defaults: new { controller = "Admin", action = "Edit" },
				constraints: new { productId = @"\d+" }
			);

			routes.MapRoute(
				name: null,
				url: "Admin",
				defaults: new { controller = "Admin", action = "Index" }
			);

			#endregion

			#region ProductController
			routes.MapRoute(
				name: null,
				url: "",
				defaults: new { controller = "Product", action = "List", category = (string)null, page = 1 }
			);

			routes.MapRoute(
				name: null,
				url: "Page{page}",
				defaults: new { controller = "Product", action = "List", category = (string)null },
				constraints: new { page = @"\d+" }
			);

			routes.MapRoute(
				name: null,
				url: "{category}",
				defaults: new { controller = "Product", action = "List", page = 1 }
			);

			routes.MapRoute(
				name: null,
				url: "{category}/Page{page}",
				defaults: new { controller = "Product", action = "List" },
				constraints: new { page = @"\d+" }
			);
			#endregion

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}"
			);
		}
	}
}