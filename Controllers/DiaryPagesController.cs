using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiaryApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace DiaryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiaryPagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DiaryPagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/DiaryPages
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiaryPage>>> GetPages()
        {
            return await _context.Pages.ToListAsync();
        }

        // GET: api/DiaryPages/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<DiaryPage>> GetDiaryPage(int id)
        {
            var diaryPage = await _context.Pages.FindAsync(id);

            if (diaryPage == null)
            {
                return NotFound();
            }

            return diaryPage;
        }

        // PUT: api/DiaryPages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDiaryPage(int id, DiaryPage diaryPage)
        {
            if (id != diaryPage.id)
            {
                return BadRequest();
            }

            _context.Entry(diaryPage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiaryPageExists(id))
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

        // POST: api/DiaryPages
        [HttpPost]
        public async Task<ActionResult<DiaryPage>> PostDiaryPage(DiaryPage diaryPage)
        {
            _context.Pages.Add(diaryPage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDiaryPage", new { id = diaryPage.id }, diaryPage);
        }

        // DELETE: api/DiaryPages/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DiaryPage>> DeleteDiaryPage(int id)
        {
            var diaryPage = await _context.Pages.FindAsync(id);
            if (diaryPage == null)
            {
                return NotFound();
            }

            _context.Pages.Remove(diaryPage);
            await _context.SaveChangesAsync();

            return diaryPage;
        }

        private bool DiaryPageExists(int id)
        {
            return _context.Pages.Any(e => e.id == id);
        }
    }
}
