using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.Order
{
    public class OrderService : ServiceBase, IOrderService
    {
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly NavigationManager _navigationManager;

        public OrderService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, NavigationManager navigationManager) : base(httpClient)
        {
            _authStateProvider = authenticationStateProvider;
            _navigationManager = navigationManager;
        }

        private async Task<bool> IsUserAuthenticated() => (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;

        public async Task PlaceOrder()
        {
            if (await IsUserAuthenticated())
            {
                await _httpClient.PostAsync("api/order", null);
            }
            else
            {
                _navigationManager.NavigateTo("login");
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
