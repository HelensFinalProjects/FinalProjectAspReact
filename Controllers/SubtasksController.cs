using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalAspReact.Data;
using FinalAspReact.Models;
using Microsoft.Build.Framework;

namespace FinalAspReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubtasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SubtasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Subtasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subtask>>> GetSubtasks()
        {
          if (_context.Subtasks == null)
          {
              return NotFound();
          }
            return await _context.Subtasks
                .Include(x => x.MyTask)
                .ToListAsync();
        }

        // GET: api/Subtasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subtask>> GetSubtask([FromRoute] int id)
        {
          if (_context.Subtasks == null)
          {
              return NotFound();
          }
            var subtask = await _context.Subtasks.FindAsync(id);

            if (subtask == null)
            {
                return NotFound();
            }

            return subtask;
        }

        // PUT: api/Subtasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubtask([FromRoute] int id, [FromForm] Subtask subtask)
        {
            if (id != subtask.Id)
            {
                return BadRequest();
            }

            _context.Entry(subtask).State = EntityState.Modified;


                try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubtaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Subtasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Subtask>> PostSubtask([FromForm] Subtask subtask)
        {
          if (_context.Subtasks == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Subtasks'  is null.");
          }
            _context.Subtasks.Add(subtask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubtask", new { id = subtask.Id }, subtask);
        }

        // DELETE: api/Subtasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubtask([FromRoute] int id)
        {
            if (_context.Subtasks == null)
            {
                return NotFound();
            }
            var subtask = await _context.Subtasks.FindAsync(id);
            if (subtask == null)
            {
                return NotFound();
            }

            _context.Subtasks.Remove(subtask);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubtaskExists(int id)
        {
            return (_context.Subtasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
