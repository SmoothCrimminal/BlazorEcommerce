using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.Cart;
using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.Dtos;

namespace BlazorEcommerce.Server.Services.Order
{
    public class OrderService : ServiceBase, IOrderService
    {
        private readonly ICartService _cartService;

        public OrderService(DataContext dbContext, ICartService cartService, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
            _cartService = cartService;
        }

        public async Task<ServiceResponse<bool>> PlaceOrder()
        {
            var products = (await _cartService.GetDbCartProducts()).Data;
            var totalPrice = decimal.Zero;

            products.ForEach(p => totalPrice += p.Price * p.Quantity);

            var orderItems = new List<OrderItem>();
            products.ForEach(p => orderItems.Add(new OrderItem
            {
                ProductId = p.ProductId,
                ProductTypeId = p.ProductTypeId,
                Quantity = p.Quantity,
                TotalPrice = p.Price * p.Quantity
            }));

            var order = new BlazorEcommerce.Shared.Order
            {
                UserId = GetUserId(),
                OrderDate = DateTime.UtcNow,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            _dbContext.Orders.Add(order);

            _dbContext.CartItems.RemoveRange(_dbContext.CartItems.Where(ci => ci.UserId == GetUserId()));
            await _dbContext.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<List<OrderOverviewDto>>> GetOrders()
        {
            var res = new ServiceResponse<List<OrderOverviewDto>>();
            var orders = await _dbContext.Orders.Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product).Where(o => o.UserId == GetUserId())
                .OrderByDescending(o => o.OrderDate).ToListAsync();

            var orderResponse = new List<OrderOverviewDto>();
            orders.ForEach(o => orderResponse.Add(new OrderOverviewDto()
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                Product = o.OrderItems.Count > 1 ? $"{o.OrderItems.First().Product.Title} and {o.OrderItems.Count - 1} more..."
                                                 : o.OrderItems.First().Product.Title,
                ProductImageUrl = o.OrderItems.First().Product.ImageUrl
            }));

            res.Data = orderResponse;

            return res;
        }

        public async Task<ServiceResponse<OrderDetailsDto>> GetOrderDetails(int orderId)
        {
            var response = new ServiceResponse<OrderDetailsDto>();
            var order = await _dbContext.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                             .Include(o => o.OrderItems).ThenInclude(oi => oi.ProductType)
                             .Where(o => o.UserId == GetUserId() && o.Id == orderId).OrderByDescending(o => o.OrderDate).FirstOrDefaultAsync();

            if (order is null)
            {
                response.IsSuccess = false;
                response.Message = "Order not found.";
                return response;
            }

            var orderDetailsResponse = new OrderDetailsDto
            {
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                Products = new List<OrderDetailsProductDto>()
            };

            order.OrderItems.ForEach(item => orderDetailsResponse.Products.Add(new OrderDetailsProductDto
            {
                ProductId = item.ProductId,
                ImageUrl = item.Product.ImageUrl,
                ProductType = item.ProductType.Name,
                Quantity = item.Quantity,
                Title = item.Product.Title,
                TotalPrice = item.TotalPrice
            }));

            response.Data = orderDetailsResponse;

            return response;
        }
    }
}
