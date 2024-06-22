using MediatR;
using RealEstateApp.Core.Application.Interfaces.Services.Account;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.Features.Agents.Commands.ChangeStatusAgent
{
    public class ChangeStatusAgentCommand : IRequest<Response<int>>
    {
        [SwaggerParameter(Description = "ID of the agent whose status is going to be changed")]
        [Required]
        public string Id { get; set; }
        [SwaggerParameter(Description = "Status of the agent")]
        [Required]
        public bool Status { get; set; }
    }

    public class ChangeStatusAgentCommandHandler : IRequestHandler<ChangeStatusAgentCommand, Response<int>>
    {
        private readonly IAccountService _accountService;

        public ChangeStatusAgentCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<Response<int>> Handle(ChangeStatusAgentCommand command, CancellationToken cancellationToken)
        {
            var agent = await _accountService.GetUserById(command.Id);
            //if (agent == null) throw new ApiException($"There isn't any agent registered with the ID {command.Id} in the system.", (int)HttpStatusCode.NoContent);
            await _accountService.ToogleUserActiveStatusAsync(command.Id, command.Status);
            return new Response<int>(agent.Id);
        }
    }
}
