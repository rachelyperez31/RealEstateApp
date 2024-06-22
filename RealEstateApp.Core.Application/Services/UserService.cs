using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Interfaces.Services.Account;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Application.ViewModels.User;

namespace RealEstateApp.Core.Application.Services.Generic
{
    public class UserService : IUserService
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly AuthenticationResponse userViewModel;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPropertyService _propertyService;

        public UserService(IAccountService accountService, IMapper mapper, IHttpContextAccessor httpContextAccessor, IPropertyService propertyService)
        {
            _accountService = accountService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _propertyService = propertyService;
        }

        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            return await _accountService.ConfirmAccountAsync(userId, token);
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginViewModel vm)
        {
            AuthenticationRequest loginRequest = _mapper.Map<AuthenticationRequest>(vm);
            AuthenticationResponse userResponse = await _accountService.AuthenticateWebAppAsync(loginRequest);
            return userResponse;
        }
        public async Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin)
        {
            RegisterRequest registerRequest = _mapper.Map<RegisterRequest>(vm);
            return await _accountService.RegisterUserAsync(registerRequest, origin, vm.Role);
        }


        public async Task SignOutAsync()
        {
            await _accountService.SignOutAsync();
        }

        public async Task<UserViewModel> GetUserById(string id)
        {
            var user = await _accountService.GetUserById(id);
            return _mapper.Map<UserViewModel>(user);
        }
        public async Task<SaveUserViewModel> GetSaveUserById(string id)
        {
            var user = await _accountService.GetUserById(id);
            return _mapper.Map<SaveUserViewModel>(user);
        }
        
        public async Task<List<UserViewModel>> GetAll()
        {
            var users = await _accountService.GetAllUsersAsync();
            return _mapper.Map<List<UserViewModel>>(users);
        }

        public async Task<bool> UpdateUserAsync(string id, SaveUserViewModel user)
        {
            var updateUserResponse = await _accountService.UpdateProfileAsync(id, user);
            return updateUserResponse;
        }

        public async Task<UserViewModel> GetUserByUsernameAsync(string username)
        {
            return await _accountService.GetUserByUsernameAsync(username);
        }

        // Listado de Agentes

        public async Task<bool> DeleteUserAsync(string userId)
        {

            var agentProperties = await _propertyService.GetAgentProperties(userId);

            foreach (var property in agentProperties)
            {
                await _propertyService.Delete(property.Id);
            }

            return await _accountService.DeleteUserAsync(userId);
        }

        public async Task<List<UserViewModel>> GetAllAgents()
        {
            var users = await _accountService.GetAllUsersAsync();
            var agents = users.Where(x => x.Role.Contains(Roles.Agent.ToString())).ToList();
            return _mapper.Map<List<UserViewModel>>(agents);
        }

        public async Task<List<UserViewModel>> GetAllAgentsWeb()
        {
            var users = await _accountService.GetAllUsersWebAsync();
            var agents = users.Where(x => x.Role.Contains(Roles.Agent.ToString())).ToList();
            return _mapper.Map<List<UserViewModel>>(agents);
        }


        // Home 

        public async Task<int> GetActiveAgents()
        {
            var users = await _accountService.GetAllUsersAsync();
            var activeAgents = users.Where(x => x.Role.Contains(Roles.Agent.ToString()) && x.IsActive).ToList();
            return activeAgents.Count();
        }

        public async Task<int> GetInactiveAgents()
        {
            var users = await _accountService.GetAllUsersAsync();
            var inactiveAgents = users.Where(x => x.Role.Contains(Roles.Agent.ToString()) && !x.IsActive).ToList();
            return inactiveAgents.Count();
        }

        public async Task<int> GetActiveClients()
        {
            var users = await _accountService.GetAllUsersAsync();
            var activeClients = users.Where(x => x.Role.Contains(Roles.Client.ToString()) && x.IsActive).ToList();
            return activeClients.Count();
        }

        public async Task<int> GetInactiveClients()
        {
            var users = await _accountService.GetAllUsersAsync();
            var inactiveClients = users.Where(x => x.Role.Contains(Roles.Client.ToString()) && !x.IsActive).ToList();
            return inactiveClients.Count();
        }

        public async Task<int> GetActiveDevelopers()
        {
            var users = await _accountService.GetAllUsersAsync();
            var activeDevelopers = users.Where(x => x.Role.Contains(Roles.Developer.ToString()) && x.IsActive).ToList();
            return activeDevelopers.Count();
        }

        public async Task<int> GetInactiveDevelopers()
        {
            var users = await _accountService.GetAllUsersAsync();
            var inactiveDevelopers = users.Where(x => x.Role.Contains(Roles.Developer.ToString()) && !x.IsActive).ToList();
            return inactiveDevelopers.Count();
        }

        // Mantenimiento de Administradores

        public async Task<List<UserViewModel>> GetAllAdmins()
        {
            var users = await _accountService.GetAllUsersAsync();
            var agents = users.Where(x => x.Role.Contains(Roles.Admin.ToString())).ToList();
            return _mapper.Map<List<UserViewModel>>(agents);
        }


        // Mantenimiento de Desarrolladores

        public async Task<List<UserViewModel>> GetAllDevelopers()
        {
            var users = await _accountService.GetAllUsersAsync();
            var agents = users.Where(x => x.Role.Contains(Roles.Developer.ToString())).ToList();
            return _mapper.Map<List<UserViewModel>>(agents);
        }

        public async Task<SaveUserViewModel> GetUserByIdSaveAsync(string userId)
        {
            return await _accountService.GetUserByIdSaveAsync(userId);
        }
        public async Task<UserViewModel> GetUserByEmailAsync(string email)
        {
            return await _accountService.GetUserEmailIdAsync(email);
        }
        public async Task<SaveUserViewModel> GetUserByEmailSaveAsync(string email)
        {
            return await _accountService.GetUserEmailIdSaveAsync(email);
        }
        public async Task<UserViewModel> GetUserByIdAsync(string userId)
        {
            return await _accountService.GetUserByIdAsync(userId);
        }
        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordViewModel vm)
        {
            ResetPasswordRequest resetRequest = _mapper.Map<ResetPasswordRequest>(vm);
            return await _accountService.ResetPasswordAsync(resetRequest);
        }
        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordViewModel vm, string origin)
        {
            ForgotPasswordRequest forgotRequest = _mapper.Map<ForgotPasswordRequest>(vm);
            return await _accountService.ForgotPasswordAsync(forgotRequest, origin);
        }
    }
}
