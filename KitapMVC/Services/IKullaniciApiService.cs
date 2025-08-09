using KitapMVC.Models.Entities;
using KitapMVC.Models;

namespace KitapMVC.Services
{
    public interface IKullaniciApiService
    {
        Task<(Kullanici? kullanici, string? token)> LoginAsync(LoginViewModel loginModel);
        Task<Kullanici?> RegisterAsync(Kullanici kullanici);
        Task<Kullanici?> CreateKullaniciAsync(Kullanici kullanici, string sifre);
        Task<List<Kullanici>> GetKullanicilarAsync();
        Task<Kullanici?> GetKullaniciByIdAsync(int id);
        Task<bool> UpdateKullaniciAsync(Kullanici kullanici);
        Task<bool> DeleteKullaniciAsync(int id);
        Task<List<Favori>> GetFavorilerAsync();
        Task<Favori?> GetFavoriByIdAsync(int id);
        Task<bool> DeleteFavoriAsync(int id);
    }
}