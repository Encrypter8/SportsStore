using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;

namespace SportsStore.WebUI.Controllers
{
	public class NavController : Controller
	{
		private IProductRepository Repo;

		public NavController(IProductRepository productRepo)
		{
			Repo = productRepo;
		}

		public PartialViewResult Menu(string category = null)
		{
			ViewBag.SelectedCategory = category;

			IEnumerable<string> categories = Repo.Products
				.Select(x => x.Category)
				.Distinct()
				.OrderBy(x => x);

			return PartialView(categories);
		}

	}
}
