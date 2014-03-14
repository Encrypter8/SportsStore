using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
	public class ProductController : Controller
	{
		private IProductRepository Repo;

		public int PageSize = 4;

		public ProductController(IProductRepository repo)
		{
			Repo = repo;
		}

		public ViewResult List(string category, int page = 1)
		{
			ProductsListViewModel model = new ProductsListViewModel()
			{
				Products = Repo.Products
					.Where(p => category == null || p.Category == category)
					.OrderBy(p => p.ProductID)
					.Skip((page - 1) * PageSize)
					.Take(PageSize),
				PagingInfo = new PagingInfo()
				{
					CurrentPage = page,
					itemsPerPage = PageSize,
					TotalItems = category == null ? Repo.Products.Count() : Repo.Products.Where(p => p.Category == category).Count()
				},
				CurrentCategory = category
			};

			return View(model);
		}

		/// <summary>
		/// use @class to avoid conflict with class
		/// works with any reserved keywords!
		/// </summary>
		/// <param name="class"></param>
		/// <returns></returns>
		public string Test(string @class)
		{
			return @class;
		}

		public FileContentResult GetImage(int productId)
		{
			Product product = Repo.Products.FirstOrDefault(p => p.ProductID == productId);

			if (product != null)
			{
				return File(product.ImageData, product.ImageMimeType);
			}

			return null;
		}
	}
}
