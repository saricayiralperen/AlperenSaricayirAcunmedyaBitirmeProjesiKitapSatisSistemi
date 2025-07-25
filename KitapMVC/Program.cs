using KitapMVC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<KitapApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7001");
});

builder.Services.AddControllersWithViews();
// ----- BURAYA EKLENECEK KODLAR (AddHttpClient'ýn altýna ekleyebilirsin) -----
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturumun 30 dakika boþta kalýrsa sona ermesini saðlar
    options.Cookie.HttpOnly = true; // Tarayýcý tarafýndaki scriptlerin cookie'ye eriþimini engeller
    options.Cookie.IsEssential = true; // GDPR uyumluluðu için, session cookie'sinin gerekli olduðunu belirtir
});
// -------------------------------------------------------------------------
var app = builder.Build();
// ----- BURAYA EKLENECEK KOD -----
app.UseSession(); // Session servisini kullan
// ---------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
//using KitapMVC.Services;

//var builder = WebApplication.CreateBuilder(args);

//// ----- AddHttpClient KODU BURADA, DOÐRU PORT NUMARASIYLA -----
//builder.Services.AddHttpClient<KitapApiService>(client =>
//{
//    // API'nin çalýþtýðý, daha önce sabitlediðimiz adresi yazýyoruz
//    client.BaseAddress = new Uri("https://localhost:7001");
//});
//// -------------------------------------------------------------

//// Add services to the container.
//builder.Services.AddControllersWithViews();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles(); // Bu satýr önemli, statik dosyalar için
//app.UseRouting();

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();
////using KitapMVC.Services;
////var builder = WebApplication.CreateBuilder(args);
////// ----- BURAYA EKLENECEK KOD -----
////builder.Services.AddHttpClient<KitapApiService>(client =>
////{
////    // KitapApi'nin çalýþtýðý adresi buraya yazýyoruz.
////    // Bu adresi KitapApi projesinin launchSettings.json dosyasýndan bulabilirsin.
////    client.BaseAddress = new Uri("https://localhost:7158");
////});
////// ------------------------------------
////// Add services to the container.
////builder.Services.AddControllersWithViews();

////var app = builder.Build();

////// Configure the HTTP request pipeline.
////if (!app.Environment.IsDevelopment())
////{
////    app.UseExceptionHandler("/Home/Error");
////    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
////    app.UseHsts();
////}

////app.UseHttpsRedirection();
////app.UseRouting();

////app.UseAuthorization();

////app.MapStaticAssets();

////app.MapControllerRoute(
////    name: "default",
////    pattern: "{controller=Home}/{action=Index}/{id?}")
////    .WithStaticAssets();


////app.Run();
