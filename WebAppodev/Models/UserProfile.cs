using System.ComponentModel.DataAnnotations;

namespace WebAppodev.Models
{
	public class UserProfile
	{
		[Required]
		[Display(Name = "Age (Yaş)")]
		public int Age { get; set; }

		[Required]
		[Display(Name = "Weight (kg)")]
		public double Weight { get; set; }

		[Required]
		[Display(Name = "Height (cm)")]
		public double Height { get; set; }

		[Required]
		[Display(Name = "Fitness Goal")]
		public string Goal { get; set; } // e.g., Lose Weight, Build Muscle

		[Display(Name = "Upload Body Photo (Optional)")]
		public IFormFile? ImageUpload { get; set; }

		// This will hold the AI's response
		public string? AIResponse { get; set; }
	}
}