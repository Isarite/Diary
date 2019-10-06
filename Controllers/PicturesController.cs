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
    public class PicturesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PicturesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Pictures
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Picture>>> GetPictures()
        {
            return await _context.Pictures.ToListAsync();
        }

        // GET: api/Pictures/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Picture>> GetPicture(string id)
        {
            var Picture = await _context.Pictures.FindAsync(id);

            if (Picture == null)
            {
                return NotFound();
            }

            return Picture;
        }

        // PUT: api/Pictures/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPicture(string id, Picture Picture)
        {
            if (id != Picture.fileName)
            {
                return BadRequest();
            }

            _context.Entry(Picture).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PictureExists(id))
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

        // POST: api/Pictures
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Picture>> PostPicture(Picture Picture)
        {
            _context.Pictures.Add(Picture);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPicture", new { fileName = Picture.fileName }, Picture);
        }

        // DELETE: api/Pictures/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Picture>> DeletePicture(string id)
        {
            var Picture = await _context.Pictures.FindAsync(id);
            if (Picture == null)
            {
                return NotFound();
            }

            _context.Pictures.Remove(Picture);
            await _context.SaveChangesAsync();

            return Picture;
        }

        private bool PictureExists(string id)
        {
            return _context.Pictures.Any(e => e.fileName == id);
        }
    }
}
