using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestaurantOrderApp.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOrderApp.Test
{
    public class DbFixture
    {
        public ServiceProvider ServiceProvider { get; private set; }
        public DbFixture()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection
                .AddDbContext<RestaurantOrderContext>(options => options.UseSqlServer("Data Source=.;Initial Catalog=RestaurantOrder;Integrated Security=True;"),
                    ServiceLifetime.Transient);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }


    }
}
