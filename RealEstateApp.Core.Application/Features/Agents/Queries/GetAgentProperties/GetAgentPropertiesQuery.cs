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

namespace RealEstateApp.Core.Application.Features.Agents.Queries.GetAgentProperties
{
    public class GetAgentPropertiesQuery : IRequest<Response<IList<PropertyDTO>>>
    {
        // parámetros que espera este request
        [SwaggerParameter(Description = "Must enter the agent ID from whom you want to obtain his/her properties.")]
        [Required]
        public string AgentId { get; set; }
    }

    public class GetAgentPropertiesQueryHandler : IRequestHandler<GetAgentPropertiesQuery, Response<IList<PropertyDTO>>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public GetAgentPropertiesQueryHandler(IPropertyRepository propertyRepository, IAccountService accountService, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _accountService = accountService;
            _mapper = mapper;
        }
        public async Task<Response<IList<PropertyDTO>>> Handle(GetAgentPropertiesQuery request, CancellationToken cancellationToken)
        {
            return await GetAllDTOsWithInclude(request.AgentId);
        }

        #region Private
        private async Task<Response<IList<PropertyDTO>>> GetAllDTOsWithInclude(string agentId)
        {
            var propertiesList = await _propertyRepository.GetAllWithIncludeAsync(new List<string> { "TypeOfProperty", "TypeOfSale", "Improvements" });

            var agentProperties = propertiesList.Where(p => p.AgentId == agentId && !p.IsDeleted).ToList();

            /*if (agentProperties == null || agentProperties.Count == 0)
            {
                throw new ApiException($"This agent ({agentId}) has not registered any properties.", (int)HttpStatusCode.NoContent);
            }*/

            var users = await _accountService.GetAllUsersAsync();

            var agentIdToNameMap = users.ToDictionary(u => u.Id, u => $"{u.FirstName} {u.LastName}");

            var propertyDTOList = agentProperties.Select(property =>
            {
                var agentFullName = agentIdToNameMap.TryGetValue(property.AgentId, out var name) ? name : "Unknown";

                return new PropertyDTO
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
            }).ToList();

            return new Response<IList<PropertyDTO>>(propertyDTOList);
        }
        #endregion
    }
}
