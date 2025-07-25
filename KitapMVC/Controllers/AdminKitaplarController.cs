using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using KitapMVC.Services;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace KitapMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminKitaplarController : Controller
    {
        private readonly KitapApiService _kitapApiService;
        public AdminKitaplarController(KitapApiService kitapApiService)
        {
            _kitapApiService = kitapApiService;
        }

        // GET: AdminKitaplar
        public IActionResult Index()
        {
            var kitaplar = new List<KitapViewModel>();
            return View(kitaplar);
        }

        // GET: AdminKitaplar/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminKitaplar/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminKitaplar/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KitapViewModel model, IFormFile ResimDosyasi)
        {
            if (ModelState.IsValid)
            {
                // Kitap kaydını API'ya gönder
                var kitap = new KitapApi.Entities.Kitap
                {
                    Ad = model.Ad,
                    Yazar = model.Yazar,
                    Fiyat = model.Fiyat,
                    Aciklama = model.Aciklama,
                    KategoriId = model.KategoriId
                };
                // API'ya kitap ekle
                var response = await _kitapApiService.CreateKitapAsync(kitap);
                if (response != null && ResimDosyasi != null && ResimDosyasi.Length > 0)
                {
                    // Resim upload
                    var resimUrl = await _kitapApiService.UploadKitapImageAsync(response.Id, ResimDosyasi);
                    if (!string.IsNullOrEmpty(resimUrl))
                        response.ResimUrl = resimUrl;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: AdminKitaplar/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminKitaplar/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KitapViewModel model, IFormFile ResimDosyasi)
        {
            if (ModelState.IsValid)
            {
                // Kitap güncelle
                var kitap = new KitapApi.Entities.Kitap
                {
                    Id = id,
                    Ad = model.Ad,
                    Yazar = model.Yazar,
                    Fiyat = model.Fiyat,
                    Aciklama = model.Aciklama,
                    KategoriId = model.KategoriId,
                    ResimUrl = model.ResimUrl
                };
                var updateResult = await _kitapApiService.UpdateKitapAsync(kitap);
                if (updateResult && ResimDosyasi != null && ResimDosyasi.Length > 0)
                {
                    var resimUrl = await _kitapApiService.UploadKitapImageAsync(id, ResimDosyasi);
                    if (!string.IsNullOrEmpty(resimUrl))
                        kitap.ResimUrl = resimUrl;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: AdminKitaplar/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminKitaplar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
    public class KitapViewModel
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Yazar { get; set; }
        public decimal Fiyat { get; set; }
        public string Aciklama { get; set; }
        public string ResimUrl { get; set; }
        public int KategoriId { get; set; }
        public string KategoriAd { get; set; }
    }
} 