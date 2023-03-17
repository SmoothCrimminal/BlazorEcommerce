

namespace BlazorEcommerce.Client.Services.Address
{
    public interface IAddressService
    {
        Task<BlazorEcommerce.Shared.Address> GetAddress();
        Task<BlazorEcommerce.Shared.Address> AddOrUpdateAddress(BlazorEcommerce.Shared.Address address);
    }
}
