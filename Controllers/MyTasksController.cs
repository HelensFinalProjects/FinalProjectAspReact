using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalAspReact.Data;
using FinalAspReact.Models;

namespace FinalAspReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyTasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MyTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/MyTasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MyTask>>> GetMyTasks()
        {
          if (_context.MyTasks == null)
          {
              return NotFound();
          }
            return await _context.MyTasks
                .Include(x=>x.Category)
                .Include(x => x.Status)
                .ToListAsync();
        }

        // GET: api/MyTasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MyTask>> GetMyTask([FromRoute] int id)
        {
          if (_context.MyTasks == null)
          {
              return NotFound();

          }
            var subtasks = await (from master in _context.MyTasks
                                       join subtask in _context.Subtasks
                                       on master.Id equals subtask.Id
                                       where master.Id == id

                                       select new
                                       {
                                           master.Id,
                                           subtask.MyTaskId,
                                           subtask.Description,
                                           subtask.DeadLine,
                                           subtask.Done,
                                           subtask.Name
                                       }).ToListAsync();
            var myTask = await (from a in _context.MyTasks
                                where a.Id == id

                                select new
                                {
                                    a.Id,
                                    a.Name,
                                    a.DeadLine,
                                    a.Done,
                                    a.File,
                                    a.Hashtag,
                                    a.CategoryId,
                                    a.StatusId,
                                    DeleteMyTask="",
                                    subtasks= subtasks
                                }).FirstOrDefaultAsync();

            if (myTask == null)
            {
                return NotFound();
            }

            return Ok(myTask);
        }

        // PUT: api/MyTasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMyTask([FromRoute] int id, [FromForm] MyTask myTask)
        {
            if (id != myTask.Id)
            {
                return BadRequest();
            }

            _context.Entry(myTask).State = EntityState.Modified;

            //
            foreach (Subtask item in myTask.MyTasks)
            {
                /*if (item.Id == 0)
                    _context.MyTasks.Add(item);
                else*/
                    _context.Entry(item).State = EntityState.Modified;
            }

            /*foreach(var i in myTask.DeleteMyTask.Split(',').Where(x => x != ""))
            {
                Subtask y=_context.MyTasks.Find(Convert.ToInt32(i));
                _context._MyTasks.Remove(y);
            }*/
                
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MyTaskExists(id))
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

        // POST: api/MyTasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MyTask>> PostMyTask([FromForm] MyTask myTask)
        {
          if (_context.MyTasks == null)
          {
              return Problem("Entity set 'ApplicationDbContext.MyTasks'  is null.");
          }
            _context.MyTasks.Add(myTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMyTask", new { id = myTask.Id }, myTask);
        }

        // DELETE: api/MyTasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMyTask([FromRoute] int id)
        {
            if (_context.MyTasks == null)
            {
                return NotFound();
            }
            var myTask = await _context.MyTasks.FindAsync(id);
            if (myTask == null)
            {
                return NotFound();
            }

            _context.MyTasks.Remove(myTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MyTaskExists(int id)
        {
            return (_context.MyTasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
