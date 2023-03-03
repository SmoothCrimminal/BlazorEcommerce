using BlazorEcommerce.Shared.Dtos;

namespace BlazorEcommerce.Server.Services.Cart
{
    public interface ICartService
    {
        Task<ServiceResponse<List<CartProductDto>>> GetCartProducts(List<CartItem> cartItems);
    }
}
