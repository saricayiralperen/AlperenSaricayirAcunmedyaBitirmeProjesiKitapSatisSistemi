using KitapMVC.Models.Entities;

namespace KitapMVC.Models
{
    public class RaporViewModel
    {
        public int ToplamKitap { get; set; }
        public int ToplamKullanici { get; set; }
        public int ToplamKategori { get; set; }
        public int BuAySiparis { get; set; }
        public List<Siparis> SonSiparisler { get; set; } = new List<Siparis>();
        public List<Kitap> PopulerKitaplar { get; set; } = new List<Kitap>();
        public List<KitapSiparisIstatistik> KitapSiparisIstatistikleri { get; set; } = new List<KitapSiparisIstatistik>();
        public Dictionary<string, int> KategoriDagilimi { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> AylikSiparisler { get; set; } = new Dictionary<string, int>();
    }
}