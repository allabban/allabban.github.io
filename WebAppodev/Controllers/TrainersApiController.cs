using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppodev.Data;
using WebAppodev.Models;

namespace WebAppodev.Controllers
{
	// [Route("api/[controller]")] marks this as a REST API endpoint
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
		// This returns a JSON list of all trainers
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Trainer>>> GetTrainers()
		{
			return await _context.Trainers.ToListAsync();
		}

		// GET: api/TrainersApi/5
		// This returns a single trainer by ID in JSON format
		[HttpGet("{id}")]
		public async Task<ActionResult<Trainer>> GetTrainer(int id)
		{
			var trainer = await _context.Trainers.FindAsync(id);

			if (trainer == null)
			{
				return NotFound();
			}

			return trainer;
		}
	}
}