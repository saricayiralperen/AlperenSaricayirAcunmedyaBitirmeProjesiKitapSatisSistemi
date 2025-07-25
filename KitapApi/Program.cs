using Microsoft.EntityFrameworkCore;
using KitapApi.Data; // Bu using sat�r� �nemli!
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritaban� ba�lant�s� ayarlar�
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Servisleri konteynere ekleme
builder.Services.AddControllers();

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "supersecretkey12345";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "KitapApi";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// 3. Swagger iin DORU servisler
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

public static class StartupStatic
{
    public static string JwtKey = "supersecretkey12345";
    public static string JwtIssuer = "KitapApi";
}

var app = builder.Build();

// 4. HTTP istek hatt�n� yap�land�rma
// Geli�tirme ortam�ndaysak Swagger'� DO�RU �ekilde devreye al
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // JWT için
app.UseAuthorization();
app.MapControllers();
app.Run();