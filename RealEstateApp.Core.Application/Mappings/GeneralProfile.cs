using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.DTOs.Improvement;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Application.DTOs.TypeOfProperty;
using RealEstateApp.Core.Application.DTOs.TypeOfSale;
using RealEstateApp.Core.Application.Features.Improvements.Commands.CreateImprovement;
using RealEstateApp.Core.Application.Features.Improvements.Commands.UpdateImprovement;
using RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.CreateTypeOfProperty;
using RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.UpdateTypeOfProperty;
using RealEstateApp.Core.Application.Features.TypeOfSales.Commands.CreateTypeOfSale;
using RealEstateApp.Core.Application.Features.TypeOfSales.Commands.UpdateTypeOfSale;
using RealEstateApp.Core.Application.ViewModels.FavProperty;
using RealEstateApp.Core.Application.ViewModels.User;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperty;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.ViewModels.TypeOfSale;
using RealEstateApp.Core.Application.ViewModels.Improvement;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Application.ViewModels.Image;

namespace RealEstateApp.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region User Profile
            CreateMap<UserDTO, UserViewModel>()
                .ReverseMap();

            CreateMap<UserViewModel, SaveUserViewModel>()
                .ReverseMap();

            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ReverseMap();

            CreateMap<ForgotPasswordViewModel, ForgotPasswordRequest>()
                .ReverseMap();

            CreateMap<ResetPasswordViewModel, ResetPasswordRequest>()
                .ReverseMap();

            CreateMap<RegisterRequest, SaveUserViewModel>()
                .ForMember(x => x.File, opt => opt.Ignore())
                .ReverseMap();
                

            CreateMap<SaveUserViewModel, RegisterRequest>()
                .ReverseMap();
            #endregion

            #region "FavProperty Profile"
            CreateMap<FavProperty, FavPropertyViewModel>()
                .ReverseMap();

            CreateMap<FavProperty, SaveFavPropertyViewModel>()
                .ReverseMap();
            #endregion

            #region "Improvement Profile"
            CreateMap<Improvement, ImprovementDTO>()
                .ReverseMap();

            CreateMap<Improvement, ImprovementViewModel>()
               .ReverseMap();

            CreateMap<Improvement, SaveImprovementViewModel>()
               .ReverseMap();

            CreateMap<Improvement, CreateImprovementCommand>()
                .ReverseMap();

            CreateMap<Improvement, UpdateImprovementCommand>()
                .ReverseMap();

            CreateMap<UpdateImprovementResponse, Improvement>()
                .ReverseMap();
            #endregion

            #region "Property Profile"
            CreateMap<Property, PropertyDTO>()
                .ReverseMap();

            CreateMap<Property, PropertyViewModel>()
                .ReverseMap();

            CreateMap<Property, SavePropertyViewModel>()
                .ReverseMap();
            #endregion

            #region "TypeOfProperties Profile"
            CreateMap<TypeOfProperty, TypeOfPropertyDTO>()
                .ReverseMap()
                .ForMember(dest => dest.Properties, opt => opt.Ignore()); 

            CreateMap<TypeOfProperty, CreateTypeOfPropertyCommand>()
                .ReverseMap();

            CreateMap<TypeOfProperty, TypeOfPropertyViewModel>()
               .ReverseMap();

            CreateMap<TypeOfProperty, SaveTypeOfPropertyViewModel>()
               .ReverseMap();

            CreateMap<TypeOfProperty, UpdateTypeOfPropertyCommand>()
                .ReverseMap();

            CreateMap<UpdateTypeOfPropertyResponse, TypeOfProperty>()
                .ReverseMap();
            #endregion

            #region "TypeOfSales Profile"
            CreateMap<TypeOfSale, TypeOfSaleDTO>()
                .ReverseMap();

            CreateMap<TypeOfSale, TypeOfSaleViewModel>()
               .ReverseMap();

            CreateMap<TypeOfSale, SaveTypeOfSaleViewModel>()
               .ReverseMap();

            CreateMap<TypeOfSale, CreateTypeOfSaleCommand>()
                .ReverseMap();

            CreateMap<TypeOfSale, UpdateTypeOfSaleCommand>()
                .ReverseMap();

            CreateMap<UpdateTypeOfSaleResponse, TypeOfSale>()
                .ReverseMap();


            CreateMap<Image, ImageViewModel>()
                .ReverseMap();
            CreateMap<Image, SaveImageViewModel>()
                .ReverseMap();
            #endregion

            #region "Images Profile"
            CreateMap<Image, ImageViewModel>()
                .ReverseMap();
            #endregion

        }
    }
}
