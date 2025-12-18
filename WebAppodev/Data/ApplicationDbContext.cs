using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAppodev.Models;

namespace WebAppodev.Data
{
	// We use the standard explicit constructor here for clarity and compatibility
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Trainer> Trainers { get; set; }
		public DbSet<Service> Services { get; set; }
		public DbSet<Appointment> Appointments { get; set; }
	}
}