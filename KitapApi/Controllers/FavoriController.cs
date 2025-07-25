using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KitapApi.Data;
using KitapApi.Entities;

namespace KitapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FavoriController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Favori
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Favori>>> GetFavoriler()
        {
            return await _context.Favoriler
                .Include(f => f.Kullanici)
                .Include(f => f.Kitap)
                .ToListAsync();
        }

        // GET: api/Favori/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Favori>> GetFavori(int id)
        {
            var favori = await _context.Favoriler
                .Include(f => f.Kullanici)
                .Include(f => f.Kitap)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (favori == null)
            {
                return NotFound();
            }

            return favori;
        }

        // POST: api/Favori
        [HttpPost]
        public async Task<ActionResult<Favori>> CreateFavori(Favori favori)
        {
            _context.Favoriler.Add(favori);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFavori), new { id = favori.Id }, favori);
        }

        // PUT: api/Favori/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFavori(int id, Favori favori)
        {
            if (id != favori.Id)
            {
                return BadRequest();
            }

            _context.Entry(favori).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavoriExists(id))
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

        // DELETE: api/Favori/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFavori(int id)
        {
            var favori = await _context.Favoriler.FindAsync(id);
            if (favori == null)
            {
                return NotFound();
            }

            _context.Favoriler.Remove(favori);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FavoriExists(int id)
        {
            return _context.Favoriler.Any(e => e.Id == id);
        }
    }
} 