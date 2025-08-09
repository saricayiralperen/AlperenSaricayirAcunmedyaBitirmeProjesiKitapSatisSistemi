using System.Text.Json;
using KitapMVC.Models.Entities;
using KitapMVC.Models;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace KitapMVC.Services
{
    public class KitapApiService : IKullaniciApiService, IRaporApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public KitapApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private void SetAuthorizationHeader()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext?.Session == null)
                {
                    Console.WriteLine("HATA: HttpContext veya Session null!");
                    return;
                }
                
                var token = httpContext.Session.GetString("JwtToken");
                var kullaniciId = httpContext.Session.GetInt32("KullaniciId");
                var kullaniciRol = httpContext.Session.GetString("KullaniciRol");
                
                Console.WriteLine($"Session Debug - KullaniciId: {kullaniciId}, Rol: {kullaniciRol}, Token var mı: {!string.IsNullOrEmpty(token)}");
                
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    Console.WriteLine("JWT Token authorization header'a eklendi.");
                }
                else
                {
                    Console.WriteLine("UYARI: JWT Token bulunamadı! Authorization header ayarlanmadı.");
                    // Mevcut authorization header'ı temizle
                    _httpClient.DefaultRequestHeaders.Authorization = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SetAuthorizationHeader hatası: {ex.Message}");
            }
        }

        // KATEGORİ METOTLARI
        public async Task<List<Kategori>> GetKategorilerAsync()
        {
            try
            {
                Console.WriteLine($"API Base Address: {_httpClient.BaseAddress}");
                Console.WriteLine("API çağrısı yapılıyor: api/Kategoriler");
                
                // BaseAddress null ise, doğrudan tam URL ile istek yap
                var response = _httpClient.BaseAddress != null 
                    ? await _httpClient.GetAsync("api/Kategoriler")
                    : await _httpClient.GetAsync("http://localhost:7010/api/Kategoriler");
                
                Console.WriteLine($"Response Status: {response.StatusCode}");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API Hatası: {response.StatusCode} - {response.ReasonPhrase}");
                    return new List<Kategori>();
                }

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Response Content: {content}");
                var kategoriler = JsonSerializer.Deserialize<List<Kategori>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return kategoriler ?? new List<Kategori>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP İstek Hatası: {ex.Message}");
                return new List<Kategori>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Genel Hata: {ex.Message}");
                return new List<Kategori>();
            }
        }

        public async Task<Kategori?> GetKategoriByIdAsync(int id)
        {
            // BaseAddress null ise, doğrudan tam URL ile istek yap
            var response = _httpClient.BaseAddress != null 
                ? await _httpClient.GetAsync($"api/Kategoriler/{id}")
                : await _httpClient.GetAsync($"http://localhost:7010/api/Kategoriler/{id}");
            
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
            try
            {
                SetAuthorizationHeader();
                
                var jsonContent = JsonSerializer.Serialize(kategori, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                
                Console.WriteLine($"Kategori oluşturma isteği gönderiliyor: {jsonContent}");

                // BaseAddress null ise, doğrudan tam URL ile istek yap
                var response = _httpClient.BaseAddress != null 
                    ? await _httpClient.PostAsync("api/Kategoriler", content)
                    : await _httpClient.PostAsync("http://localhost:7010/api/Kategoriler", content);

                Console.WriteLine($"Kategori oluşturma yanıtı: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Kategori oluşturma hatası: {response.StatusCode} - {errorContent}");
                    throw new HttpRequestException($"Kategori oluşturulamadı. Durum kodu: {response.StatusCode}, Hata: {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Kategori oluşturma başarılı yanıt: {responseContent}");
                var yeniKategori = JsonSerializer.Deserialize<Kategori>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return yeniKategori;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kategori oluşturma genel hatası: {ex.Message}");
                throw new HttpRequestException($"Kategori oluşturulurken beklenmeyen hata oluştu: {ex.Message}");
            }
        }

        public async Task<bool> UpdateKategoriAsync(Kategori kategori)
        {
            try
            {
                SetAuthorizationHeader();
                
                var jsonContent = JsonSerializer.Serialize(kategori, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                Console.WriteLine($"Kategori güncelleme isteği gönderiliyor - ID: {kategori.Id}, Ad: {kategori.Ad}");
                Console.WriteLine($"JSON Content: {jsonContent}");

                // BaseAddress null ise, doğrudan tam URL ile istek yap
                var response = _httpClient.BaseAddress != null 
                    ? await _httpClient.PutAsync($"api/Kategoriler/{kategori.Id}", content)
                    : await _httpClient.PutAsync($"http://localhost:7010/api/Kategoriler/{kategori.Id}", content);
                
                Console.WriteLine($"API Response Status: {response.StatusCode}");
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Kategori güncelleme hatası: {response.StatusCode} - {errorContent}");
                    throw new HttpRequestException($"API çağrısı başarısız oldu. Status: {response.StatusCode}, Hata: {errorContent}");
                }
                
                return true;
            }
            catch (HttpRequestException)
            {
                throw; // HttpRequestException'ları yeniden fırlat
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kategori güncelleme işlemi sırasında hata oluştu: {ex.Message}");
                throw new HttpRequestException($"Kategori güncelleme işlemi sırasında hata oluştu: {ex.Message}", ex);
            }
        }

        public async Task<(bool Success, string ErrorMessage)> DeleteKategoriAsync(int id)
        {
            try
            {
                SetAuthorizationHeader();
                
                // BaseAddress null ise, doğrudan tam URL ile istek yap
                var response = _httpClient.BaseAddress != null 
                    ? await _httpClient.DeleteAsync($"api/Kategoriler/{id}")
                    : await _httpClient.DeleteAsync($"http://localhost:7010/api/Kategoriler/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    return (true, string.Empty);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return (false, errorContent);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }


        // KİTAP METOTLARI
        public async Task<List<Kitap>> GetKitaplarAsync()
        {
            try
            {
                Console.WriteLine($"API Base Address: {_httpClient.BaseAddress}");
                Console.WriteLine("API çağrısı yapılıyor: api/Kitaplar");
                
                // BaseAddress null ise, doğrudan tam URL ile istek yap
                var response = _httpClient.BaseAddress != null 
                    ? await _httpClient.GetAsync("api/Kitaplar")
                    : await _httpClient.GetAsync("http://localhost:7010/api/Kitaplar");
                
                Console.WriteLine($"Response Status: {response.StatusCode}");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API Hatası: {response.StatusCode} - {response.ReasonPhrase}");
                    return new List<Kitap>();
                }

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Response Content: {content}");
                var kitaplar = JsonSerializer.Deserialize<List<Kitap>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return kitaplar ?? new List<Kitap>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP İstek Hatası: {ex.Message}");
                return new List<Kitap>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Genel Hata: {ex.Message}");
                return new List<Kitap>();
            }
        }

        public async Task<Kitap?> GetKitapByIdAsync(int id)
        {
            // BaseAddress null ise, doğrudan tam URL ile istek yap
            var response = _httpClient.BaseAddress != null 
                ? await _httpClient.GetAsync($"api/Kitaplar/{id}")
                : await _httpClient.GetAsync($"http://localhost:7010/api/Kitaplar/{id}");
            
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var content = await response.Content.ReadAsStringAsync();
            var kitap = JsonSerializer.Deserialize<Kitap>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return kitap;
        }

        public async Task<List<Kitap>> GetKitaplarByKategoriAsync(int kategoriId)
        {
            try
            {
                // BaseAddress null ise, doğrudan tam URL ile istek yap
                var response = _httpClient.BaseAddress != null 
                    ? await _httpClient.GetAsync($"api/Kitaplar/kategori/{kategoriId}")
                    : await _httpClient.GetAsync($"http://localhost:7010/api/Kitaplar/kategori/{kategoriId}");
                
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API Hatası: {response.StatusCode} - {response.ReasonPhrase}");
                    return new List<Kitap>();
                }

                var content = await response.Content.ReadAsStringAsync();
                var kitaplar = JsonSerializer.Deserialize<List<Kitap>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return kitaplar ?? new List<Kitap>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kategoriye göre kitap getirme hatası: {ex.Message}");
                return new List<Kitap>();
            }
        }

        public async Task<string?> UploadKitapImageAsync(int kitapId, IFormFile file)
        {
            SetAuthorizationHeader();
            
            if (file == null || file.Length == 0)
                return null;
            using var content = new MultipartFormDataContent();
            using var stream = file.OpenReadStream();
            content.Add(new StreamContent(stream), "file", file.FileName);
            
            // BaseAddress null ise, doğrudan tam URL ile istek yap
            var response = _httpClient.BaseAddress != null 
                ? await _httpClient.PostAsync($"api/Kitaplar/upload?kitapId={kitapId}", content)
                : await _httpClient.PostAsync($"http://localhost:7010/api/Kitaplar/upload?kitapId={kitapId}", content);
                
            if (!response.IsSuccessStatusCode)
                return null;
            var responseContent = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseContent);
            if (doc.RootElement.TryGetProperty("resimUrl", out var urlProp))
                return urlProp.GetString();
            return null;
        }

        public async Task<Kitap?> CreateKitapAsync(Kitap kitap)
        {
            try
            {
                SetAuthorizationHeader();
                
                var jsonContent = JsonSerializer.Serialize(kitap, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                
                Console.WriteLine($"Kitap oluşturma isteği gönderiliyor: {jsonContent}");

                // BaseAddress null ise, doğrudan tam URL ile istek yap
                var response = _httpClient.BaseAddress != null 
                    ? await _httpClient.PostAsync("api/Kitaplar", content)
                    : await _httpClient.PostAsync("http://localhost:7010/api/Kitaplar", content);

                Console.WriteLine($"Kitap oluşturma yanıtı: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Kitap oluşturma hatası: {response.StatusCode} - {errorContent}");
                    throw new HttpRequestException($"Kitap oluşturulamadı. Durum kodu: {response.StatusCode}, Hata: {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Kitap oluşturma başarılı yanıt: {responseContent}");
                var yeniKitap = JsonSerializer.Deserialize<Kitap>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return yeniKitap;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kitap oluşturma genel hatası: {ex.Message}");
                throw new HttpRequestException($"Kitap oluşturulurken beklenmeyen hata oluştu: {ex.Message}");
            }
        }

        public async Task<bool> UpdateKitapAsync(Kitap kitap)
        {
            try
            {
                SetAuthorizationHeader();
                
                var jsonContent = JsonSerializer.Serialize(kitap, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                Console.WriteLine($"Kitap güncelleme isteği gönderiliyor - ID: {kitap.Id}, Ad: {kitap.Ad}");
                Console.WriteLine($"JSON Content: {jsonContent}");

                // BaseAddress null ise, doğrudan tam URL ile istek yap
                var response = _httpClient.BaseAddress != null 
                    ? await _httpClient.PutAsync($"api/Kitaplar/{kitap.Id}", content)
                    : await _httpClient.PutAsync($"http://localhost:7010/api/Kitaplar/{kitap.Id}", content);
                
                Console.WriteLine($"API Response Status: {response.StatusCode}");
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Kitap güncelleme hatası: {response.StatusCode} - {errorContent}");
                    throw new HttpRequestException($"API çağrısı başarısız oldu. Status: {response.StatusCode}, Hata: {errorContent}");
                }
                
                return true;
            }
            catch (HttpRequestException)
            {
                throw; // HttpRequestException'ları yeniden fırlat
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kitap güncelleme işlemi sırasında hata oluştu: {ex.Message}");
                throw new HttpRequestException($"Kitap güncelleme işlemi sırasında hata oluştu: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteKitapAsync(int id)
        {
            try
            {
                SetAuthorizationHeader();
                
                // BaseAddress null ise, doğrudan tam URL ile istek yap
                var response = _httpClient.BaseAddress != null 
                    ? await _httpClient.DeleteAsync($"api/Kitaplar/{id}")
                    : await _httpClient.DeleteAsync($"http://localhost:7010/api/Kitaplar/{id}");
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"API çağrısı başarısız oldu. Status: {response.StatusCode}, Hata: {errorContent}");
                }
                
                return true;
            }
            catch (HttpRequestException)
            {
                throw; // HttpRequestException'ları yeniden fırlat
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Kitap silme işlemi sırasında hata oluştu: {ex.Message}", ex);
            }
        }

        // SİPARİŞ METOTLARI
        public async Task<List<Siparis>> GetSiparislerAsync()
        {
            try
            {
                // BaseAddress null ise, doğrudan tam URL ile istek yap
                var response = _httpClient.BaseAddress != null 
                    ? await _httpClient.GetAsync("api/Siparis")
                    : await _httpClient.GetAsync("http://localhost:7010/api/Siparis");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Siparişler alınırken hata: {response.StatusCode}");
                    return new List<Siparis>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                var siparisler = JsonSerializer.Deserialize<List<Siparis>>(jsonContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return siparisler ?? new List<Siparis>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Siparişler alınırken hata: {ex.Message}");
                return new List<Siparis>();
            }
        }

        public async Task<List<KitapSiparisIstatistik>> GetKitapSiparisIstatistikleriAsync()
        {
            try
            {
                // BaseAddress null ise, doğrudan tam URL ile istek yap
                var response = _httpClient.BaseAddress != null 
                    ? await _httpClient.GetAsync("api/Siparis/KitapIstatistikleri")
                    : await _httpClient.GetAsync("http://localhost:7010/api/Siparis/KitapIstatistikleri");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Kitap istatistikleri alınırken hata: {response.StatusCode}");
                    return new List<KitapSiparisIstatistik>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                var istatistikler = JsonSerializer.Deserialize<List<KitapSiparisIstatistik>>(jsonContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return istatistikler ?? new List<KitapSiparisIstatistik>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kitap istatistikleri alınırken hata: {ex.Message}");
                return new List<KitapSiparisIstatistik>();
            }
        }

        public async Task<Siparis?> GetSiparisByIdAsync(int id)
        {
            try
            {
                // BaseAddress null ise, doğrudan tam URL ile istek yap
                var response = _httpClient.BaseAddress != null 
                    ? await _httpClient.GetAsync($"api/Siparis/{id}")
                    : await _httpClient.GetAsync($"http://localhost:7010/api/Siparis/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Sipariş alınırken hata: {response.StatusCode}");
                    return null;
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                var siparis = JsonSerializer.Deserialize<Siparis>(jsonContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return siparis;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sipariş alınırken hata: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteSiparisAsync(int id)
        {
            try
            {
                SetAuthorizationHeader();
                
                // BaseAddress null ise, doğrudan tam URL ile istek yap
                var response = _httpClient.BaseAddress != null 
                    ? await _httpClient.DeleteAsync($"api/Siparis/{id}")
                    : await _httpClient.DeleteAsync($"http://localhost:7010/api/Siparis/{id}");
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sipariş silinirken hata: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SiparisOnaylaAsync(int id)
        {
            try
            {
                SetAuthorizationHeader();
                
                // BaseAddress null ise, doğrudan tam URL ile istek yap
                var response = _httpClient.BaseAddress != null 
                    ? await _httpClient.PutAsync($"api/Siparis/Onayla/{id}", null)
                    : await _httpClient.PutAsync($"http://localhost:7010/api/Siparis/Onayla/{id}", null);
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sipariş onaylanırken hata: {ex.Message}");
                return false;
            }
        }

        public async Task<Siparis?> CreateSiparisAsync(Siparis siparis)
        {
            SetAuthorizationHeader();
            
            var jsonContent = JsonSerializer.Serialize(siparis, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            // BaseAddress null ise, doğrudan tam URL ile istek yap
            var response = _httpClient.BaseAddress != null 
                ? await _httpClient.PostAsync("api/Siparis", content)
                : await _httpClient.PostAsync("http://localhost:7010/api/Siparis", content);

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

        // KULLANICI METOTLARI (Admin Paneli Giriş/Kullanıcı Yönetimi için)
        public async Task<(Kullanici? kullanici, string? token)> LoginAsync(LoginViewModel loginModel)
        {
            var jsonContent = JsonSerializer.Serialize(loginModel, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            // BaseAddress null ise, doğrudan tam URL ile istek yap
            var response = _httpClient.BaseAddress != null 
                ? await _httpClient.PostAsync("api/Kullanicilar/Login", content)
                : await _httpClient.PostAsync("http://localhost:7010/api/Kullanicilar/Login", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Login hatası: {response.StatusCode} - {errorContent}");
                return (null, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseContent);
            
            Kullanici? kullanici = null;
            string? token = null;
            
            if (doc.RootElement.TryGetProperty("kullanici", out var kullaniciElement))
            {
                kullanici = JsonSerializer.Deserialize<Kullanici>(kullaniciElement.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            
            if (doc.RootElement.TryGetProperty("token", out var tokenElement))
            {
                token = tokenElement.GetString();
            }
            
            return (kullanici, token);
        }

        public async Task<Kullanici?> RegisterAsync(Kullanici kullanici)
        {
            var jsonContent = JsonSerializer.Serialize(kullanici, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            // BaseAddress null ise, doğrudan tam URL ile istek yap
            var response = _httpClient.BaseAddress != null 
                ? await _httpClient.PostAsync("api/Kullanicilar/Register", content)
                : await _httpClient.PostAsync("http://localhost:7010/api/Kullanicilar/Register", content);

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

        public async Task<Kullanici?> CreateKullaniciAsync(Kullanici kullanici, string sifre)
        {
            try
            {
                // Register endpoint'i için uygun model oluştur
                var registerModel = new
                {
                    AdSoyad = kullanici.AdSoyad,
                    Email = kullanici.Email,
                    SifreHash = sifre, // API Register endpoint'i SifreHash alanını bekliyor
                    Rol = kullanici.Rol
                };

                var jsonContent = JsonSerializer.Serialize(registerModel, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                
                Console.WriteLine($"Kullanıcı oluşturma isteği gönderiliyor: {jsonContent}");

                // BaseAddress null ise, doğrudan tam URL ile istek yap
                var response = _httpClient.BaseAddress != null 
                    ? await _httpClient.PostAsync("api/Kullanicilar/Register", content)
                    : await _httpClient.PostAsync("http://localhost:7010/api/Kullanicilar/Register", content);

                Console.WriteLine($"Kullanıcı oluşturma yanıtı: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Kullanıcı oluşturma hatası: {response.StatusCode} - {errorContent}");
                    throw new HttpRequestException($"Kullanıcı oluşturulamadı. Durum kodu: {response.StatusCode}, Hata: {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Kullanıcı oluşturma başarılı yanıt: {responseContent}");
                var yeniKullanici = JsonSerializer.Deserialize<Kullanici>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return yeniKullanici;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kullanıcı oluşturma genel hatası: {ex.Message}");
                throw new HttpRequestException($"Kullanıcı oluşturulurken beklenmeyen hata oluştu: {ex.Message}");
            }
        }

        public async Task<List<Kullanici>> GetKullanicilarAsync()
        {
            try
            {
                Console.WriteLine($"GetKullanicilarAsync başladı - BaseAddress: {_httpClient.BaseAddress}");
                
                SetAuthorizationHeader();
                
                // BaseAddress null ise, doğrudan tam URL ile istek yap
                var response = _httpClient.BaseAddress != null 
                    ? await _httpClient.GetAsync("api/Kullanicilar")
                    : await _httpClient.GetAsync("http://localhost:7010/api/Kullanicilar");
                
                Console.WriteLine($"API Response Status: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Response Content Length: {content.Length}");
                    Console.WriteLine($"API Response Content (first 500 chars): {content.Substring(0, Math.Min(500, content.Length))}");
                    
                    var kullanicilar = JsonSerializer.Deserialize<List<Kullanici>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    Console.WriteLine($"Deserialize edilen kullanıcı sayısı: {kullanicilar?.Count ?? 0}");
                    
                    if (kullanicilar != null && kullanicilar.Any())
                    {
                        Console.WriteLine($"İlk kullanıcı: Id={kullanicilar[0].Id}, AdSoyad={kullanicilar[0].AdSoyad}, Email={kullanicilar[0].Email}");
                    }
                    
                    return kullanicilar ?? new List<Kullanici>();
                }
                else
                {
                    Console.WriteLine($"API çağrısı başarısız: {response.StatusCode} - {response.ReasonPhrase}");
                }
                
                return new List<Kullanici>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetKullanicilarAsync hatası: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return new List<Kullanici>();
            }
        }

        public async Task<Kullanici?> GetKullaniciByIdAsync(int id)
        {
            SetAuthorizationHeader();
            
            // BaseAddress null ise, doğrudan tam URL ile istek yap
            var response = _httpClient.BaseAddress != null 
                ? await _httpClient.GetAsync($"api/Kullanicilar/{id}")
                : await _httpClient.GetAsync($"http://localhost:7010/api/Kullanicilar/{id}");
            
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
            try
            {
                SetAuthorizationHeader();
                
                // Kullanici nesnesini UpdateKullaniciDto'ya dönüştür
                var updateDto = new Models.DTOs.UpdateKullaniciDto
                {
                    Id = kullanici.Id,
                    AdSoyad = kullanici.AdSoyad,
                    Email = kullanici.Email,
                    Rol = kullanici.Rol,
                    KayitTarihi = kullanici.KayitTarihi,
                    SifreHash = null // Şifre güncellenmeyecek
                };
                
                var jsonContent = JsonSerializer.Serialize(updateDto, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                Console.WriteLine($"Kullanıcı güncelleme isteği gönderiliyor - ID: {kullanici.Id}, AdSoyad: {kullanici.AdSoyad}");
                Console.WriteLine($"JSON Content: {jsonContent}");

                // BaseAddress null ise, doğrudan tam URL ile istek yap
                var response = _httpClient.BaseAddress != null 
                    ? await _httpClient.PutAsync($"api/Kullanicilar/{kullanici.Id}", content)
                    : await _httpClient.PutAsync($"http://localhost:7010/api/Kullanicilar/{kullanici.Id}", content);
                
                Console.WriteLine($"API Response Status: {response.StatusCode}");
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Kullanıcı güncelleme hatası: {response.StatusCode} - {errorContent}");
                    throw new HttpRequestException($"API çağrısı başarısız oldu. Status: {response.StatusCode}, Hata: {errorContent}");
                }
                
                return true;
            }
            catch (HttpRequestException)
            {
                throw; // HttpRequestException'ları yeniden fırlat
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kullanıcı güncelleme işlemi sırasında hata oluştu: {ex.Message}");
                throw new HttpRequestException($"Kullanıcı güncelleme işlemi sırasında hata oluştu: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteKullaniciAsync(int id)
        {
            SetAuthorizationHeader();
            
            // BaseAddress null ise, doğrudan tam URL ile istek yap
            var response = _httpClient.BaseAddress != null 
                ? await _httpClient.DeleteAsync($"api/Kullanicilar/{id}")
                : await _httpClient.DeleteAsync($"http://localhost:7010/api/Kullanicilar/{id}");
            
            return response.IsSuccessStatusCode;
        }

        // FAVORİ METOTLARI
        public async Task<List<Favori>> GetFavorilerAsync()
        {
            var response = _httpClient.BaseAddress != null 
                ? await _httpClient.GetAsync("api/Favori")
                : await _httpClient.GetAsync("http://localhost:7010/api/Favori");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Favori API Response: {content}");
            var favoriler = JsonSerializer.Deserialize<List<Favori>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Console.WriteLine($"Deserialize edilen favori sayısı: {favoriler?.Count ?? 0}");
            return favoriler ?? new List<Favori>();
        }

        public async Task<Favori?> GetFavoriByIdAsync(int id)
        {
            var response = _httpClient.BaseAddress != null 
                ? await _httpClient.GetAsync($"api/Favori/{id}")
                : await _httpClient.GetAsync($"http://localhost:7010/api/Favori/{id}");
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
            SetAuthorizationHeader();
            
            Console.WriteLine($"CreateFavoriAsync çağrıldı - KitapId: {favori.KitapId}, KullaniciId: {favori.KullaniciId}");
            
            var jsonContent = JsonSerializer.Serialize(favori, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            Console.WriteLine($"JSON Content: {jsonContent}");
            
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            
            // BaseAddress null ise, doğrudan tam URL ile istek yap
            var response = _httpClient.BaseAddress != null 
                ? await _httpClient.PostAsync("api/Favori", content)
                : await _httpClient.PostAsync("http://localhost:7010/api/Favori", content);
                
            Console.WriteLine($"API Response Status: {response.StatusCode}");
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Favori oluşturma hatası: {response.StatusCode} - {errorContent}");
                return null;
            }
            
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API Response Content: {responseContent}");
            
            var yeniFavori = JsonSerializer.Deserialize<Favori>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Console.WriteLine($"Favori başarıyla oluşturuldu - ID: {yeniFavori?.Id}");
            
            return yeniFavori;
        }

        public async Task<bool> DeleteFavoriAsync(int id)
        {
            SetAuthorizationHeader();
            
            var response = _httpClient.BaseAddress != null 
                ? await _httpClient.DeleteAsync($"api/Favori/{id}")
                : await _httpClient.DeleteAsync($"http://localhost:7010/api/Favori/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ChangePasswordAsync(int kullaniciId, string yeniSifre)
        {
            SetAuthorizationHeader();
            
            var changePasswordData = new
            {
                YeniSifre = yeniSifre
            };
            
            var json = JsonSerializer.Serialize(changePasswordData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = _httpClient.BaseAddress != null 
                ? await _httpClient.PutAsync($"api/kullanicilar/{kullaniciId}/change-password", content)
                : await _httpClient.PutAsync($"http://localhost:7010/api/kullanicilar/{kullaniciId}/change-password", content);
            
            return response.IsSuccessStatusCode;
        }

    }
}
