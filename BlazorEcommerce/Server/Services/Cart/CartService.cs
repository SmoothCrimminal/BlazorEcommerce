using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.Dtos;
using System.Security.Claims;

namespace BlazorEcommerce.Server.Services.Cart
{
    public class CartService : ServiceBase, ICartService
    {

        public CartService(DataContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }

        public async Task<ServiceResponse<List<CartProductDto>>> GetCartProducts(List<CartItem> cartItems)
        {
            var result = new ServiceResponse<List<CartProductDto>>()
            {
                Data = new List<CartProductDto>()
            };

            foreach (var item in cartItems)
            {
                var product = await _dbContext.Products.Where(p => p.Id == item.ProductId).FirstOrDefaultAsync();
                if (product is null)
                    continue;

                var productVariant = await _dbContext.ProductVariants.Where(v => v.ProductId == item.ProductId && v.ProductTypeId == item.ProductTypeId)
                                                                    .Include(v => v.ProductType)
                                                                    .FirstOrDefaultAsync();
                if (productVariant is null)
                    continue;

                var cartProduct = new CartProductDto
                {
                    ProductId = product.Id,
                    Title = product.Title,
                    ImageUrl = product.ImageUrl,
                    Price = productVariant.Price,
                    ProductType = productVariant.ProductType.Name,
                    ProductTypeId = productVariant.ProductTypeId,
                    Quantity = item.Quantity
                };

                result.Data.Add(cartProduct);
            }

            return result;
        }

        public async Task<ServiceResponse<List<CartProductDto>>> StoreCartItems(List<CartItem> cartItems)
        {
            var userId = GetUserId();

            cartItems.ForEach(ci => ci.UserId = userId);
            _dbContext.CartItems.AddRange(cartItems);
            await _dbContext.SaveChangesAsync();

            return await GetDbCartProducts();
        }

        public async Task<ServiceResponse<int>> GetCartItemsCount()
        {
            var count = await _dbContext.CartItems.Where(ci => ci.UserId == GetUserId()).CountAsync();
            return new ServiceResponse<int> { Data = count };
        }

        public async Task<ServiceResponse<List<CartProductDto>>> GetDbCartProducts()
        {
            return await GetCartProducts(await _dbContext.CartItems.Where(ci => ci.UserId == GetUserId()).ToListAsync());
        }

        public async Task<ServiceResponse<bool>> AddToCart(CartItem cartItem)
        {
            cartItem.UserId = GetUserId();

            var sameItem = await _dbContext.CartItems.FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId
                                                                        && ci.ProductTypeId == cartItem.ProductTypeId && ci.UserId == cartItem.UserId);
            if (sameItem is null)
            {
                _dbContext.CartItems.Add(cartItem);
            }
            else
            {
                sameItem.Quantity += cartItem.Quantity;
            }

            await _dbContext.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem)
        {
            var dbCartItem = await _dbContext.CartItems.FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId
                                                                        && ci.ProductTypeId == cartItem.ProductTypeId && ci.UserId == cartItem.UserId);

            if (dbCartItem is null)
            {
                return new ServiceResponse<bool> { Data = false, Message = "Cart item does not exist", IsSuccess = false };
            }

            dbCartItem.Quantity = cartItem.Quantity;
            await _dbContext.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<bool>> RemoveItemFromCart(int productId, int productTypeId)
        {
            var dbCartItem = await _dbContext.CartItems.FirstOrDefaultAsync(ci => ci.ProductId == productId
            && ci.ProductTypeId == productTypeId && ci.UserId == GetUserId());

            if (dbCartItem is null)
            {
                return new ServiceResponse<bool> { Data = false, Message = "Cart item does not exist", IsSuccess = false };
            }

            _dbContext.CartItems.Remove(dbCartItem);
            await _dbContext.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
