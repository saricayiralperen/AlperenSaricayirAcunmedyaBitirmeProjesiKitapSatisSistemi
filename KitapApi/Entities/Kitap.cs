using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitapApi.Entities
{
    public class Kitap
    {
        [Key]
        public int Id { get; set; }
        public string Ad { get; set; } = null!;
        public string Yazar { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Fiyat { get; set; }
        public string? Aciklama { get; set; }
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