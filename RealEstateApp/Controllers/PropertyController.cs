using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Image;
using RealEstateApp.Core.Application.ViewModels.Property;

namespace RealEstateApp.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IPropertyService _propertyService;
        //private readonly AuthenticationResponse _currentUser;
        private readonly ITypeOfPropertyService _typeOfPropertyService;
        private readonly ITypeOfSaleService _typeOfSaleService;
        private readonly IImprovementService _improvementService;
        private readonly IImageService _imageService;
        private readonly IUserService _userService;


        public PropertyController(IImageService imageService, IPropertyService propertyService, ITypeOfPropertyService typeOfPropertyService, ITypeOfSaleService typeOfSaleService, IImprovementService improvementService, IUserService userService)
        {
            _imageService = imageService;
            _propertyService = propertyService;
            _typeOfPropertyService = typeOfPropertyService;
            _typeOfSaleService = typeOfSaleService;
            _improvementService = improvementService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> MyProperties(string id)
        {
            var Properties = await _propertyService.GetAgentProperties(id);

            return View(Properties);
        }

        [Authorize(Policy = "ExcludeAdmin")]
        public async Task<IActionResult> Info(int id)
        {

            var property = await _propertyService.GetPropertyByIdWithIncludes(id);
            var agent = await _userService.GetUserById(property.AgentId);

            var propiedadagentInfo = new PropertyAgentViewModel
            {
                Agent = agent,
                Propiedad = property
            };

            return View(propiedadagentInfo);
        }

        #region CRUD Agente
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> Create()
        {
            SavePropertyViewModel vm = new();
            vm.TypeOfPropertys = await _typeOfPropertyService.GetAllViewModel();
            vm.TypeOfSales = await _typeOfSaleService.GetAllViewModel();
            vm.Improvements = await _improvementService.GetAllViewModel();

            return View("SaveProperty", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SavePropertyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.TypeOfPropertys = await _typeOfPropertyService.GetAllViewModel();
                vm.TypeOfSales = await _typeOfSaleService.GetAllViewModel();
                vm.Improvements = await _improvementService.GetAllViewModel();
                return View("SaveProperty", vm);
            }
            //var origin = Request.Headers["origin"];

            SavePropertyViewModel property = await _propertyService.Add(vm);

            foreach (var improvementId in vm.ImprovementsId)
            {
                await _propertyService.AddPropertyImprovements((int)property.Id, improvementId);
            }

            if (property.Id.HasValue)
            {
                property.ImgUrl = UploadFile(vm.File, property.Id.ToString());
                await _propertyService.Update(property, (int)property.Id);

                {
                    SaveImageViewModel saveImage = new SaveImageViewModel();

                    saveImage.ImageUrl = property.ImgUrl;
                    saveImage.PropertyId = (int)property.Id;
                    saveImage.UserId = HttpContext.Session.Get<AuthenticationResponse>("user").Id;

                    await _imageService.Add(saveImage);

                    if (vm.File2 != null)
                    {
                        saveImage.ImageUrl = UploadFile(vm.File2, property.Id.ToString());
                        await _imageService.Add(saveImage);
                    }
                    if (vm.File3 != null)
                    {
                        saveImage.ImageUrl = UploadFile(vm.File3, property.Id.ToString());
                        await _imageService.Add(saveImage);
                    }
                    if (vm.File4 != null)
                    {
                        saveImage.ImageUrl = UploadFile(vm.File4, property.Id.ToString());
                        await _imageService.Add(saveImage);
                    }
                }
            }


            return RedirectToRoute(new { controller = "Property", action = "MyProperties", id = property.AgentId });

        }

        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> Edit(int id)
        {
            SavePropertyViewModel vm = await _propertyService.GetByIdSaveViewModel(id);
            List<int> previousImprovementIds = vm.ImprovementsId.ToList();
            vm.TypeOfPropertys = await _typeOfPropertyService.GetAllViewModel();
            vm.TypeOfSales = await _typeOfSaleService.GetAllViewModel();
            vm.Improvements = await _improvementService.GetAllViewModel();

            ViewBag.PreviousImprovementsIds = previousImprovementIds;
            ViewBag.PreviousImages = await _imageService.GetImagesForPropertyId(id);

            return View("SaveProperty", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePropertyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm = await _propertyService.GetByIdSaveViewModel((int)vm.Id);
                List<int> previousImprovementIds = vm.ImprovementsId.ToList();
                vm.TypeOfPropertys = await _typeOfPropertyService.GetAllViewModel();
                vm.TypeOfSales = await _typeOfSaleService.GetAllViewModel();
                vm.Improvements = await _improvementService.GetAllViewModel();
                ViewBag.PreviousImprovementsIds = previousImprovementIds;
                return View("SaveProperty", vm);
            }

            SavePropertyViewModel property = await _propertyService.GetByIdSaveViewModel((int)vm.Id);

            if (vm.ImprovementsId != null && vm.ImprovementsId.Any())
            {
                await _propertyService.RemovePropertyImprovements((int)property.Id);

                foreach (var improvementId in vm.ImprovementsId)
                {
                    await _propertyService.AddPropertyImprovements((int)property.Id, improvementId);
                }
            }


            vm.AgentId = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            //SavePropertyViewModel property = await _propertyService.GetByIdSaveViewModel((int)vm.Id);

            List<IFormFile> newFiles = new List<IFormFile> { vm.File, vm.File2, vm.File3, vm.File4 }.Where(f => f != null).ToList();

            List<ImageViewModel> existingImages = await _imageService.GetImagesForPropertyId((int)vm.Id);

            int totalImagesAllowed = 4;
            int existingImagesToRemove = Math.Max(0, existingImages.Count + newFiles.Count - totalImagesAllowed);

            for (int i = 0; i < existingImagesToRemove; i++)
            {
                await _imageService.Delete(existingImages[i].Id);
            }

            foreach (var file in newFiles)
            {
                if (existingImages.Count < totalImagesAllowed)
                {
                    string imageUrl = UploadFile(file, vm.Id.ToString());
                    SaveImageViewModel saveImage = new SaveImageViewModel
                    {
                        ImageUrl = imageUrl,
                        PropertyId = (int)property.Id,
                        UserId = vm.AgentId
                    };

                    await _imageService.Add(saveImage);
                    existingImages.Add(new ImageViewModel { ImageUrl = imageUrl });
                }
            }

            await _propertyService.Update(vm, (int)vm.Id);
            return RedirectToRoute(new { controller = "Property", action = "MyProperties", id = vm.AgentId });
        }

        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> Delete(int id)
        {
            return View(await _propertyService.GetByIdSaveViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            var vm = await _propertyService.GetByIdSaveViewModel(id);
            await _propertyService.Delete(id);

            string basePath = $"/Images/propiedad/{vm.Code}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (Directory.Exists(path))
            {
                DirectoryInfo directory = new(path);

                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo folder in directory.GetDirectories())
                {
                    folder.Delete(true);
                }

                Directory.Delete(path);
            }

            return RedirectToRoute(new { controller = "Property", action = "MyProperties", id = vm.AgentId });
        }
        #endregion


        #region Private Methods
        private string UploadFile(IFormFile file, string id, bool isEditMode = false, string imagePath = "")
        {
            if (isEditMode)
            {
                if (file == null)
                {
                    return imagePath;
                }
            }
            string basePath = $"/Images/propiedad/{id}";
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
        #endregion
    }
}

