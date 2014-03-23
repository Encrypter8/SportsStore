using System.Collections.Generic;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace SportsStore.WebUI.Controllers
{
	public class TestController : Controller
	{
		private IProductRepository Repo;

		public TestController(IProductRepository repo)
		{
			Repo = repo;
		}

		public ViewResult DataTypes()
		{
			DataTypes dataTypes = new DataTypes()
			{
				DateTime = DateTime.Now
			};

			return View(dataTypes);
		}

		public ActionResult GetDataAsJson()
		{
			IEnumerable<Product> products = Repo.Products;

			string json = JsonConvert.SerializeObject(products);

			return Content(json);
		}

	}
}
