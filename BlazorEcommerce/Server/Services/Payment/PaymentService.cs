using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.Auth;
using BlazorEcommerce.Server.Services.Cart;
using BlazorEcommerce.Server.Services.Order;
using Stripe;
using Stripe.Checkout;

namespace BlazorEcommerce.Server.Services.Payment
{
    public class PaymentService : ServiceBase, IPaymentService
    {
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;

        public PaymentService(ICartService cartService, IAuthService authService, IOrderService orderService, IConfiguration configuration,
            DataContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
            _cartService = cartService;
            _authService = authService;
            _orderService = orderService;
            _configuration = configuration;

            StripeConfiguration.ApiKey = _configuration["Authentication:StripeApiKey"];
        }

        public async Task<Session> CreateCheckoutSession()
        {
            var products = (await _cartService.GetDbCartProducts()).Data;
            var lineItems = new List<SessionLineItemOptions>();

            products.ForEach(product => lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = product.Price * 100,
                    Currency = "pln",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Title,
                        Images = new List<string> { product.ImageUrl }
                    }
                },
                Quantity = product.Quantity
            }));

            var options = new SessionCreateOptions
            {
                CustomerEmail = GetUserEmailAddress(),
                ShippingAddressCollection = new SessionShippingAddressCollectionOptions
                {
                    AllowedCountries = new List<string> { "PL", "US" }
                },
                PaymentMethodTypes = new List<string>
                {
                    "card",
                    "blik"
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:7173/order-success",
                CancelUrl = "https://localhost:7173/cart"
            };

            var service = new SessionService();

            Session session = service.Create(options);
            return session;
        }

        public async Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request)
        {
            var json = await new StreamReader(request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, request.Headers["Stripe-Signature"], _configuration["Authentication:WHSecret"]);
                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    var user = await _authService.GetUserByEmail(session.CustomerEmail);
                    await _orderService.PlaceOrder(user.Id);
                }

                return new ServiceResponse<bool> { Data = true };
            }
            catch (StripeException e)
            {
                return new ServiceResponse<bool> { Data = false, IsSuccess = false, Message = e.Message };
            }
        }
    }
}
