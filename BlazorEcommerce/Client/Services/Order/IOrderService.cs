﻿using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.Dtos;

namespace BlazorEcommerce.Client.Services.Order
{
    public interface IOrderService
    {
        Task PlaceOrder();
        Task<List<OrderOverviewDto>> GetOrders();
        Task<OrderDetailsDto> GetOrderDetails(int orderId);
    }
}
