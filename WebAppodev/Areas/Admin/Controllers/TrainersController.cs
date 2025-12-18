using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Needed for [Authorize]
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppodev.Data;
using WebAppodev.Models;

namespace WebAppodev.Areas.Admin.Controllers
{
	// --- THIS IS THE CRITICAL FIX ---
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	// --------------------------------
	public class TrainersController : Controller
	{
		private readonly ApplicationDbContext _context;

		public TrainersController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Admin/Trainers
		public async Task<IActionResult> Index()
		{
			return View(await _context.Trainers.ToListAsync());
		}

		// GET: Admin/Trainers/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var trainer = await _context.Trainers
				.FirstOrDefaultAsync(m => m.Id == id);
			if (trainer == null)
			{
				return NotFound();
			}

			return View(trainer);
		}

		// GET: Admin/Trainers/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Admin/Trainers/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,FullName,Specialty,ExperienceYears")] Trainer trainer)
		{
			if (ModelState.IsValid)
			{
				_context.Add(trainer);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(trainer);
		}

		// GET: Admin/Trainers/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var trainer = await _context.Trainers.FindAsync(id);
			if (trainer == null)
			{
				return NotFound();
			}
			return View(trainer);
		}

		// POST: Admin/Trainers/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Specialty,ExperienceYears")] Trainer trainer)
		{
			if (id != trainer.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(trainer);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!TrainerExists(trainer.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(trainer);
		}

		// GET: Admin/Trainers/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var trainer = await _context.Trainers
				.FirstOrDefaultAsync(m => m.Id == id);
			if (trainer == null)
			{
				return NotFound();
			}

			return View(trainer);
		}

		// POST: Admin/Trainers/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var trainer = await _context.Trainers.FindAsync(id);
			if (trainer != null)
			{
				_context.Trainers.Remove(trainer);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool TrainerExists(int id)
		{
			return _context.Trainers.Any(e => e.Id == id);
		}
	}
}