using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.ViewModels.Property;
using WebApp.RealEstateApp.Middlewares;
using Microsoft.AspNetCore.Authorization;

namespace RealEstateApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly ITypeOfPropertyService _typeOfPropertyService;
        //private readonly AuthenticationResponse _currentUser;

        public HomeController(IPropertyService propertyService, ITypeOfPropertyService typeOfPropertyService)
        {
            _propertyService = propertyService;
            _typeOfPropertyService = typeOfPropertyService;
            //_currentUser = HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        [Authorize(Policy = "ExcludeAdmin&Agent")]
        public async Task<IActionResult> Index(FilterPropertyViewModel vm)
        {
            ViewBag.TypeOfProperty = await _typeOfPropertyService.GetAllViewModel();
            return View(await _propertyService.GetAllViewModelWithFilter(vm));
        }

        public async Task<IActionResult> TooglePropertyFavStatus(string code)
        {
            var currentUser = HttpContext.Session.Get<AuthenticationResponse>("user");
            await _propertyService.ToogleFavStatus(currentUser.Id, code);

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
    }
}

