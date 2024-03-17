using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); // include iþleminde aldýðýmýz hatayý çözmek için
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Infotech API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Lütfen JWT Bearer token bilginizi giriniz.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
       Reference = new OpenApiReference
       {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
       }
      },
      new string[] { }
    }
  });
});
builder.Services.AddDbContext<DatabaseContext>(); // veritabaný iþlemleri için

// kendi yazdýðýmýz servisleri uygulamada kullanabilmek için ekliyoruz aksi taktirde System.InvalidOperationException: Unable to resolve service for type 'Service.IService`1[Entities.User]' .... hatasý veriyor.
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        // validasyon yapmak istediðimiz alanlarý ayarlýyoruz
        ValidateAudience = true, // kitleyi doðrula
        ValidateIssuer = true, // token vereni doðrula
        ValidateLifetime = true, // token yaþam süresini doðrula
        ValidateIssuerSigningKey = true, // token verenin imzalama anahtarýný doðrula
        ValidIssuer = builder.Configuration["Token:Issuer"], // token veren saðlayýcý
        ValidAudience = builder.Configuration["Token:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])), // token imzalama anahtarý
        ClockSkew = TimeSpan.Zero // saat varký olmasýn
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("default", policy =>
    {
        policy.WithOrigins("https://localhost:7059").AllowAnyHeader().AllowAnyMethod(); // apiye istek atma iznine sahip domainler
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(); // resim vb dosyalarý api de saklamak için
app.UseAuthentication(); // api de oturum açmayý etkinleþtir
app.UseAuthorization();
app.UseCors("default"); // default adýný verdiðimiz politikayý kullan
app.MapControllers();

app.Run();
