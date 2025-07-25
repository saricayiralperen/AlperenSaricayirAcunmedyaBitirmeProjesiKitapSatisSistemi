using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KitapApi.Data;
using KitapApi.Entities;
using Microsoft.AspNetCore.Authorization;

namespace KitapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KategorilerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KategorilerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Kategoriler
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kategori>>> GetKategoriler()
        {
            return await _context.Kategoriler.ToListAsync();
        }

        // GET: api/Kategoriler/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Kategori>> GetKategori(int id)
        {
            var kategori = await _context.Kategoriler.FindAsync(id);

            if (kategori == null)
            {
                return NotFound();
            }

            return kategori;
        }

        // POST: api/Kategoriler
        // Yeni bir kategori ekler
        [HttpPost]
        public async Task<ActionResult<Kategori>> CreateKategori(Kategori kategori)
        {
            _context.Kategoriler.Add(kategori);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetKategori), new { id = kategori.Id }, kategori);
        }

        // PUT: api/Kategoriler/5
        // Mevcut bir kategoriyi günceller
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKategori(int id, Kategori kategori)
        {
            if (id != kategori.Id)
            {
                return BadRequest("Güncellenmek istenen kategorinin ID'si ile URL'deki ID eşleşmiyor.");
            }

            _context.Entry(kategori).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KategoriExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Başarılı güncelleme için içerik yok
        }

        // DELETE: api/Kategoriler/5
        // Belirli bir ID'ye sahip kategoriyi siler
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteKategori(int id)
        {
            var kategori = await _context.Kategoriler.FindAsync(id);
            if (kategori == null)
            {
                return NotFound();
            }

            _context.Kategoriler.Remove(kategori);
            await _context.SaveChangesAsync();

            return NoContent(); // Başarılı silme için içerik yok
        }

        private bool KategoriExists(int id)
        {
            return _context.Kategoriler.Any(e => e.Id == id);
        }
    }
}