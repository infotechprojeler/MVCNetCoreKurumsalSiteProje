using Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Admin/Main/Login";
}); // Cookie based Authentication
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

app.UseAuthorization();

// A�a��daki kod blo�u projeye sa� t�klay�p add new scaffolded item diyerek a��lan men�den Mvc Area > �sim verdik(Admin) dedi�imizde projeye areas klas�r� ve i�ine admin panel alan� ekleniyor, Txt dosyas� ��k�yor oradaki kodu kopyalay�p a�a�� yap��t�r�yoruz ve endpoints i app yap�yoruz
app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
