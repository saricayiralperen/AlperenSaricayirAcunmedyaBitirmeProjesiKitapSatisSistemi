using System.Text.Json;
using KitapApi.Entities; // Kitap, Kategori, Siparis, Kullanici, SepetItem, SiparisDetay gibi entity'ler için
using KitapMVC.Models; // LoginViewModel için (BU SATIRIN KESİNLİKLE VAR OLDUĞUNDAN VE DOĞRU OLDUĞUNDAN EMİN OL!)
using System.Net.Http; // HttpClient ve StringContent için (BU SATIRIN KESİNLİKLE VAR OLDUĞUNDAN VE DOĞRU OLDUĞUNDAN EMİN OL!)
using System.Text; // Encoding.UTF8 için (BU SATIRIN KESİNLİKLE VAR OLDUĞUNDAN VE DOĞRU OLDUĞUNDAN EMİN OL!)
using Microsoft.AspNetCore.Http;

namespace KitapMVC.Services
{
    public class KitapApiService
    {
        private readonly HttpClient _httpClient;

        public KitapApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // KATEGORİ METOTLARI
        public async Task<List<Kategori>> GetKategorilerAsync()
        {
            var response = await _httpClient.GetAsync("api/Kategoriler");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var kategoriler = JsonSerializer.Deserialize<List<Kategori>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return kategoriler ?? new List<Kategori>();
        }

        public async Task<Kategori?> GetKategoriByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Kategoriler/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var content = await response.Content.ReadAsStringAsync();
            var kategori = JsonSerializer.Deserialize<Kategori>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return kategori;
        }

        public async Task<Kategori?> CreateKategoriAsync(Kategori kategori)
        {
            var jsonContent = JsonSerializer.Serialize(kategori, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Kategoriler", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Kategori oluşturma hatası: {response.StatusCode} - {errorContent}");
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var yeniKategori = JsonSerializer.Deserialize<Kategori>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return yeniKategori;
        }

        public async Task<bool> UpdateKategoriAsync(Kategori kategori)
        {
            var jsonContent = JsonSerializer.Serialize(kategori, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Kategoriler/{kategori.Id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteKategoriAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Kategoriler/{id}");
            return response.IsSuccessStatusCode;
        }


        // KİTAP METOTLARI
        public async Task<List<Kitap>> GetKitaplarAsync()
        {
            var response = await _httpClient.GetAsync("api/Kitaplar");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var kitaplar = JsonSerializer.Deserialize<List<Kitap>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return kitaplar ?? new List<Kitap>();
        }

        public async Task<Kitap?> GetKitapByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Kitaplar/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var content = await response.Content.ReadAsStringAsync();
            var kitap = JsonSerializer.Deserialize<Kitap>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return kitap;
        }

        public async Task<string?> UploadKitapImageAsync(int kitapId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;
            using var content = new MultipartFormDataContent();
            using var stream = file.OpenReadStream();
            content.Add(new StreamContent(stream), "file", file.FileName);
            var response = await _httpClient.PostAsync($"api/Kitaplar/upload?kitapId={kitapId}", content);
            if (!response.IsSuccessStatusCode)
                return null;
            var responseContent = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseContent);
            if (doc.RootElement.TryGetProperty("resimUrl", out var urlProp))
                return urlProp.GetString();
            return null;
        }

        // SİPARİŞ METOTLARI
        public async Task<Siparis?> CreateSiparisAsync(Siparis siparis)
        {
            var jsonContent = JsonSerializer.Serialize(siparis, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Siparisler", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Sipariş oluşturma hatası: {response.StatusCode} - {errorContent}");
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var olusturulanSiparis = JsonSerializer.Deserialize<Siparis>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return olusturulanSiparis;
        }

        public async Task<Siparis?> GetSiparisByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Siparisler/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var content = await response.Content.ReadAsStringAsync();
            var siparis = JsonSerializer.Deserialize<Siparis>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return siparis;
        }

        // KULLANICI METOTLARI (Admin Paneli Giriş/Kullanıcı Yönetimi için)
        public async Task<Kullanici?> LoginAsync(LoginViewModel loginModel)
        {
            var jsonContent = JsonSerializer.Serialize(loginModel, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Kullanicilar/Login", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Login hatası: {response.StatusCode} - {errorContent}");
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var kullanici = JsonSerializer.Deserialize<Kullanici>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return kullanici;
        }

        public async Task<Kullanici?> RegisterAsync(Kullanici kullanici)
        {
            var jsonContent = JsonSerializer.Serialize(kullanici, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Kullanicilar/Register", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Kayıt hatası: {response.StatusCode} - {errorContent}");
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var yeniKullanici = JsonSerializer.Deserialize<Kullanici>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return yeniKullanici;
        }

        public async Task<List<Kullanici>> GetKullanicilarAsync()
        {
            var response = await _httpClient.GetAsync("api/Kullanicilar");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var kullanicilar = JsonSerializer.Deserialize<List<Kullanici>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return kullanicilar ?? new List<Kullanici>();
        }

        public async Task<Kullanici?> GetKullaniciByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Kullanicilar/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var content = await response.Content.ReadAsStringAsync();
            var kullanici = JsonSerializer.Deserialize<Kullanici>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return kullanici;
        }

        public async Task<bool> UpdateKullaniciAsync(Kullanici kullanici)
        {
            var jsonContent = JsonSerializer.Serialize(kullanici, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Kullanicilar/{kullanici.Id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteKullaniciAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Kullanicilar/{id}");
            return response.IsSuccessStatusCode;
        }

        // FAVORİ METOTLARI
        public async Task<List<Favori>> GetFavorilerAsync()
        {
            var response = await _httpClient.GetAsync("api/Favori");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var favoriler = JsonSerializer.Deserialize<List<Favori>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return favoriler ?? new List<Favori>();
        }

        public async Task<Favori?> GetFavoriByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Favori/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var content = await response.Content.ReadAsStringAsync();
            var favori = JsonSerializer.Deserialize<Favori>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return favori;
        }

        public async Task<Favori?> CreateFavoriAsync(Favori favori)
        {
            var jsonContent = JsonSerializer.Serialize(favori, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Favori", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Favori oluşturma hatası: {response.StatusCode} - {errorContent}");
                return null;
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            var yeniFavori = JsonSerializer.Deserialize<Favori>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return yeniFavori;
        }

        public async Task<bool> DeleteFavoriAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Favori/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}