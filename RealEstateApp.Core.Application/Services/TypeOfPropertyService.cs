using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services.Generic;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperty;
using RealEstateApp.Core.Domain.Entities;


namespace RealEstateApp.Core.Application.Services
{
    public class TypeOfPropertyService : GenericService<SaveTypeOfPropertyViewModel, TypeOfPropertyViewModel, TypeOfProperty>, ITypeOfPropertyService
    {
        private readonly ITypeOfPropertyRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _userViewModel;
        private readonly IUserService _userService;
        private readonly IFavPropertyRepository _favPropertyRepository;

        public TypeOfPropertyService(ITypeOfPropertyRepository  repository, IMapper mapper, IFavPropertyRepository favPropertyRepository, IHttpContextAccessor httpContextAccessor, IUserService userService) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _favPropertyRepository = favPropertyRepository;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public override async Task<List<TypeOfPropertyViewModel>> GetAllViewModel()
        {
            var topList = await base.GetAllViewModel();

            return _mapper.Map<List<TypeOfPropertyViewModel>>(topList);
        }

    }
}
