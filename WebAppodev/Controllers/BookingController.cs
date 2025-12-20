using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppodev.Data;
using WebAppodev.Models;

namespace WebAppodev.Controllers
{
	[Authorize]
	public class BookingController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;

		public BookingController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		// GET: Booking/MyAppointments
		// Shows the list of user's bookings with grayed out past items
		public async Task<IActionResult> MyAppointments()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null) return RedirectToAction("Login", "Account");

			var myAppointments = await _context.Appointments
				.Include(a => a.Trainer)
				.Include(a => a.Service)
				.Where(a => a.MemberId == user.Id)
				.OrderByDescending(a => a.Date)
				.ToListAsync();

			return View(myAppointments);
		}

		// GET: Booking/Create
		public IActionResult Create()
		{
			ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName");
			ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name");
			return View();
		}

		// POST: Booking/Create
		// Handles overlap logic using Duration
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Date,TrainerId,ServiceId")] Appointment appointment)
		{
			ModelState.Remove("MemberId");
			ModelState.Remove("Member");
			ModelState.Remove("Trainer");
			ModelState.Remove("Service");

			var user = await _userManager.GetUserAsync(User);
			if (user == null) return RedirectToAction("Login", "Account");

			appointment.MemberId = user.Id;
			appointment.Status = "Pending";

			// 1. GET DURATION
			// We need to know how long the service takes to calculate the End Time
			var selectedService = await _context.Services.FindAsync(appointment.ServiceId);

			// If service wasn't found (shouldn't happen), reload page
			if (selectedService == null)
			{
				ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName", appointment.TrainerId);
				ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);
				return View(appointment);
			}

			// Calculate New Appointment Times
			DateTime newStart = appointment.Date;
			DateTime newEnd = appointment.Date.AddMinutes(selectedService.Duration);

			// 2. CHECK FOR CONFLICTS
			// We pull existing appointments for this Trainer OR this User
			var conflicts = await _context.Appointments
				.Include(a => a.Service) // Need to include Service to know THEIR duration
				.Where(a =>
					(a.TrainerId == appointment.TrainerId || a.MemberId == user.Id)
					&& a.Status != "Cancelled"
					&& a.Status != "Rejected")
				.ToListAsync();

			bool isConflict = false;

			foreach (var existing in conflicts)
			{
				// Calculate Existing Appointment Times
				// If existing.Service is null (legacy data), assume 60 mins
				int existingDuration = existing.Service != null ? existing.Service.Duration : 60;

				DateTime existingStart = existing.Date;
				DateTime existingEnd = existing.Date.AddMinutes(existingDuration);

				// LOGIC: Overlap exists if (StartA < EndB) AND (EndA > StartB)
				if (existingStart < newEnd && existingEnd > newStart)
				{
					isConflict = true;
					break;
				}
			}

			if (isConflict)
			{
				ModelState.AddModelError("", $"Time Conflict! The trainer or you are busy during this time ({selectedService.Duration} mins).");
			}

			if (ModelState.IsValid)
			{
				_context.Add(appointment);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(MyAppointments));
			}

			// If we failed, reload the dropdowns
			ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName", appointment.TrainerId);
			ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);
			return View(appointment);
		}

		// POST: Booking/Delete/5
		// Allows user to delete Rejected or Cancelled appointments
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var appointment = await _context.Appointments.FindAsync(id);
			if (appointment != null)
			{
				// Security check: ensure the user owns this appointment
				var user = await _userManager.GetUserAsync(User);
				if (appointment.MemberId == user.Id)
				{
					// Logic check: only delete if it's "over"
					if (appointment.Status == "Rejected" || appointment.Status == "Cancelled")
					{
						_context.Appointments.Remove(appointment);
						await _context.SaveChangesAsync();
					}
				}
			}
			return RedirectToAction(nameof(MyAppointments));
		}
	}
}