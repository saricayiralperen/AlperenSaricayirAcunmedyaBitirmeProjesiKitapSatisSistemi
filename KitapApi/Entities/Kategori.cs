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
        public string Ad { get; set; } = null!;
        public string? Aciklama { get; set; } // Bu satırı ekle!
    }
}