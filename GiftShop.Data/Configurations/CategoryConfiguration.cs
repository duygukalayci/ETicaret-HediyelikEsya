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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.CategoryID);

            builder.Property(c => c.Name)
                     .IsRequired()
                     .HasMaxLength(50);

            builder.Property(c => c.Description)
                        .HasMaxLength(100);

            builder.Property(c => c.IsActive)
                     .IsRequired();

            builder.Property(c => c.CreateDate)
                     .HasDefaultValueSql("getdate()");

            builder.HasData(
                         new Category { CategoryID = 1, Name = "Çiçekler" }
                              );



        }
    }
}
