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
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); // include i�leminde ald���m�z hatay� ��zmek i�in
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
        Description = "L�tfen JWT Bearer token bilginizi giriniz.",
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
builder.Services.AddDbContext<DatabaseContext>(); // veritaban� i�lemleri i�in

// kendi yazd���m�z servisleri uygulamada kullanabilmek i�in ekliyoruz aksi taktirde System.InvalidOperationException: Unable to resolve service for type 'Service.IService`1[Entities.User]' .... hatas� veriyor.
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        // validasyon yapmak istedi�imiz alanlar� ayarl�yoruz
        ValidateAudience = true, // kitleyi do�rula
        ValidateIssuer = true, // token vereni do�rula
        ValidateLifetime = true, // token ya�am s�resini do�rula
        ValidateIssuerSigningKey = true, // token verenin imzalama anahtar�n� do�rula
        ValidIssuer = builder.Configuration["Token:Issuer"], // token veren sa�lay�c�
        ValidAudience = builder.Configuration["Token:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])), // token imzalama anahtar�
        ClockSkew = TimeSpan.Zero // saat vark� olmas�n
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

app.UseStaticFiles(); // resim vb dosyalar� api de saklamak i�in
app.UseAuthentication(); // api de oturum a�may� etkinle�tir
app.UseAuthorization();
app.UseCors("default"); // default ad�n� verdi�imiz politikay� kullan
app.MapControllers();

app.Run();
