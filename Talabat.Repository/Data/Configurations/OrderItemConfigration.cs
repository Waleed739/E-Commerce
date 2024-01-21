using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Order_Aggreation;

namespace Talabat.Repository.Identity.Data.Configurations
{
    public class OrderItemConfigration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(Oi => Oi.Product, p => p.WithOwner());

            builder.Property(Oi => Oi.Price).HasColumnType("decimal(18,2)");
        }
    }
}
