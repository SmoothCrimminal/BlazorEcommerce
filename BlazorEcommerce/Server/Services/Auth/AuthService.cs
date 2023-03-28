using BlazorEcommerce.Server.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BlazorEcommerce.Server.Services.Auth
{
    public class AuthService : ServiceBase, IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(DataContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
            _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>();

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user is null)
            {
                response.IsSuccess = false;
                response.Message = "User not found";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.IsSuccess = false;
                response.Message = "Wrong password";
            }
            else
            {
                var jwt = CreateToken(user);
                if (string.IsNullOrWhiteSpace(jwt))
                {
                    response.Message = "Could not create token";
                    response.IsSuccess = false;
                    return response;
                }

                response.Data = jwt;
            }

            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var userExists = await UserExists(user.Email);
            if (userExists)
                return new ServiceResponse<int> { IsSuccess = false, Message = "User already exists." };

            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return new ServiceResponse<int> { Data = user.Id, Message = "Registration successful!" };
        }

        public async Task<bool> UserExists(string email)
        {
            var userExists = await _dbContext.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
            if (userExists)
                return true;

            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = _configuration.GetSection("Authentication").GetValue<string>("JwtKey");
            if (string.IsNullOrWhiteSpace(token))
                return string.Empty;

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(token));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var jwtToken = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return jwt;
        }

        public async Task<ServiceResponse<bool>> ChangePassword(int userId, string password)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user is null)
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Message = "User not found"
                };

            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _dbContext.SaveChangesAsync();

            return new ServiceResponse<bool>
            {
                Data = true,
                Message = "Password has been changed"
            };
        }

        public Task<User> GetUserByEmail(string email) => _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}
