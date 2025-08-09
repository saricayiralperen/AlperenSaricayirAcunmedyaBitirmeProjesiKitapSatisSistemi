using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // ColumnType için, şuanlık olmasa da dursun

namespace KitapApi.Entities
{
    public class Kullanici
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string AdSoyad { get; set; } = string.Empty; // <-- Bu alanı Ad ve Soyad yerine kullanıyoruz

        [Required]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; } = string.Empty; // <-- Eposta yerine Email kullanıyoruz

        [Required]
        [StringLength(255)] // Şifre hash'i için daha uzun olabilir
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string SifreHash { get; set; } = string.Empty; // <-- Sifre yerine SifreHash kullanıyoruz

        [Required]
        [StringLength(50)]
        public string Rol { get; set; } = "User"; // Varsayılan rol "User"

        public DateTime KayitTarihi { get; set; } = DateTime.Now;
    }
}
////using System.ComponentModel.DataAnnotations;

//////using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations;

//namespace KitapApi.Entities
//{
//    public class Kullanici
//    {
//        [Key]
//        public int Id { get; set; }
//        public string Ad { get; set; } = null!;
//        public string Soyad { get; set; } = null!;
//        public string Eposta { get; set; } = null!;
//        public string Sifre { get; set; } = null!;
//        public string Rol { get; set; } = "User";
//    }
//}
////public class Kullanici
////{
////    [Key]
////    public int Id { get; set; }
////    public string Ad { get; set; } = null!;
////    public string Soyad { get; set; } = null!;
////    public string Eposta { get; set; } = null!;
////    public string Sifre { get; set; } = null!;
////    public string Rol { get; set; } = "User";
////}
//////public class Kullanici
//////{
//////    [Key]
//////    public int Id { get; set; }
//////    public string Ad { get; set; }
//////    public string Soyad { get; set; }
//////    public string Eposta { get; set; }
//////    public string Sifre { get; set; }
//////    public string Rol { get; set; } = "User"; // Varsayılan rol User olsun
//////}