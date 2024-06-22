using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services.Generic;
using RealEstateApp.Core.Application.ViewModels.FavProperty;
using RealEstateApp.Core.Application.ViewModels.Image;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperty;
using RealEstateApp.Core.Application.ViewModels.TypeOfSale;
using RealEstateApp.Core.Domain.Entities;
using System;

namespace RealEstateApp.Core.Application.Services
{
    public class PropertyService : GenericService<SavePropertyViewModel, PropertyViewModel, Property>, IPropertyService
    {
        private readonly IPropertyRepository _repository;
        private readonly IFavPropertyRepository _favPropertyRepository;
        private readonly IPropertyImprovementRepository _propertyImprovementRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _userViewModel;

        public PropertyService(IPropertyRepository repository, IMapper mapper, IFavPropertyRepository favPropertyRepository, IPropertyImprovementRepository propertyImprovementRepository, IImageRepository imageRepository, IHttpContextAccessor httpContextAccessor) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _favPropertyRepository = favPropertyRepository;
            _propertyImprovementRepository = propertyImprovementRepository;
            _imageRepository = imageRepository;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        Random random = new Random();

        public override async Task<SavePropertyViewModel> Add(SavePropertyViewModel vm)
        {
            vm.AgentId = _userViewModel.Id;


            string generatedCode;
            do
            {
                generatedCode = GenerateRandomCode();
            } while (await PropertyCodeExists(generatedCode));

            vm.Code = generatedCode;

            return await base.Add(vm);
        }

        public override async Task<SavePropertyViewModel> GetByIdSaveViewModel(int id)
        {
            var property = await base.GetByIdSaveViewModel(id);

            var propertyImprovement = await _propertyImprovementRepository.GetPropertyImprovementsByPropertyId(id);
            property.ImprovementsId = propertyImprovement.Where(pi => !pi.IsDeleted).Select(pi => pi.ImprovementId).ToList();
            var imgUrls = await _imageRepository.GetImagesByPropertyId(id);
            var imgsVm = _mapper.Map<List<ImageViewModel>>(imgUrls);
            property.PreviousImages = imgsVm;

            return property;
        }

        public async Task AddPropertyImprovements(int propertyId, int improvementId)
        {
            PropertyImprovement propertyImprovement = new()
            {
                PropertyId = propertyId,
                ImprovementId = improvementId
            };

            await _propertyImprovementRepository.AddAsync(propertyImprovement);
        }

        public async Task RemovePropertyImprovements(int propertyId)
        {
            var propertyImprovements = await _propertyImprovementRepository.GetPropertyImprovementsByPropertyId(propertyId);
            foreach (var propertyImprovement in propertyImprovements)
            {
                await _propertyImprovementRepository.DeleteAsync(propertyImprovement);
            }
        }

        public async Task<List<int>> GetPropertyImprovementIds(int propertyId)
        {
            var propertyImprovements = await _propertyImprovementRepository.GetPropertyImprovementsByPropertyId(propertyId);
            var improvementIds = propertyImprovements
                            .Where(pi => !pi.IsDeleted)
                            .Select(pi => pi.Id)
                            .ToList();
            return improvementIds;
        }

        private string GenerateRandomCode()
        {
            return random.Next(100000, 999999).ToString();
        }

        private async Task<bool> PropertyCodeExists(string code)
        {
            var properties = await _repository.GetAllAsync();
            return properties.Any(p => p.Code == code);
        }

        public override async Task Update(SavePropertyViewModel vm, int id)
        {
            vm.AgentId = _userViewModel.Id;
            await base.Update(vm, id);
        }

        public async Task<List<PropertyViewModel>> GetAllViewModelWithFilter(FilterPropertyViewModel filters)
        {
            var propertiesList = await _repository.GetAllWithIncludeAsync(new List<string> { "TypeOfProperty", "TypeOfSale", "Images" });

            propertiesList = propertiesList.Where(p =>
                (filters.Code == null || p.Code.Contains(filters.Code)) &&
                (!filters.TypeOfPropertyId.HasValue || p.TypeOfPropertyId == filters.TypeOfPropertyId.Value) &&
                (!filters.MinPrice.HasValue || p.Price >= filters.MinPrice.Value) &&
                (!filters.MaxPrice.HasValue || p.Price <= filters.MaxPrice.Value) &&
                (!filters.NumberOfRooms.HasValue || p.NumberOfRooms == filters.NumberOfRooms.Value) &&
                (!filters.NumberOfBathrooms.HasValue || p.NumberOfBathrooms == filters.NumberOfBathrooms.Value))
                .OrderByDescending(p => p.CreatedOn)
                .ToList();

            var propertiesVm = _mapper.Map<List<PropertyViewModel>>(propertiesList);
            var userViewModel = _httpContextAccessor.HttpContext?.Session.Get<AuthenticationResponse>("user");

            if (userViewModel != null)
            {
                foreach (var property in propertiesVm)
                {
                    var isFavorite = await _favPropertyRepository.IsPropertyFavorite(userViewModel.Id, property.Code);
                    property.IsFav = isFavorite;
                }
            }

            return propertiesVm;
        }

