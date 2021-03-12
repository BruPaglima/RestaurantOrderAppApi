using System;
using NetDevPack.Domain;

namespace RestaurantOrderApp.Domain.Models
{
    public class ClientOrderHistory: IAggregateRoot
    {
        public ClientOrderHistory()
        {
        }

        public int IdClientOrderHistory { get; private set; }
        public string ClientOrderInput { get; private set; }
        public string ClientOrderOutput { get; private set; }
        public DateTime ClientOrderDate { get; private set; }

    }
}
