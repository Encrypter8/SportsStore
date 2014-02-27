using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;
using System;
using System.Web.Mvc;
using NUnit.Framework;

namespace SportsStore.UnitTests
{
	[TestFixture]
	[Category("HtmlHelpers Extentions")]
	public class HtmlHelperTests
	{
		[Test]
		public void Can_Generate_Page_Links()
		{
			// Arrange - define an HTML helper - we need to this
			// in order to apply the extension method
			HtmlHelper myHelper = null;

			// Arrange - create PagingInfo data
			PagingInfo pagingInfo = new PagingInfo()
			{
				CurrentPage = 2,
				TotalItems = 28,
				itemsPerPage = 10
			};

			// Arrange - set up the delegate using a lambda expression
			Func<int, string> pageUrlDelegate = i => "Page" + i;

			// Act
			MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

			// Assert
			Assert.AreEqual(result.ToString(), "<a href=\"Page1\">1</a><a class=\"selected\" href=\"Page2\">2</a><a href=\"Page3\">3</a>");
		}
	}
}
