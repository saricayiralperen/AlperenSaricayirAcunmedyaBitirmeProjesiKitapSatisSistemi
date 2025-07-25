using KitapMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace KitapMVC.Controllers
{
    public class KategorilerController : Controller
    {
        private readonly KitapApiService _kitapApiService;

        public KategorilerController(KitapApiService kitapApiService)
        {
            _kitapApiService = kitapApiService;
        }

        // Bu metot, /Kategoriler adresine gidildiğinde çalışacak
        public async Task<IActionResult> Index()
        {
            var kategoriler = await _kitapApiService.GetKategorilerAsync();
            return View(kategoriler);
        }
        // GET: Kategoriler/Detail/5 (5 numaralı kategorinin detayını gösterir)
        public async Task<IActionResult> Detail(int id)
        {
            var kategori = await _kitapApiService.GetKategoriByIdAsync(id); // API servisinden kategoriyi çek
            if (kategori == null)
            {
                return NotFound(); // Kategori bulunamazsa 404 sayfasına yönlendir
            }

            ViewBag.SayfaBasligi = "Kategori Detayları"; // ViewBag örneği
            return View(kategori);
        }
    }
}