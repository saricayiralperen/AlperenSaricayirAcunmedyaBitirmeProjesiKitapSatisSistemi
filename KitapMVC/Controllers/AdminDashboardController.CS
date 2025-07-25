using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KitapMVC.Controllers
{
    [Authorize(Roles = "Admin")] // Sadece Admin rolüne sahip kullanıcılar erişebilir
    public class AdminDashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Admin Dashboard";
            return View();
        }
    }
}