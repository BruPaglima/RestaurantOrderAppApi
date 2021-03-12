using AutoMapper;
using RestaurantOrderApp.Application.Dto;
using RestaurantOrderApp.Application.Enums;
using RestaurantOrderApp.Application.IServices;
using RestaurantOrderApp.Domain.IRepositories;
using RestaurantOrderApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantOrderApp.Application.Services
{
    public class RestaurantOrderAppService : IRestaurantOrderAppService
    {
        private readonly IClientOrderHistoryRepository _clientOrderHistoryRepository;
        private readonly IMapper _mapper;

        public RestaurantOrderAppService(IClientOrderHistoryRepository clientOrderHistoryRepository, IMapper mapper)
        {
            _clientOrderHistoryRepository = clientOrderHistoryRepository;
            _mapper = mapper;
        }

        public List<ClientOrderHistoryDto> GetAllOrderHistory()
        {
            var listModel = _clientOrderHistoryRepository.GetOrderHistoryList();
            var listDto = _mapper.Map<List<ClientOrderHistoryDto>>(listModel);
            return listDto;
        }

        public void AddOrderHistory(ClientOrderHistoryDto clientOrderHistoryDto)
        {
            var clientOrderHistory = _mapper.Map<ClientOrderHistory>(clientOrderHistoryDto);
            _clientOrderHistoryRepository.AddOrderHistory(clientOrderHistory);
            _clientOrderHistoryRepository.Save();
        }

        public OrderSplitedDto SeparatePeriodFromDishes(InputOrderDto inputOrderDto)
        {
            var orderInfoDto = new OrderSplitedDto();
            List<string> stringParts = inputOrderDto.ClientOrderInput.Split(',').ToList();
            var dayPeriodString = stringParts.FirstOrDefault().ToLower();
            Enum.TryParse(dayPeriodString, out DayPeriodEnum dayPeriodEnum);

            if (dayPeriodEnum != DayPeriodEnum.morning && dayPeriodEnum != DayPeriodEnum.night)
                return null;

            orderInfoDto.DayPeriod = dayPeriodEnum;
            stringParts.RemoveAt(0);

            if (stringParts != null && stringParts.Count() != 0 && stringParts.FirstOrDefault().Trim() != "")
            {
                orderInfoDto.DishesList = stringParts;
            }
            else
            {
                return null;
            }

            return orderInfoDto;
        }

        public ClientOrderHistoryDto CreateOrderOutput(OrderSplitedDto orderInfoDto, string clientOrderInput)
        {
            var clientOrderHistoryDto = new ClientOrderHistoryDto();
            clientOrderHistoryDto.ClientOrderInput = clientOrderInput;

            if (orderInfoDto.DayPeriod == DayPeriodEnum.morning)
            {
                clientOrderHistoryDto.ClientOrderOutput = CreateMorningDishesFromInput(orderInfoDto.DishesList);
                return clientOrderHistoryDto;
            }
            else if (orderInfoDto.DayPeriod == DayPeriodEnum.night)
            {
                clientOrderHistoryDto.ClientOrderOutput = CreateNightDishesFromInput(orderInfoDto.DishesList);
                return clientOrderHistoryDto;
            }
            else
            {
                clientOrderHistoryDto.ClientOrderOutput = "error";
                return clientOrderHistoryDto;
            }
        }

        public OrderOutPutAndHistoryDto GenerateOrderOutputHistory(string clientOrderOutput, List<ClientOrderHistoryDto> historyList)
        {
            var orderOutPutAndHistoryDto = new OrderOutPutAndHistoryDto();
            orderOutPutAndHistoryDto.ClientOrderOutput = clientOrderOutput;
            orderOutPutAndHistoryDto.OrderHistory = historyList;

            return orderOutPutAndHistoryDto;
        }

        private string CreateNightDishesFromInput(List<string> dishesList)
        {
            bool haveSteak = false;
            bool havePotato = false;
            bool haveWine = false;
            bool haveCake = false;
            bool errorInvalidInput = false;
            int countPotato = 0;

            if (dishesList.Count() == 0)
                return "error";

            foreach (var dish in dishesList)
            {
                Int32.TryParse(dish, out int numValue);
                if (numValue != 0)
                {
                    Enum.TryParse(dish, out NightDishesEnum enumDish);

                    switch (enumDish)
                    {
                        case NightDishesEnum.steak:
                            {
                                if (haveSteak == false)
                                {
                                    haveSteak = true;
                                }
                                else
                                {
                                    errorInvalidInput = true;
                                }
                                break;
                            }
                        case NightDishesEnum.wine:
                            {
                                if (haveWine == false)
                                {
                                    haveWine = true;
                                }
                                else
                                {
                                    errorInvalidInput = true;
                                }
                                break;
                            }
                        case NightDishesEnum.cake:
                            {
                                if (haveCake == false)
                                {
                                    haveCake = true;
                                }
                                else
                                {
                                    errorInvalidInput = true;
                                }
                                break;
                            }
                        case NightDishesEnum.potato:
                            {
                                havePotato = true;
                                countPotato++;
                                break;
                            }
                        default:
                            {
                                errorInvalidInput = true;
                                break;
                            }
                    }

                    if (errorInvalidInput)
                        break;
                }
                else
                {
                    errorInvalidInput = true;
                    break;
                }
            }

            string dishesReturn = CreateOrdenedNightOrderList(haveSteak, haveWine, haveCake, havePotato, countPotato, errorInvalidInput);

            return dishesReturn;
        }

        private string CreateOrdenedNightOrderList(bool haveSteak, bool haveWine, bool haveCake, bool havePotato, int countPotato, bool errorInvalidInput)
        {
            string dishesReturn = "";

            if (haveSteak)
                dishesReturn = dishesReturn + NightDishesEnum.steak.ToString();

            if (havePotato)
            {
                if (dishesReturn != "")
                    dishesReturn = dishesReturn + ", ";

                if (countPotato > 1)
                {
                    dishesReturn = dishesReturn + NightDishesEnum.potato.ToString() + "(" + countPotato + "x)";
                }
                else
                {
                    dishesReturn = dishesReturn + NightDishesEnum.potato.ToString();
                }
            }

            if (haveWine)
            {
                if (dishesReturn != "")
                    dishesReturn = dishesReturn + ", ";
                dishesReturn = dishesReturn + NightDishesEnum.wine.ToString();
            }

            if (haveCake)
            {
                if (dishesReturn != "")
                    dishesReturn = dishesReturn + ", ";
                dishesReturn = dishesReturn + NightDishesEnum.cake.ToString();
            }

            if (errorInvalidInput)
            {
                if (dishesReturn != "")
                    dishesReturn = dishesReturn + ", ";
                dishesReturn = dishesReturn + "error";
            }

            return dishesReturn;
        }

        private string CreateMorningDishesFromInput(List<string> dishesList)
        {
            bool haveEggs = false;
            bool haveToast = false;
            bool haveCoffe = false;
            bool errorInvalidInput = false;
            int countCoffe = 0;

            if (dishesList.Count() == 0)
                return "error";

            foreach (var dish in dishesList)
            {
                Int32.TryParse(dish, out int numValue);
                if (numValue != 0)
                {
                    Enum.TryParse(dish, out MorningDishesEnum enumDish);

                    switch (enumDish)
                    {
                        case MorningDishesEnum.eggs:
                            {
                                if (haveEggs == false)
                                {
                                    haveEggs = true;
                                }
                                else
                                {
                                    errorInvalidInput = true;
                                }
                                break;
                            }
                        case MorningDishesEnum.toast:
                            {
                                if (haveToast == false)
                                {
                                    haveToast = true;
                                }
                                else
                                {
                                    errorInvalidInput = true;
                                }
                                break;
                            }
                        case MorningDishesEnum.coffee:
                            {
                                haveCoffe = true;
                                countCoffe++;
                                break;
                            }
                        default:
                            {
                                errorInvalidInput = true;
                                break;
                            }
                    }

                    if (errorInvalidInput)
                        break;
                }
                else
                {
                    errorInvalidInput = true;
                    break;
                }
            }

            var dishesReturn = CreateOrdenedMorningOrderList(haveEggs, haveToast, haveCoffe, countCoffe, errorInvalidInput);

            return dishesReturn;
        }

        private string CreateOrdenedMorningOrderList(bool haveEggs, bool haveToast, bool haveCoffe, int countCoffe, bool errorInvalidInput)
        {
            string dishesReturn = "";

            if (haveEggs)
                dishesReturn = dishesReturn + MorningDishesEnum.eggs.ToString();

            if (haveToast)
            {
                if (dishesReturn != "")
                    dishesReturn = dishesReturn + ", ";
                dishesReturn = dishesReturn + MorningDishesEnum.toast.ToString();
            }

            if (haveCoffe)
            {
                if (dishesReturn != "")
                    dishesReturn = dishesReturn + ", ";

                if (countCoffe > 1)
                {
                    dishesReturn = dishesReturn + MorningDishesEnum.coffee.ToString() + "(" + countCoffe + "x)";
                }
                else
                {
                    dishesReturn = dishesReturn + MorningDishesEnum.coffee.ToString();
                }
            }

            if (errorInvalidInput)
            {
                if (dishesReturn != "")
                    dishesReturn = dishesReturn + ", ";
                dishesReturn = dishesReturn + "error";
            }

            return dishesReturn;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
