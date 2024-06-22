using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.DTOs.Agent;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services.Account;
using RealEstateApp.Core.Application.Wrappers;

namespace RealEstateApp.Core.Application.Features.Agents.Queries.GetAllAgents
{
    public class GetAllAgentsQuery : IRequest<Response<IList<AgentDTO>>>
    {
        // parámetros que espera este request
    }

    public class GetAllAgentsQueryHandler : IRequestHandler<GetAllAgentsQuery, Response<IList<AgentDTO>>>
    {
        private readonly IAccountService _accountService;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public GetAllAgentsQueryHandler(IAccountService accountService, IPropertyRepository propertyRepository, IMapper mapper)
        {
            _accountService = accountService;
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        public async Task<Response<IList<AgentDTO>>> Handle(GetAllAgentsQuery request, CancellationToken cancellationToken)
        {
            return await GetAllDTOsWithInclude();
        }

        #region Private
        private async Task<Response<IList<AgentDTO>>> GetAllDTOsWithInclude()
        {
            var usersList = await _accountService.GetAllUsersAsync();
            var agentsList = usersList.Where(a => a.Role.Any(role => role == Roles.Agent.ToString()));

            //if (agentsList == null) throw new ApiException($"There are not agents registered in the system.", (int)HttpStatusCode.NoContent);

            var properties = await _propertyRepository.GetAllAsync();

            var agentDTOList = agentsList.Select(agent =>
            {
                var numberOfProperties = properties.Count(p => p.AgentId == agent.Id);

                return new AgentDTO
                {
                    Id = agent.Id,
                    FirstName = agent.FirstName,
                    LastName = agent.LastName,
                    NumberOfProperties = numberOfProperties,
                    Email = agent.Email,
                    PhoneNumber = agent.PhoneNumber,
                };
            }).ToList();

            return new Response<IList<AgentDTO>>(agentDTOList);
        }
        #endregion
    }
}

