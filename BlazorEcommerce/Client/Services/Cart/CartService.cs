using BlazorEcommerce.Client.Services.Auth;
using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.Dtos;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.Cart
{
    public class CartService : ServiceBase, ICartService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IAuthService _authService;

        public event Action OnChange;

        public CartService(ILocalStorageService localStorageService, IAuthService authService, HttpClient httpClient) : base(httpClient)
        {
            _localStorageService = localStorageService;
            _authService = authService;
        }

        public async Task AddToCart(CartItem cartItem)
        {
            if (await _authService.IsUserAuthenticated())
            {
                await _httpClient.PostAsJsonAsync("api/cart/add", cartItem);
            }
            else
            {
                var cart = await GetCart();

                var sameItem = cart.Find(x => x.ProductId == cartItem.ProductId && x.ProductTypeId == cartItem.ProductTypeId);
                if (sameItem is null)
                {
                    cart.Add(cartItem);
                }
                else
                {
                    sameItem.Quantity += cartItem.Quantity;
                }

                await _localStorageService.SetItemAsync("cart", cart);
            }

            await GetCartItemsCount();
        }

        private async Task<List<CartItem>> GetCart()
        {
            await GetCartItemsCount();
            var cart = await _localStorageService.GetItemAsync<List<CartItem>>("cart");
            if (cart is null)
            {
                cart = new List<CartItem>();
            }

            return cart;
        }

        public async Task<List<CartProductDto>> GetCartProducts()
        {
            if (await _authService.IsUserAuthenticated())
            {
                var response = await _httpClient.GetFromJsonAsync<ServiceResponse<List<CartProductDto>>>("api/cart");
                return response.Data;
            }
            else
            {
                var cartItems = await GetCart();
                if (cartItems is null)
                    return new List<CartProductDto>();

                var response = await _httpClient.PostAsJsonAsync("api/cart/products", cartItems);
                var cartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductDto>>>();
                return cartProducts.Data;
            }
        }

        public async Task RemoveProductFromCart(int productId, int productTypeId)
        {
            if (await _authService.IsUserAuthenticated())
            {
                await _httpClient.DeleteAsync($"api/cart/{productId}/{productTypeId}");
            }
            else
            {
                var cart = await GetCart();
                if (!cart.Any())
                    return;

                var cartItem = cart.Find(x => x.ProductId == productId && x.ProductTypeId == productTypeId);
                if (cartItem is not null)
                {
                    cart.Remove(cartItem);
                    await _localStorageService.SetItemAsync("cart", cart);
                }
            }
        }

        public async Task UpdateQuantity(CartProductDto product)
        {
            if (await _authService.IsUserAuthenticated())
            {
                var request = new CartItem
                {
                    ProductId = product.ProductId,
                    Quantity = product.Quantity,
                    ProductTypeId = product.ProductTypeId
                };

                await _httpClient.PutAsJsonAsync("api/cart/update-quantity", request);
            }
            else
            {
                var cart = await GetCart();
                if (!cart.Any())
                    return;

                var cartItem = cart.Find(x => x.ProductId == product.ProductId && x.ProductTypeId == product.ProductTypeId);
                if (cartItem is not null)
                {
                    cartItem.Quantity = product.Quantity;
                    await _localStorageService.SetItemAsync("cart", cart);
                }
            }  
        }

        public async Task StoreCartItems(bool emptyLocalCart = false)
        {
            var cart = await GetCart();

            await _httpClient.PostAsJsonAsync("api/cart", cart);

            if (emptyLocalCart)
            {
                await _localStorageService.RemoveItemAsync("cart");
            }
        }

        public async Task GetCartItemsCount()
        {
            if (await _authService.IsUserAuthenticated())
            {
                var res = await _httpClient.GetFromJsonAsync<ServiceResponse<int>>("api/cart/count");
                var count = res.Data;

                await _localStorageService.SetItemAsync("cartItemsCount", count);
            }
            else
            {
                var cart = await GetCart();
                await _localStorageService.SetItemAsync("cartItemsCount", cart is not null ? cart.Count : 0);
            }

            OnChange.Invoke();
        }
    }
}
