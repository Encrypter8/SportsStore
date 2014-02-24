using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

		public ProductController(IProductRepository productRepo)
		{
			this.Repo = productRepo;
		}

		public ViewResult List(int page = 1)
		{
			ProductsListViewModel model = new ProductsListViewModel()
			{
				Products = Repo.Products
					.OrderBy(p => p.ProductID)
					.Skip((page - 1) * PageSize)
					.Take(PageSize),
				PagingInfo = new PagingInfo()
				{
					CurrentPage = page,
					itemsPerPage = PageSize,
					TotalItems = Repo.Products.Count()
				}
			};

			return View(model);
		}

    }
}
