using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.User;

namespace RealEstateApp.Controllers
{
    public class AgentController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;

        public AgentController(IUserService userService, IPropertyService propertyService)
        {
            _userService = userService;
            _propertyService = propertyService;
        }

        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> Index(string id)
        {
            var properties = await _propertyService.GetAllByUserId(id);
            return View(properties);
        }

        [Authorize(Policy = "ExcludeAdmin&Agent")]
        public async Task<IActionResult> Agents(string firstName = null)
        {
            var agents = await _userService.GetAllAgentsWeb();
            agents = agents.Where(agent => agent.IsActive).OrderBy(agent => agent.FirstName)
                   .ThenBy(agent => agent.LastName)
                   .ToList();

            if (!string.IsNullOrEmpty(firstName))
            {
                firstName = firstName.ToLower();

                agents = agents.Where(agent => agent.FirstName.ToLower().Contains(firstName) ||
                                               agent.LastName.ToLower().Contains(firstName))
                               .ToList();
            }

            return View(agents);
        }

        public async Task<IActionResult> Info(string id)
        {
            var user = await _userService.GetUserById(id);
            var properties = await _propertyService.GetAllByUserId(id);

            var agentInfoViewModel = new AgentInfoViewModel
            {
                User = user,
                Properties = properties
            };

            return View(agentInfoViewModel);
        }

        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> Edit(string id)
        {
            SaveUserViewModel vm = await _userService.GetSaveUserById(id);
            return View("Edit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveUserViewModel vm)
        {
            SaveUserViewModel UserVm = await _userService.GetSaveUserById(vm.Id);

            if (!ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(vm.Password) && string.IsNullOrEmpty(vm.ConfirmPassword))
                {
                    vm.Password = UserVm.Password;
                    vm.ConfirmPassword = UserVm.ConfirmPassword;
                    vm.Role = "Agent";
                    vm.IdentificationCard = UserVm.IdentificationCard;
                    ModelState.Remove("Password");
                    ModelState.Remove("ConfirmPassword");
                    ModelState.Remove("Role");
                    ModelState.Remove("IdentificationCard");
                    ModelState.Remove("File");
                }

            }
            if (!ModelState.IsValid)
            {
                return View("Edit", vm);
            }

            string imagePath = string.IsNullOrEmpty(UserVm.ImageUrl) ? "" : UserVm.ImageUrl;
            vm.ImageUrl = UploadFile(vm.File, vm.Email, true, imagePath);
            vm.IsActive = true;
            await _userService.UpdateUserAsync(vm.Id, vm);

            return RedirectToRoute(new { controller = "Agent", action = "Profile", id = UserVm.Id });
        }

        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> Profile(string id)
        {
            UserViewModel vm = await _userService.GetUserById(id);
            return View(vm);
        }

        private string UploadFile(IFormFile file, string Email, bool isEditMode = false, string imagePath = "")
        {
            if (isEditMode)
            {
                if (file == null)
                {
                    return imagePath;
                }
            }
            string basePath = $"/Images/Users/{Email}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            //create folder if not exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //get file extension
            Guid guid = Guid.NewGuid();
            FileInfo fileInfo = new(file.FileName);
            string fileName = guid + fileInfo.Extension;

            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (isEditMode)
            {
                string[] oldImagePart = imagePath.Split("/");
                string oldImagePath = oldImagePart[^1];
                string completeImageOldPath = Path.Combine(path, oldImagePath);

                if (System.IO.File.Exists(completeImageOldPath))
                {
                    System.IO.File.Delete(completeImageOldPath);
                }
            }
            return $"{basePath}/{fileName}";
        }


    }
}
