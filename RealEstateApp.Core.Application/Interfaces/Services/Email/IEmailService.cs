using RealEstateApp.Core.Application.DTOs.Email;

namespace RealEstateApp.Core.Application.Interfaces.Services.Email
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
