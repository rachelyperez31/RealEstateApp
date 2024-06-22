using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Helpers;

namespace RealEstateApp.Middlewares
{
    public class ValidateUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidateUserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public AuthenticationResponse GetUser()
        {
            return _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public bool HasUser()
        {
            return GetUser() != null;
        }
    }
}
