using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.DTOs.Agent;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services.Account;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.Features.Agents.Queries.GetAgentById
{
    public class GetAgentByIdQuery : IRequest<Response<AgentDTO>>
    {
        // parámetros que espera este request
        [SwaggerParameter(Description = "Must enter the agent ID you want to obtain.")]
        [Required]
        public string Id { get; set; }
    }

    public class GetAgentByIdQueryHandler : IRequestHandler<GetAgentByIdQuery, Response<AgentDTO>>
    {
        private readonly IAccountService _accountService;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public GetAgentByIdQueryHandler(IAccountService accountService, IPropertyRepository propertyRepository, IMapper mapper)
        {
            _accountService = accountService;
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        public async Task<Response<AgentDTO>> Handle(GetAgentByIdQuery request, CancellationToken cancellationToken)
        {
            return await GetDTOWithInclude(request.Id);
        }

        #region Private
        private async Task<Response<AgentDTO>> GetDTOWithInclude(string id)
        {
            var agent = await _accountService.GetUserById(id);

            //if (agent == null) throw new ApiException($"There isn't any agent registered with the ID {id} in the system.", (int)HttpStatusCode.NoContent);
            if (agent == null) return new Response<AgentDTO>(null);

            var properties = await _propertyRepository.GetAllAsync();
            var numberOfProperties = properties.Count(p => p.AgentId == agent.Id);

            AgentDTO agentDTO = new()
            {
                Id = agent.Id,
                FirstName = agent.FirstName,
                LastName = agent.LastName,
                NumberOfProperties = numberOfProperties,
                Email = agent.Email,
                PhoneNumber = agent.PhoneNumber,
            };

            return new Response<AgentDTO>(agentDTO);
        }
        #endregion
    }
}
