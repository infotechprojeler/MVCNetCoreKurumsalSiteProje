using Data;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ContactsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/<ContactsController>
        [HttpGet]
        public IEnumerable<Contact> Get()
        {
            return _context.Contacts.ToList();
        }

        // GET api/<ContactsController>/5
        [HttpGet("{id}")]
        public async Task<Contact> GetAsync(int id)
        {
            return await _context.Contacts.FindAsync(id);
        }

        // POST api/<ContactsController>
        [HttpPost]
        public async Task PostAsync([FromBody] Contact value)
        {
            await _context.Contacts.AddAsync(value);
            await _context.SaveChangesAsync();
        }

        // PUT api/<ContactsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Contact value)
        {
            _context.Contacts.Update(value);
            _context.SaveChanges();
        }

        // DELETE api/<ContactsController>/5
        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id) // metot async olunca void değil task olmalı!
        {
            var kayit = await _context.Contacts.FindAsync(id);
            _context.Contacts.Remove(kayit);
            await _context.SaveChangesAsync();
        }
    }
}
