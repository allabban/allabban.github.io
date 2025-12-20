using System.ComponentModel.DataAnnotations;

namespace WebAppodev.Models
{
	public class UserProfile
	{
		[Required] public int Age { get; set; }
		[Required] public double Weight { get; set; }
		[Required] public double Height { get; set; }
		[Required] public string Goal { get; set; }

		public IFormFile? ImageUpload { get; set; }
		public string? AIResponse { get; set; }

		// Fixes "UserProfile does not contain definition for GeneratedImageBase64"
		public string? GeneratedImageBase64 { get; set; }
	}
}