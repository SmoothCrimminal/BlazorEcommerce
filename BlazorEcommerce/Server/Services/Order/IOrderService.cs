using BlazorEcommerce.Shared.Dtos;

namespace BlazorEcommerce.Server.Services.Order
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrder(int userId);
        Task<ServiceResponse<List<OrderOverviewDto>>> GetOrders();
        Task<ServiceResponse<OrderDetailsDto>> GetOrderDetails(int orderId);
    }
}
