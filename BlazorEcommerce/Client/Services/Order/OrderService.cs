using BlazorEcommerce.Client.Services.Auth;
using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.Order
{
    public class OrderService : ServiceBase, IOrderService
    {
        private readonly IAuthService _authService;
        private readonly NavigationManager _navigationManager;

        public OrderService(HttpClient httpClient, IAuthService authService, NavigationManager navigationManager) : base(httpClient)
        {
            _authService = authService;
            _navigationManager = navigationManager;
        }

        public async Task<string> PlaceOrder()
        {
            if (await _authService.IsUserAuthenticated())
            {
                var res = await _httpClient.PostAsync("api/payment/checkout", null);
                var url = await res.Content.ReadAsStringAsync();
                return url;
            }
            else
            {
                return "login";
            }
        }

        public async Task<List<OrderOverviewDto>> GetOrders()
        {
            var res = await _httpClient.GetFromJsonAsync<ServiceResponse<List<OrderOverviewDto>>>("api/order");
            return res.Data;
        }

        public async Task<OrderDetailsDto> GetOrderDetails(int orderId)
        {
            var res = await _httpClient.GetFromJsonAsync<ServiceResponse<OrderDetailsDto>>($"api/order/{orderId}");
            return res.Data;
        }
    }
}
