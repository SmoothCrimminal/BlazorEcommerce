using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Server.Services.Address
{
    public interface IAddressService
    {
        Task<ServiceResponse<Shared.Address>> GetAddress();
        Task<ServiceResponse<Shared.Address>> AddOrUpdate(Shared.Address address);
    }
}
