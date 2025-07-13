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
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(od => od.OrderDetailID);

            builder.Property(od => od.Quantity)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(od => od.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(od => od.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .HasComputedColumnSql("[Quantity] * [UnitPrice]", stored: true);

            // Doğru ilişki tanımı
            builder.HasOne(od => od.Order)    // OrderDetail'den Order'a bire çok ilişki
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