        public async Task<List<PropertyViewModel>> GetAllByUserId(string id)
        {
            var propertiesList = await _repository.GetAllWithIncludeAsync(new List<string> { "TypeOfProperty", "TypeOfSale", "Images" });

            propertiesList = propertiesList.Where(p =>
                (p.AgentId == id))
                .ToList();

            return _mapper.Map<List<PropertyViewModel>>(propertiesList);
        }

        public async Task<List<FavPropertyViewModel>> GetFavProperties(string clientId)
        {
            var favProperties = await _favPropertyRepository.GetFavoriteProperties(clientId);

            var favoritePropertyCodes = favProperties.Select(p => p.PropertyCode).ToList();

            var allProperties = await _repository.GetAllWithIncludeAsync(
                new List<string> { "TypeOfProperty", "TypeOfSale", "Images", "Improvements" });

            var propertiesData = allProperties.Where(p => favoritePropertyCodes.Contains(p.Code)).ToList();

            var favPropertiesVm = new List<FavPropertyViewModel>();

            foreach (var favProperty in favProperties)
            {
                var propertyData = propertiesData.FirstOrDefault(p => p.Code == favProperty.PropertyCode);
                if (propertyData != null)
                {
                    var propertyVm = new FavPropertyViewModel
                    {
                        UserId = favProperty.UserId,
                        PropertyId = favProperty.PropertyCode,
                        Property = new PropertyViewModel
                        {
                            Id = propertyData.Id,
                            Code = propertyData.Code,
                            Price = propertyData.Price,
                            NumberOfRooms = propertyData.NumberOfRooms,
                            NumberOfBathrooms = propertyData.NumberOfBathrooms,
                            Size = propertyData.Size,
                            Description = propertyData.Description,
                            AgentId = propertyData.AgentId,
                            TypeOfPropertyId = propertyData.TypeOfPropertyId,
                            TypeOfProperty = propertyData.TypeOfProperty != null ? new TypeOfPropertyViewModel
                            {
                                Id = propertyData.TypeOfProperty.Id,
                                Name = propertyData.TypeOfProperty.Name
                            } : null,
                            TypeOfSaleId = propertyData.TypeOfSaleId,
                            TypeOfSale = propertyData.TypeOfSale != null ? new TypeOfSaleViewModel
                            {
                                Id = propertyData.TypeOfSale.Id,
                                Name = propertyData.TypeOfSale.Name
                            } : null,
                            IsFav = true,
                            Images = propertyData.Images?.Select(img => new ImageViewModel { ImageUrl = img.ImageUrl }).ToList()
                        }
                    };

                    favPropertiesVm.Add(propertyVm);
                }
            }

            return favPropertiesVm;
        }

        public async Task ToogleFavStatus(string clientId, string code)
        {
            var favProperty = await _favPropertyRepository.GetFavoriteProperty(clientId, code);

            if (favProperty == null)
            {
                FavProperty newFavProperty = new()
                {
                    UserId = clientId,
                    PropertyCode = code
                };
                await _favPropertyRepository.AddAsync(newFavProperty);
            }
            else if (favProperty.IsDeleted)
            {
                favProperty.IsDeleted = false;
                favProperty.DeletedOn = null;
                favProperty.DeletedBy = null;
                await _favPropertyRepository.UpdateAsync(favProperty, favProperty.UserId, favProperty.PropertyCode);
            }
            else
            {
                await _favPropertyRepository.DeleteAsync(favProperty);
            }
        }
        public async Task<PropertyViewModel> GetPropertyByIdWithIncludes(int id)
        {
            var propertiesWithIncludes = await _repository.GetAllWithIncludeAsync(new List<string> { "TypeOfProperty", "TypeOfSale", "Images", "Improvements" });
            var property = propertiesWithIncludes.FirstOrDefault(p => p.Id == id);



            return _mapper.Map<PropertyViewModel>(property);
        }

        public async Task<List<PropertyViewModel>> GetAgentProperties(string agentId)
        {
            var properties = await _repository.GetAllWithIncludeAsync(new List<string> { "TypeOfProperty", "TypeOfSale", "Images" });
            var agentProperties = properties.Where(x => x.AgentId == agentId).ToList();
            return _mapper.Map<List<PropertyViewModel>>(agentProperties);
        }
        public async Task<int> GetAgentPropertiesCount(string agentId)
        {
            var properties = await _repository.GetAllAsync();
            var agentProperties = properties.Where(x => x.AgentId == agentId && !x.IsDeleted).ToList();
            return agentProperties.Count();
        }

        public async Task<int> GetRegisteredProperties()
        {
            var properties = await _repository.GetAllAsync();
            var activeProperties = properties.Where(p => !p.IsDeleted);
            return activeProperties.Count();
        }
    }
}
