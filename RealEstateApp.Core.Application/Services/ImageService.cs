using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services.Generic;
using RealEstateApp.Core.Application.ViewModels.Image;
using RealEstateApp.Core.Application.ViewModels.User;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Services
{
    public class ImageService : GenericService<SaveImageViewModel, ImageViewModel, Image>, IImageService
    {

        private readonly IImageRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _userViewModel;
        private readonly IUserService _userService;

        public ImageService(IImageRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserService userService) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public override async Task<SaveImageViewModel> Add(SaveImageViewModel vm)
        {
            vm.UserId = _userViewModel.Id;

            return await base.Add(vm);
        }

        public async Task<List<ImageViewModel>> GetImagesForPropertyId(int propertyId)
        {
            List<Image> images = await _repository.GetImagesByPropertyId(propertyId);

            List<ImageViewModel> imageViewModels = _mapper.Map<List<ImageViewModel>>(images);

            return imageViewModels;
        }

        public override async Task Update(SaveImageViewModel vm, int id)
        {
            vm.UserId = _userViewModel.Id;

            await base.Update(vm, id);
        }
    }
}
