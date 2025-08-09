//using System.ComponentModel.DataAnnotations;

//public class Kategori
//{
//    [Key]
//    public int Id { get; set; }
//    public string Ad { get; set; } = null!;
//}
using System.ComponentModel.DataAnnotations;

namespace KitapApi.Entities
{
    public class Kategori
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olabilir.")]
        public string Ad { get; set; } = null!;
        
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string? Aciklama { get; set; } // Bu satırı ekle!
    }
}