using System.ComponentModel.DataAnnotations; // <--- This was missing!

namespace WebAppodev.Models
{
	public class Trainer
	{
		public int Id { get; set; }

		[Required]
		public string FullName { get; set; }

		[Required]
		public string Specialty { get; set;} // [cite: 16]

		public int ExperienceYears { get; set; }
	}
}