using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Controllers
{
	public class TestController : Controller
	{
		public ViewResult DataTypes()
		{
			DataTypes dataTypes = new DataTypes()
			{
				DateTime = DateTime.Now
			};

			return View(dataTypes);
		}

	}
}
