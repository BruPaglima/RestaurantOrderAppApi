using System;

namespace RestaurantOrderApp.Application.Dto
{
    public class ClientOrderHistoryDto
    {
        public ClientOrderHistoryDto()
        {
            ClientOrderDate = DateTime.Now;
        }

        public string ClientOrderInput { get; set; }

        public string ClientOrderOutput { get; set; }

        public DateTime ClientOrderDate { get; set; }
    }
}
