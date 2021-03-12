using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RestaurantOrderApp.Application.Dto;
using RestaurantOrderApp.Application.IServices;

namespace RestaurantOrderAppApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RestaurantOrderController : ControllerBase
    {
        private readonly IRestaurantOrderAppService _restaurantOrderAppService;

        public RestaurantOrderController(IRestaurantOrderAppService restaurantOrderAppService)
        {
            _restaurantOrderAppService = restaurantOrderAppService;
        }

        [HttpPost]
        [EnableCors("MyPolicy")]
        public IActionResult CreateOrder([FromBody] InputOrderDto inputOrderDto)
        {
            var orderSplitedDto = _restaurantOrderAppService.SeparatePeriodFromDishes(inputOrderDto);

            if (orderSplitedDto == null)
                return BadRequest("Invalid Input");

            var clientOrderHistoryDto = _restaurantOrderAppService.CreateOrderOutput(orderSplitedDto, inputOrderDto.ClientOrderInput);
            _restaurantOrderAppService.AddOrderHistory(clientOrderHistoryDto);
            var clientOrderHistoryList = _restaurantOrderAppService.GetAllOrderHistory();
            var orderOutPutAndHistoryDto = _restaurantOrderAppService.GenerateOrderOutputHistory(clientOrderHistoryDto.ClientOrderOutput, clientOrderHistoryList);

            return Ok(orderOutPutAndHistoryDto);
        }
    }
}
