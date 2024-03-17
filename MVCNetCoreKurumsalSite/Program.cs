using Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Service;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// Authentication : Oturum a�ma ayarlar�
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Admin/Main/Login";
    x.AccessDeniedPath = "/AccessDenied"; // giri� yetkisi olmayanlar� y�nlendirmek istedi�imiz sayfa
    x.Cookie.Name = "Account"; // olu�acak cookiye isim verebiliyoruz
    x.Cookie.MaxAge = TimeSpan.FromDays(1); // olu�acak cookie nin ya�am s�resini belirleyebiliyoruz
    x.Cookie.IsEssential = true; // kal�c� cookie olu�sun
}); // Cookie based Authentication

// Authorization : Yetkilendirme (oturum a�an kullan�c�n�n a�mak istedi�i sayfaya eri�im yetkisi var m� kontrol�.)
builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("AdminPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Admin", "SuperAdmin")); // AdminPolicy yetkisiyle giri� yapan Admin ve SuperAdmin yetkili kullan�c�lar AdminPolicy kullan�lan ekranlara eri�ebilirler. Di�er "User", "Personal" vb yetkisiyle giri� yapanlar bu ekranlara eri�emezler.
    x.AddPolicy("UserPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Admin", "SuperAdmin", "User", "Personal")); // UserPolicy kullanan ekranlarda bu rollerdeki t�m kullan�c�lar ekranlar� a�abilir.
});

builder.Services.AddDbContext<DatabaseContext>(); // veritaban� i�lemleri i�in

// kendiz yazd���m�z servisi a�a��daki �ekilde uygulamaya tan�t�yoruz, yoksa �al��m�yor!!
builder.Services.AddScoped<ICategoryService, CategoryService>(); // burada uygulamaya �unu belirtiyoruz: E�er sana ICategoryService den bir nesne kullan�lmak istenirse, git CategoryService s�n�f�ndan bir nesne olu�tur ve onu kullan�ma sun.

builder.Services.AddScoped(typeof(IService<>), typeof(Service<>)); // generic servisimizi bu �ekilde ekliyoruz

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession(); // Session kullanabilmek i�in yukarda servis olarak ekledikten sonra burada kullanmak istedi�imizi belirtiyoruz

app.UseRouting();

app.UseAuthentication(); // uygulamada Authentication � etkin k�l
// Dikkat burada s�ralama �nemli! �nce Authentication sonra Authorization gelecek.
app.UseAuthorization();// uygulamada Authorization � etkin k�l

// A�a��daki kod blo�u projeye sa� t�klay�p add new scaffolded item diyerek a��lan men�den Mvc Area > �sim verdik(Admin) dedi�imizde projeye areas klas�r� ve i�ine admin panel alan� ekleniyor, Txt dosyas� ��k�yor oradaki kodu kopyalay�p a�a�� yap��t�r�yoruz ve endpoints i app yap�yoruz
app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "custom",
    pattern: "{customurl?}/{controller=Home}/{action=Index}/{id?}"); // adres �ubu�unda �r�n ad� kategori ad� vb ge�irmek istersek bu �ekilde �zel url yap�s� ekleyebiliyoruz.

app.Run();
