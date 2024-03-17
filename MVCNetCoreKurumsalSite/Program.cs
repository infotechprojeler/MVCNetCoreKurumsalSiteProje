using Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Service;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// Authentication : Oturum açma ayarlarý
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Admin/Main/Login";
    x.AccessDeniedPath = "/AccessDenied"; // giriþ yetkisi olmayanlarý yönlendirmek istediðimiz sayfa
    x.Cookie.Name = "Account"; // oluþacak cookiye isim verebiliyoruz
    x.Cookie.MaxAge = TimeSpan.FromDays(1); // oluþacak cookie nin yaþam süresini belirleyebiliyoruz
    x.Cookie.IsEssential = true; // kalýcý cookie oluþsun
}); // Cookie based Authentication

// Authorization : Yetkilendirme (oturum açan kullanýcýnýn açmak istediði sayfaya eriþim yetkisi var mý kontrolü.)
builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("AdminPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Admin", "SuperAdmin")); // AdminPolicy yetkisiyle giriþ yapan Admin ve SuperAdmin yetkili kullanýcýlar AdminPolicy kullanýlan ekranlara eriþebilirler. Diðer "User", "Personal" vb yetkisiyle giriþ yapanlar bu ekranlara eriþemezler.
    x.AddPolicy("UserPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Admin", "SuperAdmin", "User", "Personal")); // UserPolicy kullanan ekranlarda bu rollerdeki tüm kullanýcýlar ekranlarý açabilir.
});

builder.Services.AddDbContext<DatabaseContext>(); // veritabaný iþlemleri için

// kendiz yazdýðýmýz servisi aþaðýdaki þekilde uygulamaya tanýtýyoruz, yoksa çalýþmýyor!!
builder.Services.AddScoped<ICategoryService, CategoryService>(); // burada uygulamaya þunu belirtiyoruz: Eðer sana ICategoryService den bir nesne kullanýlmak istenirse, git CategoryService sýnýfýndan bir nesne oluþtur ve onu kullanýma sun.

builder.Services.AddScoped(typeof(IService<>), typeof(Service<>)); // generic servisimizi bu þekilde ekliyoruz

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

app.UseSession(); // Session kullanabilmek için yukarda servis olarak ekledikten sonra burada kullanmak istediðimizi belirtiyoruz

app.UseRouting();

app.UseAuthentication(); // uygulamada Authentication ý etkin kýl
// Dikkat burada sýralama önemli! önce Authentication sonra Authorization gelecek.
app.UseAuthorization();// uygulamada Authorization ý etkin kýl

// Aþaðýdaki kod bloðu projeye sað týklayýp add new scaffolded item diyerek açýlan menüden Mvc Area > Ýsim verdik(Admin) dediðimizde projeye areas klasörü ve içine admin panel alaný ekleniyor, Txt dosyasý çýkýyor oradaki kodu kopyalayýp aþaðý yapýþtýrýyoruz ve endpoints i app yapýyoruz
app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "custom",
    pattern: "{customurl?}/{controller=Home}/{action=Index}/{id?}"); // adres çubuðunda ürün adý kategori adý vb geçirmek istersek bu þekilde özel url yapýsý ekleyebiliyoruz.

app.Run();
