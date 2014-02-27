using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;

namespace SportsStore.UnitTests
{
	[TestFixture]
	[Category("Product Controller")]
	public class ProductControllerTests
	{
		private Mock<IProductRepository> Mock { get; set; }

		[SetUp]
		public void Setup()
		{
			Mock = new Mock<IProductRepository>();
			Mock.Setup(m => m.Products).Returns(new Product[] {
				new Product { ProductID = 1, Name = "P1", Category = "Cat1", ImageData = new byte[] { }, ImageMimeType = "image/png" },
				new Product { ProductID = 2, Name = "P2", Category = "Cat2" },
				new Product { ProductID = 3, Name = "P3", Category = "Cat1" },
				new Product { ProductID = 4, Name = "P4", Category = "Cat2" },
				new Product { ProductID = 5, Name = "P5", Category = "Cat3" }
			}.AsQueryable());
		}

		[TearDown]
		public void TearDown()
		{
			Mock = null;
		}

		[Test]
		public void Can_Paginate()
		{
			// Arrange
			ProductController controller = new ProductController(Mock.Object);
			controller.PageSize = 3;

			// Act
			ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

			// Assert
			Product[] prodArray = result.Products.ToArray();

			Assert.IsTrue(prodArray.Length == 2);
			Assert.AreEqual(prodArray[0].Name, "P4");
			Assert.AreEqual(prodArray[1].Name, "P5");
		}

		[Test]
		public void Can_Send_Pagination_View_Model()
		{
			// Arrange
			ProductController controller = new ProductController(Mock.Object);
			controller.PageSize = 3;

			// Act
			ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

			// Assert
			PagingInfo pageInfo = result.PagingInfo;
			Assert.AreEqual(pageInfo.CurrentPage, 2);
			Assert.AreEqual(pageInfo.itemsPerPage, 3);
			Assert.AreEqual(pageInfo.TotalItems, 5);
			Assert.AreEqual(pageInfo.TotalPages, 2);
		}

		[Test]
		public void Can_Filter_Products()
		{
			// Arrange
			ProductController controller = new ProductController(Mock.Object);
			controller.PageSize = 3;

			// Act
			Product[] result = ((ProductsListViewModel)controller.List("Cat2", 1).Model).Products.ToArray();

			// Assert
			Assert.AreEqual(result.Length, 2);
			Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
			Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
		}

		[Test]
		public void Can_Create_Categories()
		{
			// Arrange
			NavController target = new NavController(Mock.Object);

			// Act
			string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray();

			// Assert
			Assert.AreEqual(results.Length, 3);
			Assert.AreEqual(results[0], "Cat1");
			Assert.AreEqual(results[1], "Cat2");
			Assert.AreEqual(results[2], "Cat3");
		}

		[Test]
		public void Indicates_Selected_Category()
		{
			// Arrange
			NavController target = new NavController(Mock.Object);

			string categoryToSelect = "Cat2";

			// Act
			string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

			// Assert
			Assert.AreEqual(categoryToSelect, result);
		}

		[Test]
		public void Generate_Category_Specific_Product_Count()
		{
			// Arrange
			ProductController target = new ProductController(Mock.Object);

			// Act
			int res1 = ((ProductsListViewModel)target.List("Cat1").Model).PagingInfo.TotalItems;
			int res2 = ((ProductsListViewModel)target.List("Cat2").Model).PagingInfo.TotalItems;
			int res3 = ((ProductsListViewModel)target.List("Cat3").Model).PagingInfo.TotalItems;
			int resAll = ((ProductsListViewModel)target.List(null).Model).PagingInfo.TotalItems;

			// Assert
			Assert.AreEqual(res1, 2);
			Assert.AreEqual(res2, 2);
			Assert.AreEqual(res3, 1);
			Assert.AreEqual(resAll, 5);
		}

		[Test]
		public void Can_Retrieve_Image_Date()
		{
			// Arrange
			ProductController target = new ProductController(Mock.Object);

			// Act
			ActionResult result = target.GetImage(1);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOf<FileResult>(result);
			Assert.AreEqual("image/png", ((FileResult)result).ContentType);
		}

		[Test]
		public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
		{
			// Arrange
			ProductController target = new ProductController(Mock.Object);

			// Act
			ActionResult result = target.GetImage(100);

			// Assert
			Assert.IsNull(result);
		}
	}
}
