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
    public class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessage>
    {
        public void Configure(EntityTypeBuilder<ContactMessage> builder)
        {
            builder.HasKey(cm => cm.ContactMessageID);

            builder.Property(cm => cm.Name)
                .HasMaxLength(50);

            builder.Property(cm => cm.Email)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(cm => cm.Subject)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(cm => cm.Message)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(cm => cm.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(cm => cm.IsRead)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}
