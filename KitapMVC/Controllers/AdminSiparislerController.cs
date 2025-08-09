using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitapMVC.Models;
using KitapMVC.Services;

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
            try
            {
                var siparisler = await _kitapApiService.GetSiparislerAsync();
                var siparisViewModels = siparisler.Select(s => new SiparisViewModel
                {
                    Id = s.Id,
                    KullaniciAd = s.Kullanici?.AdSoyad ?? "Bilinmeyen Kullanıcı",
                    KullaniciEmail = s.Kullanici?.Email ?? "Bilinmeyen Email",
                    SiparisTarihi = s.SiparisTarihi.ToString("dd.MM.yyyy HH:mm"),
                    ToplamTutar = s.ToplamTutar,
                    Durum = s.Durum ?? "Beklemede",
                    SiparisDetaylari = s.SiparisDetaylari?.Select(sd => new SiparisDetayViewModel
                    {
                        KitapAd = sd.Kitap?.Ad ?? "Bilinmeyen Kitap",
                        Adet = sd.Adet,
                        Fiyat = sd.Fiyat
                    }).ToList() ?? new List<SiparisDetayViewModel>()
                }).ToList();

                return View(siparisViewModels);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Siparişler yüklenirken hata oluştu: {ex.Message}";
                return View(new List<SiparisViewModel>());
            }
        }

        // GET: AdminSiparisler/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var siparis = await _kitapApiService.GetSiparisByIdAsync(id);
                if (siparis == null)
                {
                    TempData["ErrorMessage"] = "Sipariş bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }

                var siparisViewModel = new SiparisViewModel
                {
                    Id = siparis.Id,
                    KullaniciAd = siparis.Kullanici?.AdSoyad ?? "Bilinmeyen Kullanıcı",
                    KullaniciEmail = siparis.Kullanici?.Email ?? "Bilinmeyen Email",
                    SiparisTarihi = siparis.SiparisTarihi.ToString("dd.MM.yyyy HH:mm"),
                    ToplamTutar = siparis.ToplamTutar,
                    Durum = siparis.Durum,
                    SiparisDetaylari = siparis.SiparisDetaylari?.Select(sd => new SiparisDetayViewModel
                    {
                        KitapAd = sd.Kitap?.Ad ?? "Bilinmeyen Kitap",
                        Adet = sd.Adet,
                        Fiyat = sd.Fiyat
                    }).ToList() ?? new List<SiparisDetayViewModel>()
                };

                return View(siparisViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Sipariş detayları yüklenirken hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: AdminSiparisler/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var siparis = await _kitapApiService.GetSiparisByIdAsync(id);
                if (siparis == null)
                {
                    TempData["ErrorMessage"] = "Sipariş bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }

                var siparisViewModel = new SiparisViewModel
                {
                    Id = siparis.Id,
                    KullaniciAd = siparis.Kullanici?.AdSoyad ?? "Bilinmeyen Kullanıcı",
                    KullaniciEmail = siparis.Kullanici?.Email ?? "Bilinmeyen Email",
                    SiparisTarihi = siparis.SiparisTarihi.ToString("dd.MM.yyyy HH:mm"),
                    ToplamTutar = siparis.ToplamTutar
                };

                return View(siparisViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Sipariş bilgileri yüklenirken hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: AdminSiparisler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Sipariş silme işlemi için API'ye DELETE isteği gönder
                var sonuc = await _kitapApiService.DeleteSiparisAsync(id);
                if (sonuc)
                {
                    TempData["SuccessMessage"] = "Sipariş başarıyla silindi.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Sipariş silinirken hata oluştu.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Sipariş silinirken hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: AdminSiparisler/Onayla/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Onayla(int id)
        {
            try
            {
                var sonuc = await _kitapApiService.SiparisOnaylaAsync(id);
                if (sonuc)
                {
                    TempData["SuccessMessage"] = "Sipariş başarıyla onaylandı.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Sipariş onaylanırken hata oluştu.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Sipariş onaylanırken hata oluştu: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}