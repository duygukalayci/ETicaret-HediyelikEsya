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
    public class SliderConfiguration : IEntityTypeConfiguration<Slider>
    {
        public void Configure(EntityTypeBuilder<Slider> builder)
        {
            builder.HasKey(s => s.SliderID);

            builder.Property(s => s.Title)
                .IsRequired()
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100);
            
            builder.Property(s => s.ImageUrl)
                .HasColumnType("nvarchar(150)")
                .HasMaxLength(150);


        }
    }
}
