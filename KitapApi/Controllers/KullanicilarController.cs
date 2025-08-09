using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KitapApi.Data;
using KitapApi.Entities;
using KitapApi.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel.DataAnnotations; // LoginModel için
using KitapApi.Attributes; // ApiAuthorize için
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace KitapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KullanicilarController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KullanicilarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Yardımcı Metot: Şifre Hash'leme (BCrypt kullanarak)
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        
        // Yardımcı Metot: Şifre Doğrulama (BCrypt kullanarak)
        private bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        // POST: api/Kullanicilar/Register
        // Yeni bir kullanıcı kaydı yapar
        [HttpPost("Register")]
        public async Task<ActionResult<Kullanici>> Register(Kullanici kullanici)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (await _context.Kullanicilar.AnyAsync(u => u.Email == kullanici.Email))
            {
                return BadRequest("Bu email adresi zaten kullanılıyor.");
            }

            kullanici.SifreHash = HashPassword(kullanici.SifreHash); // Gelen şifreyi hash'le
            if (string.IsNullOrEmpty(kullanici.Rol)) // Eğer rol belirtilmediyse varsayılan "User" olsun
            {
                kullanici.Rol = "User";
            }
            kullanici.KayitTarihi = DateTime.Now;

            _context.Kullanicilar.Add(kullanici);
            await _context.SaveChangesAsync();

            // Şifre hash'ini güvenlik nedeniyle döndürme
            kullanici.SifreHash = "";
            return CreatedAtAction(nameof(GetKullanici), new { id = kullanici.Id }, kullanici);
        }

        // POST: api/Kullanicilar/Login
        // Kullanıcı girişi yapar ve geçerli ise kullanıcı nesnesini döner
        [HttpPost("Login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                var kullanici = await _context.Kullanicilar.FirstOrDefaultAsync(u => u.Email == loginModel.Email);

                if (kullanici == null || !VerifyPassword(loginModel.Sifre, kullanici.SifreHash))
                {
                    return Unauthorized("Geçersiz e-posta veya şifre.");
                }

                // JWT Token üret
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = System.Text.Encoding.UTF8.GetBytes(StartupStatic.JwtKey); // Statik key
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, kullanici.Id.ToString()),
                    new Claim(ClaimTypes.Email, kullanici.Email),
                    new Claim(ClaimTypes.Role, kullanici.Rol)
                };
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(2),
                    Issuer = StartupStatic.JwtIssuer,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwt = tokenHandler.WriteToken(token);

                // Şifre hash'ini güvenlik nedeniyle döndürme
                kullanici.SifreHash = "";
                return Ok(new { token = jwt, kullanici });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Login hatası: {ex.Message}");
            }
        }

        // GET: api/Kullanicilar
        // Tüm kullanıcıları listeler (Admin için)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Kullanici>>> GetKullanicilar()
        {
            var kullanicilar = await _context.Kullanicilar.ToListAsync();
            
            // Şifre hash'lerini güvenlik nedeniyle temizle
            foreach (var kullanici in kullanicilar)
            {
                kullanici.SifreHash = "";
            }
            
            return kullanicilar;
        }

        // GET: api/Kullanicilar/5
        // Belirli bir ID'ye sahip kullanıcıyı getirir
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Kullanici>> GetKullanici(int id)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            // Kullanıcı sadece kendi bilgilerini görebilir veya admin tüm kullanıcıları görebilir
            if (currentUserRole != "Admin" && currentUserId != id.ToString())
            {
                return Forbid("Bu kullanıcının bilgilerine erişim yetkiniz yok.");
            }
            
            var kullanici = await _context.Kullanicilar.FindAsync(id);

            if (kullanici == null)
            {
                return NotFound();
            }

            // Şifre hash'ini güvenlik nedeniyle döndürme
            kullanici.SifreHash = "";
            return kullanici;
        }

        // PUT: api/Kullanicilar/5
        // Belirli bir ID'ye sahip kullanıcıyı günceller
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateKullanici(int id, UpdateKullaniciDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (id != updateDto.Id)
            {
                return BadRequest("Güncellenmek istenen kullanıcının ID'si ile URL'deki ID eşleşmiyor.");
            }

            // Mevcut kullanıcıyı veritabanından çek
            var mevcutKullanici = await _context.Kullanicilar.FindAsync(id);
            if (mevcutKullanici == null)
            {
                return NotFound();
            }

            // Sadece gönderilen alanları güncelle
            mevcutKullanici.AdSoyad = updateDto.AdSoyad;
            mevcutKullanici.Email = updateDto.Email;
            mevcutKullanici.Rol = updateDto.Rol;

            // Eğer şifre gönderilmişse, yeni şifreyi hash'le
            if (!string.IsNullOrEmpty(updateDto.SifreHash))
            {
                mevcutKullanici.SifreHash = HashPassword(updateDto.SifreHash);
            }
            // Şifre gönderilmemişse mevcut şifre korunur

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KullaniciExists(id))
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

        // DELETE: api/Kullanicilar/5
        // Belirli bir ID'ye sahip kullanıcıyı siler
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteKullanici(int id)
        {
            var kullanici = await _context.Kullanicilar.FindAsync(id);
            if (kullanici == null)
            {
                return NotFound();
            }

            _context.Kullanicilar.Remove(kullanici);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Kullanicilar/5/change-password
        // Kullanıcının şifresini değiştirir (Sadece Admin)
        [HttpPut("{id}/change-password")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var kullanici = await _context.Kullanicilar.FindAsync(id);
            if (kullanici == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            // Yeni şifreyi hash'le ve güncelle
            kullanici.SifreHash = HashPassword(request.YeniSifre);
            
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Şifre başarıyla değiştirildi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Şifre değiştirme işlemi sırasında bir hata oluştu.", error = ex.Message });
            }
        }

        private bool KullaniciExists(int id)
        {
            return _context.Kullanicilar.Any(e => e.Id == id);
        }
    }

    // Login işlemi için basit bir model (Bu sınıf genellikle aynı dosya içinde tanımlanır)
    public class LoginModel
    {
        [Required(ErrorMessage = "Email adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Şifre zorunludur.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string Sifre { get; set; } = string.Empty;
    }

    // Şifre değiştirme işlemi için model
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "Yeni şifre zorunludur.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string YeniSifre { get; set; } = string.Empty;
    }
}