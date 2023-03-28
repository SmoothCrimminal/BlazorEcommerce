using BlazorEcommerce.Server.Data;
using System.Security.Claims;

namespace BlazorEcommerce.Server.Services
{
    public abstract class ServiceBase
    {
        internal readonly DataContext _dbContext;
        internal readonly IHttpContextAccessor _httpContextAccessor;

        public ServiceBase(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        protected int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        protected string GetUserEmailAddress() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
    }
}
