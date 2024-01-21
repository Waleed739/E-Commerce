using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggreation;

namespace Talabat.Repository.Identity.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(O => O.ShippingAddress, sh => sh.WithOwner());  // 1 to 1 total
            builder.Property(o => o.Status)
                   .HasConversion(
                           os => os.ToString(),
                           os => (OrderStatus)Enum.Parse(typeof(OrderStatus), os));

            builder.Property(O => O.SubTotal).HasColumnType("decimal(18,2)");

            builder.HasOne(o=>o.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(o => o.Items).WithOne().OnDelete(DeleteBehavior.Cascade);




        }
    }
}
