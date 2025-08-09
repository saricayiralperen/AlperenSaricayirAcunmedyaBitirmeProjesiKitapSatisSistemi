using Microsoft.AspNetCore.Mvc;
using KitapMVC.Services;
using KitapMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace KitapMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IKullaniciApiService _kullaniciApiService;

        public AdminController(IKullaniciApiService kullaniciApiService)
        {
            _kullaniciApiService = kullaniciApiService;
        }

        // GET: /Admin/Login
        [HttpGet]
        public IActionResult Login()
        {
            ViewData["Title"] = "Admin Girişi";
            return View();
        }

        // POST: /Admin/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var (kullanici, token) = await _kullaniciApiService.LoginAsync(model);

                if (kullanici != null && !string.IsNullOrEmpty(token) && kullanici.Rol == "Admin")
                {
                    // Önceki session'ları ve cookie'leri tamamen temizle
                    HttpContext.Session.Clear();
                    try
                    {
                        await HttpContext.SignOutAsync("CookieAuth");
                    }
                    catch
                    {
                        // Cookie auth yoksa hata vermesin
                    }
                    
                    // JWT token'ı session'a kaydet
                    HttpContext.Session.SetString("JwtToken", token);
                    
                    // Session bilgilerini set et (layout'ta kullanılıyor)
                    HttpContext.Session.SetInt32("KullaniciId", kullanici.Id);
                    HttpContext.Session.SetString("KullaniciAd", kullanici.AdSoyad);
                    HttpContext.Session.SetString("KullaniciEmail", kullanici.Email);
                    HttpContext.Session.SetString("KullaniciRol", kullanici.Rol);
                    
                    // Cookie authentication için claims oluştur
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, kullanici.Id.ToString()),
                        new Claim(ClaimTypes.Name, kullanici.AdSoyad),
                        new Claim(ClaimTypes.Email, kullanici.Email),
                        new Claim(ClaimTypes.Role, kullanici.Rol)
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, "CookieAuth");

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
                    };

                    await HttpContext.SignInAsync(
                        "CookieAuth",
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    TempData["SuccessMessage"] = "Admin olarak başarıyla giriş yaptınız!";
                    return RedirectToAction("Index", "AdminDashboard");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Geçersiz e-posta veya şifre veya admin yetkisi yok.");
                }
            }
            ViewData["Title"] = "Admin Girişi";
            return View(model);
        }

        // GET: /Admin/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Tüm session'ı temizle
            HttpContext.Session.Clear();
            
            // Cookie authentication'dan çıkış yap
            try
            {
                await HttpContext.SignOutAsync("CookieAuth");
            }
            catch
            {
                // Cookie auth yoksa hata vermesin
            }
            
            TempData["InfoMessage"] = "Admin panelinden başarıyla çıkış yaptınız.";
            return RedirectToAction("Index", "Home");
        }

        // GET: /Admin/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}