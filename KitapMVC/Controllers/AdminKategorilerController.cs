using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitapMVC.Models;
using KitapMVC.Services;
using KitapMVC.Models.Entities;

namespace KitapMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminKategorilerController : Controller
    {
        private readonly KitapApiService _kitapApiService;

        public AdminKategorilerController(KitapApiService kitapApiService)
        {
            _kitapApiService = kitapApiService;
        }

        // GET: AdminKategoriler
        public async Task<IActionResult> Index()
        {
            try
            {
                var kategoriler = await _kitapApiService.GetKategorilerAsync();
                var viewModels = kategoriler.Select(k => new KategoriViewModel
                {
                    Id = k.Id,
                    Ad = k.Ad ?? string.Empty,
                    Aciklama = k.Aciklama ?? string.Empty
                }).ToList();
                return View(viewModels);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Kategoriler yüklenirken hata oluştu: {ex.Message}";
                return View(new List<KategoriViewModel>());
            }
        }

        // GET: AdminKategoriler/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var kategori = await _kitapApiService.GetKategoriByIdAsync(id);
                if (kategori == null)
                {
                    return NotFound();
                }
                
                // Kategoriye ait kitapları getir
                var kitaplar = await _kitapApiService.GetKitaplarByKategoriAsync(id);
                
                var viewModel = new KategoriViewModel
                {
                    Id = kategori.Id,
                    Ad = kategori.Ad ?? string.Empty,
                    Aciklama = kategori.Aciklama ?? string.Empty,
                    Kitaplar = kitaplar
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Kategori detayları yüklenirken hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: AdminKategoriler/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminKategoriler/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KategoriViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var kategori = new Kategori
                    {
                        Ad = model.Ad,
                        Aciklama = model.Aciklama
                    };
                    
                    var yeniKategori = await _kitapApiService.CreateKategoriAsync(kategori);
                    if (yeniKategori != null)
                    {
                        TempData["SuccessMessage"] = "Kategori başarıyla eklendi.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Kategori eklenirken bir hata oluştu.";
                    }
                }
                catch (HttpRequestException httpEx)
                {
                    TempData["ErrorMessage"] = $"Kategori eklenirken hata oluştu: {httpEx.Message}";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Beklenmeyen hata oluştu: {ex.Message}";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Lütfen tüm alanları doğru şekilde doldurun.";
            }
            return View(model);
        }

        // GET: AdminKategoriler/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var kategori = await _kitapApiService.GetKategoriByIdAsync(id);
                if (kategori == null)
                {
                    return NotFound();
                }
                var viewModel = new KategoriViewModel
                {
                    Id = kategori.Id,
                    Ad = kategori.Ad,
                    Aciklama = kategori.Aciklama ?? string.Empty
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Kategori yüklenirken hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: AdminKategoriler/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KategoriViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var kategori = new Kategori
                    {
                        Id = model.Id,
                        Ad = model.Ad,
                        Aciklama = model.Aciklama
                    };
                    
                    var basarili = await _kitapApiService.UpdateKategoriAsync(kategori);
                    if (basarili)
                    {
                        TempData["SuccessMessage"] = "Kategori başarıyla güncellendi.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Kategori güncellenirken bir hata oluştu.";
                    }
                }
                catch (HttpRequestException httpEx)
                {
                    // API çağrısı ile ilgili özel hata mesajı
                    TempData["ErrorMessage"] = $"Kategori güncellenirken hata oluştu: {httpEx.Message}";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Beklenmeyen hata oluştu: {ex.Message}";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Lütfen tüm alanları doğru şekilde doldurun.";
            }
            return View(model);
        }

        // GET: AdminKategoriler/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var kategori = await _kitapApiService.GetKategoriByIdAsync(id);
                if (kategori == null)
                {
                    return NotFound();
                }
                var viewModel = new KategoriViewModel
                {
                    Id = kategori.Id,
                    Ad = kategori.Ad ?? string.Empty,
                    Aciklama = kategori.Aciklama ?? string.Empty
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Kategori yüklenirken hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: AdminKategoriler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var (success, errorMessage) = await _kitapApiService.DeleteKategoriAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Kategori başarıyla silindi.";
                }
                else
                {
                    // API'den gelen hata mesajını temizle ve kullanıcı dostu hale getir
                    var cleanErrorMessage = errorMessage.Replace("\"", "").Trim();
                    TempData["ErrorMessage"] = cleanErrorMessage;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Hata: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}