using BlazorEcommerce.Server.Services.Order;
using BlazorEcommerce.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<OrderOverviewDto>>>> GetOrders()
        {
            var res = await _orderService.GetOrders();
            return Ok(res);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<ServiceResponse<OrderDetailsDto>>> GetOrderDetails(int orderId)
        {
            var res = await _orderService.GetOrderDetails(orderId);
            return Ok(res);
        }
    }
}
