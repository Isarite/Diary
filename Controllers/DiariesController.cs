using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiaryApp.Models;
using Microsoft.AspNetCore.Authorization;
using DiaryApp.Resources;
using Remotion.Linq.Clauses;

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
        [Authorize(Roles = "Administrator,User")]
        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiaryResource>>> GetDiaries()
        {
            //TODO: get only one user's diaries
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FindAsync(userId);
            var diaries =   _context.Diaries.Where(d => d.user.Id.Equals(userId)).ToList();
            var result = new List<DiaryResource>();
            foreach (Diary diary in diaries)
                //result.Add(new DiaryResource(diary.id, diary.name, diary.pages.Count(),diary.created,diary.edited));
                result.Add(new DiaryResource(diary.id, diary.name,diary.created, diary.edited));
            return Ok(result);
        }

        // GET: api/Diaries/5
        [Authorize(Roles = "Administrator,User")]
        [HttpGet("{id}")]
        public async Task<ActionResult<DiaryResource>> GetDiary(string id)
        {
            var diary = await _context.Diaries.FindAsync(id);

            if (diary == null)
            {
                return NotFound();
            }
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FindAsync(userId);
            if (!user.Equals(diary.user))
                return Unauthorized();
            //var result = new DiaryResource(diary.id, diary.name, diary.pages.Count(), diary.created, diary.edited);
            var result = new DiaryResource(diary.id, diary.name, diary.created, diary.edited);
            return Ok(result);
        }

        // PUT: api/Diaries/5
        [Authorize(Roles = "Administrator,User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDiary(string id, DiaryResource diary)
        {
            if (id != diary.Id || !DiaryExists(id))
            {
                return BadRequest();
            }
            var result = await _context.Diaries.FindAsync(id);
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FindAsync(userId);
            if (!user.Equals(result.user))
                return Unauthorized();
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
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // POST: api/Diaries
        [Authorize(Roles = "Administrator,User")]
        [HttpPost]
        public async Task<ActionResult<DiaryResource>> PostDiary([FromBody]string name)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FindAsync(userId);
            var diary = new Diary {name = name, created = DateTime.Now, user = user};
            diary.edited = diary.created;
            //diary.user = from token
            _context.Diaries.Add(diary);
            await _context.SaveChangesAsync();
            //var result = new DiaryResource(diary.id, diary.name, diary.pages.Count(), diary.created, diary.edited);

            var result = new DiaryResource(diary.id, diary.name, diary.created, diary.edited);
            return Ok(result);
            //return CreatedAtAction("GetDiary", new { Id = diary.id }, diary);
        }

        // DELETE: api/Diaries/5
        [Authorize(Roles = "Administrator,User")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Diary>> DeleteDiary(string id)
        {
            var diary = await _context.Diaries.FindAsync(id);
            if (diary == null)
            {
                return NotFound();
            }
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FindAsync(userId);
            if (!user.Equals(diary.user))
            {
                return Unauthorized();
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
