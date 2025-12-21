using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppodev.Data;
using WebAppodev.Models;

namespace WebAppodev.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class AppointmentsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public AppointmentsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Admin/Appointments
		public async Task<IActionResult> Index()
		{
			// Include Member, Trainer, and Service data to show names, not just IDs
			var appointments = await _context.Appointments
				.Include(a => a.Member)
				.Include(a => a.Trainer)
				.Include(a => a.Service)
				.OrderByDescending(a => a.Date) // Show newest first
				.ToListAsync();

			return View(appointments);
		}

		// POST: Approve Appointment
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Approve(int id)
		{
			var appointment = await _context.Appointments.FindAsync(id);
			if (appointment != null)
			{
				appointment.Status = "Confirmed"; // Change status
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Index));
		}
		// POST: Admin/Appointments/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			var appointment = await _context.Appointments.FindAsync(id);

			if (appointment != null)
			{
				if (appointment.Date < DateTime.Now)
				{
					_context.Appointments.Remove(appointment);
					await _context.SaveChangesAsync();
				}
				else
				{
					_context.Appointments.Remove(appointment);
					await _context.SaveChangesAsync();
				}
			}
			return RedirectToAction(nameof(Index));
		}

		// POST: Cancel Appointment
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Cancel(int id)
		{
			var appointment = await _context.Appointments.FindAsync(id);
			if (appointment != null)
			{
				appointment.Status = "Cancelled";
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Index));
		}
	}
}