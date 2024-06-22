using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.ViewModels.User;

namespace RealEstateApp.Core.Application.Interfaces.Services.Account
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateWebApiAsync(AuthenticationRequest request);
        Task<AuthenticationResponse> AuthenticateWebAppAsync(AuthenticationRequest request);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<List<UserViewModel>> GetAllUsersWebAsync();
        Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, string? origin = null, string? role = null);
        Task SignOutAsync();
        Task ToogleUserActiveStatusAsync(string id, bool status);
        Task<string> ConfirmAccountAsync(string userId, string token);
        Task<UserViewModel> GetUserByUsernameAsync(string username);
        Task<UserViewModel> GetUserById(string id);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> UpdateProfileAsync(string id, SaveUserViewModel profile);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
        public Task<UserViewModel> GetUserByIdAsync(string userId);
        public Task<UserViewModel> GetUserEmailIdAsync(string email);
        public Task<SaveUserViewModel> GetUserByIdSaveAsync(string userId);
        public Task<SaveUserViewModel> GetUserEmailIdSaveAsync(string email);


    }
}
