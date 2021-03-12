using RestaurantOrderApp.Application.Enums;
using System.Collections.Generic;

namespace RestaurantOrderApp.Application.Dto
{
    public class OrderSplitedDto
    {
        public DayPeriodEnum DayPeriod { get; set; }

        public List<string> DishesList { get; set; }
    }
}
