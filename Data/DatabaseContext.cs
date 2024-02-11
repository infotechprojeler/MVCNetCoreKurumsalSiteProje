using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        // dbsetleri hazırladıktan sonra önce enable-migrations ile göçü aktif edip sonra update-database ile db yi oluşturuyoruz
        // veritabanı oluştuktan sonra projeye add new scaffolded item menüsünden adı admin olan bir area ekliyoruz
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"database=(LocalDb)\MSSQLLocalDB; initial catalog=MVCKurumsalSiteProje; integrated security=True;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
