using System.ComponentModel;

namespace RestaurantOrderApp.Application.Enums
{
    public enum NightDishesEnum
    {
        [Description("steak")]
        steak = 1,

        [Description("potato")]
        potato = 2,

        [Description("wine")]
        wine = 3,

        [Description("cake")]
        cake = 4,

    }
}
