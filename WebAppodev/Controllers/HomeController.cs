using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Needed for LINQ (Include, ToList)
using WebAppodev.Data;
using WebAppodev.Models;

namespace WebAppodev.Controllers
{
	public class HomeController : Controller
	{
		// 1. Declare the variables here so the whole class can see them
		private readonly ILogger<HomeController> _logger;
		private readonly ApplicationDbContext _context;

		// 2. Inject them in the constructor
		public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult Index()
		{
			// --- LINQ REQUIREMENT IMPLEMENTATION ---
			// Calculate "Top Trainers" stats for the homepage

			var topTrainers = _context.Appointments
				.Include(a => a.Trainer)           // Join with Trainers table
				.GroupBy(a => a.Trainer.FullName)  // Group by Trainer Name
				.Select(g => new
				{
					Name = g.Key,
					Count = g.Count()
				})
				.OrderByDescending(x => x.Count)   // Sort by most popular
				.Take(3)                           // Get top 3
				.ToList();

			// Pass data to View
			ViewBag.TopTrainers = topTrainers;

			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}