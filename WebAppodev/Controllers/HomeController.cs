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
		public IActionResult Index(string sortOrder)
		{
			// 1. Base Query: Calculate counts for everyone
			var trainerStats = _context.Appointments
				.Include(a => a.Trainer)
				.GroupBy(a => a.Trainer.FullName)
				.Select(g => new
				{
					Name = g.Key,
					Count = g.Count()
				});

			// 2. Apply Sorting Logic based on the button clicked
			if (sortOrder == "Name")
			{
				// Sort A-Z (Show all, or you can add .Take(5) if the list is too long)
				ViewBag.TopTrainers = trainerStats.OrderBy(x => x.Name).ToList();
				ViewBag.HeaderTitle = "Trainers (A-Z)";
			}
			else
			{
				// Default: Top 3 Popular
				ViewBag.TopTrainers = trainerStats.OrderByDescending(x => x.Count).Take(3).ToList();
				ViewBag.HeaderTitle = "Most Popular Trainers";
			}

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