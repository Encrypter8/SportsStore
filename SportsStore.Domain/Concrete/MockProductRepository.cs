using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using Newtonsoft.Json;
using System.Linq;

namespace SportsStore.Domain.Concrete
{
	public class MockProductRepository : IProductRepository
	{
		public IQueryable<Product> Products { get { return GetProductsFromFile(); } }

		public void SaveProduct(Product product)
		{
			// do nothing
		}

		public Product DeleteProduct(int productID)
		{
			return new Product();
		}

		private IQueryable<Product> GetProductsFromFile()
		{
			string path = Globals.LocalPath + @"SportsStore.Domain\MockData\Products.json";

			string output;

			using (var myFile = new System.IO.StreamReader(path))
			{
				output = myFile.ReadToEnd();
				myFile.Close();
			}

			return JsonConvert.DeserializeObject<IEnumerable<Product>>(output).AsQueryable();
		}
	}
}
