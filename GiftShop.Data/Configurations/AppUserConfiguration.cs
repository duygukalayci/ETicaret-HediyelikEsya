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
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(x => x.Surname)
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(x => x.Phone)
                .IsRequired()
                .HasColumnType("varchar(15)")
                .HasMaxLength(15);

            builder.Property(x => x.Password)
                .IsRequired()
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50);

            builder.Property(x => x.UserName)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);


            //Bu satır sayesinde migration ile veritabanı oluştuğunda AppUser tablosunda otomatik bu kullanıcı olur.Genellikle admin kullanıcıyı otomatik eklemek için kullanılır.
            builder.HasData(new AppUser
            {
                AppUserID = 1,
                Name = "Test",
                Surname = "User",
                Email = "admineticaret@gmail.com",
                Password = "123",
                Phone = "1234567890",
                UserName = "Admin"
            }
            );
        }
    }
}
