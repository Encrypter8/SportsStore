using Moq;
using NUnit.Framework;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
	[TestFixture]
	[Category("Admin Security")]
	class AdminSecurityTests
	{
		private Mock<IAuthProvider> Mock { get; set; }

		[SetUp]
		public void Setup()
		{
			Mock = new Mock<IAuthProvider>();
		}

		[TearDown]
		public void TearDown()
		{
			Mock = null;
		}

		[Test]
		public void Can_Login_With_Valid_Credentials()
		{
			// Arrange
			Mock.Setup(m => m.Authenticate("admin", "secret")).Returns(true);

			LoginViewModel model = new LoginViewModel()
			{
				UserName = "admin",
				Password = "secret"
			};

			AccountController target = new AccountController(Mock.Object);

			// Act
			ActionResult result = target.Login(model, "/MyUrl");

			// Assert
			Assert.IsInstanceOf<RedirectResult>(result);
			Assert.AreEqual("/MyUrl", ((RedirectResult)result).Url);
		}

		[Test]
		public void Cannot_Login_With_Invalid_Credentials()
		{
			// Arrange
			Mock.Setup(m => m.Authenticate("badUser", "badPass")).Returns(false);

			LoginViewModel model = new LoginViewModel()
			{
				UserName = "badUser",
				Password = "badPass"
			};

			AccountController target = new AccountController(Mock.Object);

			// Act
			ActionResult result = target.Login(model, "/MyUrl");

			// Assert
			Assert.IsInstanceOf<ViewResult>(result);
			Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
		}
	}
}
