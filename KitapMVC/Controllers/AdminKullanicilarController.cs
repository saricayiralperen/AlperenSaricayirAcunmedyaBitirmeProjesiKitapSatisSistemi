using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KitapMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminKullanicilarController : Controller
    {
        // GET: AdminKullanicilar
        public IActionResult Index()
        {
            var kullanicilar = new List<KullaniciViewModel>();
            return View(kullanicilar);
        }

        // GET: AdminKullanicilar/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminKullanicilar/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminKullanicilar/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(KullaniciViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: AdminKullanicilar/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminKullanicilar/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, KullaniciViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: AdminKullanicilar/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminKullanicilar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
    public class KullaniciViewModel
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
        public string KayitTarihi { get; set; }
    }
} 