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
    public class ShipmentConfiguration: IEntityTypeConfiguration<Shipment>
    {

        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.HasKey(s => s.ShipmentID);

            builder.Property(s => s.Carrier)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.TrackingNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.ShippedDate)
                .HasColumnType("datetime");

            builder.Property(s => s.DeliveredDate)
                .HasColumnType("datetime");

            // Relationship with Order
            builder.HasOne(s => s.Order)
                .WithOne(o => o.Shipment)
                .HasForeignKey<Shipment>(s => s.OrderID);
        }
    }
}
