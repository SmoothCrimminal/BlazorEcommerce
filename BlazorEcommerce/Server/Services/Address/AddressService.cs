using BlazorEcommerce.Client.Services.Auth;
using BlazorEcommerce.Server.Data;

namespace BlazorEcommerce.Server.Services.Address
{
    public class AddressService : ServiceBase, IAddressService
    {
        public AddressService(DataContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }

        public async Task<ServiceResponse<Shared.Address>> AddOrUpdate(Shared.Address address)
        {
            var response = new ServiceResponse<Shared.Address>();
            var dbAddress = (await GetAddress()).Data;
            if (dbAddress is null)
            {
                address.UserId = GetUserId();
                _dbContext.Addresses.Add(address);
                response.Data = address;
            }
            else
            {
                dbAddress.FirstName = address.FirstName;
                dbAddress.LastName = address.LastName;
                dbAddress.State = address.State;
                dbAddress.Country = address.Country;
                dbAddress.City = address.City;
                dbAddress.Zip = address.Zip;
                dbAddress.Street = address.Street;
                response.Data = dbAddress;
            }

            await _dbContext.SaveChangesAsync();

            return response;
        }

        public async Task<ServiceResponse<Shared.Address>> GetAddress()
        {
            int userId = GetUserId();
            var address = await _dbContext.Addresses.FirstOrDefaultAsync(x => x.UserId == userId);

            return new ServiceResponse<Shared.Address> { Data = address };
        }
    }
}
