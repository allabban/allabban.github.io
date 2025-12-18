using System.ComponentModel.DataAnnotations;

namespace WebAppodev.Models
{
	public class Service
	{
		public int Id { get; set; }

		[Required]
		[Display(Name = "Hizmet Adı")]
		public string Name { get; set; } // e.g., "Personal Training", "Yoga Class"

		[Display(Name = "Süre (Dakika)")]
		public int DurationMinutes { get; set; } // e.g., 60

		[Display(Name = "Ücret")]
		public decimal Price { get; set; }

		// Optional: Link to specific trainers if not all trainers do all services
		// public ICollection<Trainer> Trainers { get; set; } 
	}
}