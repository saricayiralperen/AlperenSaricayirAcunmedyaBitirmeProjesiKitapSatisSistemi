using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KitapMVC.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace KitapMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminKategorilerController : Controller
    {
        // Burada API ile iletişim için bir servis veya HttpClient kullanılabilir.
        // Şimdilik örnek veri ile temel iskelet oluşturuluyor.

        // GET: AdminKategoriler
        public IActionResult Index()
        {
            // Kategorileri API'den çekip listeleyeceğiz (şimdilik boş liste)
            var kategoriler = new List<KategoriViewModel>();
            return View(kategoriler);
        }

        // GET: AdminKategoriler/Details/5
        public IActionResult Details(int id)
        {
            // Detayları API'den çek
            return View();
        }

        // GET: AdminKategoriler/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminKategoriler/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(KategoriViewModel model)
        {
            if (ModelState.IsValid)
            {
                // API'ye yeni kategori ekle
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: AdminKategoriler/Edit/5
        public IActionResult Edit(int id)
        {
            // API'den kategori çek
            return View();
        }

        // POST: AdminKategoriler/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, KategoriViewModel model)
        {
            if (ModelState.IsValid)
            {
                // API'de kategoriyi güncelle
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: AdminKategoriler/Delete/5
        public IActionResult Delete(int id)
        {
            // API'den kategori çek
            return View();
        }

        // POST: AdminKategoriler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // API'den kategoriyi sil
            return RedirectToAction(nameof(Index));
        }
    }
    // Kategori için basit bir ViewModel (geliştirilebilir)
    public class KategoriViewModel
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Aciklama { get; set; }
    }
} 