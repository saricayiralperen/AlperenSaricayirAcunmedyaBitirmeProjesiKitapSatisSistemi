//using Microsoft.EntityFrameworkCore;
//using KitapApi.Entities; // Bu satır Entity'leri bulması için önemli
using Microsoft.EntityFrameworkCore;
using KitapApi.Entities;

// Namespace'in doğru olduğundan emin oluyoruz: KitapApi.Data
namespace KitapApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Kitap> Kitaplar { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Favori> Favoriler { get; set; }
        public DbSet<Siparis> Siparisler { get; set; }
        public DbSet<SiparisDetay> SiparisDetaylari { get; set; }
    }
}
//namespace KitapApi.Data
//{
//    public class ApplicationDbContext : DbContext
//    {
//        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
//        {
//        }

//        public DbSet<Kategori> Kategoriler { get; set; }
//        public DbSet<Kitap> Kitaplar { get; set; }
//        public DbSet<Kullanici> Kullanicilar { get; set; }
//        public DbSet<Favori> Favoriler { get; set; }
//        public DbSet<Siparis> Siparisler { get; set; }
//        public DbSet<SiparisDetay> SiparisDetaylari { get; set; }
//    }
//}
////// Dosyanın en üstünde bu namespace olmalı!
////namespace KitapApi.Data;

////using Microsoft.EntityFrameworkCore;

////public class ApplicationDbContext : DbContext
////{
////    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
////    {
////    }

////    public DbSet<Kategori> Kategoriler { get; set; }
////    public DbSet<Kitap> Kitaplar { get; set; }
////    public DbSet<Kullanici> Kullanicilar { get; set; }
////    public DbSet<Favori> Favoriler { get; set; }
////    public DbSet<Siparis> Siparisler { get; set; }
////    public DbSet<SiparisDetay> SiparisDetaylari { get; set; }
////}
/////*public class ApplicationDbContext : DbContext
////{
////    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
////    {
////    }

////    public DbSet<Kategori> Kategoriler { get; set; }
////    public DbSet<Kitap> Kitaplar { get; set; }
////    public DbSet<Kullanici> Kullanicilar { get; set; }
////    public DbSet<Favori> Favoriler { get; set; }
////    public DbSet<Siparis> Siparisler { get; set; }
////    public DbSet<SiparisDetay> SiparisDetaylari { get; set; }
////}*/