using System.Collections.Generic;
using NetDevPack.Data;
using RestaurantOrderApp.Domain.Models;

namespace RestaurantOrderApp.Domain.IRepositories
{
    public interface IClientOrderHistoryRepository : IRepository<ClientOrderHistory>
    {
        List<ClientOrderHistory> GetOrderHistoryList();
        void AddOrderHistory(ClientOrderHistory clientOrderHistory);
        void Save();
    }
}
