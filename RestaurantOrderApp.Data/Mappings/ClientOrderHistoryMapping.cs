using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantOrderApp.Domain.Models;

namespace RestaurantOrderApp.Data.Mappings
{ 
    public class ClientOrderHistoryMapping : IEntityTypeConfiguration<ClientOrderHistory>
    {
        public void Configure(EntityTypeBuilder<ClientOrderHistory> entity)
        {

            entity.HasKey(e => e.IdClientOrderHistory);

            entity.ToTable("CLIENTORDERHISTORY");

            entity.Property(e => e.ClientOrderInput)
                                .IsRequired()
                                .HasColumnName("CLIENTORDERINPUT")
                                .HasMaxLength(500);

            entity.Property(e => e.ClientOrderOutput)
                                .IsRequired()
                                .HasColumnName("CLIENTORDEROUTPUT")
                                .HasMaxLength(100);

            entity.Property(e => e.ClientOrderDate)
                                .HasColumnName("CLIENTORDERDATE")
                                .HasColumnType("smalldatetime");
        }
    }
}
