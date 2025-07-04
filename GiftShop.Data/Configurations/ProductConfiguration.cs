using GiftShop.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.ProductID);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .HasMaxLength(400);

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.Stock)
                .IsRequired()
                .HasColumnType("int");
                


            builder.Property(p => p.ImageUrl)
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");
            builder.Property(p => p.CreateDate)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(p => p.IsActive)
                .IsRequired()
                .HasDefaultValue(true);


            // Kategori ilişkisi
            builder.HasOne(p => p.Category)

                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
