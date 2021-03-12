using NetDevPack.Data;
using RestaurantOrderApp.Data.Context;
using RestaurantOrderApp.Domain.IRepositories;
using RestaurantOrderApp.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantOrderApp.Data.Repositories
{
    public class ClientOrderHistoryRepository : IClientOrderHistoryRepository
    {
        protected readonly RestaurantOrderContext _context;

        public ClientOrderHistoryRepository(RestaurantOrderContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public List<ClientOrderHistory> GetOrderHistoryList()
        {
            var ClientOrderHistoryList = _context.ClientOrderHistory.ToList();                
  
            return ClientOrderHistoryList;
        }

        public void AddOrderHistory(ClientOrderHistory clientOrderHistory) 
        {
            _context.ClientOrderHistory.Add(clientOrderHistory);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }

}
