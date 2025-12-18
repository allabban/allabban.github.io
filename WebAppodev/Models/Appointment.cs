using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebAppodev.Models
{
	public class Appointment
	{
		public int Id { get; set; }

		[Required]
		[Display(Name = "Randevu Tarihi")]
		public DateTime Date { get; set; }

		// Foreign Key to the User (Member)
		[Required]
		public string MemberId { get; set; }
		public IdentityUser Member { get; set; }

		// Foreign Key to the Trainer
		[Required]
		public int TrainerId { get; set; }
		public Trainer Trainer { get; set; }

		// Foreign Key to the Service (Class type)
		[Required]
		public int ServiceId { get; set; }
		public Service Service { get; set; }

		public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled
	}
}