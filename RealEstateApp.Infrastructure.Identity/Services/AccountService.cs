using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.DTOs.Email;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Services.Account;
using RealEstateApp.Core.Application.Interfaces.Services.Email;
using RealEstateApp.Core.Application.ViewModels.User;
using RealEstateApp.Core.Domain.Settings;
using RealEstateApp.Infrastructure.Identity.Entities;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RealEstateApp.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly JWTSettings _jwtSettings;

        public AccountService
                            (
                            UserManager<ApplicationUser> userManager, 
                            SignInManager<ApplicationUser> signInManager,
                            IEmailService emailService,
                            IOptions<JWTSettings> jwtSettings
                            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthenticationResponse> AuthenticateWebApiAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No existe ningún usuario:'{request.UserName}'";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Clave incorrecta del el usuario :'{request.UserName}'";
                return response;
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);

            response.Id = user.Id;
            response.Email = user.Email;
            response.UserName = user.UserName;
            response.IsActive = user.IsActive;

            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            if (!roles.Any(role => role == Roles.Admin.ToString() || role == Roles.Developer.ToString() ))
            {
                response.HasError = true;
                throw new ApiException($"El usuario no tiene los roles necesarios para acceder a la API", (int)HttpStatusCode.Unauthorized);
                //
                //response.Error = $"El usuario no tiene los roles necesarios para acceder a la API";
                //return response;
            }

            response.Roles = roles.ToList();
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var refreshToken = GenerateRefreshToken();
            response.RefreshToken = refreshToken.Token;

            return response;
        }

        public async Task<AuthenticationResponse> AuthenticateWebAppAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No hay cuentas registradas con '{request.UserName}'";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);
            var roleList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            if (!user.IsActive)
            {
                if (roleList.Any(role => role == Roles.Client.ToString()))
                {
                    response.HasError = true;
                    response.Error = $"Cuenta no activada para '{request.UserName}', Revise su Correo.";
                    return response;
                }
                response.HasError = true;
                response.Error = $"Cuenta no activada para '{request.UserName}', comuníquese con el administrador.";
                return response;
            }

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Credenciales incorrectas.";
                return response;
            }

            

            if (!roleList.Any(role => role == Roles.Admin.ToString() || role == Roles.Client.ToString() || role == Roles.Agent.ToString()))
            {
                response.HasError = true;
                response.Error = $"El usuario no tiene los roles necesarios para acceder a la aplicación web";
                return response;
            }

            response.Id = user.Id;
            response.UserName = user.UserName;
            response.IsActive = user.IsActive;
            response.Roles = roleList.ToList();

            return response;
        }

        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return $"No hay cuentas registradas con este usuario";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                user.IsActive = true;
                _userManager.UpdateAsync(user);
                return $"Cuenta confirmada para {user.Email}. Bienvenido a RealEstateApp.";
                
            }
            else
            {
                return $"Ha ocurrido un error durante la confirmación de {user.Email}.";
            }
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return false;
            }

            return true;
        }
        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            ForgotPasswordResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Accounts registered with {request.Email}";
                return response;
            }

            var verificationUri = await SendForgotPasswordUri(user, origin);

            await _emailService.SendAsync(new Core.Application.DTOs.Email.EmailRequest()
            {
                To = user.Email,
                Body = $"Please reset your account visiting this URL {verificationUri}",
                Subject = "reset password"
            });


            return response;
        }
        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ResetPasswordResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Accounts registered with {request.Email}";
                return response;
            }

            request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"An error occurred while reset password";
                return response;
            }

            return response;
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDTOs = new List<UserDTO?>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userDTOs.Add(new UserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Password = user.PasswordHash,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IdentificationCard = user.IdentificationCard,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Role = roles.ToList(),
                    IsActive = user.IsActive
                });
            }

            return userDTOs;
        }
        public async Task<List<UserViewModel>> GetAllUsersWebAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDTOs = new List<UserViewModel?>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userDTOs.Add(new UserViewModel
                {
                    Id = user.Id,
                    Username = user.UserName,
                    ImageUrl = user.ImageUrl,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IdentificationCard = user.IdentificationCard,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Role = roles.ToList(),
                    IsActive = user.IsActive
                });
            }

            return userDTOs;
        }

        public async Task<UserViewModel> GetUserById(string id)
         {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IdentificationCard = user.IdentificationCard,
                Username = user.UserName,
                IsActive = user.IsActive,
                ImageUrl = user.ImageUrl
            };

            return userViewModel;
        }

        public async Task<UserViewModel> GetUserByUsernameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return null;
            }

            var userViewModel = new UserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IdentificationCard = user.IdentificationCard,
                Username = user.UserName,
            };

            return userViewModel;
        }

        public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, string? origin, string? role)
        {
            RegisterResponse response = new RegisterResponse()
            {
                HasError = false
            }; 

            var userWithUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithUserName != null)
            {
                response.HasError = true;
                response.Error = $"El nombre de usuario '{request.UserName}' ya está registrado.";
                return response;
            }

            var userWithEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithEmail != null)
            {
                response.HasError = true;
                response.Error = $"El email '{request.Email}' ya está registrado";
                return response;
            }

            if (!IsPasswordValid(request.Password))
            {
                response.HasError = true;
                response.Error = "La contraseña proporcionada no cumple con los requisitos de seguridad.";
                return response;
            }

            var user = new ApplicationUser
            {
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                IdentificationCard = request.IdentificationCard,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                IsActive = false,
                ImageUrl = request.ImageUrl ?? null,
                
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                if (role == Roles.Client.ToString())
                {
                    await _userManager.AddToRoleAsync(user, Roles.Client.ToString());
                    var confirmation = await SendVerificationEmailUri(user, origin);
                    await _emailService.SendAsync(new EmailRequest()
                    {
                        To = user.Email,
                        Body = $"Bienvenido a RealEstateApp. Por favor, confirme su cuenta haciendo click en este link {confirmation}",
                        Subject = "Confirmación de Registro"
                    });

                }
                else if (role == Roles.Agent.ToString())
                {
                    await _userManager.AddToRoleAsync(user, Roles.Agent.ToString());
                }
                else if (role == Roles.Developer.ToString())
                {
                    user.IsActive = true;
                    user.EmailConfirmed = true;
                    await _userManager.AddToRoleAsync(user, Roles.Developer.ToString());
                }
                else if (role == Roles.Admin.ToString())
                {
                    user.IsActive = true;
                    user.EmailConfirmed = true;
                    await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }

                await _userManager.UpdateAsync(user);

            }
            else
            {
                response.HasError = true;
                response.Error = "Ha ocurrido un error al momento del registro.";
                return response;
            }

            return response;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task ToogleUserActiveStatusAsync(string id, bool status)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
               user.IsActive = status;
            }
            await _userManager.UpdateAsync(user);
        }

        public async Task<bool> UpdateProfileAsync(string id, SaveUserViewModel profile)
        {
            var user = await _userManager.FindByIdAsync(id); 

            if (user == null)
            {
                throw new ApplicationException($"No se pudo encontrar el usuario con ID {id}.");
            }

            user.UserName = profile.Username;
            user.Email = profile.Email;
            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
            user.PhoneNumber = profile.PhoneNumber;
            user.IdentificationCard = profile.IdentificationCard;
            user.ImageUrl = profile.ImageUrl;
            user.IsActive = profile.IsActive;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return false;
            }

            return true;
        }
        public async Task<UserViewModel> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"No se pudo encontrar el usuario con ID {userId}.");
            }
            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                ImageUrl = user.ImageUrl,
                FirstName = user.FirstName,
            };
            return userViewModel;
        }
        public async Task<SaveUserViewModel> GetUserByIdSaveAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"No se pudo encontrar el usuario con ID {userId}.");
            }
            var userViewModel = new SaveUserViewModel
            {
                Username = user.UserName,
                LastName = user.LastName,
                ImageUrl = user.ImageUrl,
                FirstName = user.FirstName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
            return userViewModel;
        }
        public async Task<UserViewModel> GetUserEmailIdAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new ApplicationException($"No se pudo encontrar el usuario con email {email}.");
            }
            var userViewModel = new UserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageUrl = user.ImageUrl,
                Username = user.UserName,
                Email = user.Email,
            };
            return userViewModel;
        }

        public async Task<SaveUserViewModel> GetUserEmailIdSaveAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new ApplicationException($"No se pudo encontrar el usuario con email {email}.");
            }

            var userViewModel = new SaveUserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageUrl = user.ImageUrl,
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IdentificationCard = user.IdentificationCard,
                Id = user.Id

            };

            return userViewModel;
        }

        #region "Private Methods"
        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(4),
                Created = DateTime.UtcNow
            };
        }
        private async Task<string> SendForgotPasswordUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ResetPassword";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "token", code);

            return verificationUri;
        }
        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName)
                ,new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                ,new Claim(JwtRegisteredClaimNames.Email,user.Email)
                ,new Claim("uid",user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecToken;
        }

        private bool IsPasswordValid(string password)
        {
            PasswordOptions opts = new PasswordOptions()
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            if (string.IsNullOrEmpty(password) || password.Length < opts.RequiredLength)
                return false;

            int uniqueChars = password.Distinct().Count();

            if (uniqueChars < opts.RequiredUniqueChars)
                return false;

            if (opts.RequireDigit && !password.Any(char.IsDigit))
                return false;

            if (opts.RequireLowercase && !password.Any(char.IsLower))
                return false;

            if (opts.RequireUppercase && !password.Any(char.IsUpper))
                return false;

            if (opts.RequireNonAlphanumeric && password.All(char.IsLetterOrDigit))
                return false;

            return true;
        }

        private async Task<string> SendVerificationEmailUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ConfirmEmail";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "token", code);

            return verificationUri;
        }
        #endregion
    }
}
