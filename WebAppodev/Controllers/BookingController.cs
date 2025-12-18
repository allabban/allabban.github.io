using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppodev.Data;
using WebAppodev.Models;

namespace WebAppodev.Controllers
{
	[Authorize] // Only logged-in members can book
	public class BookingController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;

		public BookingController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		// GET: Show the booking form
		public IActionResult Create()
		{
			// Send lists to the View for Dropdowns
			ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName");
			ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name");
			return View();
		}

		// POST: Process the booking
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Date,TrainerId,ServiceId")] Appointment appointment)
		{
			// --- FIX START: Remove validation for fields we handle automatically ---
			ModelState.Remove("MemberId");
			ModelState.Remove("Member");
			ModelState.Remove("Trainer");
			ModelState.Remove("Service");
			// --- FIX END ---

			// 1. Set the Member ID automatically (Logged in user)
			var user = await _userManager.GetUserAsync(User);
			appointment.MemberId = user.Id;
			appointment.Status = "Pending";

			// 2. Check for Conflicts
			bool isBusy = _context.Appointments.Any(a =>
				a.TrainerId == appointment.TrainerId &&
				a.Date == appointment.Date);

			if (isBusy)
			{
				ModelState.AddModelError("", "This trainer is already booked at that time.");
			}

			if (ModelState.IsValid)
			{
				_context.Add(appointment);
				await _context.SaveChangesAsync();
				return RedirectToAction("Index", "Home");
			}

			// If we got here, something failed. Reload the lists so the form isn't empty.
			ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName", appointment.TrainerId);
			ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId); // Ensure "Name" matches your Service model property
			return View(appointment);
		}
	}
}