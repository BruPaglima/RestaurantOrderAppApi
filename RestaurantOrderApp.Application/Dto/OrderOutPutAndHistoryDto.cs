using System.Collections.Generic;

namespace RestaurantOrderApp.Application.Dto
{
    public class OrderOutPutAndHistoryDto
    {
        public string ClientOrderOutput { get; set; }

        public List<ClientOrderHistoryDto> OrderHistory { get; set; }
    }
}
