using Microsoft.AspNetCore.Identity;
using WebAppodev.Models;

namespace WebAppodev.Data
{
	public static class DbSeeder
	{
		public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
		{
			// Get the RoleManager and UserManager from the service provider
			var userManager = service.GetService<UserManager<IdentityUser>>();
			var roleManager = service.GetService<RoleManager<IdentityRole>>();

			// 1. Create Roles if they don't exist
			await CreateRoleAsync(roleManager, "Admin");
			await CreateRoleAsync(roleManager, "Trainer");
			await CreateRoleAsync(roleManager, "Member");

			// 2. Create the Admin User (Required by Project)
			// CHANGE THIS EMAIL to your specific student number!
			var adminEmail = "G231210563@sakarya.edu.tr";
			var adminUser = await userManager.FindByEmailAsync(adminEmail);

			if (adminUser == null)
			{
				adminUser = new IdentityUser
				{
					UserName = adminEmail,
					Email = adminEmail,
					EmailConfirmed = true
				};

				// Create user with password "sau"
				var result = await userManager.CreateAsync(adminUser, "sau");

				if (result.Succeeded)
				{
					// Assign Admin role
					await userManager.AddToRoleAsync(adminUser, "Admin");
				}
			}
		}

		private static async Task CreateRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
		{
			if (!await roleManager.RoleExistsAsync(roleName))
			{
				await roleManager.CreateAsync(new IdentityRole(roleName));
			}
		}
	}
}