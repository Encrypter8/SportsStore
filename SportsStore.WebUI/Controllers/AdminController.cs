using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Controllers
{
	[Authorize]
    public class AdminController : Controller
    {
		private IProductRepository Repo;

		public AdminController(IProductRepository repo)
		{
			Repo = repo;
		}

        public ViewResult Index()
        {
            return View(Repo.Products);
        }

		public ViewResult Create()
		{
			return View("Edit", new Product());
		}

		public ViewResult Edit(int productId)
		{
			Product product = Repo.Products.FirstOrDefault(p => p.ProductID == productId);

			return View(product);
		}

		[HttpPost]
		public ActionResult Edit(Product product)
		{
			if (ModelState.IsValid)
			{
				Repo.SaveProduct(product);
				TempData["message"] = String.Format("{0} has been saved", product.Name);
				return RedirectToAction("Index");
			}
			else
			{
				// there is something wrong with data value
				return View(product);
			}
		}

		[HttpPost]
		public ActionResult Delete(int productId)
		{
			Product deletedProduct = Repo.DeleteProduct(productId);

			if (deletedProduct != null)
			{
				TempData["message"] = String.Format("{0} was deleted", deletedProduct.Name);
			}

			return RedirectToAction("Index");
		}

    }
}
