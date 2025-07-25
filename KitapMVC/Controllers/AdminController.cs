using Microsoft.AspNetCore.Mvc;
using KitapMVC.Services; // KullaniciApiService için
using KitapMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims; // ClaimTypes için
using System.Collections.Generic; // List için
using System.Threading.Tasks; // async Task için
using System; // DateTimeOffset için

namespace KitapMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly KullaniciApiService _kullaniciApiService;

        public AdminController(KullaniciApiService kullaniciApiService)
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
                var kullanici = await _kullaniciApiService.LoginAsync(model);

                if (kullanici != null)
                {
                    // Başarılı giriş: Kullanıcıyı cookie tabanlı oturuma al
                    // Dikkat: Aşağıdaki 'Claim' ve 'ClaimsIdentity' gibi tiplerin önünde 'System.Security.Claims'
                    // veya 'System.Collections.Generic' gibi tam yolları belirtilmiştir.
                    // Bu, derleyicinin doğru tipi bulmasına yardımcı olur.
                    var claims = new System.Collections.Generic.List<System.Security.Claims.Claim>
                    {
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, kullanici.Id.ToString()),
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, kullanici.AdSoyad),
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, kullanici.Email),
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, kullanici.Rol)
                    };

                    var claimsIdentity = new System.Security.Claims.ClaimsIdentity(
                        claims, Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = System.DateTimeOffset.UtcNow.AddMinutes(60)
                    };

                    await HttpContext.SignInAsync(
                        Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme,
                        new System.Security.Claims.ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    TempData["SuccessMessage"] = "Başarıyla giriş yaptınız!";
                    return RedirectToAction("Index", "AdminDashboard"); // Admin paneli anasayfasına yönlendir
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Geçersiz e-posta veya şifre.");
                }
            }
            ViewData["Title"] = "Admin Girişi";
            return View(model);
        }

        // GET: /Admin/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["InfoMessage"] = "Başarıyla çıkış yaptınız.";
            return RedirectToAction("Index", "Home"); // Anasayfaya yönlendir
        }

        // GET: /Admin/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
//using Microsoft.AspNetCore.Mvc;

//namespace KitapMVC.Controllers
//{
//    public class AdminController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//    }
//}
