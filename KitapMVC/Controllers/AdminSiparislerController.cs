using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using KitapMVC.Services;
using System.Threading.Tasks;
using System.Linq;

namespace KitapMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminSiparislerController : Controller
    {
        private readonly KitapApiService _kitapApiService;
        public AdminSiparislerController(KitapApiService kitapApiService)
        {
            _kitapApiService = kitapApiService;
        }

        // GET: AdminSiparisler
        public async Task<IActionResult> Index()
        {
            var siparisler = await _kitapApiService.GetSiparislerAsync();
            var vmList = siparisler.Select(s => new SiparisViewModel
            {
                Id = s.Id,
                KullaniciAd = s.Kullanici?.AdSoyad ?? "-",
                SiparisTarihi = s.SiparisTarihi.ToString("g"),
                ToplamTutar = s.ToplamTutar,
                SiparisDetaylari = s.SiparisDetaylari?.Select(d => new SiparisDetayViewModel
                {
                    KitapAd = d.Kitap?.Ad ?? "-",
                    Adet = d.Adet,
                    Fiyat = d.Fiyat
                }).ToList() ?? new List<SiparisDetayViewModel>()
            }).ToList();
            return View(vmList);
        }

        // GET: AdminSiparisler/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminSiparisler/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminSiparisler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
    public class SiparisViewModel
    {
        public int Id { get; set; }
        public string KullaniciAd { get; set; }
        public string SiparisTarihi { get; set; }
        public decimal ToplamTutar { get; set; }
        public List<SiparisDetayViewModel> SiparisDetaylari { get; set; }
    }
    public class SiparisDetayViewModel
    {
        public string KitapAd { get; set; }
        public int Adet { get; set; }
        public decimal Fiyat { get; set; }
    }
} 