using System.ComponentModel;

namespace RestaurantOrderApp.Application.Enums
{
    public enum DayPeriodEnum
    {
        [Description("morning")]
        morning = 1,

        [Description("night")]
        night = 2,
    }
}