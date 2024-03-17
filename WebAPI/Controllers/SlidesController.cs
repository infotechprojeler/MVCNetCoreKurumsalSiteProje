using Data;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SlidesController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public SlidesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/<SlidesController>
        [HttpGet]
        public async Task<IEnumerable<Slide>> GetAsync()
        {
            return await _context.Slides.ToListAsync();
        }

        // GET api/<SlidesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Slide>> GetAsync(int id)
        {
            var kayit = await _context.Slides.FindAsync(id);
            if (kayit == null)
            {
                return NotFound();
            }
            return kayit;
        }

        // POST api/<SlidesController>
        [HttpPost]
        public async Task<ActionResult<Slide>> Post([FromBody] Slide value)
        {
            await _context.Slides.AddAsync(value);
            await _context.SaveChangesAsync();
            return Ok(value);
        }

        // PUT api/<SlidesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Slide>> Put(int id, [FromBody] Slide value)
        {
            _context.Slides.Update(value);
            await _context.SaveChangesAsync();
            return Ok(value);
        }

        // DELETE api/<SlidesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Slide>> Delete(int id)
        {
            var kayit = await _context.Slides.FindAsync(id);
            if (kayit == null)
            {
                return NotFound();
            }
            _context.Slides.Remove(kayit);
            await _context.SaveChangesAsync();
            return Ok(kayit);
        }
    }
}
