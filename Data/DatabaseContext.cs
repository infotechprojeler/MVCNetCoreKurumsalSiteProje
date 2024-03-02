﻿using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class DatabaseContext : DbContext
    {
        // Burada entity framework kullanabilmek için Data projesine sağ tıklayıp nuget e bağlanıp entityframeworkcore.sqlserver ve tools paketlerini projeye ekliyoruz!
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Slide> Slides { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        // dbsetleri hazırladıktan sonra burada enable-migrations yapmıyoruz!!!

        // Veritabanı yoksa aşağıdaki package manager console u açıyoruz (Tools > Nuget package manager > package manager console)
        // Önce DefaultProject kısmından Data katamanını seçiyoruz
        // Sonra add-migration InitialCreate komutunu yazıp enter a basıp işlemi bekliyoruz.
        // Sonra aynı alana yine data katmanı seçiliyken update-database diyerek veritabanını oluşturuyoruz

        // veritabanı oluştuktan sonra projeye add new scaffolded item menüsünden adı admin olan bir area ekliyoruz
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"server=(LocalDb)\MSSQLLocalDB; database=MVCNetCoreKurumsalSiteProje; integrated security=True; trustservercertificate=True;");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Bu metot veritabanı oluştuktan sonra varsayılan kullanıcı ekleme veya örnek kategori ürün vb ekleme işlemleri için seed data işlerimizi yapabileceğimiz metottur.

            // .net core da seed data aşağıdaki şekilde tanımlanıyor
            // Not : burada Id yi de manuel veriyoruz!
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Email = "admin@mvckurumsal.net",
                IsActive = true,
                IsAdmin = true,
                Name = "Admin",
                Surname = "User",
                Password = "Admin123"
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
