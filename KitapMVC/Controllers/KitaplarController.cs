using KitapMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace KitapMVC.Controllers
{
    public class KitaplarController : Controller
    {
        private readonly KitapApiService _kitapApiService;

        // Dependency Injection ile API servisimizi çağırıyoruz
        public KitaplarController(KitapApiService kitapApiService)
        {
            _kitapApiService = kitapApiService;
        }

        // Bu metot, /Kitaplar adresine gidildiğinde çalışacak
        public async Task<IActionResult> Index()
        {
            var kitaplar = await _kitapApiService.GetKitaplarAsync();
            // Kullanıcı kimliği örnek: session veya identity'den alınabilir
            int? kullaniciId = null;
            if (HttpContext.Session.GetInt32("KullaniciId") != null)
                kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            ViewBag.KullaniciId = kullaniciId;
            ViewData["Aciklama"] = "Tüm kitapları kategori, fiyat ve resimleriyle birlikte görebilirsiniz.";
            return View(kitaplar);
        }

        // GET: Kitaplar/Detail/5 (5 numaralı kitabın detayını gösterir)
        public async Task<IActionResult> Detail(int id)
        {
            var kitap = await _kitapApiService.GetKitapByIdAsync(id);
            if (kitap == null)
            {
                TempData["ErrorMessage"] = "Aradığınız kitap bulunamadı.";
                return RedirectToAction("Index");
            }

            // Ödevde istenen ViewBag kullanımı için örnek
            ViewBag.SayfaBasligi = "Kitap Detayları";

            return View(kitap);
        }
    }
}