using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Infrastructure
{
	public class CustomRedirectResult : ActionResult
	{
		private readonly string _url;

		public CustomRedirectResult(string url)
		{
			_url = url;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			string fullUrl = UrlHelper.GenerateContentUrl(_url, context.HttpContext);
			context.HttpContext.Response.Redirect(fullUrl);
		}
	}
}