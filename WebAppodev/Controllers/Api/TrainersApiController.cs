using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppodev.Data;
using WebAppodev.Models;

namespace FitnessCenterApp.Controllers.Api
{
	[Route("api/[controller]")]
	[ApiController]
	public class TrainersApiController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public TrainersApiController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/TrainersApi
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Trainer>>> GetTrainers()
		{
			return await _context.Trainers.ToListAsync();
		}

		// POST: api/TrainersApi (To add data via Postman)
		[HttpPost]
		public async Task<ActionResult<Trainer>> PostTrainer(Trainer trainer)
		{
			_context.Trainers.Add(trainer);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetTrainers), new { id = trainer.Id }, trainer);
		}
	}
}