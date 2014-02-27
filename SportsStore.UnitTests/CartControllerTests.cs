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
	[Category("Cart Controller")]
	class CartControllerTests
	{
		private Mock<IProductRepository> Mock { get; set; }

		[SetUp]
		public void Setup()
		{
			Mock = new Mock<IProductRepository>();
		}

		[TearDown]
		public void TearDown()
		{
			Mock = null;
		}

		[Test]
		public void Can_Add_To_Cart()
		{
			// Arrange
			// this test needs a single item in the Mock Repo
			Mock.Setup(m => m.Products).Returns(new Product[]{
				new Product() { ProductID = 1, Name = "P1", Category = "Apples" }
			}.AsQueryable());

			Cart cart = new Cart();

			CartController target = new CartController(Mock.Object, null);

			// Act
			target.AddToCart(cart, 1, null);

			// Assert
			Assert.AreEqual(cart.Lines.Count(), 1, "Number of Cart Lines Failed");
			Assert.AreEqual(cart.Lines.First().Product.ProductID, 1, "First Cart Line ProductID Failed");
		}

		[Test]
		public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
		{
			// Arrange
			Cart cart = new Cart();

			CartController target = new CartController(Mock.Object, null);

			// Act
			RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");

			// Assert
			Assert.AreEqual(result.RouteValues["action"], "Index");
			Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
		}

		[Test]
		public void Can_View_Cart_Contents()
		{
			// Arrange
			Cart cart = new Cart();

			CartController target = new CartController(null, null);

			// Act
			CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

			// Assert
			Assert.AreSame(result.Cart, cart);
			Assert.AreEqual(result.ReturnUrl, "myUrl");
		}

		[Test]
		public void Cannot_Checkout_Empty_Cart()
		{
			// Arrange
			Mock<IOrderProcessor> Mock = new Mock<IOrderProcessor>();

			Cart cart = new Cart();
			ShippingDetails shippingDetails = new ShippingDetails();

			CartController target = new CartController(null, Mock.Object);

			// Act
			ViewResult result = target.Checkout(cart, shippingDetails);

			// Assert
			Mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
		}

		[Test]
		public void Cannot_Checkout_Invalid_ShippingDetails()
		{
			// Arrange
			Mock<IOrderProcessor> Mock = new Mock<IOrderProcessor>();

			Cart cart = new Cart();
			cart.AddItem(new Product(), 1);

			CartController target = new CartController(null, Mock.Object);
			target.ModelState.AddModelError("error", "error");

			// Act
			ViewResult result = target.Checkout(cart, new ShippingDetails());

			// Assert - check that the order hasn't been passed on to the processor
			Mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
		}

		[Test]
		public void Can_Checkout_And_Submit_Order()
		{
			// Arrange
			Mock<IOrderProcessor> Mock = new Mock<IOrderProcessor>();

			Cart cart = new Cart();
			cart.AddItem(new Product(), 1);

			CartController target = new CartController(null, Mock.Object);

			// Act
			ViewResult result = target.Checkout(cart, new ShippingDetails());

			// Assert
			Mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once());

			Assert.AreEqual("Completed", result.ViewName);
			Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
		}
	}
}
