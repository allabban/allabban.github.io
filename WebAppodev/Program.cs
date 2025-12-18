using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAppodev.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// --- MODIFIED SECTION START ---
// We replaced 'AddDefaultIdentity' with 'AddIdentity' to support Roles and simple passwords.
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
	// 1. Disable account confirmation for easier testing
	options.SignIn.RequireConfirmedAccount = false;

	// 2. Relax password rules to allow "sau" (Project Requirement)
	options.Password.RequireDigit = false;
	options.Password.RequireLowercase = false;
	options.Password.RequireUppercase = false;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequiredLength = 3;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI(); // This ensures Login/Register pages still work
				 // --- MODIFIED SECTION END ---

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Keep your existing default route below this:
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}")
	.WithStaticAssets();


app.MapRazorPages()
   .WithStaticAssets();


using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	await WebAppodev.Data.DbSeeder.SeedRolesAndAdminAsync(services);
}


app.Run();