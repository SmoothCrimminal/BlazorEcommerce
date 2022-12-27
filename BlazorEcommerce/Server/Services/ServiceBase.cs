using BlazorEcommerce.Server.Data;

namespace BlazorEcommerce.Server.Services
{
    public abstract class ServiceBase
    {
        internal readonly DataContext _dbContext;
        public ServiceBase(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
