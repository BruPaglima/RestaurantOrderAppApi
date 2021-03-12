using RestaurantOrderApp.Application.Dto;
using System;
using System.Collections.Generic;

namespace RestaurantOrderApp.Application.IServices
{
    public interface IRestaurantOrderAppService : IDisposable
    {
        List<ClientOrderHistoryDto> GetAllOrderHistory();
        OrderSplitedDto SeparatePeriodFromDishes(InputOrderDto inputOrderDto);
        ClientOrderHistoryDto CreateOrderOutput(OrderSplitedDto orderInfoDto, string clientOrderInput);
        void AddOrderHistory(ClientOrderHistoryDto clientOrderHistoryDto);
        OrderOutPutAndHistoryDto GenerateOrderOutputHistory(string clientOrderOutput, List<ClientOrderHistoryDto> historyList);
    }
}
