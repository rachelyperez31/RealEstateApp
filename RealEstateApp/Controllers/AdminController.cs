using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Improvement;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperty;
using RealEstateApp.Core.Application.ViewModels.TypeOfSale;
using RealEstateApp.Core.Application.ViewModels.User;
using RealEstateApp.Core.Application.Helpers;

namespace RealEstateApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private readonly IUserService _userService;
        private readonly ITypeOfPropertyService _typeOfPropertyService;
        private readonly ITypeOfSaleService _typeOfSaleService;
        private readonly IImprovementService _improvementService;

        public AdminController(IUserService userService, ITypeOfPropertyService typeOfPropertyService, 
                        ITypeOfSaleService typeOfSaleService, IImprovementService improvementService)
        {
            _userService = userService;
            _typeOfPropertyService = typeOfPropertyService;
            _typeOfSaleService = typeOfSaleService;
            _improvementService = improvementService;
        }

        #region General
        public IActionResult Home()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveStatus(string id, string role)
        {
            SaveUserViewModel vm = await _userService.GetSaveUserById(id);

            vm.IsActive = !vm.IsActive;

            await _userService.UpdateUserAsync(vm.Id, vm);

            if (role == Roles.Admin.ToString())
            {
                return RedirectToAction("UsersMaintenance", "Admin", new { role = "Admin" });
            }
            else if (role == Roles.Developer.ToString())
            {
                return RedirectToAction("UsersMaintenance", "Admin", new { role = "Developer" });
            }
            else
            {
                return RedirectToAction("Agents", "Admin");
            }
        }
        #endregion


        #region Listado de Agentes
        public async Task<IActionResult> Agents()
        {
            return View(await _userService.GetAllAgents());
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAgent(string id)
        {

            await _userService.DeleteUserAsync(id);

            return RedirectToRoute(new { controller = "Admin", action = "Agents" });
        }
        #endregion


        #region Mantenimiento de Administradores Y Desarrolladores
        // Mantenimiento de Administradores Y Desarrolladores

        public async Task<IActionResult> UsersMaintenance(string role)
        {
            ViewData["Title"] = role == Roles.Admin.ToString() ? "Mantenimiento de Administradores" : "Mantenimiento de Desarrolladores";
            List<UserViewModel> users = role == Roles.Admin.ToString() ? await _userService.GetAllAdmins() : await _userService.GetAllDevelopers();
            ViewBag.Role = role;
            return View("Maintenances", users);
        }

        public IActionResult CreateUser(string role)
        {
            SaveUserViewModel userVm = new()
            {
                Role = role
            };

            return View(userVm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var origin = Request.Headers["origin"];
            RegisterResponse response = await _userService.RegisterAsync(vm, origin);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }
            TempData["UserSucceed"] = "Usuario agregado exitosamente.";
            //return RedirectToRoute(new { controller = "Admin", action = "Home" });;
            return RedirectToAction("UsersMaintenance", new { role = vm.Role });
        }

        public async Task<IActionResult> EditUser(string id, string role)
        {
            SaveUserViewModel vm = await _userService.GetSaveUserById(id);
            vm.Role = role;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(SaveUserViewModel vm)
        {
            SaveUserViewModel UserVm = await _userService.GetSaveUserById(vm.Id);

            if (!ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(vm.Password) && string.IsNullOrEmpty(vm.ConfirmPassword))
                {
                    vm.Password = UserVm.Password;
                    vm.ConfirmPassword = UserVm.ConfirmPassword;
                    ModelState.Remove("Password");
                    ModelState.Remove("ConfirmPassword");
                }
                if (string.IsNullOrEmpty(vm.PhoneNumber))
                {
                    vm.PhoneNumber = UserVm.PhoneNumber;
                    ModelState.Remove("PhoneNumber");
                }

            }
            if (!ModelState.IsValid)
            {
                return View("EditUser", vm);
            }

            vm.IsActive = UserVm.IsActive;
            await _userService.UpdateUserAsync(vm.Id, vm);
            TempData["UserSucceed"] = "Usuario editado exitosamente.";
            return RedirectToAction("UsersMaintenance", new { role = vm.Role });
        }
        #endregion


        #region Mantenimiento de Tipos de Propiedad
        // Mantenimiento de Tipos de Propiedad (ToP - Type of Property)
        public async Task<IActionResult> TypeOfProperties()
        {
            return View(await _typeOfPropertyService.GetAllViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateToP(SaveTypeOfPropertyViewModel vm)
        {

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Ocurrió un error agregando el tipo de propiedad.";
                return RedirectToAction("TypeOfProperties");
            }

            await _typeOfPropertyService.Add(vm);
            TempData["ToPSucceed"] = "Tipo de Propiedad agregado exitosamente.";
            return RedirectToAction("TypeOfProperties");
        }

        [HttpPost]
        public async Task<IActionResult> EditToP(SaveTypeOfPropertyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Ocurrió un error editando el tipo de propiedad.";
                return RedirectToAction("TypeOfProperties");
            }

            await _typeOfPropertyService.Update(vm, vm.Id);
            TempData["ToPSucceed"] = "Tipo de Propiedad editado exitosamente.";
            return RedirectToAction("TypeOfProperties");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteToP(int id)
        {

            await _typeOfPropertyService.Delete(id);
            TempData["ToPSucceed"] = "Tipo de Propiedad eliminado exitosamente.";
            return RedirectToAction("TypeOfProperties");
        }
        #endregion


        #region Mantenimiento de Tipos de Ventas
        // Mantenimiento de Tipos de Ventas (ToS - Type of Sale)
        public async Task<IActionResult> TypeOfSales()
        {
            return View(await _typeOfSaleService.GetAllViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateToS(SaveTypeOfSaleViewModel vm)
        {

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Ocurrió un error agregando el tipo de venta.";
                return RedirectToAction("TypeOfSales");
            }

            await _typeOfSaleService.Add(vm);
            TempData["ToSSucceed"] = "Tipo de Venta agregado exitosamente.";
            return RedirectToAction("TypeOfSales");
        }


        [HttpPost]
        public async Task<IActionResult> EditToS(SaveTypeOfSaleViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Ocurrió un error editando el tipo de venta.";
                return RedirectToAction("TypeOfSales");
            }

            await _typeOfSaleService.Update(vm, vm.Id);
            TempData["ToSSucceed"] = "Tipo de Venta editado exitosamente.";
            return RedirectToAction("TypeOfSales");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteToS(int id)
        {

            await _typeOfSaleService.Delete(id);
            TempData["ToSSucceed"] = "Tipo de Venta eliminado exitosamente.";
            return RedirectToAction("TypeOfSales");
        }

        #endregion


        #region Mantenimiento de Mejoras
        // Mantenimiento de Mejoras (Improvements)
        public async Task<IActionResult> Improvements()
        {
            return View(await _improvementService.GetAllViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateImprovement(SaveImprovementViewModel vm)
        {

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Ocurrió un error agregando la mejora.";
                return RedirectToAction("Improvements");
            }

            await _improvementService.Add(vm);
            TempData["ImprovementSucceed"] = "Mejora agregada exitosamente.";
            return RedirectToAction("Improvements");
        }


        [HttpPost]
        public async Task<IActionResult> EditImprovement(SaveImprovementViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Ocurrió un error editando la mejora.";
                return RedirectToAction("Improvements");
            }

            await _improvementService.Update(vm, vm.Id);
            TempData["ImprovementSucceed"] = "Mejora editada exitosamente.";
            return RedirectToAction("Improvements");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImprovement(int id)
        {

            await _improvementService.Delete(id);
            TempData["ImprovementSucceed"] = "Mejora eliminada exitosamente.";
            return RedirectToAction("Improvements");
        }
        #endregion
    }
}
