using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Shared.Dtos;

namespace BlazorEcommerce.Server.Services.Cart
{
    public class CartService : ServiceBase, ICartService
    {
        public CartService(DataContext dbContext) : base(dbContext)
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
    }
}
