using Microsoft.EntityFrameworkCore;
using NetDevPack.Data;
using RestaurantOrderApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOrderApp.Data.Context
{
    public partial class RestaurantOrderContext : DbContext, IUnitOfWork
    {
        public RestaurantOrderContext(DbContextOptions<RestaurantOrderContext> options) : base(options)
        {
        }

        public virtual DbSet<ClientOrderHistory> ClientOrderHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(RestaurantOrderContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}
