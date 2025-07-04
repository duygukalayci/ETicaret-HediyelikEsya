using GiftShop.Data.Configurations;
using GiftShop.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop.Data
{
    public class DatabaseContext: DbContext
    {
        //public DbSet<AppUser> AppUsers { get; set; }
        //public DbSet<Product> Products { get; set; }
        //public DbSet<Order> Orders { get; set; }
        //public DbSet<OrderDetail> OrderDetails { get; set; }
        //public DbSet<Category> Categories { get; set; }
        //public DbSet<ContactMessage> ContactMessages { get; set; }
        //public DbSet<Slider> Sliders { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=GiftShopDb; Trusted_Connection=True; TrustServerCertificate=True;");
        //    base.OnConfiguring(optionsBuilder);

        //}

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    //modelBuilder.ApplyConfiguration(new AppUserConfiguration());
        //    //modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        //    // tüm configuration sınıflarını otomatik olarak uygular.
        //    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        //    base.OnModelCreating(modelBuilder);
        //}


        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Shipment> Shipments { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Burada config class’ların hepsini uygular
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }



    }
}
