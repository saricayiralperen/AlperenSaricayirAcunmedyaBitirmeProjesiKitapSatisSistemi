using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitapMVC.Models;
using KitapMVC.Models.Entities;
using KitapMVC.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public async Task<IActionResult> Index()
        {
            try
            {
                var kitaplar = await _kitapApiService.GetKitaplarAsync();
                return View(kitaplar);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Kitaplar yüklenirken hata oluştu: " + ex.Message;
                return View(new List<KitapMVC.Models.Entities.Kitap>());
            }
        }

        // GET: AdminKitaplar/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var kitap = await _kitapApiService.GetKitapByIdAsync(id);
                if (kitap == null)
                {
                    return NotFound();
                }

                var model = new KitapViewModel
                {
                    Id = kitap.Id,
                    Ad = kitap.Ad ?? string.Empty,
                    Yazar = kitap.Yazar ?? string.Empty,
                    Fiyat = kitap.Fiyat,
                    KategoriId = kitap.KategoriId,
                    KategoriAd = kitap.Kategori?.Ad ?? "Kategori Bulunamadı",
                    Aciklama = kitap.Aciklama ?? string.Empty,
                    ResimUrl = kitap.ResimUrl ?? string.Empty
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Kitap detayları yüklenirken hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: AdminKitaplar/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var kategoriler = await _kitapApiService.GetKategorilerAsync();
                ViewBag.Kategoriler = new SelectList(kategoriler, "Id", "Ad");
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Kategoriler yüklenirken hata oluştu: " + ex.Message;
                ViewBag.Kategoriler = new SelectList(new List<object>(), "Id", "Ad");
                return View();
            }
        }

        // POST: AdminKitaplar/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KitapViewModel model, IFormFile ResimDosyasi)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var kitap = new KitapMVC.Models.Entities.Kitap
                    {
                        Ad = model.Ad,
                        Yazar = model.Yazar,
                        Fiyat = model.Fiyat,
                        KategoriId = model.KategoriId,
                        Aciklama = model.Aciklama
                    };

                    var createdKitap = await _kitapApiService.CreateKitapAsync(kitap);
                    
                    if (createdKitap != null && ResimDosyasi != null)
                    {
                        await _kitapApiService.UploadKitapImageAsync(createdKitap.Id, ResimDosyasi);
                    }

                    TempData["SuccessMessage"] = "Kitap başarıyla eklendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (HttpRequestException httpEx)
                {
                    TempData["ErrorMessage"] = $"Kitap eklenirken hata oluştu: {httpEx.Message}";
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

            // Hata durumunda kategorileri tekrar yükle
            try
            {
                var kategoriler = await _kitapApiService.GetKategorilerAsync();
                ViewBag.Kategoriler = new SelectList(kategoriler, "Id", "Ad", model.KategoriId);
            }
            catch
            {
                ViewBag.Kategoriler = new SelectList(new List<object>(), "Id", "Ad");
            }
            return View(model);
        }

        // GET: AdminKitaplar/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var kitap = await _kitapApiService.GetKitapByIdAsync(id);
                if (kitap == null)
                {
                    return NotFound();
                }

                var kategoriler = await _kitapApiService.GetKategorilerAsync();
                ViewBag.Kategoriler = new SelectList(kategoriler, "Id", "Ad", kitap.KategoriId);

                var model = new KitapViewModel
                {
                    Id = kitap.Id,
                    Ad = kitap.Ad ?? string.Empty,
                    Yazar = kitap.Yazar ?? string.Empty,
                    Fiyat = kitap.Fiyat,
                    KategoriId = kitap.KategoriId,
                    Aciklama = kitap.Aciklama ?? string.Empty,
                    ResimUrl = kitap.ResimUrl ?? string.Empty
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Kitap bilgileri yüklenirken hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: AdminKitaplar/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KitapViewModel model, IFormFile ResimDosyasi)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var kitap = new KitapMVC.Models.Entities.Kitap
                    {
                        Id = model.Id,
                        Ad = model.Ad,
                        Yazar = model.Yazar,
                        Fiyat = model.Fiyat,
                        KategoriId = model.KategoriId,
                        Aciklama = model.Aciklama,
                        ResimUrl = model.ResimUrl
                    };

                    var updateResult = await _kitapApiService.UpdateKitapAsync(kitap);
                    
                    if (ResimDosyasi != null)
                    {
                        await _kitapApiService.UploadKitapImageAsync(id, ResimDosyasi);
                    }

                    TempData["SuccessMessage"] = "Kitap başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (HttpRequestException httpEx)
                {
                    // API çağrısı ile ilgili özel hata mesajı
                    if (httpEx.Message.Contains("Forbidden") || httpEx.Message.Contains("401"))
                    {
                        TempData["ErrorMessage"] = "Kitap güncellenirken hata oluştu: Yetkiniz bulunmuyor. Lütfen tekrar giriş yapın.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Kitap güncellenirken hata oluştu: " + httpEx.Message;
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Kitap güncellenirken hata oluştu: " + ex.Message;
                }
            }
            else
            {
                // ModelState hatalarını göster
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["ErrorMessage"] = "Form doğrulama hatası: " + errors;
            }

            // Hata durumunda kategorileri tekrar yükle
            try
            {
                var kategoriler = await _kitapApiService.GetKategorilerAsync();
                ViewBag.Kategoriler = new SelectList(kategoriler, "Id", "Ad", model.KategoriId);
            }
            catch
            {
                ViewBag.Kategoriler = new SelectList(new List<object>(), "Id", "Ad");
            }
            return View(model);
        }

        // GET: AdminKitaplar/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var kitap = await _kitapApiService.GetKitapByIdAsync(id);
                if (kitap == null)
                {
                    return NotFound();
                }
                
                // Kitap entity'sini KitapViewModel'e dönüştür
                var viewModel = new KitapViewModel
                {
                    Id = kitap.Id,
                    Ad = kitap.Ad,
                    Yazar = kitap.Yazar,
                    Fiyat = kitap.Fiyat,
                    KategoriId = kitap.KategoriId,
                    KategoriAd = kitap.Kategori?.Ad ?? "Bilinmiyor"
                };
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Kitap bilgileri yüklenirken hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: AdminKitaplar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _kitapApiService.DeleteKitapAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Kitap başarıyla silindi.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Kitap silinirken bir hata oluştu. Kitap bulunamadı veya silme işlemi başarısız oldu.";
                }
            }
            catch (HttpRequestException httpEx)
            {
                // API çağrısı ile ilgili özel hata mesajı
                if (httpEx.Message.Contains("Forbidden"))
                {
                    TempData["ErrorMessage"] = "Kitap silinirken hata oluştu: Yetkiniz bulunmuyor. Lütfen tekrar giriş yapın.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Kitap silinirken hata oluştu: " + httpEx.Message;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Kitap silinirken hata oluştu: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}