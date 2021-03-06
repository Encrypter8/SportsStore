﻿using System.ComponentModel.DataAnnotations;

namespace SportsStore.Domain.Entities
{
	public class ShippingDetails
	{
		[Required(ErrorMessage = "Please enter a name")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Please enter the first address line")]
		public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string Line3 { get; set; }

		[Required(ErrorMessage = "Please enter a city")]
		public string City { get; set; }

		[Required(ErrorMessage = "Please enter a staet")]
		public string State { get; set; }

		public string Zip { get; set; }

		[Required(ErrorMessage = "Please enter a country")]
		public string Country { get; set; }

		public bool GiftWrap { get; set; }

		public ShippingType ShippingType { get; set; }
	}

	public enum ShippingType
	{
		Standard,
		Express
	}
}
