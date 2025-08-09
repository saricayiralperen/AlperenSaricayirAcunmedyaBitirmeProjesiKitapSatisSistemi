using KitapApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace KitapApi.Data
{
    public static class DataSeeder
    {
        public static async Task SeedDataAsync(ApplicationDbContext context)
        {
            // Veritabanının oluşturulduğundan emin ol
            await context.Database.EnsureCreatedAsync();

            // Kategoriler için seeding kontrolü
            bool kategorilerExists = await context.Kategoriler.AnyAsync();
            bool kitaplarExists = await context.Kitaplar.AnyAsync();
            bool adminExists = await context.Kullanicilar.AnyAsync(u => u.Email == "admin@kitap.com");
            bool testUserExists = await context.Kullanicilar.AnyAsync(u => u.Email == "test@kitap.com");

            // Kategoriler ekle
            var kategoriler = new List<Kategori>
            {
                new Kategori { Ad = "Roman", Aciklama = "Roman kategorisi" },
                new Kategori { Ad = "Bilim Kurgu", Aciklama = "Bilim kurgu kitapları" },
                new Kategori { Ad = "Tarih", Aciklama = "Tarih kitapları" },
                new Kategori { Ad = "Felsefe", Aciklama = "Felsefe kitapları" },
                new Kategori { Ad = "Çocuk", Aciklama = "Çocuk kitapları" },
                new Kategori { Ad = "Edebiyat", Aciklama = "Edebiyat kitapları" },
                new Kategori { Ad = "Bilim", Aciklama = "Bilim kitapları" },
                new Kategori { Ad = "Sanat", Aciklama = "Sanat kitapları" }
            };

            if (!kategorilerExists)
            {
                context.Kategoriler.AddRange(kategoriler);
                await context.SaveChangesAsync();
            }

            // Kitaplar ekle
            var kitaplar = new List<Kitap>
            {
                new Kitap { Ad = "Suç ve Ceza", Yazar = "Fyodor Dostoyevski", Fiyat = 25.50m, Aciklama = "Klasik Rus edebiyatının başyapıtı", KategoriId = 1, ResimUrl = "https://via.placeholder.com/400x600/2C3E50/FFFFFF?text=Suç+ve+Ceza%0AFyodor+Dostoyevski" },
                new Kitap { Ad = "Bilinçaltının Gücü", Yazar = "Joseph Murphy", Fiyat = 30.00m, Aciklama = "Kişisel gelişim kitabı", KategoriId = 4, ResimUrl = "https://via.placeholder.com/400x600/34495E/FFFFFF?text=Bilinçaltının+Gücü%0AJoseph+Murphy" },
                new Kitap { Ad = "Bilinmeyen Bir Kadının Mektubu", Yazar = "Stefan Zweig", Fiyat = 15.75m, Aciklama = "Duygusal bir aşk hikayesi", KategoriId = 1, ResimUrl = "https://via.placeholder.com/400x600/8E44AD/FFFFFF?text=Bilinmeyen+Bir+Kadının+Mektubu%0AStefan+Zweig" },
                new Kitap { Ad = "1984", Yazar = "George Orwell", Fiyat = 22.00m, Aciklama = "Distopik roman", KategoriId = 2, ResimUrl = "https://via.placeholder.com/400x600/E74C3C/FFFFFF?text=1984%0AGeorge+Orwell" },
                new Kitap { Ad = "Hayvan Çiftliği", Yazar = "George Orwell", Fiyat = 18.50m, Aciklama = "Alegorik roman", KategoriId = 1, ResimUrl = "https://via.placeholder.com/400x600/27AE60/FFFFFF?text=Hayvan+Çiftliği%0AGeorge+Orwell" },
                new Kitap { Ad = "Dune", Yazar = "Frank Herbert", Fiyat = 35.00m, Aciklama = "Bilim kurgu klasiği", KategoriId = 2, ResimUrl = "https://via.placeholder.com/400x600/F39C12/FFFFFF?text=Dune%0AFrank+Herbert" },
                new Kitap { Ad = "Sapiens", Yazar = "Yuval Noah Harari", Fiyat = 28.75m, Aciklama = "İnsanlık tarihi", KategoriId = 3, ResimUrl = "https://via.placeholder.com/400x600/3498DB/FFFFFF?text=Sapiens%0AYuval+Noah+Harari" },
                new Kitap { Ad = "Homo Deus", Yazar = "Yuval Noah Harari", Fiyat = 32.00m, Aciklama = "Geleceğin tarihi", KategoriId = 3, ResimUrl = "https://via.placeholder.com/400x600/9B59B6/FFFFFF?text=Homo+Deus%0AYuval+Noah+Harari" },
                new Kitap { Ad = "Küçük Prens", Yazar = "Antoine de Saint-Exupéry", Fiyat = 12.50m, Aciklama = "Çocuklar için klasik", KategoriId = 5, ResimUrl = "https://via.placeholder.com/400x600/E67E22/FFFFFF?text=Küçük+Prens%0AAntoine+de+Saint-Exupéry" },
                new Kitap { Ad = "Simyacı", Yazar = "Paulo Coelho", Fiyat = 20.00m, Aciklama = "Felsefi roman", KategoriId = 4, ResimUrl = "https://via.placeholder.com/400x600/16A085/FFFFFF?text=Simyacı%0APaulo+Coelho" },
                new Kitap { Ad = "Kürk Mantolu Madonna", Yazar = "Sabahattin Ali", Fiyat = 16.25m, Aciklama = "Türk edebiyatı klasiği", KategoriId = 6, ResimUrl = "https://via.placeholder.com/400x600/D35400/FFFFFF?text=Kürk+Mantolu+Madonna%0ASabahattin+Ali" },
                new Kitap { Ad = "Harry Potter ve Felsefe Taşı", Yazar = "J.K. Rowling", Fiyat = 24.00m, Aciklama = "Fantastik roman", KategoriId = 5, ResimUrl = "https://via.placeholder.com/400x600/8E44AD/FFFFFF?text=Harry+Potter+ve+Felsefe+Taşı%0AJ.K.+Rowling" },
                new Kitap { Ad = "Yüzüklerin Efendisi", Yazar = "J.R.R. Tolkien", Fiyat = 45.00m, Aciklama = "Fantastik epik", KategoriId = 2, ResimUrl = "https://via.placeholder.com/400x600/2ECC71/FFFFFF?text=Yüzüklerin+Efendisi%0AJ.R.R.+Tolkien" },
                new Kitap { Ad = "Vadideki Zambak", Yazar = "Honoré de Balzac", Fiyat = 19.50m, Aciklama = "Fransız edebiyatı", KategoriId = 6, ResimUrl = "https://via.placeholder.com/400x600/E74C3C/FFFFFF?text=Vadideki+Zambak%0AHonoré+de+Balzac" },
                new Kitap { Ad = "Beyaz Geceler", Yazar = "Fyodor Dostoyevski", Fiyat = 14.75m, Aciklama = "Romantik hikaye", KategoriId = 1, ResimUrl = "https://via.placeholder.com/400x600/34495E/FFFFFF?text=Beyaz+Geceler%0AFyodor+Dostoyevski" },
                new Kitap { Ad = "Çalıkuşu", Yazar = "Reşat Nuri Güntekin", Fiyat = 17.00m, Aciklama = "Türk edebiyatı klasiği", KategoriId = 6, ResimUrl = "https://via.placeholder.com/400x600/F39C12/FFFFFF?text=Çalıkuşu%0AReşat+Nuri+Güntekin" },
                new Kitap { Ad = "Aşk-ı Memnu", Yazar = "Halit Ziya Uşaklıgil", Fiyat = 21.50m, Aciklama = "Türk edebiyatı", KategoriId = 6, ResimUrl = "https://via.placeholder.com/400x600/C0392B/FFFFFF?text=Aşk-ı+Memnu%0AHalit+Ziya+Uşaklıgil" },
                new Kitap { Ad = "Tutunamayanlar", Yazar = "Oğuz Atay", Fiyat = 26.00m, Aciklama = "Modern Türk edebiyatı", KategoriId = 6, ResimUrl = "https://via.placeholder.com/400x600/7F8C8D/FFFFFF?text=Tutunamayanlar%0AOğuz+Atay" },
                 new Kitap { Ad = "Kuyucaklı Yusuf", Yazar = "Sabahattin Ali", Fiyat = 23.25m, Aciklama = "Anadolu'nun hikayesi", KategoriId = 1, ResimUrl = "https://via.placeholder.com/400x600/1ABC9C/FFFFFF?text=Kuyucaklı+Yusuf%0ASabahattin+Ali" },
                 new Kitap { Ad = "Masumiyet Müzesi", Yazar = "Orhan Pamuk", Fiyat = 29.00m, Aciklama = "Nobel ödüllü yazarın aşk romanı", KategoriId = 1, ResimUrl = "https://via.placeholder.com/400x600/2980B9/FFFFFF?text=Masumiyet+Müzesi%0AOrhan+Pamuk" },
                new Kitap { Ad = "Fahrenheit 451", Yazar = "Ray Bradbury", Fiyat = 20.75m, Aciklama = "Distopik bilim kurgu", KategoriId = 2, ResimUrl = "https://via.placeholder.com/400x600/E67E22/FFFFFF?text=Fahrenheit+451%0ARay+Bradbury" },
                new Kitap { Ad = "Brave New World", Yazar = "Aldous Huxley", Fiyat = 22.50m, Aciklama = "Distopik roman", KategoriId = 2, ResimUrl = "https://via.placeholder.com/400x600/8E44AD/FFFFFF?text=Brave+New+World%0AAldous+Huxley" },
                new Kitap { Ad = "Zamanın Kısa Tarihi", Yazar = "Stephen Hawking", Fiyat = 31.00m, Aciklama = "Fizik ve kozmoloji", KategoriId = 7, ResimUrl = "https://via.placeholder.com/400x600/2C3E50/FFFFFF?text=Zamanın+Kısa+Tarihi%0AStephen+Hawking" },
                new Kitap { Ad = "Sanat Tarihi", Yazar = "Ernst Gombrich", Fiyat = 42.00m, Aciklama = "Sanat tarihi rehberi", KategoriId = 8, ResimUrl = "https://via.placeholder.com/400x600/D35400/FFFFFF?text=Sanat+Tarihi%0AErnst+Gombrich" },
                new Kitap { Ad = "Meditations", Yazar = "Marcus Aurelius", Fiyat = 18.00m, Aciklama = "Stoik felsefe", KategoriId = 4, ResimUrl = "https://via.placeholder.com/400x600/27AE60/FFFFFF?text=Meditations%0AMarcus+Aurelius" },
                new Kitap { Ad = "Nicomachean Ethics", Yazar = "Aristotle", Fiyat = 25.00m, Aciklama = "Etik felsefesi", KategoriId = 4, ResimUrl = "https://via.placeholder.com/400x600/34495E/FFFFFF?text=Nicomachean+Ethics%0AAristotle" },
                new Kitap { Ad = "The Republic", Yazar = "Plato", Fiyat = 27.50m, Aciklama = "Politik felsefe", KategoriId = 4, ResimUrl = "https://via.placeholder.com/400x600/9B59B6/FFFFFF?text=The+Republic%0APlato" },
                new Kitap { Ad = "İnsan Hakları Evrensel Beyannamesi", Yazar = "Birleşmiş Milletler", Fiyat = 8.00m, Aciklama = "İnsan hakları metni", KategoriId = 3, ResimUrl = "https://via.placeholder.com/400x600/3498DB/FFFFFF?text=İnsan+Hakları+Evrensel+Beyannamesi%0ABirleşmiş+Milletler" },
                new Kitap { Ad = "Osmanlı Tarihi", Yazar = "İlber Ortaylı", Fiyat = 35.50m, Aciklama = "Osmanlı İmparatorluğu tarihi", KategoriId = 3, ResimUrl = "https://via.placeholder.com/400x600/C0392B/FFFFFF?text=Osmanlı+Tarihi%0Aİlber+Ortaylı" },
                new Kitap { Ad = "Nutuk", Yazar = "Mustafa Kemal Atatürk", Fiyat = 15.00m, Aciklama = "Kurtuluş Savaşı anıları", KategoriId = 3, ResimUrl = "https://via.placeholder.com/400x600/E74C3C/FFFFFF?text=Nutuk%0AMustafa+Kemal+Atatürk" },
                new Kitap { Ad = "Alice Harikalar Diyarında", Yazar = "Lewis Carroll", Fiyat = 13.75m, Aciklama = "Çocuk klasiği", KategoriId = 5, ResimUrl = "https://via.placeholder.com/400x600/F39C12/FFFFFF?text=Alice+Harikalar+Diyarında%0ALewis+Carroll" },
                new Kitap { Ad = "Pinokyo", Yazar = "Carlo Collodi", Fiyat = 11.50m, Aciklama = "Çocuk masalı", KategoriId = 5, ResimUrl = "https://via.placeholder.com/400x600/16A085/FFFFFF?text=Pinokyo%0ACarlo+Collodi" },
                new Kitap { Ad = "Heidi", Yazar = "Johanna Spyri", Fiyat = 14.00m, Aciklama = "Çocuk romanı", KategoriId = 5, ResimUrl = "https://via.placeholder.com/400x600/2ECC71/FFFFFF?text=Heidi%0AJohanna+Spyri" }
            };

            if (!kitaplarExists)
            {
                context.Kitaplar.AddRange(kitaplar);
                await context.SaveChangesAsync();
            }

            // Admin kullanıcı ekle (eğer yoksa)
            if (!adminExists)
            {
                var adminUser = new Kullanici
                {
                    AdSoyad = "Admin User",
                    Email = "admin@kitap.com",
                    SifreHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Rol = "Admin",
                    KayitTarihi = DateTime.Now
                };

                context.Kullanicilar.Add(adminUser);
                await context.SaveChangesAsync();
            }

            // Test kullanıcı ekle (eğer yoksa)
            if (!testUserExists)
            {
                var testUser = new Kullanici
                {
                    AdSoyad = "Test User",
                    Email = "test@kitap.com",
                    SifreHash = BCrypt.Net.BCrypt.HashPassword("test123"),
                    Rol = "User",
                    KayitTarihi = DateTime.Now
                };

                context.Kullanicilar.Add(testUser);
                await context.SaveChangesAsync();
            }
        }
    }
}