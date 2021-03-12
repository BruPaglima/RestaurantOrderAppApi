using System.ComponentModel.DataAnnotations;

namespace RestaurantOrderApp.Application.Dto
{
    public class InputOrderDto
    {
        [Required]
        public string ClientOrderInput { get; set; }
    }
}
