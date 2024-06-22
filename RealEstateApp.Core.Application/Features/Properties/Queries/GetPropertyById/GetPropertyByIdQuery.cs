using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Application.DTOs.TypeOfProperty;
using RealEstateApp.Core.Application.DTOs.TypeOfSale;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services.Account;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.Properties.Queries.GetPropertyById
{
    public class GetPropertyByIdQuery : IRequest<Response<PropertyDTO>>
    {
        // parámetros que espera este request
        [SwaggerParameter(Description = "ID of the property you want to obtain.")]
        [Required]
        public int Id { get; set; }
    }

    public class GetPropertyByIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, Response<PropertyDTO>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public GetPropertyByIdQueryHandler(IPropertyRepository propertyRepository, IAccountService accountService, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<Response<PropertyDTO>> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            return await GetDTOWithInclude(request.Id);
        }

        #region Private
        private async Task<Response<PropertyDTO>> GetDTOWithInclude(int id)
        {
            var propertiesList = await _propertyRepository.GetAllWithIncludeAsync(new List<string> { "TypeOfProperty", "TypeOfSale", "Improvements" });
            var property = propertiesList.FirstOrDefault(p => p.Id == id && !p.IsDeleted);

            //if (property == null) throw new ApiException($"There isn't any property with this ID {id} in the system.", (int)HttpStatusCode.NoContent);

            if (property == null) return new Response<PropertyDTO>(null);

            var agent = await _accountService.GetUserById(property.AgentId);
            var agentFullName = $"{agent.FirstName} {agent.LastName}";

            var propertyDTO = new PropertyDTO
            {
                Id = property.Id,
                Code = property.Code,
                TypeOfProperty = _mapper.Map<TypeOfPropertyDTO>(property.TypeOfProperty),
                TypeOfPropertyId = property.TypeOfPropertyId,
                TypeOfPropertyName = property.TypeOfProperty.Name,
                TypeOfSale = _mapper.Map<TypeOfSaleDTO>(property.TypeOfSale),
                TypeOfSaleId = property.TypeOfSaleId,
                TypeOfSaleName = property.TypeOfSale.Name,
                Price = property.Price,
                Size = property.Size,
                NumberOfRooms = property.NumberOfRooms,
                NumberOfBathRooms = property.NumberOfBathrooms,
                Description = property.Description,
                Improvements = property.Improvements.Select(i => i.Name).ToList(),
                AgentFullName = agentFullName,
                AgentId = property.AgentId
            };

            return new Response<PropertyDTO>(propertyDTO);
        }
        #endregion
    }
}
