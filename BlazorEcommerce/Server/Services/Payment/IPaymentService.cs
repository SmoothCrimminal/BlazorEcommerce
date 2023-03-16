using Stripe.Checkout;

namespace BlazorEcommerce.Server.Services.Payment
{
    public interface IPaymentService
    {
        Task<Session> CreateCheckoutSession();
        Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request);
    }
}
