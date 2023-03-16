using BlazorEcommerce.Shared.Dtos;

namespace BlazorEcommerce.Server.Services.Cart
{
    public interface ICartService
    {
        Task<ServiceResponse<List<CartProductDto>>> GetCartProducts(List<CartItem> cartItems);
        Task<ServiceResponse<List<CartProductDto>>> StoreCartItems(List<CartItem> cartItems);
        Task<ServiceResponse<int>> GetCartItemsCount();
        Task<ServiceResponse<List<CartProductDto>>> GetDbCartProducts(int? userId = null);
        Task<ServiceResponse<bool>> AddToCart(CartItem cartItem);
        Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem);
        Task<ServiceResponse<bool>> RemoveItemFromCart(int productId, int productTypeId);
    }
}
