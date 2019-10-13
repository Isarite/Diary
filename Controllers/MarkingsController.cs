using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiaryApp.Models;
using DiaryApp.Resources;

namespace DiaryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarkingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MarkingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Markings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Marking>>> GetMarkings(string DiaryId)
        {
            return await _context.Markings.ToListAsync();
        }

        // GET: api/Markings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Marking>> GetMarking(string id)
        {
            var marking = await _context.Markings.FindAsync(id);

            if (marking == null)
            {
                return NotFound();
            }

            return marking;
        }

        // PUT: api/Markings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMarking(string id, Marking marking)
        {
            if (id != marking.id)
            {
                return BadRequest();
            }

            _context.Entry(marking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MarkingExists(id))
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

        // POST: api/Markings
        [HttpPost]
        public async Task<ActionResult<Marking>> PostMarking(MarkingResource marking)
        {
            var result = new Marking();
            result.start = marking.Start;
            result.end = marking.End;
            await _context.Markings.AddAsync(result);
            var diaryPage = await _context.Pages.Where(w => w.id.Equals(marking.PageId)).FirstOrDefaultAsync();
            //diaryPage.markings.Add(result);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Markings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Marking>> DeleteMarking(string id)
        {
            var marking = await _context.Markings.FindAsync(id);
            if (marking == null)
            {
                return NotFound();
            }

            _context.Markings.Remove(marking);
            await _context.SaveChangesAsync();

            return marking;
        }

        private bool MarkingExists(string id)
        {
            return _context.Markings.Any(e => e.id == id);
        }
    }
}
