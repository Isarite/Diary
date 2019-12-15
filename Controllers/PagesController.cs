using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiaryApp.Models;
using DiaryApp.Resources;
using Microsoft.AspNetCore.Authorization;

namespace DiaryApp.Controllers
{
    [Authorize(Roles = "Administrator,User")]
    [Route("api/[controller]")]
    [ApiController]
    public class PagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/DiaryPages
        [Route("[action]/{diaryId}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiaryPageResource>>> GetPages(string diaryId)
        {
            var diary = await _context.Diaries.FindAsync(diaryId);
            var pages = from m in _context.Pages select m;
            var result = await pages.Where(d => d.diary.Equals(diary)).ToListAsync();
            var transformed = new List<DiaryPageResource>();
            foreach (var page in result)
            {
                transformed.Add(new DiaryPageResource() {number = page.number,text = page.text, id = page.id});
            }

            return Ok(transformed);
        }

        // GET: api/DiaryPages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DiaryPage>> GetDiaryPage(string id)
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
        public async Task<IActionResult> PutDiaryPage(string id, DiaryPageResource diaryPage)
        {
            var page = _context.Pages.Find(diaryPage.id);
            page.text = diaryPage.text;
            _context.Entry(page).State = EntityState.Modified;

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
        [HttpPost("{id}")]
        public async Task<ActionResult<DiaryPage>> PostDiaryPage([FromBody]DiaryPage diaryPage, string id)
        {
            var diary = await _context.Diaries.FindAsync(id);
            if (diary == null)
                return NotFound();
            diaryPage.diary = await _context.Diaries.FindAsync(id);
            _context.Pages.Add(diaryPage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDiaryPage", 
                new DiaryPageResource{ id = diaryPage.id, number = diaryPage.number, text = diaryPage.text});
        }

        // DELETE: api/DiaryPages/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DiaryPage>> DeleteDiaryPage(string id)
        {
            var diaryPage = await _context.Pages.FindAsync(id);
            if (diaryPage == null)
            {
                return NotFound();
            }

            _context.Pages.Remove(diaryPage);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool DiaryPageExists(string id)
        {
            return _context.Pages.Any(e => e.id == id);
        }
    }
}
