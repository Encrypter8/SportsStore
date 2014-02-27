using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SportsStore.Domain.Entities
{
	public class Product
	{
		[HiddenInput(DisplayValue = false)]
		public int ProductID { get; set; }

		[Required(ErrorMessage = "Please enter a Product Name")]
		public string Name { get; set; }

		[DataType(DataType.MultilineText)]
		[Required(ErrorMessage = "Please enter a Description")]
		public string Description { get; set; }

		[Required]
		[Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive Price")]
		public decimal Price { get; set; }

		[Required(ErrorMessage = "Please specify a Category")]
		public string Category { get; set; }
	}
}
