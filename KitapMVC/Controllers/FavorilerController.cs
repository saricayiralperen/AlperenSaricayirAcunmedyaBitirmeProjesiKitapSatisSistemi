using Microsoft.AspNetCore.Mvc;
using KitapApi.Entities;
using KitapMVC.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace KitapMVC.Controllers
{
    public class FavorilerController : Controller
    {
        private readonly KitapApiService _kitapApiService;
        public FavorilerController(KitapApiService kitapApiService)
        {
            _kitapApiService = kitapApiService;
        }

        // Favorilerim sayfası
        public async Task<IActionResult> Index(int? kullaniciId)
        {
            // Kullanıcı kimliği parametreyle veya oturumdan alınabilir
            var favoriler = await _kitapApiService.GetFavorilerAsync();
            if (kullaniciId.HasValue)
                favoriler = favoriler.Where(f => f.KullaniciId == kullaniciId.Value).ToList();
            return View(favoriler);
        }

        // Favorilere ekle
        [HttpPost]
        public async Task<IActionResult> Ekle(int kitapId, int kullaniciId)
        {
            var favori = new Favori { KitapId = kitapId, KullaniciId = kullaniciId };
            await _kitapApiService.CreateFavoriAsync(favori);
            TempData["SuccessMessage"] = "Kitap favorilere eklendi!";
            return Redirect(Request.Headers["Referer"].ToString());
        }

        // Favorilerden çıkar
        [HttpPost]
        public async Task<IActionResult> Sil(int favoriId)
        {
            await _kitapApiService.DeleteFavoriAsync(favoriId);
            TempData["InfoMessage"] = "Kitap favorilerden çıkarıldı.";
            return RedirectToAction("Index");
        }
    }
} 