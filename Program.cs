using Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DatabaseContext>(); // veritaban� i�lemleri i�in

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
