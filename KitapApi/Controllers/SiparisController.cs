using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KitapApi.Data;
using KitapApi.Entities;

namespace KitapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiparisController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SiparisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Siparis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Siparis>>> GetSiparisler()
        {
            return await _context.Siparisler
                .Include(s => s.Kullanici)
                .Include(s => s.SiparisDetaylari)
                    .ThenInclude(sd => sd.Kitap)
                .ToListAsync();
        }

        // GET: api/Siparis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Siparis>> GetSiparis(int id)
        {
            var siparis = await _context.Siparisler
                .Include(s => s.Kullanici)
                .Include(s => s.SiparisDetaylari)
                    .ThenInclude(sd => sd.Kitap)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (siparis == null)
            {
                return NotFound();
            }

            return siparis;
        }

        // POST: api/Siparis
        [HttpPost]
        public async Task<ActionResult<Siparis>> CreateSiparis(Siparis siparis)
        {
            _context.Siparisler.Add(siparis);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSiparis), new { id = siparis.Id }, siparis);
        }

        // PUT: api/Siparis/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSiparis(int id, Siparis siparis)
        {
            if (id != siparis.Id)
            {
                return BadRequest();
            }

            _context.Entry(siparis).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiparisExists(id))
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

        // DELETE: api/Siparis/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSiparis(int id)
        {
            var siparis = await _context.Siparisler.FindAsync(id);
            if (siparis == null)
            {
                return NotFound();
            }

            _context.Siparisler.Remove(siparis);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SiparisExists(int id)
        {
            return _context.Siparisler.Any(e => e.Id == id);
        }
    }
} 