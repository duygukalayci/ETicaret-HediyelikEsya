using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftShop.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiftShop.Data.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(c => c.AppUserID);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            builder.Property(x => x.Surname)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            builder.Property(x => x.Phone)
                .IsRequired()
                .HasMaxLength(11) // Modelde 11 haneli olduğu belirtilmişti
                .HasColumnType("varchar(11)");

            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            builder.Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(30) // Modelde [StringLength(30)]
                .HasColumnType("varchar(30)");

            // Örnek kullanıcı (admin)
            builder.HasData(new AppUser
            {
                AppUserID = 1,
                Name = "Test",
                Surname = "User",
                Email = "admineticaret@gmail.com",
                Password = "123",
                Phone = "12345678901", // 11 hane olmalı
                UserName = "Admin",
                IsAdmin = true
            });
        }
    }
}