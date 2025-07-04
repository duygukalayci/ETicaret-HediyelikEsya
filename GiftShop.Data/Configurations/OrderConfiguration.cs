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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.OrderID);

            builder.Property(o => o.OrderDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(o => o.TotalAmount)
                 .HasColumnType("decimal(10,2)")
                 .IsRequired(false);




            builder.Property(o => o.Status)
                     .IsRequired()
                     .HasColumnType("varchar(20)")
                     .HasMaxLength(20);
         


        }
    }
}
