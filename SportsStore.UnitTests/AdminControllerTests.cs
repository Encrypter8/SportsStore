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
	[Category("Admin Controller")]
	class AdminControllerTests
	{
		private Mock<IProductRepository> Mock { get; set; }

		[SetUp]
		public void Setup()
		{
			Mock = new Mock<IProductRepository>();
			Mock.Setup(m => m.Products).Returns(new Product[] {
				new Product { ProductID = 1, Name = "P1", Category = "Cat1" },
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
		public void Index_Contains_All_Products()
		{
			// Arrange
			AdminController target = new AdminController(Mock.Object);

			// Act
			Product[] result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();

			// Assert
			Assert.AreEqual(result.Length, 5);
			Assert.AreEqual("P1", result[0].Name);
			Assert.AreEqual("P2", result[1].Name);
			Assert.AreEqual("P3", result[2].Name);
			Assert.AreEqual("P4", result[3].Name);
			Assert.AreEqual("P5", result[4].Name);
		}

		[Test]
		public void Can_Edit_Product()
		{
			// Arrange
			AdminController target = new AdminController(Mock.Object);

			// Act
			Product p1 = target.Edit(1).ViewData.Model as Product;
			Product p2 = target.Edit(2).ViewData.Model as Product;
			Product p3 = target.Edit(3).ViewData.Model as Product;

			// Assert
			Assert.AreEqual(1, p1.ProductID);
			Assert.AreEqual(2, p2.ProductID);
			Assert.AreEqual(3, p3.ProductID);
		}

		[Test]
		public void Cannot_Edit_Nonexistent_Product()
		{
			// Arrange
			AdminController target = new AdminController(Mock.Object);

			// Act
			Product result = (Product)target.Edit(14).ViewData.Model;

			// Assert
			Assert.IsNull(result);
		}

		[Test]
		public void Can_Save_Valid_Changes()
		{
			// Arrange
			AdminController target = new AdminController(Mock.Object);

			Product product = new Product() { Name = "Test" };

			// Act
			ActionResult result = target.Edit(product);

			// Assert
			Mock.Verify(m => m.SaveProduct(product));
			Assert.IsNotInstanceOf<ViewResult>(result);
		}

		[Test]
		public void Cannot_Save_Invalid_Changes()
		{
			// Arrange
			Product product = new Product() { Name = "Test" };

			AdminController target = new AdminController(Mock.Object);
			target.ModelState.AddModelError("error", "error");

			// act
			ActionResult result = target.Edit(product);

			// Assert
			Mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());

			Assert.IsInstanceOf<ViewResult>(result);
		}

		[Test]
		public void Can_Delete_Valid_Products()
		{
			// Arrange
			Product product = Mock.Object.Products.ElementAt(1);

			AdminController target = new AdminController(Mock.Object);

			// Act
			target.Delete(product.ProductID);

			// Assert
			Mock.Verify(m => m.DeleteProduct(product.ProductID));
		}

	}
}
