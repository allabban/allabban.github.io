using System.ComponentModel.DataAnnotations;

namespace WebAppodev.Models
{
	public class Service
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string Description { get; set; }

		// Fixes "Service does not contain definition for Duration"
		[Range(15, 240)]
		[Display(Name = "Duration (Minutes)")]
		public int Duration { get; set; } = 60;

		// Fixes "Service does not contain definition for Price"
		[Range(0, 10000)]
		public decimal Price { get; set; }
	}
}