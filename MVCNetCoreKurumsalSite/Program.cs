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

app.UseAuthorization();

// Aþaðýdaki kod bloðu projeye sað týklayýp add new scaffolded item diyerek açýlan menüden Mvc Area > Ýsim verdik(Admin) dediðimizde projeye areas klasörü ve içine admin panel alaný ekleniyor, Txt dosyasý çýkýyor oradaki kodu kopyalayýp aþaðý yapýþtýrýyoruz ve endpoints i app yapýyoruz
app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
