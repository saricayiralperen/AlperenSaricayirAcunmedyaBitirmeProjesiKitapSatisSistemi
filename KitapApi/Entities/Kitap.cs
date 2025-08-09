using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitapApi.Entities
{
    public class Kitap
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Kitap adı zorunludur.")]
        [StringLength(200, ErrorMessage = "Kitap adı en fazla 200 karakter olabilir.")]
        public string Ad { get; set; } = null!;
        
        [Required(ErrorMessage = "Yazar adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Yazar adı en fazla 100 karakter olabilir.")]
        public string Yazar { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 999999.99, ErrorMessage = "Fiyat 0.01 ile 999999.99 arasında olmalıdır.")]
        public decimal Fiyat { get; set; }
        
        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
        public string? Aciklama { get; set; }
        
        [StringLength(500, ErrorMessage = "Resim URL'si en fazla 500 karakter olabilir.")]
        public string? ResimUrl { get; set; }

        public int KategoriId { get; set; }
        public Kategori? Kategori { get; set; } // <<<<<< ÖNEMLİ DEĞİŞİKLİK BURADA!
    }
}
////using System.ComponentModel.DataAnnotations;
////using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace KitapApi.Entities
//{
//    public class Kitap
//    {
//        [Key]
//        public int Id { get; set; }
//        public string Ad { get; set; } = null!;
//        public string Yazar { get; set; } = null!;

//        [Column(TypeName = "decimal(18,2)")]
//        public decimal Fiyat { get; set; }
//        public string? Aciklama { get; set; }
//        public string? ResimUrl { get; set; }

//        public int KategoriId { get; set; }
//        public Kategori Kategori { get; set; } = null!;
//    }
//}
////public class Kitap
////{
////    [Key]
////    public int Id { get; set; }
////    public string Ad { get; set; } = null!;
////    public string Yazar { get; set; } = null!;

////    [Column(TypeName = "decimal(18,2)")]
////    public decimal Fiyat { get; set; }
////    public string? Aciklama { get; set; }
////    public string? ResimUrl { get; set; }

////    // Foreign Key
////    public int KategoriId { get; set; }
////    // Navigation Property
////    public Kategori Kategori { get; set; } = null!;
////}
//////public class Kitap
//////{
//////    [Key]
//////    public int Id { get; set; }
//////    public string Ad { get; set; }
//////    public string Yazar { get; set; }

//////    [Column(TypeName = "decimal(18,2)")]
//////    public decimal Fiyat { get; set; }
//////    public string? Aciklama { get; set; }
//////    public string? ResimUrl { get; set; }

//////    // Foreign Key
//////    public int KategoriId { get; set; }
//////    // Navigation Property
//////    public Kategori Kategori { get; set; }
////} /* using System.ComponentModel.DataAnnotations;
////using System.ComponentModel.DataAnnotations.Schema;

////public class Kitap
////{
////    [Key]
////    public int Id { get; set; }
////    public string Ad { get; set; } = null!; // Düzeltme
////    public string Yazar { get; set; } = null!; // Düzeltme

////    [Column(TypeName = "decimal(18,2)")]
////    public decimal Fiyat { get; set; }

////    public string? Aciklama { get; set; } // Bu zaten nullable, dokunmaya gerek yok
////    public string? ResimUrl { get; set; } // Bu zaten nullable, dokunmaya gerek yok

////    public int KategoriId { get; set; }
////    public Kategori Kategori { get; set; } = null!; // Düzeltme
////}*/