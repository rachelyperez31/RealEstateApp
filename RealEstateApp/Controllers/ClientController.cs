using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.ViewModels.Property;
using Microsoft.AspNetCore.Authorization;

namespace RealEstateApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly IPropertyService _propertyService;

        public ClientController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> ClientFavProperties()
        {
            var client = HttpContext.Session.Get<AuthenticationResponse>("user");
            var favProperties = await _propertyService.GetFavProperties(client.Id);

            return View(favProperties);
        }


    }
}
