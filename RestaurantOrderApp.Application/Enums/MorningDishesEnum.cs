using System.ComponentModel;

namespace RestaurantOrderApp.Application.Enums
{
    public enum MorningDishesEnum
    {
        [Description("eggs")]
        eggs = 1,

        [Description("toast")]
        toast = 2,

        [Description("coffee")]
        coffee = 3,
    }
}
