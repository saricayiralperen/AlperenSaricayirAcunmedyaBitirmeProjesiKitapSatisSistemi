using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using KitapMVC.Services;
using System.Threading.Tasks;
using System.Linq;

namespace KitapMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminFavorilerController : Controller
    {
        private readonly KitapApiService _kitapApiService;
        public AdminFavorilerController(KitapApiService kitapApiService)
        {
            _kitapApiService = kitapApiService;
        }

        // GET: AdminFavoriler
        public async Task<IActionResult> Index()
        {
            var favoriler = await _kitapApiService.GetFavorilerAsync();
            var vmList = favoriler.Select(f => new FavoriViewModel
            {
                Id = f.Id,
                KullaniciAd = f.Kullanici?.AdSoyad ?? "-",
                KitapAd = f.Kitap?.Ad ?? "-"
            }).ToList();
            return View(vmList);
        }

        // GET: AdminFavoriler/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminFavoriler/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminFavoriler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
    public class FavoriViewModel
    {
        public int Id { get; set; }
        public string KullaniciAd { get; set; }
        public string KitapAd { get; set; }
    }
} 