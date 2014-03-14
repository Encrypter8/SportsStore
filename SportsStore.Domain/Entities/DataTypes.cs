using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Entities
{
	public class DataTypes
	{
		//[DataType(DataType.Custom)]
		public string Custom { get; set; }

		[DataType(DataType.Date)]
		public DateTime DateTime { get; set; }

		//[DataType(DataType.Date)]
		public string Date { get; set; }

		//[DataType(DataType.Time)]
		public string Time { get; set; }

		//[DataType(DataType.Duration)]
		public string Duration { get; set; }

		//[DataType(DataType.PhoneNumber)]
		public string PhoneNumber { get; set; }
	}
}
