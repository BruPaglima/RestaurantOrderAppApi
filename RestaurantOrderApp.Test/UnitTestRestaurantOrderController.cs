using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RestaurantOrderApp.Application.Dto;
using RestaurantOrderApp.Application.Enums;
using RestaurantOrderApp.Application.Services;
using RestaurantOrderApp.Data.Context;
using RestaurantOrderApp.Data.Repositories;
using RestaurantOrderApp.Domain.Models;
using System.Collections.Generic;
using Xunit;

namespace RestaurantOrderApp.Test
{
    public class UnitTestRestaurantOrderController : IClassFixture<DbFixture>
    {
        private ServiceProvider _serviceProvider;
        private IMapper _mapper;

        public UnitTestRestaurantOrderController(DbFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ClientOrderHistory, ClientOrderHistoryDto>();
                cfg.CreateMap<ClientOrderHistoryDto, ClientOrderHistory>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public void TestSplitDishesWithSuccess()
        {
            using (var context = _serviceProvider.GetService<RestaurantOrderContext>())
            {
                var repo = new ClientOrderHistoryRepository(context);
                var service = new RestaurantOrderAppService(repo, _mapper);

                var inputOrderDto = new InputOrderDto();
                inputOrderDto.ClientOrderInput = "morning, 1, 2, 3, 3, 3";

                var expectedResponse = new OrderSplitedDto();
                var expectedListDishes = new List<string>();
                expectedListDishes.Add(" 1");
                expectedListDishes.Add(" 2");
                expectedListDishes.Add(" 3");
                expectedListDishes.Add(" 3");
                expectedListDishes.Add(" 3");

                expectedResponse.DayPeriod = DayPeriodEnum.morning;
                expectedResponse.DishesList = expectedListDishes;

                var response = service.SeparatePeriodFromDishes(inputOrderDto);

                Assert.Equal(expectedListDishes, response.DishesList);
            }
        }

        [Fact]
        public void TestSplitDishesWithError()
        {
            using (var context = _serviceProvider.GetService<RestaurantOrderContext>())
            {
                var repo = new ClientOrderHistoryRepository(context);
                var service = new RestaurantOrderAppService(repo, _mapper);

                var inputOrderDto = new InputOrderDto();
                inputOrderDto.ClientOrderInput = "afternoon, 1, 2, 3, 3, 3";

                var response = service.SeparatePeriodFromDishes(inputOrderDto);

                Assert.Null(response);
            }
        }

        [Fact]
        public void TestCreateOrderOutputWithSuccess()
        {
            using (var context = _serviceProvider.GetService<RestaurantOrderContext>())
            {
                var repo = new ClientOrderHistoryRepository(context);
                var service = new RestaurantOrderAppService(repo, _mapper);
                var input = "night, 1, 2, 2, 4";

                var orderSplitedDto = new OrderSplitedDto();
                var listDishes = new List<string>();
                listDishes.Add("1");
                listDishes.Add("2");
                listDishes.Add("2");
                listDishes.Add("4");
                orderSplitedDto.DayPeriod = DayPeriodEnum.night;
                orderSplitedDto.DishesList = listDishes;

                var response = service.CreateOrderOutput(orderSplitedDto, input);

                Assert.Equal("steak, potato(2x), cake", response.ClientOrderOutput);
            }
        }

        [Fact]
        public void TestCreateOrderOutputWithError()
        {
            using (var context = _serviceProvider.GetService<RestaurantOrderContext>())
            {
                var repo = new ClientOrderHistoryRepository(context);
                var service = new RestaurantOrderAppService(repo, _mapper);
                var input = "night, 1, 1";

                var orderSplitedDto = new OrderSplitedDto();
                var listDishes = new List<string>();
                listDishes.Add("1");
                listDishes.Add("1");
                orderSplitedDto.DayPeriod = DayPeriodEnum.night;
                orderSplitedDto.DishesList = listDishes;

                var response = service.CreateOrderOutput(orderSplitedDto, input);

                Assert.Equal("steak, error", response.ClientOrderOutput);
            }
        }

        
    }
     
 
}
