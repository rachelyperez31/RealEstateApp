using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services.Generic;
using RealEstateApp.Core.Application.ViewModels.Improvement;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperty;
using RealEstateApp.Core.Application.ViewModels.TypeOfSale;
using RealEstateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Services
{
    public class TypeOfSaleService : GenericService<SaveTypeOfSaleViewModel, TypeOfSaleViewModel, TypeOfSale>, ITypeOfSaleService
    {

        private readonly ITypeOfSaleRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _userViewModel;
        private readonly IUserService _userService;

        public TypeOfSaleService(ITypeOfSaleRepository repository, IMapper mapper, IFavPropertyRepository favPropertyRepository, IHttpContextAccessor httpContextAccessor, IUserService userService) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public override async Task<List<TypeOfSaleViewModel>> GetAllViewModel()
        {
            var topList = await base.GetAllViewModel();

            return _mapper.Map<List<TypeOfSaleViewModel>>(topList);
        }
    }
}
