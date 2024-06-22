using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.DTOs.Account
{
    public class AuthenticationRequest
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
