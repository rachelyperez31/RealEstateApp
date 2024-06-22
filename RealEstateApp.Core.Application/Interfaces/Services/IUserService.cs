using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Application.ViewModels.User;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IUserService 
    {
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<AuthenticationResponse> LoginAsync(LoginViewModel vm);
        Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin);
        Task SignOutAsync();
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordViewModel vm, string origin);
        Task<bool> DeleteUserAsync(string userId);
        Task<int> GetActiveAgents();
        Task<int> GetActiveClients();
        Task<int> GetActiveDevelopers();
        Task<int> GetInactiveAgents();
        Task<int> GetInactiveClients();
        Task<int> GetInactiveDevelopers();
        Task<List<UserViewModel>> GetAll();
        Task<List<UserViewModel>> GetAllAgents();
        Task<List<UserViewModel>> GetAllAgentsWeb();
        Task<List<UserViewModel>> GetAllAdmins();
        Task<List<UserViewModel>> GetAllDevelopers();
        Task<UserViewModel> GetUserById(string id);
        Task<SaveUserViewModel> GetSaveUserById(string id);
        Task<UserViewModel> GetUserByUsernameAsync(string username);
        Task<bool> UpdateUserAsync(string id, SaveUserViewModel user);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordViewModel vm);

        public Task<SaveUserViewModel> GetUserByIdSaveAsync(string userId);
        public Task<SaveUserViewModel> GetUserByEmailSaveAsync(string email);
        public Task<UserViewModel> GetUserByIdAsync(string userId);
        public Task<UserViewModel> GetUserByEmailAsync(string email);
        
    }
}
