using System.ComponentModel.DataAnnotations; // <--- This was missing!

namespace WebAppodev.Models
{
    public class Trainer
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Specialty { get; set; }

        public int ExperienceYears { get; set; }
    }
}