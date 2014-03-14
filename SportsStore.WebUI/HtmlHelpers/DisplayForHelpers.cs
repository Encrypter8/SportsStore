using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace SportsStore.WebUI.HtmlHelpers
{
	public static class DisplayForHelpers
	{
		public static MvcHtmlString EditorFieldFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TValue>> expression)
		{
			return htmlHelper.EditorFieldFor(expression, new Dictionary<string, object>());
		}

		public static MvcHtmlString EditorFieldFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
		{
			if (expression == null) { throw new ArgumentNullException("expression"); }

			ModelMetadata metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			//Type valueType = typeof(TValue);
			//DataType dataType = DataType.Text;
			string labelText = metaData.DisplayName ?? metaData.PropertyName;

			//if (!String.IsNullOrEmpty((metaData.DataTypeName)))
			//{
			//	Enum.TryParse(metaData.DataTypeName, out dataType);
			//}

			TagBuilder container = new TagBuilder("div");
			container.AddCssClass("labeled-input-field");

			// we want to clear out the default setting of id
			// if it was passed as part of htmlAttributes, we use that
			// otherwise, set it to String.Empty
			if (!htmlAttributes.ContainsKey("id"))
			{
				htmlAttributes["id"] = String.Empty;
			}

			MvcHtmlString input = htmlHelper.TextBoxFor(expression, htmlAttributes);


			StringBuilder sb = new StringBuilder();
			sb.Append("<label>");
			sb.Append("<span class=\"text\">" + htmlHelper.Encode(labelText) + "</span>");
			sb.Append("<span class=\"input\">" + input.ToString() + "</span>");
			sb.Append(htmlHelper.ValidationMessageFor(expression));
			sb.Append("</label>");

			container.InnerHtml = sb.ToString();

			return new MvcHtmlString(container.ToString());
		}
	}
}