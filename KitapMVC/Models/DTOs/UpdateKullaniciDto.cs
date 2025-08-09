using System.ComponentModel.DataAnnotations;

namespace KitapMVC.Models.DTOs
{
    public class UpdateKullaniciDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad alanı zorunludur.")]
        [StringLength(100)]
        public string AdSoyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rol alanı zorunludur.")]
        [StringLength(50)]
        public string Rol { get; set; } = string.Empty;

        // Şifre opsiyonel - sadece değiştirilmek istendiğinde gönderilir
        [StringLength(255)]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string? SifreHash { get; set; }

        public DateTime KayitTarihi { get; set; }
    }
}