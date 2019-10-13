using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiaryApp.Models;
using Microsoft.AspNetCore.Authorization;
using DiaryApp.Resources;

namespace DiaryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiariesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DiariesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Diaries
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiaryResource>>> GetDiaries()
        {
            //TODO: get only one user's diaries
            var diaries = await _context.Diaries.ToListAsync();
            var result = new List<DiaryResource>();
            foreach (Diary diary in diaries)
                //result.Add(new DiaryResource(diary.id, diary.name, diary.pages.Count(),diary.created,diary.edited));
                result.Add(new DiaryResource(diary.id, diary.name, 0, diary.created, diary.edited));
            return Ok(result);
        }

        // GET: api/Diaries/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<DiaryResource>> GetDiary(string id)
        {
            var diary = await _context.Diaries.FindAsync(id);

            if (diary == null)
            {
                return NotFound();
            }
            //var result = new DiaryResource(diary.id, diary.name, diary.pages.Count(), diary.created, diary.edited);
            var result = new DiaryResource(diary.id, diary.name, 0, diary.created, diary.edited);
            return Ok(result);
        }

        // PUT: api/Diaries/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDiary(string id, DiaryResource diary)
        {
            if (id != diary.Id)
            {
                return BadRequest();
            }
            if (!DiaryExists(id))
            {
                return BadRequest();
            }
            var result = await _context.Diaries.FindAsync(id);
            result.name = diary.Name;
            result.edited = DateTime.Now;
            _context.Entry(result).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiaryExists(id))
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

        // POST: api/Diaries
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<DiaryResource>> PostDiary([FromBody]string name)
        {
            var diary = new Diary();
            diary.name = name;
            diary.created = DateTime.Now;
            diary.edited = diary.created;
            //diary.user = from token
            _context.Diaries.Add(diary);
            await _context.SaveChangesAsync();
            //var result = new DiaryResource(diary.id, diary.name, diary.pages.Count(), diary.created, diary.edited);

            var result = new DiaryResource(diary.id, diary.name, 0, diary.created, diary.edited);
            return Ok(result);
            //return CreatedAtAction("GetDiary", new { Id = diary.id }, diary);
        }

        // DELETE: api/Diaries/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Diary>> DeleteDiary(string id)
        {
            var diary = await _context.Diaries.FindAsync(id);
            if (diary == null)
            {
                return NotFound();
            }

            _context.Diaries.Remove(diary);
            await _context.SaveChangesAsync();

            return diary;
        }

        private bool DiaryExists(string id)
        {
            return _context.Diaries.Any(e => e.id == id);
        }
    }
}
