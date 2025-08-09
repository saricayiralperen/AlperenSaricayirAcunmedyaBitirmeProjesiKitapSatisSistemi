namespace KitapMVC.Models
{
    public class KitapSiparisIstatistik
    {
        public int KitapId { get; set; }
        public string KitapAd { get; set; } = string.Empty;
        public int ToplamSiparisSayisi { get; set; }
        public int SiparisAdedi { get; set; }
    }
}